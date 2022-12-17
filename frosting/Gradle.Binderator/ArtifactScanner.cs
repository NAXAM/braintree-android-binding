namespace Gradle.Binderator;

public static class ArtifactScanner
{
    public static List<ArtifactModel> Scan(
        List<ArtifactModel> existingArtifacts,
        List<ArtifactModel> externalArtifacts,
        string groupId,
        string artifactId,
        string version,
        Action<string> log,
        List<string> missingArtifacts = null)
    {
        List<ArtifactModel> artifacts = new();
        missingArtifacts ??= new();
        var homeFolder = Platform.IsWindows
                    ? Environment.SpecialFolder.LocalApplicationData
                    : Environment.SpecialFolder.Personal;
        var homeFolderPath = Environment.GetFolderPath(homeFolder);
        var artifactVersionFolderPath = Path.Combine(
            homeFolderPath,
            $".gradle/caches/modules-2/files-2.1/{groupId}/{artifactId}/{version}"
        );

        if (!Directory.Exists(artifactVersionFolderPath))
        {
            log?.Invoke(
                $"ARTIFACT FOLDER MISSING >> {groupId}:{artifactId}-{version} << {artifactVersionFolderPath}"
            );
            missingArtifacts.Add($"{groupId}:{artifactId}-{version} << {artifactVersionFolderPath}");

            return artifacts;
        }

        var files = Directory.GetFiles(
            artifactVersionFolderPath,
            "*.*",
            SearchOption.AllDirectories
        );

        var artifactPomFileName = $"{artifactId}-{version}.pom";
        var pomFilePath = files.FirstOrDefault(x => x.EndsWith(artifactPomFileName));

        if (pomFilePath == null) return artifacts;

        var xmlDocument = new XmlDocument();
        xmlDocument.Load(pomFilePath);

        // Add the namespace.  
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
        nsmgr.AddNamespace("mvn", "http://maven.apache.org/POM/4.0.0");

        var packaging = xmlDocument.DocumentElement.SelectSingleNode("descendant::mvn:packaging", nsmgr)?.InnerText;

        var artifact = new ArtifactModel
        {
            GroupId = groupId,
            ArtifactId = artifactId,
            Version = version,
            NugetVersion = version,
            NugetPackageId = CreateNugetId(groupId, artifactId),
            Files = files.Select(x => x.Replace(homeFolderPath, string.Empty).Trim('/')).ToArray(),
            Packaging = packaging,
        };
        artifacts.Add(artifact);

        var dependencies = xmlDocument.DocumentElement.SelectNodes("descendant::mvn:dependency", nsmgr);

        var parentArtifactIds = new List<(string, string)>();
        foreach (XmlNode dependency in dependencies)
        {
            var scope = dependency.SelectSingleNode("descendant::mvn:scope", nsmgr).InnerText;
            if (scope == "test") continue;            

            var xgroupId = dependency.SelectSingleNode("descendant::mvn:groupId", nsmgr).InnerText;
            var xartifactId = dependency.SelectSingleNode("descendant::mvn:artifactId", nsmgr).InnerText;
            var existingArtifact = existingArtifacts.FirstOrDefault(
                x => x.GroupId == xgroupId && x.ArtifactId == xartifactId
            );

            var xversion = dependency.SelectSingleNode("descendant::mvn:version", nsmgr)?.InnerText;
            
            if (existingArtifact != null && xversion != existingArtifact.Version)
            {
                parentArtifactIds.Add((existingArtifact.NugetPackageId, scope));
                log?.Invoke(
                    $"ARTIFACT EXISTS >> {xgroupId}:{xartifactId}-{xversion} << {existingArtifact.Version}"
                );

                continue;
            }

            var parentArtifact = externalArtifacts
                                ?.FirstOrDefault(
                                    x => x.GroupId == xgroupId &&
                                        x.ArtifactId == xartifactId);

            if (parentArtifact == null)
            {
                var parentArtifacts = Scan(
                    existingArtifacts,
                    externalArtifacts,
                    xgroupId,
                    xartifactId,
                    xversion,
                    log,
                    missingArtifacts);

                if (parentArtifacts.Count == 0) continue;

                artifacts.AddRange(parentArtifacts);
                existingArtifacts.AddRange(parentArtifacts);
                artifacts = artifacts.Distinct().ToList();
                parentArtifactIds.Add((parentArtifacts[0].NugetPackageId, scope));
            }
            else
            {
                if (xversion != parentArtifact.Version)
                {
                    log?.Invoke(
                        $"EXTERNAL ARTIFACT >> {xgroupId}:{xartifactId}-{xversion} << {parentArtifact.Version}"
                    );
                }
                artifacts.Add(parentArtifact);
                parentArtifactIds.Add((parentArtifact.NugetPackageId, scope));
            }
        }
        artifact.ParentArtifacts = parentArtifactIds.ToArray();

        return artifacts;
    }

    static string CreateNugetId(string groupId, string artifactId)
    {
        return string.Join(
                ".",
                groupId.Split('.')
                    .Select(x => x.ToCSharpName())
                ) + "." + artifactId.ToCSharpName();
    }
}

