using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml;
using Cake.Common.IO;
using Cake.Core.Diagnostics;
using Cake.Frosting;
using Gradle.Binderator;
using Xamarin.Build.Download;

[TaskName("init")]
public sealed class InitializeTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        List<string> missingArtifacts = new();
        List<ArtifactModel> artifacts = ArtifactScanner.Scan(
            new List<ArtifactModel>(),
            ExternalDependencies.Artifacts,
            "com.braintreepayments.api",
            "drop-in",
            "6.5.0",
            log => { if (log.Contains("MISSING")) context.Log.Information(log); },
            missingArtifacts
        );

        if (missingArtifacts.Count > 0)
        {
            missingArtifacts.Clear();
            artifacts = ArtifactScanner.Scan(
                artifacts,
                ExternalDependencies.Artifacts,
                "com.braintreepayments.api",
                "drop-in",
                "6.5.0",
                log => { if (log.Contains("MISSING")) context.Log.Information(log); },
                missingArtifacts
            );

            if (missingArtifacts.Count > 0)
            {
                throw new InvalidOperationException("BIG MISTAKE >> PLEASE CHECK!");
            }
        }

        var configPath = Path.Combine(context.BasePath, "artifacts.g.json");
        if (File.Exists(configPath))
        {
            File.Delete(configPath);
        }

        var json = JsonSerializer.Serialize(artifacts);
        File.WriteAllText(configPath, json);

        var sourceFolderPath = System.IO.Path.Combine(
            context.BasePath,
            "source"
            );

        var artifactsToBind = artifacts
            .Where(x => !x.DependencyOnly)
            .ToList();

        foreach (var artifact in artifactsToBind)
        {
            var artifactFolderPath = System.IO.Path.Combine(
                sourceFolderPath,
                artifact.GroupId,
                artifact.ArtifactId
            );

            if (Directory.Exists(artifactFolderPath)) continue;

            var bindingDefaultZipFilePath = System.IO.Path.Combine(
                context.BasePath,
                "binding-default.zip"
            );
            context.Unzip(
                bindingDefaultZipFilePath,
                artifactFolderPath
            );
        }

        context.Artifacts = artifacts;
    }
}
