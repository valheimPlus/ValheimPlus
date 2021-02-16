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
- Shared Map System with a setting that respects player map visibility settings
- Hotkey options for fowards and backwards roll.
- Advanced Building Mode
- Advanced Editing Mode
- Stamina Multipliers
- Option to remove screen shake

(You only get map progression when you are online)

# Advanced Building Mode
How it works.
1. You freeze the item by pressing the configurated key (F1 is default).
2. You can modify the item position and rotation with the following key combinations:
- Arrow Up/Down/Left/Right = moves the building object in the respective direction.
- Arrow Up/Down + Control = moves the building object up and down
- ScrollWheel = rotates the building object on the Y axis.
- ScrollWheel + Control = rotates the building object on the X axis.
- ScrollWheel + left Alt = rotates the building object on the Z axis.
(Pressing Shift at any moment in time increases the distance/rotation angle by *3)
3. Build the object with a mouse click.

*Building objects build with this system are not excempt from the structure/support system!*

*They are also not able to be build inside dungeons or placed in restricted areas!*

# Advanced Editing Mode
How it works.
*You cannot be in Build mode (hammer, hoe or terrain tool).*
1. You select the item with the configurated key (Numpad0 is default).
2. You can modify the item position and rotation with the following key combinations:
- Arrow Up/Down/Left/Right = moves the building object in the respective direction.
- Arrow Up/Down + Control = moves the building object up and down
- ScrollWheel = rotates the building object on the Y axis.
- ScrollWheel + Control = rotates the building object on the X axis.
- ScrollWheel + left Alt = rotates the building object on the Z axis.
- resetAdvancedEditingMode HotKey = resets the position and rotation to the inital values.
(Pressing Shift at any moment in time increases the distance/rotation angle by *3)
3. Press the confirmPlacementOfAdvancedEditingMode Hotkey to confirm the changes.
*3. To Abort the editing mode and reset the object press abortAndExitAdvancedEditingMode HotKey*
 
*People you are playing with will not be able to see the item being moved until you confirm the placement.* 

*Building objects build with this system are not excempt from the structure/support system!*

# Server
If you want to host a server, you can repeat the usual process of installing the mod just like you would do for your game by unpack everything into your root folder of the server. 
Please be aware that **EVERY** client(user) that connects needs to have the mod installed to have everything working as intended.

# Join the Discord
![ValheimPlus Icon](https://raw.githubusercontent.com/nxPublic/ValheimPlus/master/ico.png)
https://discord.gg/AmH6Va97GT

# Support the Project on Patreon
(No obligation, people asked how they could support me.)

https://www.patreon.com/valheimPlus


# Configuration File

The Config files name is supposed to be "valheim_plus.cfg" it needs to be placed in "BepInEx\config"

You will also need to place the "INIFileParser.dll" into the "BepInEx\plugins" folder (its supplied by default within the release versions)

# Currently Supported Configuration (0.6)
```INI
[Player]
enabled=false
; enable/disable Player changes

baseMegingjordBuff=150
; default is 150 (float)
; Ingame Tooltip is not affected

baseMaximumWeight=300
; default is 300 (float)

baseAutoPickUpRange=2
; default is 2 (float)

disableCameraShake=false
; enable/disable screen shake


[Food]
enabled=false
; enable/disable Food changes

foodDuration=0
; default is 0, this is a percent value. (default + foodDuration%)
; 100 is 100% increased food duration.
; currently does not properly show in item tooltips


[Fermenter]
enabled=false
; enable/disable Fermenter changes

fermenterDuration=2400
; default is 2400 (float) (24 ingame hours ?)
; lower is faster

fermenterItemsProduced=4
; default is 4 (integer) items per fermenter process


[Furnace]
enabled=false
; enable/disable Furnace changes

maximumOre=10
; default is 10 (int)

maximumCoal=20
; default is 20 (int)

productionSpeed=10
; default it 10 (float)
; lower is faster

coalUsedPerProduct=2
; default is 2 (int)


[Kiln]
; Responsible for changing Charcoal Kiln stats

enabled=true
; enable/disable Kiln changes

productionSpeed=10
; default it 10 (float)
; lower is faster

maximumWood=25
; default 25


[Items]
enabled=false
; enable/disable Building changes

noTeleportPrevention=false
; default is false (boolean)

baseItemWeight=0
; default is 0, this is a percent value. (default - foodDurationMultiplier%)
; 10 is 10% weight reduction of every item, 50 is 50%, -50% is +50% weight.
; if you set this to a -number, the base weight will be increased by that much in %


[Building]
enabled=true
; enable/disable Building changes

noInvalidPlacementRestriction=false
; (boolean) Removes the "Invalid Placement" restriction

noWeatherDamage=false
; Removes weather/rain damage on building objects

maximumPlacementDistance=5
; default 5(float)


[Beehive]
enabled=false
; enable/disable Beehive changes

maximumHoneyPerBeehive=4
; (integer) default is 4.

honeyProductionSpeed=10
; (float), default is 10. I suggest to not go lower than 5.
; lower is faster


[Server]
enabled=false
; enable/disable Server changes

maxPlayers=10
; (int) default is 10

disableServerPassword=false
; (boolean) default is false


[Map]
enabled=false
; enable/disable Map changes

exploreRadius=100
; default 100 (float), the radius around each player that get explored

shareMapProgression=false
; default false (boolean), shares the map progress (reveal) across all players
; players need to be online to receive map progression
; only shares the map progression of people that have selected to be visible on the map


[Hotkeys]
; https://docs.unity3d.com/ScriptReference/KeyCode.html <- a list of keycodes
rollForwards=F9
; roll forward on button press

rollBackwards=F10
; roll backwards on button press

enterAdvancedBuildingMode=F1
; Freeze Object and allow advanced controls

exitAdvancedBuildingMode=F3
; Unfreeze Object and use default place mode

enterAdvancedEditingMode=Keypad0
; the object you are looking at will be selected to be modified using AEM

confirmPlacementOfAdvancedEditingMode=KeypadEnter
; Confirms Placement of selected and modified object

resetAdvancedEditingMode=F7
; Resets the position and rotation of the object selected with AEM

abortAndExitAdvancedEditingMode=F8
; Resets the position and rotation of the object selected with AEM and stops AEM mode

[AdvancedBuildingMode]
enabled=false
; enable/disable advanced building mode, more info on the github page
[AdvancedEditingMode]
enabled=false
; enable/disable advanced editing mode, more info on the github page
; EXPERIMENTAL - Please be aware that i am limited in the amount of things i can test before releasing a feature. Please report any bugs to the Repository as Issues.

[Stamina]
enabled=false
dodgeStaminaUsage=10
; default 10(float)
encumberedStaminaDrain=10
; default 10(float)
sneakStaminaDrain=10
; default 5(float)
runStaminaDrain=10
; default 10(float)
staminaRegenDelay=0.5
; default 1(float)
staminaRegen=5
; default 5(float)
swimStaminaDrain=5
; default 5(float)
```

# Valheim Plus Compiler Requirements

How to setup the development enviroment to compile ValheimPlus yourself.

1. Download this package:
https://mega.nz/file/0UAlxQwK#47InGOb8ViI6GyBDArpbhkbMTBklXdyRSmAc4-BZpJY

2. Unpack into your Valheim root folder and overwrite every file when asked.

3. Download this this repositories executable version.
Repo: https://github.com/MrPurple6411/Bepinex-Tools/releases/tag/1.0.0-Publicizer
Exec: https://mega.nz/file/oQxEjCJI#_XPXEjwLfv9zpcF2HRakYzepMwaUXflA9txxhx4tACA

4. Drag and drop all assembly_.dll files onto "AssemblyPublicizer.exe"

5. Add all dll's of "\Valheim\valheim_Data\Managed" and "publicized_assemblies" folder as references to the project.

(Except : "mscorlib.dll", "System.Configuration.dll", "System.Core.dll", "System.dll", "System.Xml.dll")

6. Add all BepInEx dll's ("Valheim\BepInEx\core") as refernces to the project.

(Except : "0Harmony.dll", "0Harmony20.dll")

7. Add Ini-parser and HarmonyX via nu-get

# Credits

nx#8830 - All (as of now)
MrPurple6411#0415 (BepInEx Valheim version, AssemblyPublicizer)