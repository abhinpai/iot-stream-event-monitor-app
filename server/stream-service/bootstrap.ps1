<#

.SYNOPSIS
This bootstrap script fetches the build scripts 

.DESCRIPTION
This script downloads cake bootstrap and dockerbuild scripts for iniating docker build with specified parameters. 

.EXAMPLE
./bootstrap.ps1 
./bootstrap.ps1 -Target Publish
./bootstrap.ps1 -Target Quality


#>

param(
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$scriptArgs
)
   
function IsLinux() {
    "$($ENV:OS)" -eq ""
}

function IsBambooBuild() {
    Write-Host("Checking if its a bamboo build")
    return Test-Path Env:bamboo_*
}


Write-Host "Downloading build.ps1 script"
Invoke-WebRequest "https://bitbucket.honeywell.com/projects/CEDOTNET/repos/devops.build/raw/dotnetbuild.ps1" -OutFile build.ps1

$buildScriptArgs = @();
$buildScriptArgs += $scriptArgs

Write-Host "Downloading dockerbuild.ps1 script"
Invoke-WebRequest "https://bitbucket.honeywell.com/projects/CEDOTNET/repos/devops.build/raw/dockerbuild.ps1" -OutFile dockerbuild.ps1

if(IsLinux) {
    ./dockerbuild.ps1 ce-devops-dotnetcore-build-docker-stable-local.artifactory-na.honeywell.com/docker.dotnetcore.linux.build:latest $buildScriptArgs
} else{
    if(IsBambooBuild) {
        ./dockerbuild.ps1 ce-devops-tools-docker-docker-stable-local.artifactory-na.honeywell.com/docker.dotnetcore.build.coverity.windows:latest $buildScriptArgs
    } else {
        Write-Host "Executing build on Windows so running directly rather than via Docker container. There is a known AnyConnect issue that blocks mounting of windows volumes into linux containers. A request for IT support has been raised."
        Invoke-Expression "`./build.ps1` $buildScriptArgs"
    }
}

exit $LASTEXITCODE

