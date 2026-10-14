<#
.SYNOPSIS
    Builds a deployable package of WeShopAlot: the Angular client, the Web API and the database seeder.

.DESCRIPTION
    Runs on a development machine (it needs the .NET SDK, Node.js and npm). Nothing is built on the server.

    Steps:
      1. Angular client   ng build --configuration production, into the API's wwwroot
                          (through scripts/ng.mjs, so the build timestamp is set)
      2. Web API          dotnet publish -c Release           -> <package>\Site
      3. Product images   copies the API's Content folder     -> <package>\Site\Content
                          (dotnet publish leaves it out; the API serves it at /Content)
      4. Seeder           dotnet publish -c Release           -> <package>\Seeder
      5. Zip              <package>.zip

    The package is framework-dependent: the server runs it with the ASP.NET Core Hosting Bundle.
    No settings or secrets are in it; the server supplies those as environment variables.

    See docs/Deployment.md for the server side.

.PARAMETER OutputPath
    Where to write the package. Defaults to ..\publish relative to this script (ignored by git).

.PARAMETER CleanInstall
    Run `npm ci` before building the client (a fresh node_modules, as the build gate does).
    By default npm install runs only when node_modules is missing.

.PARAMETER NoZip
    Leave the package as folders; don't create the zip.

.EXAMPLE
    .\scripts\Publish-WeShopAlot.ps1

.EXAMPLE
    .\scripts\Publish-WeShopAlot.ps1 -CleanInstall -OutputPath D:\drop
#>
[CmdletBinding()]
param(
    [string]$OutputPath = (Join-Path $PSScriptRoot '..\publish'),
    [switch]$CleanInstall,
    [switch]$NoZip
)

$ErrorActionPreference = 'Stop'

$solution = (Resolve-Path (Join-Path $PSScriptRoot '..\Solution')).Path
$client   = Join-Path $solution 'Client'
$api      = Join-Path $solution 'WeShopAlot..WebAPI'
$seeder   = Join-Path $solution 'WeShopAlot.Data.ConsoleApp'

$stamp    = Get-Date -Format 'yyyyMMdd-HHmm'
$package  = Join-Path $OutputPath "WeShopAlot-$stamp"
$site     = Join-Path $package 'Site'
$seed     = Join-Path $package 'Seeder'

function Step([string]$Message) {
    Write-Host ''
    Write-Host "== $Message" -ForegroundColor Cyan
}

function Run([string]$Exe, [string[]]$Arguments, [string]$WorkingDirectory) {
    Push-Location $WorkingDirectory
    try {
        & $Exe @Arguments
        if ($LASTEXITCODE -ne 0) { throw "$Exe $($Arguments -join ' ') exited with code $LASTEXITCODE" }
    }
    finally { Pop-Location }
}

Step 'Checking tools'
foreach ($tool in 'dotnet', 'node', 'npm') {
    if (-not (Get-Command $tool -ErrorAction SilentlyContinue)) {
        throw "$tool is not on the PATH. See docs/Prerequisites.md, then open a new terminal."
    }
}
Write-Host "dotnet $(dotnet --version), node $(node --version), npm $(npm --version)"

if (Test-Path $package) { Remove-Item $package -Recurse -Force }
New-Item -ItemType Directory -Path $site, $seed -Force | Out-Null

Step 'Angular client -> wwwroot'
if ($CleanInstall) {
    Run 'npm' @('ci') $client
}
elseif (-not (Test-Path (Join-Path $client 'node_modules'))) {
    Run 'npm' @('install') $client
}
Run 'node' @('scripts/ng.mjs', 'build', '--configuration', 'production') $client

Step 'Web API -> Site'
Run 'dotnet' @('publish', $api, '-c', 'Release', '-o', $site, '--nologo') $solution
# Only Development loads this file, and a server must not run as Development; leave it out so nobody wonders.
Remove-Item (Join-Path $site 'appsettings.Development.json') -ErrorAction SilentlyContinue

Step 'Product images -> Site\Content'
Copy-Item (Join-Path $api 'Content') (Join-Path $site 'Content') -Recurse -Force

Step 'Seeder -> Seeder'
Run 'dotnet' @('publish', $seeder, '-c', 'Release', '-o', $seed, '--nologo') $solution

if (-not $NoZip) {
    Step 'Zip'
    $zip = "$package.zip"
    if (Test-Path $zip) { Remove-Item $zip -Force }
    Compress-Archive -Path (Join-Path $package '*') -DestinationPath $zip
    Remove-Item $package -Recurse -Force
    Write-Host "Package: $zip"
}
else {
    Write-Host "Package: $package"
}

Write-Host ''
Write-Host 'Next: copy it to the server and follow docs/Deployment.md from step 5.' -ForegroundColor Green
