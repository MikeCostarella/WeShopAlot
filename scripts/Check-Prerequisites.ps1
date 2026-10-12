<#
.SYNOPSIS
    Checks whether this machine has everything needed to build and run WeShopAlot.

.DESCRIPTION
    Read-only: it installs nothing, starts nothing, and changes no settings.
    Secret values are never printed, only whether they are set and well formed.

    Each check reports one of:
      OK    ready
      TODO  the machine is fine, but a one-time setup step is still to do
            (npm install, seed the database, start Redis, ...)
      WARN  optional, or works with a caveat
      FAIL  missing or wrong; WeShopAlot will not run until it is fixed

    Exit code: 0 = ready to run, 2 = no failures but TODO steps remain, 1 = at least one FAIL.

    Runs in Windows PowerShell 5.1 (built into Windows) and PowerShell 7.
    From Command Prompt, use check-prerequisites.cmd next to this file.

    See docs/Prerequisites.md for what each item is for and how to install it.

.PARAMETER SolutionPath
    The Solution folder. Defaults to ..\Solution relative to this script.

.PARAMETER Offline
    Skip the internet reachability checks (NuGet, npm, Stripe).

.EXAMPLE
    .\scripts\Check-Prerequisites.ps1
.EXAMPLE
    .\scripts\Check-Prerequisites.ps1 -Offline
#>
[CmdletBinding()]
param(
    [string]$SolutionPath,
    [switch]$Offline
)

Set-StrictMode -Version 2.0
$ErrorActionPreference = 'Stop'

# ---------------------------------------------------------------- setup

if (-not $SolutionPath) { $SolutionPath = Join-Path (Split-Path -Parent $PSScriptRoot) 'Solution' }
if (-not (Test-Path (Join-Path $SolutionPath 'WeShopAlot.sln'))) {
    Write-Host "Can't find WeShopAlot.sln in '$SolutionPath'. Run this from the repository's scripts folder, or pass -SolutionPath." -ForegroundColor Red
    exit 1
}
$SolutionPath = (Resolve-Path $SolutionPath).Path
$ApiProject     = Join-Path $SolutionPath 'WeShopAlot..WebAPI\WeShopAlot.WebAPI.csproj'
$SeederProject  = Join-Path $SolutionPath 'WeShopAlot.Data.ConsoleApp\WeShopAlot.Data.ConsoleApp.csproj'
$ClientPath     = Join-Path $SolutionPath 'Client'

$onWindows = ($PSVersionTable.PSEdition -eq 'Desktop') -or ((Get-Variable IsWindows -ErrorAction SilentlyContinue) -and $IsWindows)

$results = New-Object System.Collections.Generic.List[object]

function Add-Result {
    param([string]$Area, [string]$Item, [ValidateSet('OK','TODO','WARN','FAIL')][string]$Status, [string]$Detail, [string]$Fix = '')
    $results.Add([pscustomobject]@{ Area = $Area; Item = $Item; Status = $Status; Detail = $Detail; Fix = $Fix })
    $color = @{ OK = 'Green'; TODO = 'Cyan'; WARN = 'Yellow'; FAIL = 'Red' }[$Status]
    Write-Host ('  [{0,-4}] ' -f $Status) -ForegroundColor $color -NoNewline
    Write-Host ('{0}: {1}' -f $Item, $Detail)
    if ($Fix -and $Status -ne 'OK') { Write-Host ('         -> {0}' -f $Fix) -ForegroundColor DarkGray }
}

function Write-Section([string]$Title) { Write-Host ''; Write-Host $Title -ForegroundColor White }

function Get-CommandPath([string]$Name) {
    $c = Get-Command $Name -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($c) { return $c.Source }
    return $null
}

# Runs a program and returns @{ Code; Out } without throwing.
function Invoke-Quiet([string]$File, [string[]]$Arguments) {
    # Windows PowerShell turns native stderr into terminating errors under 'Stop'; relax it here.
    $ErrorActionPreference = 'Continue'
    try {
        $out = & $File @Arguments 2>&1 | Out-String
        return @{ Code = $LASTEXITCODE; Out = $out.Trim() }
    } catch {
        return @{ Code = -1; Out = $_.Exception.Message }
    }
}

function ConvertTo-Version([string]$Text) {
    $m = [regex]::Match($Text, '(\d+)\.(\d+)(?:\.(\d+))?')
    if (-not $m.Success) { return $null }
    $patch = 0; if ($m.Groups[3].Success) { $patch = [int]$m.Groups[3].Value }
    return [version]::new([int]$m.Groups[1].Value, [int]$m.Groups[2].Value, $patch)
}

# Enough of npm's semver ranges for an "engines" field: ||, ^, ~, >=, >, <=, <, =, bare versions.
function Test-SemverRange([version]$Version, [string]$Range) {
    foreach ($alt in ($Range -split '\|\|')) {
        $ok = $true
        foreach ($cmp in ($alt.Trim() -split '\s+')) {
            if (-not $cmp) { continue }
            $m = [regex]::Match($cmp, '^(\^|~|>=|<=|>|<|=)?v?(\d+(?:\.\d+){0,2})')
            if (-not $m.Success) { $ok = $false; break }
            $op = $m.Groups[1].Value; $v = ConvertTo-Version $m.Groups[2].Value
            switch ($op) {
                '^'  { $upper = if ($v.Major -gt 0) { [version]::new($v.Major + 1, 0, 0) } else { [version]::new(0, $v.Minor + 1, 0) }
                       if (-not ($Version -ge $v -and $Version -lt $upper)) { $ok = $false } }
                '~'  { $upper = [version]::new($v.Major, $v.Minor + 1, 0)
                       if (-not ($Version -ge $v -and $Version -lt $upper)) { $ok = $false } }
                '>=' { if (-not ($Version -ge $v)) { $ok = $false } }
                '>'  { if (-not ($Version -gt $v)) { $ok = $false } }
                '<=' { if (-not ($Version -le $v)) { $ok = $false } }
                '<'  { if (-not ($Version -lt $v)) { $ok = $false } }
                default { if ($Version -ne $v) { $ok = $false } }
            }
            if (-not $ok) { break }
        }
        if ($ok) { return $true }
    }
    return $false
}

function Test-TcpPort([string]$HostName, [int]$Port, [int]$TimeoutMs = 2000) {
    $client = New-Object System.Net.Sockets.TcpClient
    try {
        $task = $client.ConnectAsync($HostName, $Port)
        return ($task.Wait($TimeoutMs) -and $client.Connected)
    } catch { return $false } finally { $client.Dispose() }
}

# Sends PING to Redis; returns the reply line (+PONG, -NOAUTH ...) or $null.
function Invoke-RedisPing([string]$HostName, [int]$Port) {
    $client = New-Object System.Net.Sockets.TcpClient
    try {
        $task = $client.ConnectAsync($HostName, $Port)
        if (-not ($task.Wait(2000) -and $client.Connected)) { return $null }
        $stream = $client.GetStream(); $stream.ReadTimeout = 2000
        $bytes = [Text.Encoding]::ASCII.GetBytes("PING`r`n")
        $stream.Write($bytes, 0, $bytes.Length)
        $buffer = New-Object byte[] 256
        $n = $stream.Read($buffer, 0, $buffer.Length)
        return ([Text.Encoding]::ASCII.GetString($buffer, 0, $n)).Trim()
    } catch { return $null } finally { $client.Dispose() }
}

function Get-UserSecretsId([string]$Csproj) {
    if (-not (Test-Path $Csproj)) { return $null }
    $m = [regex]::Match((Get-Content $Csproj -Raw), '<UserSecretsId>([^<]+)</UserSecretsId>')
    if ($m.Success) { return $m.Groups[1].Value.Trim() }
    return $null
}

function Get-UserSecretsFile([string]$Id) {
    if ($onWindows) { return Join-Path $env:APPDATA "Microsoft\UserSecrets\$Id\secrets.json" }
    return Join-Path $HOME ".microsoft/usersecrets/$Id/secrets.json"
}

# Flattens { "Token": { "Key": "x" } } and { "Token:Key": "x" } to the same "Token:Key" keys.
function ConvertTo-FlatSettings($Node, [string]$Prefix, [hashtable]$Into) {
    foreach ($p in $Node.PSObject.Properties) {
        $key = if ($Prefix) { "${Prefix}:$($p.Name)" } else { $p.Name }
        if ($p.Value -is [System.Management.Automation.PSCustomObject]) { ConvertTo-FlatSettings $p.Value $key $Into }
        else { $Into[$key] = [string]$p.Value }
    }
}

function Read-UserSecrets([string]$Csproj) {
    $id = Get-UserSecretsId $Csproj
    $info = @{ Id = $id; File = $null; Exists = $false; Values = @{}; Error = $null }
    if (-not $id) { return $info }
    $info.File = Get-UserSecretsFile $id
    if (-not (Test-Path $info.File)) { return $info }
    $info.Exists = $true
    try {
        $json = Get-Content $info.File -Raw
        if ($json.Trim()) { ConvertTo-FlatSettings ($json | ConvertFrom-Json) '' $info.Values }
    } catch { $info.Error = $_.Exception.Message }
    return $info
}

function Get-Setting([hashtable]$Values, [string]$Key) {
    # Configuration keys are case-insensitive; environment variables use __ for :.
    foreach ($k in $Values.Keys) { if ($k -ieq $Key) { return $Values[$k] } }
    $envValue = [Environment]::GetEnvironmentVariable(($Key -replace ':', '__'))
    if ($envValue) { return $envValue }
    return $null
}

function Get-ListeningProcess([int]$Port) {
    try {
        $c = Get-NetTCPConnection -State Listen -LocalPort $Port -ErrorAction Stop | Select-Object -First 1
        if ($c) {
            $p = Get-Process -Id $c.OwningProcess -ErrorAction SilentlyContinue
            if ($p) { return $p.ProcessName }
            return "PID $($c.OwningProcess)"
        }
    } catch { }
    return $null
}

Write-Host ''
Write-Host 'WeShopAlot prerequisite check' -ForegroundColor White
Write-Host "Solution: $SolutionPath"
Write-Host ("PowerShell {0} on {1}" -f $PSVersionTable.PSVersion, [Environment]::OSVersion.VersionString)

# ---------------------------------------------------------------- tools

Write-Section 'Tools'

# Windows: the WPF desktop client needs it; everything else is cross-platform.
if ($onWindows) {
    $build = [Environment]::OSVersion.Version.Build
    if ([Environment]::Is64BitOperatingSystem -and $build -ge 10240) {
        Add-Result 'Tools' 'Windows' 'OK' ("64-bit, build $build")
    } else {
        Add-Result 'Tools' 'Windows' 'WARN' ("build $build") 'Use 64-bit Windows 10 or 11.'
    }
} else {
    Add-Result 'Tools' 'Windows' 'WARN' 'not Windows: the API and Angular client will run, the WPF client will not' 'Build the API and Client projects individually instead of the whole solution.'
}

# .NET 10 SDK
$requiredSdkMajor = 10
$csprojText = Get-Content $ApiProject -Raw
$tfm = [regex]::Match($csprojText, '<TargetFramework>net(\d+)\.')
if ($tfm.Success) { $requiredSdkMajor = [int]$tfm.Groups[1].Value }
$dotnet = Get-CommandPath 'dotnet'
if (-not $dotnet) {
    Add-Result 'Tools' '.NET SDK' 'FAIL' 'dotnet is not on PATH' "winget install Microsoft.DotNet.SDK.$requiredSdkMajor   (then open a new terminal)"
} else {
    $sdks = (Invoke-Quiet $dotnet @('--list-sdks')).Out -split "`r?`n" | Where-Object { $_ -match '^\d' }
    $match = $sdks | Where-Object { $_ -match "^$requiredSdkMajor\." } | Select-Object -Last 1
    if ($match) {
        Add-Result 'Tools' ".NET $requiredSdkMajor SDK" 'OK' ($match -split ' ')[0]
    } else {
        $have = if ($sdks) { ($sdks | ForEach-Object { ($_ -split ' ')[0] }) -join ', ' } else { 'none' }
        Add-Result 'Tools' ".NET $requiredSdkMajor SDK" 'FAIL' "installed SDKs: $have" "winget install Microsoft.DotNet.SDK.$requiredSdkMajor"
    }
    $runtimes = (Invoke-Quiet $dotnet @('--list-runtimes')).Out
    if ($runtimes -match "Microsoft\.AspNetCore\.App $requiredSdkMajor\.") {
        Add-Result 'Tools' 'ASP.NET Core runtime' 'OK' "$requiredSdkMajor.x (comes with the SDK)"
    } elseif ($match) {
        Add-Result 'Tools' 'ASP.NET Core runtime' 'FAIL' "no Microsoft.AspNetCore.App $requiredSdkMajor.x" 'Repair or reinstall the .NET SDK.'
    }
    if ($onWindows) {
        if ($runtimes -match "Microsoft\.WindowsDesktop\.App $requiredSdkMajor\.") {
            Add-Result 'Tools' 'Windows Desktop runtime' 'OK' "$requiredSdkMajor.x (for the WPF client)"
        } elseif ($match) {
            Add-Result 'Tools' 'Windows Desktop runtime' 'WARN' "no Microsoft.WindowsDesktop.App $requiredSdkMajor.x; only the WPF client needs it" 'Reinstall the .NET SDK with the Windows installer.'
        }
    }
}

# Node.js, against the range Angular itself declares in package-lock.json
$nodeRange = '^22.22.3 || ^24.15.0 || >=26.0.0'
$rangeSource = 'built-in default'
$lockFile = Join-Path $ClientPath 'package-lock.json'
if (Test-Path $lockFile) {
    $m = [regex]::Match((Get-Content $lockFile -Raw), '(?s)"node_modules/@angular/core"\s*:\s*\{.*?"engines"\s*:\s*\{\s*"node"\s*:\s*"([^"]+)"')
    if ($m.Success) { $nodeRange = $m.Groups[1].Value; $rangeSource = '@angular/core engines' }
}
$node = Get-CommandPath 'node'
if (-not $node) {
    Add-Result 'Tools' 'Node.js' 'FAIL' "node is not on PATH (needs $nodeRange)" 'winget install OpenJS.NodeJS.LTS   (then open a new terminal)'
} else {
    $nodeText = (Invoke-Quiet $node @('--version')).Out
    $nodeVersion = ConvertTo-Version $nodeText
    if ($nodeVersion -and (Test-SemverRange $nodeVersion $nodeRange)) {
        Add-Result 'Tools' 'Node.js' 'OK' "$nodeText (needs $nodeRange, from $rangeSource)"
    } else {
        Add-Result 'Tools' 'Node.js' 'FAIL' "$nodeText does not satisfy $nodeRange ($rangeSource)" 'winget upgrade OpenJS.NodeJS.LTS   (or install a version in the range)'
    }
}
$npm = $null
if ($onWindows) { $npm = Get-CommandPath 'npm.cmd' }   # npm.cmd avoids execution-policy prompts for npm.ps1
if (-not $npm) { $npm = Get-CommandPath 'npm' }
if ($node -and -not $npm) {
    Add-Result 'Tools' 'npm' 'FAIL' 'npm is not on PATH' 'Reinstall Node.js; npm comes with it.'
} elseif ($npm) {
    $npmText = (Invoke-Quiet $npm @('--version')).Out
    Add-Result 'Tools' 'npm' 'OK' $npmText
}

# Git: needed to clone and to commit, not to run
$git = Get-CommandPath 'git'
if ($git) { Add-Result 'Tools' 'Git' 'OK' ((Invoke-Quiet $git @('--version')).Out -replace '^git version ', '') }
else { Add-Result 'Tools' 'Git' 'WARN' 'not on PATH; only needed to clone and commit' 'winget install Git.Git' }

# Docker: the easy way to run Redis locally
$docker = Get-CommandPath 'docker'
$dockerRunning = $false
if (-not $docker) {
    Add-Result 'Tools' 'Docker' 'WARN' 'not installed; needed for Redis unless you run Redis some other way' 'winget install Docker.DockerDesktop   (then start Docker Desktop)'
} else {
    $server = Invoke-Quiet $docker @('version', '--format', '{{.Server.Version}}')
    if ($server.Code -eq 0 -and $server.Out -match '^\d') {
        $dockerRunning = $true
        Add-Result 'Tools' 'Docker' 'OK' "engine $($server.Out) is running"
    } else {
        Add-Result 'Tools' 'Docker' 'TODO' 'installed, but the engine is not running' 'Start Docker Desktop and wait for "Engine running".'
    }
    $compose = Invoke-Quiet $docker @('compose', 'version', '--short')
    if ($compose.Code -eq 0) { Add-Result 'Tools' 'Docker Compose' 'OK' $compose.Out }
    else { Add-Result 'Tools' 'Docker Compose' 'WARN' "'docker compose' is not available" 'Update Docker Desktop; Compose v2 is built in.' }
}

# HTTPS development certificate: the API (7244) and ng serve (4200) both use it
if ($dotnet) {
    $cert = Invoke-Quiet $dotnet @('dev-certs', 'https', '--check', '--trust')
    if ($cert.Code -eq 0) {
        Add-Result 'Tools' 'HTTPS dev certificate' 'OK' 'present and trusted'
    } else {
        Add-Result 'Tools' 'HTTPS dev certificate' 'FAIL' 'missing or not trusted; the browser will refuse https://localhost:7244 and :4200' 'dotnet dev-certs https --trust   (click Yes on the prompt)'
    }
}

# Optional tools
$stripe = Get-CommandPath 'stripe'
if ($stripe) { Add-Result 'Tools' 'Stripe CLI (optional)' 'OK' ((Invoke-Quiet $stripe @('--version')).Out) }
else { Add-Result 'Tools' 'Stripe CLI (optional)' 'WARN' 'not installed; only needed to forward payment webhooks to localhost' 'winget install Stripe.StripeCli' }

if ($onWindows) {
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    $vs = $null
    if (Test-Path $vswhere) { $vs = (Invoke-Quiet $vswhere @('-latest', '-products', '*', '-property', 'displayName')).Out }
    if ($vs) { Add-Result 'Tools' 'Visual Studio (optional)' 'OK' $vs }
    else { Add-Result 'Tools' 'Visual Studio (optional)' 'WARN' 'not found; the command line and VS Code are enough' 'Optional: Visual Studio 2026 with "ASP.NET and web development" and ".NET desktop development".' }
}

# ---------------------------------------------------------------- configuration

Write-Section 'Configuration (user secrets; values are never shown)'

$api = Read-UserSecrets $ApiProject
$dbType = $null; $sqlConn = $null; $redisConn = $null
if (-not $api.Id) {
    Add-Result 'Config' 'API user secrets' 'FAIL' 'no <UserSecretsId> in WeShopAlot.WebAPI.csproj' 'Restore the csproj from git.'
} elseif ($api.Error) {
    Add-Result 'Config' 'API user secrets' 'FAIL' "secrets.json is not valid JSON: $($api.Error)" "Fix or delete $($api.File), then set the values again."
} elseif (-not $api.Exists) {
    Add-Result 'Config' 'API user secrets' 'TODO' 'none set yet' 'Set them with dotnet user-secrets; see "Secrets" in README.md.'
} else {
    Add-Result 'Config' 'API user secrets' 'OK' ("{0} values set" -f $api.Values.Count)
}

$fixApi = 'cd Solution\WeShopAlot..WebAPI ; dotnet user-secrets set'

$dbType = Get-Setting $api.Values 'Settings:DbServerType'
if (-not $dbType) {
    Add-Result 'Config' 'Settings:DbServerType' 'FAIL' 'not set; every database request fails with "No database server type was chosen"' "$fixApi `"Settings:DbServerType`" `"SQLServer`""
} elseif ($dbType -notin @('SQLServer', 'PostgresSql')) {
    Add-Result 'Config' 'Settings:DbServerType' 'FAIL' "'$dbType' is not SQLServer or PostgresSql (case-sensitive)" "$fixApi `"Settings:DbServerType`" `"SQLServer`""
} else {
    Add-Result 'Config' 'Settings:DbServerType' 'OK' $dbType
}

$sqlConn = Get-Setting $api.Values 'ConnectionStrings:WeShopAlotSQLConnection'
if ($sqlConn) { Add-Result 'Config' 'ConnectionStrings:WeShopAlotSQLConnection' 'OK' 'set' }
else { Add-Result 'Config' 'ConnectionStrings:WeShopAlotSQLConnection' 'FAIL' 'not set' "$fixApi `"ConnectionStrings:WeShopAlotSQLConnection`" `"Server=(localdb)\MSSQLLocalDB;Database=WeShopAlot;Trusted_Connection=true;TrustServerCertificate=true`"" }

$redisConn = Get-Setting $api.Values 'ConnectionStrings:Redis'
if ($redisConn) { Add-Result 'Config' 'ConnectionStrings:Redis' 'OK' 'set' }
else { Add-Result 'Config' 'ConnectionStrings:Redis' 'FAIL' 'not set; the basket and the product list fail' "$fixApi `"ConnectionStrings:Redis`" `"localhost:6379`"" }

# HMAC-SHA512 needs a key of at least 512 bits: 64 bytes of UTF-8.
$tokenKey = Get-Setting $api.Values 'Token:Key'
if (-not $tokenKey) {
    Add-Result 'Config' 'Token:Key' 'FAIL' 'not set; sign-in cannot issue tokens' "$fixApi `"Token:Key`" `"<64+ random characters>`""
} elseif ([Text.Encoding]::UTF8.GetByteCount($tokenKey) -lt 64) {
    Add-Result 'Config' 'Token:Key' 'FAIL' ("only {0} bytes; HMAC-SHA512 needs at least 64, or login fails with IDX10720" -f [Text.Encoding]::UTF8.GetByteCount($tokenKey)) 'Generate a longer one; docs/Prerequisites.md shows a one-liner.'
} else {
    Add-Result 'Config' 'Token:Key' 'OK' ("{0} bytes" -f [Text.Encoding]::UTF8.GetByteCount($tokenKey))
}

$stripeSecret = Get-Setting $api.Values 'StripeSettings:SecretKey'
if (-not $stripeSecret) {
    Add-Result 'Config' 'StripeSettings:SecretKey' 'FAIL' 'not set; checkout fails at Go to Payment' "$fixApi `"StripeSettings:SecretKey`" `"sk_test_...`"   (Stripe Dashboard, test mode, Developers > API keys)"
} elseif ($stripeSecret -like 'sk_live_*' -or $stripeSecret -like 'rk_live_*') {
    Add-Result 'Config' 'StripeSettings:SecretKey' 'FAIL' 'this is a LIVE key: real cards, real money' 'Use the test-mode key (sk_test_...) for development.'
} elseif ($stripeSecret -like 'sk_test_*' -or $stripeSecret -like 'rk_test_*') {
    Add-Result 'Config' 'StripeSettings:SecretKey' 'OK' 'test-mode key'
} else {
    Add-Result 'Config' 'StripeSettings:SecretKey' 'WARN' 'set, but does not look like a Stripe secret key (sk_test_...)' 'Copy it again from the Stripe Dashboard in test mode.'
}

# The publishable key the Angular client ships and the secret key must come from the same Stripe
# account, or Stripe rejects the payment. Keys from one account share the characters after the
# mode prefix, so compare those (a heuristic: a mismatch is worth a look, not proof).
$envFile = Join-Path $ClientPath 'src\environments\environment.ts'
if ($stripeSecret -and (Test-Path $envFile)) {
    $pk = [regex]::Match((Get-Content $envFile -Raw), 'pk_(test|live)_([A-Za-z0-9]+)')
    $sk = [regex]::Match($stripeSecret, '^[sr]k_(test|live)_([A-Za-z0-9]+)')
    if ($pk.Success -and $sk.Success) {
        $n = [Math]::Min(16, [Math]::Min($pk.Groups[2].Value.Length, $sk.Groups[2].Value.Length))
        if ($pk.Groups[2].Value.Substring(0, $n) -ceq $sk.Groups[2].Value.Substring(0, $n)) {
            Add-Result 'Config' 'Stripe key pair' 'OK' 'publishable key in the client matches the secret key''s account'
        } else {
            Add-Result 'Config' 'Stripe key pair' 'WARN' 'the client''s publishable key looks like it is from a different Stripe account than the secret key; payments will fail' 'Put your own pk_test_ key in Client\src\environments\environment*.ts, appsettings.json (StripeSettings:PublishableKey), and the WPF client (WESHOPALOT_STRIPE_PK).'
        }
    }
}

$whSecret = Get-Setting $api.Values 'StripeSettings:WhSecret'
if ($whSecret -like 'whsec_*') { Add-Result 'Config' 'StripeSettings:WhSecret' 'OK' 'set' }
elseif ($whSecret) { Add-Result 'Config' 'StripeSettings:WhSecret' 'WARN' 'set, but does not start with whsec_' 'Copy the secret that "stripe listen" prints.' }
else { Add-Result 'Config' 'StripeSettings:WhSecret' 'WARN' 'not set; orders stay Pending because webhooks cannot be verified' 'stripe listen --forward-to https://localhost:7244/api/payment/webhook, then set the whsec_ it prints.' }

if (Get-Setting $api.Values 'AutoMapper:LicenseKey') { Add-Result 'Config' 'AutoMapper:LicenseKey (optional)' 'OK' 'set' }
else { Add-Result 'Config' 'AutoMapper:LicenseKey (optional)' 'WARN' 'not set; AutoMapper logs a license warning but works' 'Optional: AutoMapper offers a free Community license key.' }

# The seeder has its own user secrets
$seed = Read-UserSecrets $SeederProject
$seedType = Get-Setting $seed.Values 'Settings:DbServerType'
$seedConn = Get-Setting $seed.Values 'ConnectionStrings:WeShopAlotSQLConnection'
if ($seed.Error) {
    Add-Result 'Config' 'Seeder user secrets' 'FAIL' "secrets.json is not valid JSON: $($seed.Error)" "Fix or delete $($seed.File)."
} elseif ($seedType -and $seedConn) {
    if ($sqlConn -and $seedConn -ne $sqlConn) {
        Add-Result 'Config' 'Seeder user secrets' 'WARN' 'set, but the connection string differs from the API''s, so it seeds a different database' 'Use the same ConnectionStrings:WeShopAlotSQLConnection in both projects.'
    } else {
        Add-Result 'Config' 'Seeder user secrets' 'OK' 'DbServerType and connection string set'
    }
} else {
    Add-Result 'Config' 'Seeder user secrets' 'TODO' 'DbServerType and/or connection string not set; needed once, to create and seed the database' 'cd Solution\WeShopAlot.Data.ConsoleApp ; dotnet user-secrets set ... (same two values as the API)'
}

# ---------------------------------------------------------------- services

Write-Section 'Services'

# Database
if ($dbType -eq 'PostgresSql') {
    $pgHost = 'localhost'; $pgPort = 5432
    if ($sqlConn) {
        $h = [regex]::Match($sqlConn, '(?i)(?:^|;)\s*(?:Host|Server)\s*=\s*([^;,]+)'); if ($h.Success) { $pgHost = $h.Groups[1].Value.Trim() }
        $p = [regex]::Match($sqlConn, '(?i)(?:^|;)\s*Port\s*=\s*(\d+)'); if ($p.Success) { $pgPort = [int]$p.Groups[1].Value }
    }
    if (Test-TcpPort $pgHost $pgPort) { Add-Result 'Services' 'PostgreSQL' 'OK' "listening on ${pgHost}:$pgPort (credentials not tested)" }
    else { Add-Result 'Services' 'PostgreSQL' 'FAIL' "nothing on ${pgHost}:$pgPort" 'Start PostgreSQL, or switch Settings:DbServerType to SQLServer.' }
} else {
    # SQL Server or LocalDB. Default to LocalDB when nothing is configured yet.
    $cs = $sqlConn
    if (-not $cs) { $cs = 'Server=(localdb)\MSSQLLocalDB;Database=WeShopAlot;Trusted_Connection=true;TrustServerCertificate=true' }
    $isLocalDb = $cs -match '(?i)\(localdb\)'

    if ($isLocalDb -and $onWindows) {
        $sqllocaldb = Get-CommandPath 'SqlLocalDB'
        if (-not $sqllocaldb) {
            $sqllocaldb = Get-ChildItem "$env:ProgramFiles\Microsoft SQL Server\*\Tools\Binn\SqlLocalDB.exe" -ErrorAction SilentlyContinue |
                Sort-Object FullName | Select-Object -Last 1 -ExpandProperty FullName
        }
        if ($sqllocaldb) {
            $ver = (Invoke-Quiet $sqllocaldb @('versions')).Out -split "`r?`n" | Select-Object -Last 1
            Add-Result 'Services' 'SQL Server LocalDB' 'OK' $ver
        } else {
            Add-Result 'Services' 'SQL Server LocalDB' 'FAIL' 'not installed' 'Install LocalDB (Visual Studio "Data storage and processing" workload, or the SQL Server Express installer > LocalDB), or point the connection string at another SQL Server.'
        }
    }

    $sqlType = 'System.Data.SqlClient.SqlConnection' -as [type]
    if (-not $sqlType) {
        Add-Result 'Services' 'SQL Server connection' 'WARN' 'cannot test from this PowerShell' 'Run check-prerequisites.cmd (Windows PowerShell) to test the connection.'
    } else {
        $conn = $null
        $b = $null
        try { $b = New-Object System.Data.SqlClient.SqlConnectionStringBuilder $cs } catch { }
        if (-not $b) {
            Add-Result 'Services' 'SQL Server connection' 'WARN' 'the connection string uses options this PowerShell''s SQL client does not know; not tested' 'The API may still connect; run it and check the startup log.'
        } else {
            try {
                # The builder is a dictionary, so PowerShell needs the indexer, not .InitialCatalog.
                $dbName = [string]$b['Initial Catalog']; if (-not $dbName) { $dbName = 'WeShopAlot' }
                $b['Initial Catalog'] = 'master'
                $b['Connect Timeout'] = $(if ($isLocalDb) { 30 } else { 8 })   # LocalDB starts on first connect
                $conn = New-Object System.Data.SqlClient.SqlConnection $b.ConnectionString
                $conn.Open()
                $server = [string]$b['Data Source']
                Add-Result 'Services' 'SQL Server connection' 'OK' "connected to $server"

                $cmd = $conn.CreateCommand()
                $cmd.CommandText = 'SELECT DB_ID(@db)'
                [void]$cmd.Parameters.AddWithValue('@db', $dbName)
                $dbId = $cmd.ExecuteScalar()
                if ($dbId -is [DBNull] -or $null -eq $dbId) {
                    Add-Result 'Services' "Database '$dbName'" 'TODO' 'does not exist yet' 'cd Solution ; dotnet run --project WeShopAlot.Data.ConsoleApp'
                } else {
                    $conn.ChangeDatabase($dbName)
                    # The tables live in the WSA schema (see the Initial migration). Find Product in
                    # whatever schema it is in, preferring WSA, rather than assuming dbo.
                    $cmd = $conn.CreateCommand()
                    $cmd.CommandText = "SELECT TOP 1 QUOTENAME(s.name) + '.' + QUOTENAME(t.name) FROM sys.tables t JOIN sys.schemas s ON s.schema_id = t.schema_id WHERE t.name = 'Product' ORDER BY CASE s.name WHEN 'WSA' THEN 0 ELSE 1 END"
                    $productTable = $cmd.ExecuteScalar()
                    $count = -1
                    if ($productTable -and -not ($productTable -is [DBNull])) {
                        $cmd = $conn.CreateCommand()
                        $cmd.CommandText = "SELECT COUNT(*) FROM $productTable"
                        $count = [int]$cmd.ExecuteScalar()
                    }
                    if ($count -gt 0) { Add-Result 'Services' "Database '$dbName'" 'OK' "$count products" }
                    elseif ($count -eq 0) { Add-Result 'Services' "Database '$dbName'" 'TODO' 'exists but has no products' 'cd Solution ; dotnet run --project WeShopAlot.Data.ConsoleApp' }
                    else { Add-Result 'Services' "Database '$dbName'" 'TODO' 'exists but has no tables (migrations not applied)' 'cd Solution ; dotnet run --project WeShopAlot.Data.ConsoleApp' }
                }
            } catch {
                $ex = $_.Exception; while ($ex.InnerException) { $ex = $ex.InnerException }
                $msg = $ex.Message -replace '\s+', ' '
                if ($msg.Length -gt 160) { $msg = $msg.Substring(0, 160) + '...' }
                Add-Result 'Services' 'SQL Server connection' 'FAIL' $msg 'Check that SQL Server/LocalDB is installed and the connection string is right.'
            } finally {
                if ($conn) { $conn.Dispose() }
            }
        }
    }
}

# Redis
$redisHost = 'localhost'; $redisPort = 6379
if ($redisConn) {
    $first = ($redisConn -split ',')[0].Trim()
    if ($first -match '^\[?([^\]]+?)\]?:(\d+)$') { $redisHost = $Matches[1]; $redisPort = [int]$Matches[2] }
    elseif ($first -and $first -notmatch '=') { $redisHost = $first }
}
$pong = Invoke-RedisPing $redisHost $redisPort
if ($pong -eq '+PONG') {
    Add-Result 'Services' 'Redis' 'OK' "answering on ${redisHost}:$redisPort"
} elseif ($pong -match 'NOAUTH|WRONGPASS') {
    Add-Result 'Services' 'Redis' 'OK' "answering on ${redisHost}:$redisPort (password required; not tested)"
} elseif ($dockerRunning) {
    Add-Result 'Services' 'Redis' 'TODO' "nothing answering on ${redisHost}:$redisPort" 'cd Solution ; docker compose up -d'
} elseif ($docker) {
    Add-Result 'Services' 'Redis' 'TODO' "nothing answering on ${redisHost}:$redisPort" 'Start Docker Desktop, then: cd Solution ; docker compose up -d'
} else {
    Add-Result 'Services' 'Redis' 'FAIL' "nothing answering on ${redisHost}:$redisPort, and Docker is not installed" 'Install Docker Desktop (winget install Docker.DockerDesktop), then: cd Solution ; docker compose up -d'
}

# Ports the app will use
if ($onWindows) {
    foreach ($port in @(@{ N = 7244; W = 'API (https)' }, @{ N = 5094; W = 'API (http)' }, @{ N = 4200; W = 'Angular dev server' })) {
        $owner = Get-ListeningProcess $port.N
        if ($owner) { Add-Result 'Services' "Port $($port.N)" 'WARN' "$($port.W): already in use by $owner" 'Fine if that is WeShopAlot already running; otherwise stop that process.' }
        else { Add-Result 'Services' "Port $($port.N)" 'OK' "free for the $($port.W)" }
    }
}

# ---------------------------------------------------------------- client

Write-Section 'Angular client'

if (Test-Path (Join-Path $ClientPath 'node_modules\@angular\core\package.json')) {
    Add-Result 'Client' 'npm packages' 'OK' 'installed (Client\node_modules)'
} else {
    Add-Result 'Client' 'npm packages' 'TODO' 'not installed yet' 'cd Solution\Client ; npm install'
}
if ((Test-Path (Join-Path $ClientPath 'ssl\localhost.pem')) -and (Test-Path (Join-Path $ClientPath 'ssl\localhost.key'))) {
    Add-Result 'Client' 'Dev server certificate' 'OK' 'exported to Client\ssl'
} else {
    Add-Result 'Client' 'Dev server certificate' 'TODO' 'not exported yet; "npm start" exports it automatically' 'cd Solution\Client ; npm start'
}

# ---------------------------------------------------------------- network

if (-not $Offline) {
    Write-Section 'Internet (skip with -Offline)'
    foreach ($site in @(
        @{ Host = 'api.nuget.org'; Why = 'dotnet restore' },
        @{ Host = 'registry.npmjs.org'; Why = 'npm install' },
        @{ Host = 'api.stripe.com'; Why = 'payments' })) {
        if (Test-TcpPort $site.Host 443 3000) { Add-Result 'Network' $site.Host 'OK' "reachable ($($site.Why))" }
        else { Add-Result 'Network' $site.Host 'WARN' "not reachable; $($site.Why) will fail" 'Check the network, proxy, or firewall.' }
    }
}

# ---------------------------------------------------------------- summary

$fail = @($results | Where-Object Status -eq 'FAIL')
$todo = @($results | Where-Object Status -eq 'TODO')
$warn = @($results | Where-Object Status -eq 'WARN')

Write-Host ''
Write-Host ('Summary: {0} OK, {1} TODO, {2} WARN, {3} FAIL' -f @($results | Where-Object Status -eq 'OK').Count, $todo.Count, $warn.Count, $fail.Count) -ForegroundColor White
if ($fail.Count -gt 0) {
    Write-Host 'Fix these first:' -ForegroundColor Red
    $fail | ForEach-Object { Write-Host ("  - {0}: {1}" -f $_.Item, $_.Fix) }
}
if ($todo.Count -gt 0) {
    Write-Host 'Then these one-time setup steps:' -ForegroundColor Cyan
    $todo | ForEach-Object { Write-Host ("  - {0}: {1}" -f $_.Item, $_.Fix) }
}
if ($fail.Count -eq 0 -and $todo.Count -eq 0) {
    Write-Host 'Ready. Run the API with:  cd Solution ; dotnet run --project "WeShopAlot..WebAPI" --launch-profile https' -ForegroundColor Green
}
Write-Host 'Details: docs\Prerequisites.md'
Write-Host ''

if ($fail.Count -gt 0) { exit 1 }
if ($todo.Count -gt 0) { exit 2 }
exit 0
