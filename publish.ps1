param(
    [Parameter(Mandatory)]
    [ValidateSet('Debug','Release')]
    [System.String]$Target,
    
    [Parameter(Mandatory)]
    [System.String]$TargetPath,

    [Parameter(Mandatory)]
    [System.String]$ValheimPath,

    [Parameter(Mandatory)]
    [System.String]$PublishPath
)

function Create-BepInEx([System.String]$BasePath) {
    # make sure basepath exists
    $base = New-Item -ItemType Directory -Path "$BasePath" -Force;
    
    # create \BepInEx
    $bepinex = $base.CreateSubdirectory("BepInEx");
    
    # create \BepInEx\core and copy core dlls from build
    $core = $bepinex.CreateSubdirectory("core");
    Copy-Item -Path "$TargetPath" -Filter BepInEx* -Recurse -Destination $core -Force

    $conf = $bepinex.CreateSubdirectory("config");
    $plug = $bepinex.CreateSubdirectory("plugins");
    return $base
}

Write-Host "Publishing V+ for $Target from $TargetPath"

if ($Target.Equals("Debug")) {
    Write-Host "Copying Debug dll to Valheim installation $ValheimPath"

    $valheim = Create-BepInEx($ValheimPath);
}

if ($Target.Equals("Release")) {
    Write-Host "Building release packages to $PublishPath"

    New-Item -ItemType Directory -Path "$PublishPath" -Force;
    $winclient = Create-BepInEx("$PublishPath\WinClient");
    $winserver = Create-BepInEx("$PublishPath\WinServer");
    $unixserver = Create-BepInEx("$PublishPath\UnixServer");
}