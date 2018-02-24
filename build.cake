#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

// Cake Addins
#addin nuget:?package=Cake.FileHelpers&version=2.0.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var VERSION = "2.10.0";
var NUGET_SUFIX = "";

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var solutionPath = "./braintree-droid.sln";
var artifacts = new [] {
    new Artifact {
        AssemblyInfoPath = "./Naxam.BraintreeDataCollector.Droid/Properties/AssemblyInfo.cs",
        NuspecPath = "./braintree-datacollector.nuspec",
        DownloadUrl = "http://central.maven.org/maven2/com/braintreepayments/api/data-collector/{0}/data-collector-{0}.jar",
        JarPath = "./Naxam.BraintreeDataCollector.Droid/Jars/data-collector.jar"
    },
    // new Artifact {
    //     AssemblyInfoPath = "./Naxam.BrainTree.Droid/Properties/AssemblyInfo.cs",
    //     NuspecPath = "./braintree.nuspec",
    //     DownloadUrl = "http://central.maven.org/maven2/com/braintreepayments/api/braintree/{0}/braintree-{0}.aar",
    //     JarPath = "./Naxam.BrainTree.Droid/Jars/braintree.aar"
    // }
};

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Downloads")
    .Does(() =>
{
    foreach(var artifact in artifacts) {
        var downloadUrl = string.Format(artifact.DownloadUrl, VERSION);
        var jarPath = string.Format(artifact.JarPath, VERSION);

        DownloadFile(downloadUrl, jarPath);
    }
});

Task("Clean")
    .Does(() =>
{
    CleanDirectory("./packages");

    var nugetPackages = GetFiles("./*.nupkg");

    foreach (var package in nugetPackages)
    {
        DeleteFile(package);
    }
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionPath);
});

Task("Build")
    .IsDependentOn("Downloads")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild(solutionPath, settings => settings.SetConfiguration(configuration));
});

Task("UpdateVersion")
    .Does(() => 
{
    foreach(var artifact in artifacts) {
        ReplaceRegexInFiles(artifact.AssemblyInfoPath, "\\[assembly\\: AssemblyVersion([^\\]]+)\\]", string.Format("[assembly: AssemblyVersion(\"{0}\")]", VERSION));
    }
});

Task("Pack")
    .IsDependentOn("UpdateVersion")
    .IsDependentOn("Build")
    .Does(() =>
{
    foreach(var artifact in artifacts) {
        NuGetPack(artifact.NuspecPath, new NuGetPackSettings {
            Version = VERSION+NUGET_SUFIX,
            // Dependencies = new []{
            //     new NuSpecDependency {
            //         Id = "Naxam.BrainTree.Core",
            //         Version = VERSION+NUGET_SUFIX
            //     },
            //     new NuSpecDependency {
            //         Id = "Naxam.Paypal.OneTouch",
            //         Version = VERSION+NUGET_SUFIX
            //     },
            //     new NuSpecDependency {
            //         Id = "Xamarin.GooglePlayServices.Wallet",
            //         Version = "60.1142.0"
            //     }
            // }, TODO
            ReleaseNotes = new [] {
                $"Braintree SDK - DataCollector v{VERSION}"
            }
        });
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

class Artifact {
    public string AssemblyInfoPath { get; set; }

    public string SolutionPath { get; set; }

    public string DownloadUrl  { get; set; }

    public string JarPath { get; set; }

    public string NuspecPath { get; set; }
}