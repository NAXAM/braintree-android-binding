using System.Linq;
using Gradle.Binderator;
using DependpencyList = System.Collections.Generic.List<Gradle.Binderator.ArtifactModel>;

static class ExternalDependencies
{
    public static DependpencyList Artifacts = new DependpencyList
    {
        new ArtifactModel
        {
            GroupId = "androidx.interpolator",
            ArtifactId = "interpolator",
            Version = "1.0.0",
            NugetVersion = "1.0.0.15",
            NugetPackageId = "Xamarin.AndroidX.Interpolator",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.drawerlayout",
            ArtifactId = "drawerlayout",
            Version = "1.0.0",
            NugetVersion = "1.0.0.5",
            NugetPackageId = "Xamarin.AndroidX.DrawerLayout",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.customview",
            ArtifactId = "customview",
            Version = "1.0.0",
            NugetVersion = "1.0.0.2",
            NugetPackageId = "Xamarin.AndroidX.CustomView",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.activity",
            ArtifactId = "activity",
            Version = "1.2.3",
            NugetVersion = "1.2.3",
            NugetPackageId = "Xamarin.AndroidX.Activity",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.appcompat",
            ArtifactId = "appcompat",
            Version = "1.3.0",
            NugetVersion = "1.3.0",
            NugetPackageId = "Xamarin.AndroidX.AppCompat",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.savedstate",
            ArtifactId = "savedstate",
            Version = "1.1.0",
            NugetVersion = "1.1.0.8",
            NugetPackageId = "Xamarin.AndroidX.SavedState",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.lifecycle",
            ArtifactId = "lifecycle-viewmodel",
            Version = "2.3.1",
            NugetVersion = "2.3.1.3",
            NugetPackageId = "Xamarin.AndroidX.Lifecycle.ViewModel",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.lifecycle",
            ArtifactId = "lifecycle-runtime",
            Version = "2.3.1",
            NugetVersion = "2.3.1.4",
            NugetPackageId = "Xamarin.AndroidX.Lifecycle.Runtime",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.startup",
            ArtifactId = "startup-runtime",
            Version = "1.1.0",
            NugetVersion = "1.1.0.4",
            NugetPackageId = "Xamarin.AndroidX.Startup.StartupRuntime",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.tracing",
            ArtifactId = "tracing",
            Version = "1.0.0",
            NugetVersion = "1.0.0.6",
            NugetPackageId = "Xamarin.AndroidX.Tracing.Tracing",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "org.jetbrains.kotlin",
            ArtifactId = "kotlin-stdlib-jdk8",
            Version = "1.5.31",
            NugetVersion = "1.5.31.4",
            NugetPackageId = "Xamarin.Kotlin.StdLib.Jdk8",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.lifecycle",
            ArtifactId = "lifecycle-common",
            Version = "2.3.1",
            NugetVersion = "2.3.1.3",
            NugetPackageId = "Xamarin.AndroidX.Lifecycle.Common",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.cursoradapter",
            ArtifactId = "cursoradapter",
            Version = "1.0.0",
            NugetVersion = "1.0.0.15",
            NugetPackageId = "Xamarin.AndroidX.CursorAdapter",
            DependencyOnly = true,
        },
        new ArtifactModel {
            GroupId = "com.squareup.okhttp3",
            ArtifactId = "okhttp",
            Version = "4.9.3",
            NugetVersion = "4.9.3.2",
            NugetPackageId = "Square.OkHttp3",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.arch.core",
            ArtifactId = "core-common",
            Version = "2.1.0",
            NugetVersion = "2.1.0.16",
            NugetPackageId = "Xamarin.AndroidX.Arch.Core.Common",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.core",
            ArtifactId = "core",
            Version = "1.7.0",
            NugetVersion = "1.7.0.2",
            NugetPackageId = "Xamarin.AndroidX.Core",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.core",
            ArtifactId = "core-ktx",
            Version = "1.7.0",
            NugetVersion = "1.7.0.2",
            NugetPackageId = "Xamarin.AndroidX.Core.Core.Ktx",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.arch.core",
            ArtifactId = "core-runtime",
            Version = "2.1.0",
            NugetVersion = "2.1.0.16",
            NugetPackageId = "Xamarin.AndroidX.Arch.Core.Runtime",
            DependencyOnly = true,
        },

        new ArtifactModel
        {
            GroupId = "org.jetbrains.kotlin",
            ArtifactId = "kotlin-stdlib-jdk7",
            Version = "1.5.31",
            NugetVersion = "1.5.31.4",
            NugetPackageId = "Xamarin.Kotlin.StdLib.Jdk7",
            DependencyOnly = true,
        },

        new ArtifactModel
        {
            GroupId = "org.jetbrains.kotlin",
            ArtifactId = "kotlin-stdlib-common",
            Version = "1.5.31",
            NugetVersion = "1.5.31.4",
            NugetPackageId = "Xamarin.Kotlin.StdLib.Common",
            DependencyOnly = true,
        },

        new ArtifactModel
        {
            GroupId = "org.jetbrains.kotlin",
            ArtifactId = "kotlin-stdlib",
            Version =  "1.5.31",
            NugetVersion =  "1.5.31.4",
            NugetPackageId = "Xamarin.Kotlin.StdLib",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "com.google.code.gson",
            ArtifactId = "gson",
            Version = "2.8.6",
            NugetVersion = "2.8.6",
            NugetPackageId = "GoogleGson",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.core",
            ArtifactId = "core-ktx",
            Version = "1.7.0",
            NugetVersion = "1.7.0.2",
            NugetPackageId = "Xamarin.AndroidX.Core.Core.Ktx",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "com.google.android.material",
            ArtifactId = "material",
            Version = "1.4.0",
            NugetVersion = "1.4.0.6",
            NugetPackageId = "Xamarin.Google.Android.Material",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "com.google.android.gms",
            ArtifactId = "play-services-wallet",
            Version = "16.0.0",
            NugetVersion = "71.1600.4",
            NugetPackageId = "Xamarin.GooglePlayServices.Wallet ",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.recyclerview",
            ArtifactId = "recyclerview",
            Version = "1.2.1",
            NugetVersion = "1.2.1.8",
            NugetPackageId = "Xamarin.AndroidX.RecyclerView",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.lifecycle",
            ArtifactId = "lifecycle-runtime-ktx",
            Version = "2.4.1",
            NugetVersion = "2.4.1.2",
            NugetPackageId = "Xamarin.AndroidX.Lifecycle.Runtime.Ktx",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.fragment",
            ArtifactId = "fragment",
            Version = "1.4.1",
            NugetVersion = "1.4.1.2",
            NugetPackageId = "Xamarin.AndroidX.Fragment",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.constraintlayout",
            ArtifactId = "constraintlayout",
            Version = "2.0.4",
            NugetVersion = "2.0.4.3",
            NugetPackageId = "Xamarin.AndroidX.ConstraintLayout",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.cardview",
            ArtifactId = "cardview",
            Version = "1.0.0",
            NugetVersion = "1.0.0.17",
            NugetPackageId = "Xamarin.AndroidX.CardView",
            DependencyOnly = true,
        },
        new ArtifactModel
        {
            GroupId = "androidx.annotation",
            ArtifactId = "annotation",
            Version = "1.1.0",
            NugetVersion = "1.1.0.9",
            NugetPackageId = "Xamarin.AndroidX.Annotation",
            DependencyOnly = true,
        },
    };
}
