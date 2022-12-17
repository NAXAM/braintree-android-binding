using System.Collections.Generic;
using Cake.Core;
using Cake.Frosting;
using Gradle.Binderator;
using Xamarin.Build.Download;

public class BuildContext : FrostingContext
{
    public bool Delay { get; set; }
    public string BasePath { get; set; }
    public string GeneratedSlnPath { get; set; }

    public List<ArtifactModel> Artifacts { get; set; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        Delay = context.Arguments.HasArgument("delay");
        var defaultBasePath = System.IO.Path.Combine(
            Environment.WorkingDirectory.FullPath,
            "../../../../.."
        );
        BasePath = context.Arguments.GetArgument("base-path")
                    ?? defaultBasePath;
    }
}
