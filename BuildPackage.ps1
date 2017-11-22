$scriptDir = $MyInvocation.MyCommand.Path | split-path
$outputDir = "$scriptDir\BuildOutput"

. (Join-Path $scriptDir 'Get-Version.ps1')

$v = Get-GitVersion

md $outputDir -f | out-null
del $outputDir\*

$projectsToPackage = @(
    'Inforigami.CLI.csproj',
    'Inforigami.CLI.Host.csproj'
)

gci $scriptDir -include $projectsToPackage -recurse | 
    %{ 
        Write-Host -ForegroundColor Green "Building and packaging $_..."
        $_
    } |
    %{
        $nuspec = $_ -replace '.csproj$', '.nuspec'

        if (Test-Path $nuspec) {
            $nuspecTemp = $nuspec + '.tmp'
            
            if (Test-Path $nuspecTemp) {
                del $nuspec
            } else {
                ren $nuspec $nuspecTemp
            }

            (get-content $nuspecTemp) |
                %{ $_ -replace '\$version\$', $v.SemanticVersion } |
                add-content $nuspec

            nuget.exe pack $_ -build -Symbols -outputdirectory $outputDir -Properties Configuration=Release -MSBuildVersion 14

            del $nuspec
            ren $nuspecTemp $nuspec 
        } else {
            nuget.exe pack $_ -build -Symbols -outputdirectory $outputDir -Properties Configuration=Release -MSBuildVersion 14
        }
    }
