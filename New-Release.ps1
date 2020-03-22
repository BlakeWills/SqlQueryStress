[CmdletBinding()]
param(
	[Parameter(Mandatory)]
	[string]$Version,
	[Parameter(Mandatory)]
	[string]$GithubApiKey,
	[Parameter(Mandatory="false")]
	[string]$Project="SqlQueryStressGUI",
	[Parameter(Mandatory="false")]
	[string]$Runtime="win-x86",
	[bool]$Draft=$false,
	[bool]$PreRelease=$false
)

$ErrorActionPreference="Stop"
$LastExitCode=0

Write-Information "Starting Build"
dotnet build --configuration Release
if($LastExitCode -ne 0) { throw "dotnet build returned exit code: $LastExitCode"}

Write-Information "Starting Test"
dotnet test --configuration Release
if($LastExitCode -ne 0) { throw "dotnet test returned exit code: $LastExitCode"}

Write-Information "Publishing..."
dotnet publish $Project -p:Version=$Version -p:PublishSingleFile=$true --self-contained --runtime $Runtime --configuration Release
if($LastExitCode -ne 0) { throw "dotnet publish returned exit code: $LastExitCode"}

Write-Information "Creating archive..."
$ArtifactPath= Resolve-Path "$Project\bin\Release\netcoreapp3.1\$($Runtime)\publish\"
$releaseFileName="$($Project)-$($Runtime)-$($Version).zip"
if(Test-Path $releaseFileName) {
	Remove-Item $releaseFileName
}

7z.exe a $releaseFileName `"$ArtifactPath\*`" -aoa -o"$(Resolve-Path .)" -y
if($LastExitCode -ne 0) { throw "7z.exe returned exit code: $LastExitCode"}

Write-Information "Publishing to GitHub"
$authHeaders = @{
	Authorization = 'Basic ' + [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes($GithubApiKey + ":x-oauth-basic"));
}

$versionString = [string]::Format("v{0}", $Version);
$releaseData = @{
	tag_name = $versionString
	target_commitish = "master"
	name = $versionString
	body = "SqlQueryStress - $versionString"
	draft = $Draft
	prerelease = $PreRelease
}

$releaseParams = @{
	Uri = "https://api.github.com/repos/BlakeWills/SqlQueryStress/releases";
	Method = 'POST';
	Headers = $authHeaders
	ContentType = 'application/json';
	Body = (ConvertTo-Json $releaseData -Compress)
}

$result = Invoke-RestMethod @releaseParams 
$uploadUri = $result | Select-Object -ExpandProperty upload_url
$uploadUri = $uploadUri.Substring(0, $uploadUri.IndexOf("{"))
$uploadUri += "?name=$releaseFileName"
$uploadUri
$uploadParams = @{
	Uri = $uploadUri;
	Method = 'POST';
	Headers = $authHeaders
	ContentType = 'application/zip';
	InFile = $releaseFileName
}

$result = Invoke-RestMethod @uploadParams