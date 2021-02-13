# ValheimPlus
A HarmonyX Mod aimed at improving the gameplay quality of Valheim


# Valheim Plus Compiler Requirements

You will be dependent on a package of stripped .net/mono Unity files from the Valheim directory.
How to get BepInEx working:

1. Download this package:

https://mega.nz/file/0UAlxQwK#47InGOb8ViI6GyBDArpbhkbMTBklXdyRSmAc4-BZpJY

2. Unpack into your Valheim root folder and overwrite every file when asked.

3. Add all dll's of "\Valheim\valheim_Data\Managed" folder as references to the project.

(Except : "mscorlib.dll", "System.Configuration.dll", "System.Core.dll", "System.dll", "System.Xml.dll")

4. Add all BepInEx dll's ("Valheim\BepInEx\core") as refernces to the project.

(Except : "0Harmony.dll", "0Harmony20.dll")

# Configuration File

The Config files name is supposed to be "valheim_multipliers.cfg" it needs to be placed in "BepInEx\config"

(The name will be adjusted in the future)

You will also need to place the "INIFileParser.dll" into the "BepInEx\plugins" folder

(If the project does not properly show the dependency on the ini parser, this is the library i used : https://github.com/rickyah/ini-parser)

# Currently Supported Configuration
```INI
[Player]
baseMegingjordBuff=150
; default is 150 (float)
; Ingame Tooltip is not affected

baseMaximumWeight=300
; default is 300 (float)

baseAutoPickUpRange=2
; default is 2 (float)


[Food]
foodDurationMultiplier=0.0
; default is 0, it adds to the value meaning 0.0 is 100%, 1 is 200%.
[Items]
noTeleportPrevention=false
;default is false (boolean)
baseItemWeightReduction=0
;(float), removes from value. 1 is 100% weight reduction of every item, 0.5 is 50%.

[Building]
noInvalidPlacementRestriction=false
;(boolean)

[Beehive]
enabled=false
maximumHoneyPerBeehive=4
;(integer)
honeyProductionValue=10
;(float), default is 10. Do not go lower than 5.
```

# Credits

nx#8830 - All (as of now)
