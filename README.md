![ValheimPlus Logo](https://raw.githubusercontent.com/nxPublic/ValheimPlus/master/logo.png)

# ValheimPlus
A HarmonyX Mod aimed at improving the gameplay quality of Valheim. The mod includes several different main features including modifiers to ingame stats of players, buildings and entities and a sophisticated system to build and place objects with high precision and a system to modify already placed objects with high precision. The general goal is to provide V+ as a base modification for your gameplay to increase quality of life, change difficulty or have a better experience in general. The mod also comes with a version and configuration control system for servers and users, allowing servers to make sure that only people with the same configuration are able to join their servers.

# Features
All of these features can be adjusted by a configuration file. This also allows you to only enable very specific featues.

## Player

* Modification of stamina usage and regeneration
* Modification of stamina usage of all tools and weaponry
* Modification of carry weight
* Modification of food duration
* Modification of Unarmed Damage
* Modification of auto pickup range
* Option to force remove all screen shakes
* Modify the base amount of Unarmed damage multiplied by your unarmed skill level

## Fermenter, Beehive, Kiln and Furnace
* Modify Fermenter speed
* Modify Fermenter output amount
* Modify Beehive honey production speed
* Modify Beehive maximum honey
* Modify Furnace processing speed
* Modify Furnace maximum coal/ore
* Modify Kiln processing speed
* Modify Kiln maximum wood

## Workbench and Ward
* Modify Workbench radius
* Modify Ward radius

## Torches and Fireplaces
* Disable torches running out of fuel
* Disable fireplaces running out of fuel

## Time and Day Manipulation
* Modify total time of a day and night cycle
* Modify the speed of the time passing at night

## Structural Integrity
* Modify the structural integrity of the following materials by a modifier
  * Wood
  * Stone
  * Iron
  * Hardwood
* Disable structural integrity entirely, allowing you to place objects in free air.

## Player Hud
* Show the experience you gained for a skill in the top left corner
* Show the amount of items you have in your inventory when crafting or building a object. 

## Game Difficulty
* Modify the game difficulty multipliers applied to health and damage of enemies based on the amount of connected players.
* Change the range of where the game considers other players to be nearby.
* Add a additional amount of Players to the player count for the difficulty calculation.
* Set the difficulty calculation to a specific player count.

## Skill Experience
* Modify each skill's experience gain seperately by percent.

## Camera
* Change your FOV
* Change the maximum zoom out distance
* Change the maximum zoom out distance when in a boat

## Wagon
* Modify the physical weight of the Wagon received by items inside
* Modify the base physical weight

## Items
* Remove Item teleport prevention from all items
* Reduce Item weight of all items by %
* Modify maximum item stack size by %

## Server
* Remove Password requirement for server
* Modify maximum Players on a server
* Modify auto save interval
* Modify server data rate in kilobyte

## Map
* Activate shared position on map automatically
* Setting to force other people to share their position with you
* Remove death marker on the map on collecting your tombstone

### Shared map system
Allows you to see the explored areas on the map of other players on the server if they have their position on the map shared ingame.
*You currently only receive exploration when you are online.*

### Player visibility
Allows you to be visible on the server map by default when joining.

### Prevent player from turning off visibility
Prevents players on the server from making themselves invisible on the map.

## Custom Keybinds
* Hotkey options for fowards and backwards roll.

# Building
* Remove Building "Invalid Placement" restriction
* Remove Building Object deterioration by weather.
* Advanced Building Mode
* Advanced Editing Mode
* Structural Integrity modification system

### Advanced Building Mode | Video : https://i.imgur.com/ddQCzPy.mp4
*How it works. All mentioned hotkeys can be modified.*
1. You freeze the item by pressing the configured key (F1 is default).
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
1. You select the item with the configured key (Numpad0 is default).
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
[AdvancedBuildingMode]
; https://docs.unity3d.com/ScriptReference/KeyCode.html <- a list of keycodes

; Change false to true to enable this section, if you set this to false the mode will not be accesible
enabled=false

; Enter the advanced building mode with this key when building
enterAdvancedBuildingMode=F1

; Exit the advanced building mode with this key when building
exitAdvancedBuildingMode=F3

[AdvancedEditingMode]

; Change false to true to enable this section, if you set this to false the mode will not be accesible
enabled=false

; https://docs.unity3d.com/ScriptReference/KeyCode.html <- a list of keycodes

; Enter the advanced editing mode with this key
enterAdvancedEditingMode=Keypad0

; Reset the object to its original position and rotation
resetAdvancedEditingMode=F7

; Exit the advanced editing mode with this key and reset the object
abortAndExitAdvancedEditingMode=F8

; Confirm the placement of the object and place it
confirmPlacementOfAdvancedEditingMode=KeypadEnter

[Beehive]

; Change false to true to enable this section
enabled=false

; configure the speed at which the bees produce honey in seconds, 1200 seconds are 24 ingame hours
honeyProductionSpeed=1200

; configure the maximum amount of honey in beehives
maximumHoneyPerBeehive=4

[Building]

; Change false to true to enable this section
enabled=false

; Remove some of the Invalid placement messages, most notably provides the ability to place objects into other objects
noInvalidPlacementRestriction=false

; Removes the weather damage from rain
noWeatherDamage=false

; The maximum range that you can place build objects at
maximumPlacementDistance=5

[Items]

; Change false to true to enable this section
enabled=false

; Enables you to teleport with ores and other usually restricted objects
noTeleportPrevention=false

; Increase or reduce item weight by % percent. The value -50 will reduce item weight of every object by 50%.
baseItemWeightReduction=0

; Increase the size of all item stacks by %. The value 50 would set a usual item stack of 100 to be 150.
itemStackMultiplier=0

[Fermenter]

; Change false to true to enable this section
enabled=false

; configure the time that the fermenter takes to produce its product, 2400 seconds are 48 ingame hours
fermenterDuration=2400

; configure the total amount of produced items from a fermenter
fermenterItemsProduced=6

[Fireplace]

; If changed to enabled all fireplaces do not need to be refilled.
enabled=false

; If you enable this option only placed torches do not need to be refilled.
onlyTorches=false

[Food]

; Change false to true to enable this section
enabled=false

; Increase or reduce the time that food lasts by %. The value 50 would cause food to run out 50% slower.
foodDurationMultiplier=0


[Furnace]

; Change false to true to enable this section
enabled=false

; Maximum amount of ore in a furnace
maximumOre=10

; Maximum amount of coal in a furnace
maximumCoal=20

; The total amount of coal used to produce a single smelted ingot.
coalUsedPerProduct=2

; The time it takes for the furnace to produce a single ingot in seconds.
productionSpeed=30

[Game]

; Change false to true to enable this section
enabled=false

; The games damage multiplier per person nearby in difficultyScaleRange(m) radius.
gameDifficultyDamageScale=0.4

; The games health multiplier per person nearby in difficultyScaleRange(m) radius.
gameDifficultyHealthScale=0.4 

; Adds additional players to the difficulty calculation in multiplayer unrelated to the actual amount
extraPlayerCountNearby=0

; Sets the nearby player count always to this value + extraPlayerCountNearby
setFixedPlayerCountTo=0

; The range in meters at which other players count towards nearby players for the difficulty scale
difficultyScaleRange=200


[Hotkeys]
; https://docs.unity3d.com/ScriptReference/KeyCode.html <- a list of keycodes

; Roll forwards on key pressed
rollForwards=F9

; Roll backwards on key pressed
rollBackwards=F10


[Hud]

; Change false to true to enable this section
enabled=false

; Shows the required amount of items AND the amount of items in your inventory in build mode and while crafting.
showRequiredItems=false

; Shows small notifications about all skill experienced gained in the top left corner.
experienceGainedNotifications=false

[Kiln]

; Change false to true to enable this section
enabled=false

; Maximum amount of wood in a Kiln
maximumWood=20

; The time it takes for the Kiln to produce a single piece of coal in seconds.
productionSpeed=30

[Map]

; Change false to true to enable this section
enabled=false

; With this enabled you will receive the same exploration progression as other players on the server
shareMapProgression=false

; The radius of the map that you explore when moving
exploreRadius=100

; Automatically turn on the Map option to share your position when joining or starting a game
playerPositionPublicOnJoin=false

; Prevents you and other people on the server to turn off their map sharing option
preventPlayerFromTurningOffPublicPosition=false

; Remove the Map marker of your death when you have picked up your tombstone items
removeDeathPinOnTombstoneEmpty=false

[Player]

; Change false to true to enable this section
enabled=false

; The base amount of carry weight of your character
baseMaximumWeight=300

; Increase the buff you receive to your carry weight from Megingjord's girdle
baseMegingjordBuff=150

; Increase auto pickup range of all items
baseAutoPickUpRange=2

; Disable all types of camera shake
disableCameraShake=false

; The base unarmed damage multiplied by your skill level
baseUnarmedDamage=10

[Server]

; Change false to true to enable this section
enabled=false

; Modify the amount of players on your Server
maxPlayers=10

; Removes the requirement to have a server password
disableServerPassword=false

; This setting adds a version control check to verifiy that the server and yourself have the same configuration file installed.
enforceConfiguration=false

; This settings add a version control check to make sure that people that try to join your game or the server you try to join has V+ installed
enforceMod=true

; The total amount of data that the server and client can send per second in kilobyte
dataRate=60

; The interval in seconds that the game auto saves at (client only)
autoSaveInterval=1200

[Stamina]

; Change false to true to enable this section
enabled=false

; Changes the flat amount of stamina cost of using the dodge roll
dodgeStaminaUsage=10

; Changes the stamina drain of being overweight
encumberedStaminaDrain=10

; Changes the stamina cost of jumping
jumpStaminaDrain=10

; Changes the stamina cost of running
runStaminaDrain=10

; Changes the stamina drain by sneaking
sneakStaminaDrain=10

; Changes the total amount of stamina recovered per second
staminaRegen=5

; Changes the delay until stamina regeneration sets in
staminaRegenDelay=0.5f

; Changes the stamina drain of swimming
swimStaminaDrain=5


[StaminaUsage]
; Change false to true to enable this section
enabled=false
; Each of these values reduces the stamina drain by %. The value 50 would result in 50% less stamina cost.
axes=0
bows=0
clubs=0
knives=0
pickaxes=0
polearms=0
spears=0
swords=0
unarmed=0
hammer=0
hoe=0

[Workbench]
; Change false to true to enable this section
enabled=false

; Set the workbench radius in meters
workbenchRange=20

; Disables the roof and exposure requirement to use a workbench
disableRoofCheck=false

[Time]

; Change false to true to enable this section
enabled=false

; Total amount of time one complete day and night circle takes to complete
totalDayTimeInSeconds=1200

; Increase the speed at which time passes at night by %. The value 50 would result in a 50% reduced amount of night time.
nightTimeSpeedMultiplier=0

[Ward]

; Change false to true to enable this section
enabled=false

; The range of wards by meters
wardRange=20

[StructuralIntegrity]

; Change false to true to enable this section
enabled=false

; Disables the entire structural integrity system and allows for placement in free air, does not prevent building damage.
disableStructuralIntegrity=false;

; Each of these values reduce the loss of structural integrity by % less. The value 100 would result in disabled structural integrity and allow placement in free air.
wood=0
stone=0
iron=0
hardWood=0

[Experience]

; Change false to true to enable this section
enabled=false

; Each of these values represent the increase to experience gained by % increased. The value 50 would result in 50% increased experience gained for the respective skill by name.
swords=0
knives=0
clubs=0
polearms=0
spears=0
blocking=0
axes=0
bows=0
fireMagic=0
frostMagic=0
unarmed=0
pickaxes=0
woodCutting=0
jump=0
sneak=0
run=0
swim=0


[Camera]

; Change false to true to enable this section
enabled=false

; The maximum zoom distance to your character
cameraMaximumZoomDistance=6

; The maximum zoom distance to your character when in a boat
cameraBoatMaximumZoomDistance=6

; The game camera FOV
cameraFOV=85

[Wagon]

; Change false to true to enable this section
enabled=false

; Change the base vagon physical mass of the vagon object
wagonBaseMass=20

; This value changes the game physical weight of Vagons by +/- more/less from item weight inside. The value 50 would increase the weight by 50% more. The value -100 would remove the entire extra weight.
wagonExtraMassFromItems=0
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
* TheTerrasque - https://github.com/TheTerrasque
* Paige 'radmint' N. - https://github.com/radmint
* MrPurple6411#0415 - BepInEx Valheim version, AssemblyPublicizer
