using System;
using Cake.Common.Tools.DotNet;
using Cake.Core;
using Cake.Frosting;

[TaskName("samples")]
//[IsDependentOn(typeof(NugetTask))]
public class SamplesTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.ProcessRunner.Start(
            "dotnet",
            new Cake.Core.IO.ProcessSettings {
                Arguments = "nuget locals -c all" ,
            }
        );

        var slnPath = System.IO.Path.Combine(
            context.BasePath,
            "samples/mapbox-droid-samples.sln"
        );
        context.DotNetMSBuild(
            slnPath
        );
    }
}