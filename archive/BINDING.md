# Xamarin.Android binding libraries for Braintree Android SDK

Naxam creates and maintains Xamarin.Android bindings for Braintree Drop-In library for Android, including:

 - [drop-in](https://mvnrepository.com/artifact/com.braintreepayments.api/drop-in)
 - [braintree-core](https://mvnrepository.com/artifact/com.braintreepayments.api/braintree-core)
 - [card](https://mvnrepository.com/artifact/com.braintreepayments.api/card)
 - [google-pay](https://mvnrepository.com/artifact/com.braintreepayments.api/google-pay)
 - [paypal](https://mvnrepository.com/artifact/com.braintreepayments.api/paypal)
 - [three-d-secure](https://mvnrepository.com/artifact/com.braintreepayments.api/three-d-secure)
 - [union-pay](https://mvnrepository.com/artifact/com.braintreepayments.api/union-pay)
 - [venmo](https://mvnrepository.com/artifact/com.braintreepayments.api/venmo)
 - [data-collector](https://mvnrepository.com/artifact/com.braintreepayments.api/data-collector)
 - [shared-utils](https://mvnrepository.com/artifact/com.braintreepayments.api/shared-utils)
 - [browser-switch](https://mvnrepository.com/artifact/com.braintreepayments.api/browser-switch)
 - [card-form](https://mvnrepository.com/artifact/com.braintreepayments/card-form)
 - [cardinalmobilesdk](https://cardinaldocs.atlassian.net/wiki/spaces/CMSDK/pages/1998914459/Setting+up+CardinalMobileSDK+-+Android)

## Building

The build script for this project uses [Cake](http://cakebuild.net).  To run the build, you can use one of the bootstrapper files either for Mac or Windows (experimental support only):

The bootstrapper script will automatically download Cake.exe and all the required tools and files into the `./tools/` folder.

Required dotnet core tools:

* binderator

Optional dotnet core tools:

* cake

To update all tools: 
```
	dotnet tool uninstall 	-g Cake.Tool
	dotnet tool install     -g Cake.Tool	
	dotnet tool uninstall   -g Xamarin.AndroidBinderator.Tool
	dotnet tool install     -g Xamarin.AndroidBinderator.Tool
```

The following targets can be specified:

 - `ci` builds the kitchen sink - what we run in CI
 - `libs` builds the class library bindings (depends on `binderate`)
 - `binderate` downloads the external dependencies and generates folder structure
 - `samples` builds all of the samples (depends on `libs`)
 - `nuget` builds the nuget packages (depends on `libs`)
 - `clean` cleans up everything

***NOTE***: The `binderate` build task may take awhile to run as it downloads several large dependencies.

You may want to consider passing `--verbosity diagnostic` (or `-Verbosity diagnostic` on Windows) to the bootstrapper to enable more verbose output, including downloading progress.

**Mac**:

```
dotnet cake --target=binderate && dotnet cake --target=libs
```

Optionally run:

```
dotnet cake --target=clean
```

before the build.

To build nuget packages, samples and API diff:

**Mac**:

```
dotnet cake --target=nuget && dotnet cake --target=samples && dotnet cake --target=diff
```

### Working in Visual Studio / Xamarin Studio

Before the `.sln` files will compile in Visual Studio or Xamarin Studio, the external dependencies need to be downloaded.  This can be done by running the `dotnet cake --target=externals`.  After the externals are setup, the `.sln` files should compile in an IDE.

### Issues
- [CardinalMobileSDK issue](https://developers.braintreepayments.com/guides/3d-secure/client-side/android/v3)