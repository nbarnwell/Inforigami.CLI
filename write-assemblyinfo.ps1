param($projectName, $assemblyInfoFilename)
Set-StrictMode -Version Latest

write-host "Updating version information for $projectName..."

$scriptDir = $MyInvocation.MyCommand.Path | split-path

. (Join-Path $scriptDir 'Get-Version.ps1')

$v = Get-GitVersion

$assemblyVersion = '{0}.{1}.{2}.{3}' -f $v.Major, $v.Minor, $v.Build, $v.Revision
$AssemblyFileVersion = $assemblyVersion
$semanticVersion = $v.SemanticVersion

write-host "Building output as $assemblyVersion to $assemblyInfoFilename..."

$assemblyInfo = @"
using System.Reflection;

[assembly: AssemblyDescription("A library of classes to help you build a simple CLI application. Built from version $assemblyVersion.")]
[assembly: AssemblyVersion("$assemblyVersion")]
[assembly: AssemblyFileVersion("$assemblyVersion")]
[assembly: AssemblyInformationalVersion("$semanticVersion")]
"@

$assemblyInfo > $assemblyInfoFilename
