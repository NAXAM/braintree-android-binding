using System.Reflection;
using System.Runtime.CompilerServices;
using Android.App;

#if !NETCOREAPP
[assembly: Android.LinkerSafe]
#endif

[assembly: AssemblyMetadata ("IsTrimmable", "True")]