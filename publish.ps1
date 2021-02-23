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
        [System.IO.DirectoryInfo]$BasePath,

        [Parameter(Mandatory)]
        [ValidateSet('Windows','Unix')]
        [System.String]$TargetSystem
    )
    Write-Host "Creating BepInEx in $BasePath"

    # copy needed files for this target system
    Copy-Item -Path "$SolutionPath\resources\$TargetSystem\*" -Exclude 'BepInEx.cfg' -Destination "$BasePath" -Recurse -Force
    
    # create \BepInEx
    $bepinex = $BasePath.CreateSubdirectory('BepInEx')
    
    # create \BepInEx\core and copy core dlls from build
    $core = $bepinex.CreateSubdirectory('core');
    Copy-Item -Path "$TargetPath\*" -Filter 'BepInEx*.dll' -Destination "$core" -Force
    Copy-Item -Path "$TargetPath\*" -Filter '*Harmony*.dll' -Destination "$core" -Force
    Copy-Item -Path "$TargetPath\*" -Filter 'Mono.Cecil*.dll' -Destination "$core" -Force
    Copy-Item -Path "$TargetPath\*" -Filter 'MonoMod*.dll' -Destination "$core" -Force

    # create \BepInEx\config and copy config files
    $conf = $bepinex.CreateSubdirectory('config');
    Copy-Item -Path "$SolutionPath\resources\$TargetSystem\*" -Include 'BepInEx.cfg' -Destination "$conf" -Force
    
    # create \BepInEx\plugins and copy plugin dlls from build
    $plug = $bepinex.CreateSubdirectory('plugins');
    Copy-Item -Path "$TargetPath\*" -Include 'ValheimPlus.dll','YamlDotNet.dll','INIFileParser.dll' -Destination "$plug" -Force

    # return basepath as DirectoryInfo
    return $base
}

function Copy-Corlib{
    param(
        [Parameter(Mandatory)]
        [System.IO.DirectoryInfo]$BasePath,
        
        [Parameter(Mandatory)]
        [System.IO.DirectoryInfo]$LibPath
    )
    Write-Host "Copying unstripped_corlib to $BasePath"

    $rel = $BasePath.CreateSubdirectory('unstripped_corlib')
    Copy-Item -Path "$LibPath\*" -Filter '*.dll' -Destination "$rel" -Force

}

function Make-Archive{
    param(
        [Parameter(Mandatory)]
        [System.IO.DirectoryInfo]$BasePath
    )

    $rel = $BasePath.Parent.FullName
    $zip = $BasePath.Name + ".zip"
    
    Write-Host "Creating archive $zip for $BasePath"

    Compress-Archive -Path "$BasePath\*" -DestinationPath "$rel\$zip" -Force
}

Write-Host "Publishing V+ for $Target from $TargetPath"

if ($Target.Equals("Debug")) {
    Write-Host "Copying dlls to Valheim installation $ValheimPath"

    $valheim = New-Item -ItemType Directory -Path "$ValheimPath" -Force
    Create-BepInEx -BasePath $valheim -TargetSystem 'Windows'
}

if ($Target.Equals("Release")) {
    $rel = New-Item -ItemType Directory -Path "$SolutionPath\release" -Force
    $lib = Get-Item -Path "$ValheimPath\unstripped_corlib"

    Write-Host "Building release packages to $rel"
    
    # create all distros as folders and zip
    $winclient = New-Item -ItemType Directory -Path "$rel\WindowsClient" -Force;
    Create-BepInEx -BasePath $winclient -TargetSystem 'Windows'
    Copy-Corlib -BasePath $winclient -LibPath $lib
    Make-Archive -BasePath $winclient

    $winserver = New-Item -ItemType Directory -Path "$rel\WindowsServer" -Force
    Create-BepInEx -BasePath $winserver -TargetSystem 'Windows'
    Copy-Corlib -BasePath $winserver -LibPath $lib
    Make-Archive -BasePath $winserver

    $unixserver = New-Item -ItemType Directory -Path "$rel\UnixServer" -Force
    Create-BepInEx -BasePath $unixserver -TargetSystem 'Unix'
    Copy-Corlib -BasePath $unixserver -LibPath $lib
    Make-Archive -BasePath $unixserver

    # delete folders afterwards
    $winclient.Delete($true)
    $winserver.Delete($true)
    $unixserver.Delete($true)
}