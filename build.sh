dotnet pack -c Release -o nugets cardinalmobilesdk/CardinalCommerce.CardinalMobileSdk.Droid.sln

dotnet nuget locals -c all

dotnet cake -- "$@"