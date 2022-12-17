using System.Threading.Tasks;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Pack;
using Cake.Frosting;

[TaskName("nuget")]
[IsDependentOn(typeof(BinderateTask))]
public sealed class NugetTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var nugetsFolderPath = System.IO.Path.Combine(
            context.BasePath,
            "nugets"
        );
        var settings = new DotNetPackSettings
        {
            Configuration = "Release",
            OutputDirectory = nugetsFolderPath,
        };

        context.DotNetPack(
            context.GeneratedSlnPath,
            settings
        );
    }
}
