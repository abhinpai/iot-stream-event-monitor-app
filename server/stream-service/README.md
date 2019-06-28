# Iot-Stream-Service

**Build:**
[![BuildStatus](https://bamboo.honeywell.com/plugins/servlet/wittified/build-status/ESYOJAMJ-YXAEFBEB)](https://bamboo.honeywell.com/browse/ESYOJAMJ-YXAEFBEB)

The code in this pipeline was created from the DotNetCore Library template as part of the CE DevOps Initiative. For more information about the template see the [Design](https://confluence.honeywell.com/display/HDO/DotNetCore+Library). For more information about the build scripts in general please see [Build Script Design](https://confluence.honeywell.com/display/HDO/Build+Script+Design)

## Getting Started

These instructions will allow you to build the library component on your local machine for development and testing purposes.

This code is built using the [Cake](https://cakebuild.net/) build framework. It can be built using locally installed components or a containerized environment

The recommended editor for working on this code is [Visual Studio Code](https://code.visualstudio.com/Download). The recommended extensions can be viewed by running the VSCode command "Extensions: Show Recommended Extensions". Once the Cake extension is installed, run the VSCode command "Cake: Install intellisense support" to make it easier to work on the build scripts.

VSCode commands can be executed on _command pallete_ which can be launched using Ctrl+Shift+P key combination.

### Containerized Enviornments

On Honeywell laptops, only Linux and Mac operating systems can be used to run the build in a containerized environment.  In most cases Docker cannot be properly installed on Windows due to the policies installed by Cisco AnyConnect which prevents local volume mounting. Users who have *full* admin access to their machines can disable AnyConnect whilst using Docker, but this means they often cannot be connected to the Honeywell network at the same time, leading to other complications.

For Windows users, it is possible to use a containerized evironment by using these [Vagrant Instructions](https://confluence.honeywell.com/display/HDO/Setting+up+Ansible+and+Docker+in+Vagrant+box+for+windows+clients).

### Building on Windows

Install the following prerequisites:

* [DotNetCore SDK](https://www.microsoft.com/net/download)
* [DotNet Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework-runtime)

#### Build using full build scripts

```cmd
.\bootstrap.ps1
```

### Building on Linux and Mac using Docker

Install the following prerequisites:

* Docker version 18+
* Powershell Core [Linux](<https://docs.microsoft.com/en-us/powershell/scripting/setup/installing-powershell-core-on-linux?view=powershell-6>)/[Mac](<https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-macos?view=powershell-6>)
* Linux [Mono 5.4.1.6](https://www.mono-project.com/download/stable/#download-lin) / Mac [Mono 5.4.1.6](https://www.mono-project.com/download/stable/#download-mac)

Then run the build

```cmd
pwsh bootstrap.ps1
```

Execute `pwsh bootstrap.ps1` to download and execute the build scripts.

### Building on Linux and Mac using Local Installs

Install the following prerequisites:

* Powershell Core [Linux](<https://docs.microsoft.com/en-us/powershell/scripting/setup/installing-powershell-core-on-linux?view=powershell-6>)/[Mac](<https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-macos?view=powershell-6>)

#### Without full build script

Build the code without full build script

##### Windows

```powershell
dotnet build .\src\dotnettemplate.sln
```

##### Linux and Mac users

```bash
dotnet build ./src/dotnettemplate.sln
```

#### With full build script

Install the following pre-requisites

* [DotNetCore SDK](https://www.microsoft.com/net/download)
* Linux [Mono 5.4.1.6](https://www.mono-project.com/download/stable/#download-lin) / Mac [Mono 5.4.1.6](https://www.mono-project.com/download/stable/#download-mac)

Execute `pwsh bootstrap.ps1` to download once the `build.ps1`

And then execute the build

```bash
pwsh build.ps1
```

## Adding your own NuGet sources

If you'd like to add your own NuGet sources you can do so in two ways

* Passing a list of NuGet sources to the `BuildParameters.SetParameters` method call in `setup.cake` as follows

  ```csharp
  BuildParameters.SetParameters(context: Context, ...., nuGetSources: new List<string> {"https://myNuGetSource1", "https://myNugetSource2"});
  ```

* Passing the path to a `NuGet.config` file that contains the list of NuGet sources. You can add your additional nuget sources to the `src\nuget.config` file and pass it as a parameter to the `BuildParameters.SetParameters` method as follows

  ```csharp
  BuildParameters.SetParameters(context: Context, ...., nugetConfig: System.IO.Path.GetFullPath("src/nuget.config")});
  ```

## Debug CAKE Scripts

For debugging CAKE Scripts locally, please refer the documentation here [Debug](https://confluence.honeywell.com/display/HDO/FAQ:+How+to+debug+the+template+in+Visual+Studio+Code+and+Visual+Studio+2017)

## Contributing

TBD

## Versioning

We use [SemVer](http://semver.org/) for versioning using [GitVersion](https://github.com/GitTools/GitVersion). For the versions available, see the [artifactory repository](https://artifactory-na.honeywell.com/artifactory/webapp/#/artifacts/browse/simple/General/ce-devops-common-nuget-stable-local).

## Support

For issues related to templates, please reach out us over [Microsoft Teams](https://teams.microsoft.com/l/channel/19%3aeb42116337a84772a896087fdc5d2fcb%40thread.skype/Template%2520Support?groupId=421e0c0f-e743-45c4-a738-979eb840fc11&tenantId=96ece526-9c7d-48b0-8daf-8b93c90a5d18)