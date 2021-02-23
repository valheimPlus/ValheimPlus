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
        [System.IO.DirectoryInfo]$DistPath,

        [Parameter(Mandatory)]
        [ValidateSet('Windows','Unix')]
        [System.String]$DistSystem
    )
    Write-Host "Creating BepInEx in $DistPath"

    # copy needed files for this target system
    Copy-Item -Path "$SolutionPath\resources\$DistSystem\*" -Exclude 'BepInEx.cfg' -Destination "$DistPath" -Recurse -Force
    
    # create \BepInEx
    $bepinex = $DistPath.CreateSubdirectory('BepInEx')
    
    # create \BepInEx\core and copy core dlls from build
    $core = $bepinex.CreateSubdirectory('core');
    Copy-Item -Path "$TargetPath\*" -Filter 'BepInEx*.dll' -Destination "$core" -Force
    Copy-Item -Path "$TargetPath\*" -Filter '*Harmony*.dll' -Destination "$core" -Force
    Copy-Item -Path "$TargetPath\*" -Filter 'Mono.Cecil*.dll' -Destination "$core" -Force
    Copy-Item -Path "$TargetPath\*" -Filter 'MonoMod*.dll' -Destination "$core" -Force

    # create \BepInEx\config and copy config files
    $conf = $bepinex.CreateSubdirectory('config');
    Copy-Item -Path "$SolutionPath\resources\$DistSystem\*" -Include 'BepInEx.cfg' -Destination "$conf" -Force
    
    # create \BepInEx\plugins and copy plugin dlls from build
    $plug = $bepinex.CreateSubdirectory('plugins');
    Copy-Item -Path "$TargetPath\*" -Include 'ValheimPlus.dll','YamlDotNet.dll','INIFileParser.dll' -Destination "$plug" -Force

    # return basepath as DirectoryInfo
    return $base
}

function Copy-Corlib{
    param(
        [Parameter(Mandatory)]
        [System.IO.DirectoryInfo]$DistPath,
        
        [Parameter(Mandatory)]
        [System.IO.DirectoryInfo]$LibPath
    )
    Write-Host "Copying unstripped_corlib to $DistPath"

    $rel = $DistPath.CreateSubdirectory('unstripped_corlib')
    Copy-Item -Path "$LibPath\*" -Filter '*.dll' -Destination "$rel" -Force

}

function Copy-Config{
    param(
        [Parameter(Mandatory)]
        [System.IO.DirectoryInfo]$DistPath
    )
    Write-Host "Copying V+ config to $DistPath\BepInEx\config"

    Copy-Item -Path "$SolutionPath\*" -Include 'valheim_plus.cfg' -Destination "$DistPath\BepInEx\config" -Force

}

function Make-Archive{
    param(
        [Parameter(Mandatory)]
        [System.IO.DirectoryInfo]$DistPath
    )

    $rel = $DistPath.Parent.FullName
    $zip = $DistPath.Name + ".zip"
    
    Write-Host "Creating archive $zip for $DistPath"

    Compress-Archive -Path "$DistPath\*" -DestinationPath "$rel\$zip" -Force
}

Write-Host "Publishing V+ for $Target from $TargetPath"

if ($Target.Equals("Debug")) {
    Write-Host "Copying dlls to Valheim installation $ValheimPath"

    $valheim = New-Item -ItemType Directory -Path "$ValheimPath" -Force
    Create-BepInEx -DistPath $valheim -DistSystem 'Windows'
}

if ($Target.Equals("Release")) {
    $rel = New-Item -ItemType Directory -Path "$SolutionPath\release" -Force
    $lib = Get-Item -Path "$ValheimPath\unstripped_corlib"

    Write-Host "Building release packages to $rel"
    
    # create all distros as folders and zip
    ('Windows','Unix') | % {
        $dist = New-Item -ItemType Directory -Path "$rel\$_" -Force;
        Create-BepInEx -DistPath $dist -DistSystem $_
        Copy-Config -DistPath $dist
        Copy-Corlib -DistPath $dist -LibPath $lib
        Make-Archive -DistPath $dist
        $dist.Delete($true);
    }
}