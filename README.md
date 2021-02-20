![ValheimPlus Logo](https://raw.githubusercontent.com/nxPublic/ValheimPlus/master/logo.png)

# ValheimPlus
A HarmonyX Mod aimed at improving the gameplay quality of Valheim. The mod includes several different main features including modifiers to ingame stats of players, buildings and entities and a sophisticated system to build and place objects with high precision and a system to modify already placed objects with high precision. The general goal is to provide V+ as a base modification for your gameplay to increase quality of life, change difficulty or have a better experience in general. The mod also comes with a version and configuration control system for servers and users, allowing servers to make sure that only people with the same configuration are able to join their servers.

# Features
All of these features can be adjusted by a configuration file. This also allows you to only enable very specific featues.

## Player

* Modification of Stamina usage and regeneration
* Modification of Stamina by tools and weaponry
* Modification of carry weight
* Mofification of Food duration
* Mofification of Unarmed Damage
* Option to force all remove screen shake

## Fermenter, Beehive, Kiln and Furnace
* Modify Fermenter speed
* Modify Fermenter output amount
* Modify Beehive honey production speed
* Modify Beehive maximum honey
* Modify Furnace processing speed
* Modify Furnace maximum coal/ore
* Modify Kiln processing speed
* Modify Kiln maximum wood

## Items
* Remove Item teleport prevention from all items
* Reduce Item weight of all items by %
* Modify maximum item stack size by %

## Server
* Remove Password requirement for server
* Modify maximum Players on a server

## Shared map system
Allows you to see the explored areas on the map of other players on the server if they have their position on the map shared ingame.
*You currently only receive exploration when you are online.*

## Custom Keybinds
* Hotkey options for fowards and backwards roll.

# Building
* Remove Building "Invalid Placement" restriction
* Remove Building Object deterioration by weather.
* Advanced Building Mode
* Advanced Editing Mode
* Disable the need to refuel torches and fireplaces
* Modify the work bench ranges

### Advanced Building Mode | Video : https://i.imgur.com/ddQCzPy.mp4
*How it works. All mentioned hotkeys can be modified.*
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
  
**NOTES:**
* *Building objects build with this system are not excempt from the structure/support system!*
* *They are also not able to be build inside dungeons or placed in restricted areas!*

### Advanced Editing Mode | Video : https://imgur.com/DMb4ZUv.mp4
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
3. *To Abort the editing mode and reset the object press abortAndExitAdvancedEditingMode HotKey)*

**NOTES:**
* *People in multiplayer will not be able to see the item being moved until you confirm the placement.* 
* *Building objects build with this system are not excempt from the structure/support system!




# Installation Instructions
We supply 4 different versions of V+ with every release since version 0.8. You can find detailed instructions on how to install these varients below.

**ATTENTION FOR MULTIPLAYER**: The game and the server both should have this mod installed to prevent all kinds of different issues. If you have the mod installed and then have friends join over steam they should have the mod as well.

## **[Game] Windows (Steam)**

1. Download the [latest package called WindowsClient.zip over this link](https://github.com/nxPublic/ValheimPlus/releases/latest/). *(Scroll down and click "assets")*
2. Locate your game folder, go into steam and :
   1. Right click the valheim game in your steam library
   2. "Go to Manage" -> "Browse local files"
   2. Steam should open your game folder for you when clicked
   
**Please read the section about Server Config & Version Control (About Version Enforcement) below.**


## **[Server] Windows**

We will not explain how you create a dedicated server. This will only explain how you install the mod.
1. Download the [latest package called WindowsServer.zip over this link](https://github.com/nxPublic/ValheimPlus/releases/latest/). *(Scroll down and click "assets")*
2. Unpack the downloaded zip file it into your root server folder.

**Please read the section about Server Config & Version Control (About Version Enforcement) below.**


## **[Server] Unix**

We will not explain how you create a dedicated server. This will only explain how you install the mod.
1. Download the [latest package called UnixServer.zip over this link](https://github.com/nxPublic/ValheimPlus/releases/latest/). *(Scroll down and click "assets")*
2. Locate your server folder
3. Unpack all the contents into your root server folder.

*Make sure to execute 'chmod u+x run_bepinex.sh'*

*Make sure to run the run_bepinex.sh*

**Uses libc6-dev**

**Please read the section about Server Config & Version Control (About Version Enforcement) below.**

# What if the game updates?
Game updates are unlikely to do more than partially break ValheimPlus at worst. In case you encounter any issues, use Steam's verify integrity feature- wait for it to download/update all files.
Then simply unpack the *valheim_Data* folder from the downloaded last available v+ version package into your game folder again.
This should resolve any issues related. If you continue to have issues, contact the help channel in our discord server.

# Server Config & Version Control (About Version Enforcement)
* If you have enforceConfiguration and enforceMod enabled, only people with the same configuration and mod version can join your server and you can only join servers with the same mod and configuration.
* If you have enforceConfiguration and enforceMod disabled, you can join every server including vanilla ones as long as they allow you to. 
*(Meaning they have either no v+ mod installed or enforceMod disabled)*
* If you have enforceConfiguration disabled and enforceMod enabled, you will be able to join every server with v+ installed as long as its the same version.

**This system is working 100% reliable and is issue free.**

**If you encounter problems, you have not properly set up the configuration or your server/client is not running v+**

# Join the Discord
We have several different channel including a showcase channel and alpha testing system, allowing you to always get the newest versions available to test out.

![ValheimPlus Icon](https://raw.githubusercontent.com/nxPublic/ValheimPlus/master/ico.png)
https://discord.gg/AmH6Va97GT

# Twitter
Is used to post about releases of ValheimPlus and to potentially reply to issues.
https://twitter.com/ValheimPlus

# Support on Patreon
Supporting this Project on Patreon will allow me to dedicate more of my free time into this project. It will also pay for server costs of our new domains and our upcoming discord bot.

https://www.patreon.com/valheimPlus


# Configuration File

The Config files name is supposed to be "valheim_plus.cfg" it needs to be placed in "BepInEx\config"

You will also need to place the "INIFileParser.dll" into the "BepInEx\plugins" folder (its supplied by default within the release versions)

# Currently Supported Configuration (0.8.5)
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

experienceGainedNotifications=true
; enabled/disable EXP gained notification for skills in the top left corner

[UnarmedScaling]
enabled=false
; enable/disable changes to the Unarmed weapons scaling

baseDamage=100
; default is 100, this is the value it will approach in damage as you gain skill until capped.


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

[Fireplace]
enabled=false
; enable/disable Fireplace changes
; "disables" fuel consumption of all "fireplace" type objects (Torches/campfires/braziers), fuel can still be added, but will always stay at 1

onlyTorches=false 
; applies the effect only to torches(Torches/Scounce/Brazier)
; (boolean) default false

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
; enable/disable item changes

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

[Stamina]
enabled=false
; Each of these values reduce the stamina cost by percent
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

[WeaponsStamina]
enabled=false
Swords=0
; default 0(float)
Knives=0
; default 0(float)
Clubs=0
; default 0(float)
Polearms=0
; default 0(float)
Spears=0
; default 0(float)
Axes=0
; default 0(float)
Bows=0
; default 0(float)
Unarmed=0
; default 0(float)
Pickaxes=0
; default 0(float)

[Workbench]
enabled=false
workbenchRange=20
; default 20(float)

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

* Kevin 'nx#8830' J.- https://github.com/nxPublic
* Greg 'Zedle' G. - https://github.com/zedle
* Bruno Vasconcelos - https://github.com/Drakeny
* GaelicGamer - https://github.com/GaelicGamer
* MrPurple6411#0415 - BepInEx Valheim version, AssemblyPublicizer
