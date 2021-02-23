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

function Create-BepInEx([System.String]$BasePath, [System.String]$TargetSystem = 'Windows') {
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

    # create \BepInEx\config
    $conf = $bepinex.CreateSubdirectory('config');
    
    # create \BepInEx\plugins and copy plugin dlls from build
    $plug = $bepinex.CreateSubdirectory('plugins');
    Copy-Item -Path "$TargetPath\*" -Include 'ValheimPlus.dll','YamlDotNet.dll','INIFileParser.dll' -Destination $plug -Force

    # return basepath as DirectoryInfo
    return $base
}

Write-Host "Publishing V+ for $Target from $TargetPath"

if ($Target.Equals("Debug")) {
    Write-Host "Copying Debug dll to Valheim installation $ValheimPath"

    $valheim = Create-BepInEx($ValheimPath);
}

if ($Target.Equals("Release")) {
    Write-Host "Building release packages to $SolutionPath"

    $rel = New-Item -ItemType Directory -Path "$SolutionPath\release" -Force;
    $winclient = Create-BepInEx("$rel\WinClient", 'Windows');
    $winserver = Create-BepInEx("$rel\WinServer", 'Windows');
    $unixserver = Create-BepInEx("$rel\UnixServer", 'Unix');
}