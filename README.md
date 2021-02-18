![ValheimPlus Logo](https://raw.githubusercontent.com/nxPublic/ValheimPlus/master/logo.png)

# ValheimPlus
A HarmonyX Mod aimed at improving the gameplay quality of Valheim

# Features
* Modify Player weight values (base and Megingjord)
* Modify Food Duration
* Modify Fermenter Speed & Output size
* Modify Furnace maximum coal/ore inside and speed, coal usage
* Modify Kiln production speed and maximum amount of wood inside
* Remove Item teleport prevention from ores
* Reduce Item weight of all items by %
* Increase Item Stack sizes
* Remove Building "Invalid Placement" restriction
* Remove Building Object deterioration by weather.
* Modify Beehive production speed & maximum
* Remove Password requirement for server
* Modify maximum Players on a server
* Shared Map System with a setting that respects player map visibility settings
*(You only get map progression when you are online)*
* Hotkey options for fowards and backwards roll.
* Advanced Building Mode
* Advanced Editing Mode
* Stamina usage configuration
* Option to remove screen shake



# Advanced Building Mode
How it works.
1. You freeze the item by pressing the configurated key (F1 is default).
2. You can modify the item position and rotation with the following key combinations:
* Arrow Up/Down/Left/Right = moves the building object in the respective direction.
* Arrow Up/Down + Control = moves the building object up and down
* ScrollWheel = rotates the building object on the Y axis.
* ScrollWheel + Control = rotates the building object on the X axis.
* ScrollWheel + left Alt = rotates the building object on the Z axis.
* Numpad plus/minus to either increase or decrease the speeds, press SHIFT in addition to raise/lower by 10 instead of 1
(Pressing Shift at any moment in time increases the distance/rotation angle by *3)
3. Build the object with a mouse click.

*Building objects build with this system are not excempt from the structure/support system!*

*They are also not able to be build inside dungeons or placed in restricted areas!*

# Advanced Editing Mode
How it works.
*You cannot be in Build mode (hammer, hoe or terrain tool).*
1. You select the item with the configurated key (Numpad0 is default).
2. You can modify the item position and rotation with the following key combinations:
* Arrow Up/Down/Left/Right = moves the building object in the respective direction.
* Arrow Up/Down + Control = moves the building object up and down
* ScrollWheel = rotates the building object on the Y axis.
* ScrollWheel + Control = rotates the building object on the X axis.
* ScrollWheel + left Alt = rotates the building object on the Z axis.
* resetAdvancedEditingMode HotKey = resets the position and rotation to the inital values.
* Numpad plus/minus to either increase or decrease the speeds, press SHIFT in addition to raise/lower by 10 instead of 1
(Pressing Shift at any moment in time increases the distance/rotation angle by *3)
3. Press the confirmPlacementOfAdvancedEditingMode Hotkey to confirm the changes.

*(3. To Abort the editing mode and reset the object press abortAndExitAdvancedEditingMode HotKey)*

**NOTES:**
*People you are playing with will not be able to see the item being moved until you confirm the placement.* 
*Building objects build with this system are not excempt from the structure/support system!*


# When the game updates
Game updates are unlikely to do more than partially break ValheimPlus. In case you encounter any issues, use Steam's verify integrity feature- wait for it to download/update all files and then simply unpack the *valheim_Data* folder from the downloaded package into your game folder again.
This should resolve any issues related.

# Server
If you want to host a server, you can repeat the usual process of installing the mod just like you would do for your game except that you have to rename the folder *valheim_Data* to *valheim_server_Data* before unpacking/moving the files to the server directory. 
Please be aware that **EVERY** client(user) that connects needs to have the mod installed to have everything working as intended.

# Server Config & Version Control (About Version Enforcement)
* If you have enforceConfiguration and enforceMod enabled, only people with the same configuration and mod version can join your server and you can only join servers with the same mod and configuration.
* If you have enforceConfiguration and enforceMod disabled, you can join every server including vanilla ones as long as they allow you to. 
*(Meaning they have either no v+ mod installed or enforceMod disabled)*
* If you have enforceConfiguration disabled and enforceMod enabled, you will be able to join every server with v+ installed as long as its the same version.

# Join the Discord
![ValheimPlus Icon](https://raw.githubusercontent.com/nxPublic/ValheimPlus/master/ico.png)
https://discord.gg/AmH6Va97GT

# Twitter
Is used to post about releases of ValheimPlus and to potentially reply to issues.
https://twitter.com/ValheimPlus

# Support on Patreon
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
; default is 2400 (float) (48 ingame hours)
; lower is faster

fermenterItemsProduced=6
; default is 6 (integer) items per fermenter process


[Furnace]
enabled=false
; enable/disable Furnace changes

maximumOre=10
; default is 10 (int)

maximumCoal=20
; default is 20 (int)

productionSpeed=30
; default it 30 (float)
; lower is faster

coalUsedPerProduct=2
; default is 2 (int)


[Kiln]
; Responsible for changing Charcoal Kiln stats

enabled=false
; enable/disable Kiln changes

productionSpeed=15
; default it 15 (float)
; lower is faster

maximumWood=25
; default 25


[Items]
enabled=false
; enable/disable Building changes

noTeleportPrevention=false
; default is false (boolean)

baseItemWeight=0
; default is 0(float), this is a percent value.
; -50 is -50% item weight, 50 is +50% item weight.

itemStackMultiplier=0
; default is 0(float), this is a percent value.
; Only positive values are allowed.
; 50 would be 50% increased maximum stack size.
; !CAUTION! -> If you reduce the stack size, items above the limit are lost.

[Building]
enabled=false
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

honeyProductionSpeed=1200
; (float), default is 1200. (24 ingame hours)
; lower is faster


[Server]
enabled=false
; enable/disable Server changes

maxPlayers=10
; (int) default is 10

disableServerPassword=false
; (boolean) default is false

enforceConfiguration=true
; enforce every user trying to join your game or server to have the same mod configuration.
; NOTE: if people want to join your server with a custom configuration, they need to set this setting to false as well as the server.

enforceMod=true
; enforce every user to atleast have the mod installed when connecting to the server
; turn this off to remove version restrictions from your client and from your server


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
staminaRegen=10
; default 5(float)
swimStaminaDrain=5
; default 5(float)
jumpStaminaUsage=10
; default 10(float)
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

5. Define Enviroment Variable `VALHEIM_INSTALL` with path to Valheim Install Directory

# Credits

* Kevin 'nx#8830' J.
* "Greg 'Zedle' G. - Linux compatibility patch
* MrPurple6411#0415 - BepInEx Valheim version, AssemblyPublicizer
