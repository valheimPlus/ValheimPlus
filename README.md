![ValheimPlus Logo](https://raw.githubusercontent.com/nxPublic/ValheimPlus/master/logo.png)

# ValheimPlus
A HarmonyX Mod aimed at improving the gameplay quality of Valheim

# Features
- Modify Player weight values (base and Megingjord)
- Modify Food Duration
- Modify Fermenter Speed & Output
- Modify Furnace Maximum coal/ore, speed and coal usage
- Modify Kiln production speed
- Remove Item teleport prevention from ores
- Reduce Item weight of all items by %
- Remove Building "Invalid Placement" restriction
- Remove Building Object deterioration by weather.
- Modify Beehive production speed & maximum
- Remove Password requirement for server
- Modify maximum Players on a server
- Shared Map System with a setting to either ignore or respect player map visibility settings

(You only get map progression when you are online)


# Configuration File

The Config files name is supposed to be "valheim_plus.cfg" it needs to be placed in "BepInEx\config"

You will also need to place the "INIFileParser.dll" into the "BepInEx\plugins" folder (its supplied by default within the release versions)

# Currently Supported Configuration (0.3)
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
foodDurationMultiplier=1.0
; default is 0, it adds to the value meaning 0.0 is 100%, 1 is 200%.

[Fermenter]
fermenterDuration=2400
; default is 2400 (float) (24 ingame hours ?)
; lower is faster

fermenterItemsProduced=4
; default is 4 (integer) items per fermenter process

[Furnace]
maximumOre=10
; default is 10 (int)

maximumCoal=10
; default is 10 (int)

productionSpeed=10
; default it 10 (float)
; lower is faster

coalUsedPerProduct=4
; default is 4 (int)

[Kiln]
; Responsible for changing Charcoal Kiln stats
productionSpeed=10
; default it 10 (float)
; lower is faster


[Items]
noTeleportPrevention=false
; default is false (boolean)

baseItemWeightReduction=0
;(float), removes from value (original - (original * baseItemWeightReduction). 
; 1 is 100% weight reduction of every item, 0.5 is 50%.

[Building]
noInvalidPlacementRestriction=false
; (boolean) Removes the "Invalid Placement" restriction
noWeatherDamage=false
; Removes weather/rain damage on building objects

[Beehive]
enabled=false
; Enable/Disable beehive changes

maximumHoneyPerBeehive=4
; (integer) default is 4.

honeyProductionSpeed=10
; (float), default is 10. Do not go lower than 5.
; lower is faster



[Server]
maxPlayers=10
; (int) default is 10
disableServerPassword=false
; (boolean) default is false

[Map]
exploreRadius=100
; default 10 (float), the radius around each player that get explored
shareMapProgression=false
; default false (boolean), shares the map progress (reveal) across all players
; players need to be online to receive map progression
onlyShareMapProgressionWhenVisible=false
; only share the map progression of people that have selected to be visible on the map
```

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

5. Add Ini-parser and HarmonyX via nu-get

# Credits

nx#8830 - All (as of now)
