param(
    [Parameter(Mandatory)]
    [ValidateSet('Debug','Release')]
    [System.String]$Target,
    
    [Parameter(Mandatory)]
    [System.String]$TargetPath,

    [Parameter(Mandatory)]
    [System.String]$ValheimPath,

    [Parameter(Mandatory)]
    [System.String]$SolutionPath
)

function Create-BepInEx{
    param (
        [Parameter(Mandatory)]
        [System.String]$BasePath,

        [Parameter(Mandatory)]
        [ValidateSet('Windows','Unix')]
        [System.String]$TargetSystem
    )
    Write-Host "Creating BepInEx in $BasePath"

    # make sure basepath exists
    $base = New-Item -ItemType Directory -Path "$BasePath" -Force;

    # copy needed files
    Copy-Item -Path "$SolutionPath\resources\$TargetSystem\*" -Exclude 'BepInEx.cfg' -Destination $base -Recurse -Force
    
    # create \BepInEx
    $bepinex = $base.CreateSubdirectory('BepInEx');
    
    # create \BepInEx\core and copy core dlls from build
    $core = $bepinex.CreateSubdirectory('core');
    Copy-Item -Path "$TargetPath\*" -Filter 'BepInEx*.dll' -Destination $core -Force
    Copy-Item -Path "$TargetPath\*" -Filter '*Harmony*.dll' -Destination $core -Force
    Copy-Item -Path "$TargetPath\*" -Filter 'Mono.Cecil*.dll' -Destination $core -Force
    Copy-Item -Path "$TargetPath\*" -Filter 'MonoMod*.dll' -Destination $core -Force

    # create \BepInEx\config and copy config files
    $conf = $bepinex.CreateSubdirectory('config');
    Copy-Item -Path "$SolutionPath\resources\$TargetSystem\*" -Include 'BepInEx.cfg' -Destination $conf -Force
    
    # create \BepInEx\plugins and copy plugin dlls from build
    $plug = $bepinex.CreateSubdirectory('plugins');
    Copy-Item -Path "$TargetPath\*" -Include 'ValheimPlus.dll','YamlDotNet.dll','INIFileParser.dll' -Destination $plug -Force

    # return basepath as DirectoryInfo
    return $base
}

Write-Host "Publishing V+ for $Target from $TargetPath"

if ($Target.Equals("Debug")) {
    Write-Host "Copying dlls to Valheim installation $ValheimPath"

    $valheim = Create-BepInEx -BasePath $ValheimPath -TargetSystem 'Windows'
}

if ($Target.Equals("Release")) {
    $relpath = "$SolutionPath" + "release"
    New-Item -ItemType Directory -Path "$relpath" -Force

    Write-Host "Building release packages to $relpath"
    
    $winclient = Create-BepInEx -BasePath "$relpath\WinClient" -TargetSystem 'Windows'
    $winserver = Create-BepInEx -BasePath "$relpath\WinServer" -TargetSystem 'Windows'
    $unixserver = Create-BepInEx -BasePath "$relpath\UnixServer" -TargetSystem 'Unix'
}