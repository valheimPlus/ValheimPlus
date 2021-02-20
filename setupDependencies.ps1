$steamregkey="Registry::HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam"
$keyname="InstallPath"

$valheimId=""

$SteamPath=Get-ItemProperty -Path $steamregkey -Name $keyname | Select-Object -ExpandProperty $keyname

Write-Output $SteamPath