![ValheimPlus Logo](https://raw.githubusercontent.com/nxPublic/ValheimPlus/master/logo.png)

# ValheimPlus
A HarmonyX Mod aimed at improving the gameplay quality of Valheim. The mod includes several different main features including modifiers to ingame stats of players, buildings and entities and a sophisticated system to build and place objects with high precision and a system to modify already placed objects with high precision. The general goal is to provide V+ as a base modification for your gameplay to increase quality of life, change difficulty or have a better experience in general. The mod also comes with a version and configuration control system for servers and users, allowing servers to make sure that only people with the same configuration are able to join their servers.

# ValheimPlus Server Hosting
Below you can find a list of hosting companies that are supporting ValehimPlus with good prices and easy installation.

[![GPortal](http://valheim.plus/gportal/banner.jpg)](http://gportal.valheim.plus/)
[![ZapHosting](http://valheimplus.com/zap/692x127.jpg)](http://zap.valheim.plus/)
[![GFXHosting](https://www.gtxgaming.co.uk/wp-content/uploads/2021/02/valheim_plus_banner-3.png)](http://gtxgaming.valheim.plus/)


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
* Show the amount of time left until the Fermenter is done.
* Auto Deposit system for Furnace and Kiln to deposit items to the closest nearby chest.

## Workbench and Ward
* Modify Workbench radius
* Modify Ward radius

## Torches and Fireplaces
* Disable torches running out of fuel
* Disable fireplaces running out of fuel

## Time and Day Manipulation
*FEATURE DISABLED DUE TO ISSUES*
* Modify total time of a day and night cycle
* Modify the speed of the time passing at night

## Structural Integrity
* Modify the structural integrity of the following materials by a modifier
  * Wood
  * Stone
  * Iron
  * Hardwood
* Disable structural integrity entirely, allowing you to place objects in free air.
* Disable all damage to player built structures/objects.

## Player Hud
* Show the experience you gained for a skill in the top left corner
* Show the amount of items you have in your inventory when crafting or building a object. 
* Show your actual stamina values below your stamina bar.
* Modify your inventory sizes of player inventory, chests, boats and else.
* Disable the red flash on screen when damage is received
* Display a warning message when you try to place crops to close to another crop

## Inventory
* Modify the amount of rows and columns in each inventory of the game
* Extend player inventory up to 20 x 8
* Extend chest inventory (wood up to 10 x 8, iron up to 20 x 8)
* Includes automatically added scrollbars when container sizes exceed a certain limit
* Modify the inventory behavior so that items will be placed in the first slot rather than the last available slot

## Item Durability
* Modify the item durability of each item type separately

## Item Armor Rating
* Modify the amount of armor received from armor pieces

## Gathering
* Modify the amount of resources dropped on destruction of objects.
*This includes all types of wood (inc. elderbark), stone, iron/bronze/tin/silver/copper and chitin.*
* Modify the drop chance of resources from destroyed objects.

## Game Difficulty
* Modify the game difficulty multipliers applied to health and damage of enemies based on the amount of connected players.
* Change the range of where the game considers other players to be nearby.
* Add a additional amount of Players to the player count for the difficulty calculation.
* Set the difficulty calculation to a specific player count.
* Option to disable the use of portals

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
* Modify server data rate in kilobyte
* Automatic server configuration sync when a user joins the server to sync the configuration of V+
* Change the default of 3600 seconds until items despawn on the server

## Map
* Activate shared position on map automatically
* Setting to force other people to share their position with you

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
* Remove Building Object deterioration by water.
* Advanced Building Mode
* Advanced Editing Mode
* Structural Integrity modification system
* Modify the maximum distance you can place objects at

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
2. Locate your game folder manually or start Steam client and :
   1. Right click the Valheim game in your steam library
   2. "Go to Manage" -> "Browse local files"
   3. Steam should open your game folder
3. Extract the contents of the archive into the game folder
4. Locate valheim_plus.cfg under BepInEx\config and configure the mod to your needs
   
**Please read the section about Server Config & Version Control (About Version Enforcement) below.**
   
**Please read the section about Server Config & Version Control (About Version Enforcement) below.**


## **[Server] Windows**

[Easy to setup and ready to use ValheimPlus servers can be rented here at ZAP-Hosting.com !](https://zap-hosting.com/valheimplus)

We will not explain how you create a dedicated server. This will only explain how you install the mod.

1. Download the [latest package called WindowsServer.zip over this link](https://github.com/valheimPlus/ValheimPlus/releases/latest/). *(Scroll down and click "assets")*
2. Extract the contents of the archive into your root server folder
3. Locate valheim_plus.cfg under BepInEx\config and configure the mod to your needs

**Please read the section about Server Config & Version Control (About Version Enforcement) below.**


## **[Server] Unix**

[Easy to setup and ready to use ValheimPlus servers can be rented here at ZAP-Hosting.com !](https://zap-hosting.com/valheimplus)

We will not explain how you create a dedicated server. This will only explain how you install the mod.
1. Download the [latest package called UnixServer.zip over this link](https://github.com/valheimPlus/ValheimPlus/releases/latest/). *(Scroll down and click "assets")*
2. Extract the contents of the archive into your root server folder
3. *Make sure to execute 'chmod u+x start_server_bepinex.sh' to make the start script executable*
4. *Make sure that all your uploaded files belong to the owner and group, who owns and starts the Valheim server, e.g 'chown -R steam:steam ./valheim/*'*
5. Configure your server startup parameters:
    * If you intend to run the script directly, open it and edit the variables at the top (description included in the file).
    * If you want to define the parameters elsewhere, you can pass them to start_server_bepinex.sh as you would to the valheim server executable (description included in the file). This is recommended over the first approach, as updates will overwrite the start script and you would have to configure it again.
    * If you are using [LGSM](https://linuxgsm.com), go inside your instance config file and change executable to "./start_server_bepinex.sh" to load the mod and your server arguments. More on LGSM config files can be found on [their homepage](https://docs.linuxgsm.com/configuration/linuxgsm-config).
6. *Make sure to start the server via start_server_bepinex.sh or the mod will not be loaded*

**Uses libc6-dev**

**Most server hoster do not allow you to upload script files or make your own scripts executable (for very good reasons). You will have to wait until your hoster adapts V+ for his servers.**

**Please read the section about Server Config & Version Control (About Version Enforcement) below.**


## **[Game] Unix**

1. Download the [latest package called UnixServer.zip over this link](https://github.com/valheimPlus/ValheimPlus/releases/latest/). *(Scroll down and click "assets")*
2. Locate your game folder manually or start Steam client and :
   1. Right click the Valheim game in your steam library
   2. Go to "Manage" -> "Browse local files"
   3. Steam should open your game folder
3. Extract the contents of the archive into the game folder
4. *Make sure to execute 'chmod u+x start_game_bepinex.sh' to make the start script executable*
5. Right click the Valheim game in your steam library
6. Go to "Properties..." -> "Launch options" and input "./start_game_bepinex.sh %command%"

# What if the game updates?
Game updates are unlikely to do more than partially break ValheimPlus at worst. In case you encounter any issues, use Steam's verify integrity feature- wait for it to download/update all files.
Then simply unpack the *valheim_Data* folder from the downloaded last available v+ version package into your game folder again.
This should resolve any issues related. If you continue to have issues, contact the help channel in our discord server.

# Server Config & Version Control (About Version Enforcement)
* If you have enforceMod enabled, only people with the same mod version can join your server and you can only join servers with the same mod version.

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

You can turn off and on every feature of V+ via the config file, by default all settings are turned off.

When you are hosting a server, your server configuration file overwrites the clients configuration file on connect. 

You only need to setup your server configuration file (located in the server files) when hosting a server with V+.

If you are hosting for your friends over steam, your friends will need v+ and they will receive your local settings from your game folder.


# Contributing to ValheimPlus
Please see [CONTRIBUTING.md](CONTRIBUTING.md) for details on compiling V+ for development and contributing to the project.


# Credits

* Kevin 'nx#8830' J.- https://github.com/nxPublic
* Greg 'Zedle' G. - https://github.com/zedle
* Paige 'radmint' N. - https://github.com/radmint
* TheTerrasque - https://github.com/TheTerrasque
* Bruno Vasconcelos - https://github.com/Drakeny
* GaelicGamer - https://github.com/GaelicGamer
* Doudou 'xiaodoudou' - https://github.com/xiaodoudou
* MrPurple6411#0415 - BepInEx Valheim version, AssemblyPublicizer
* Mehdi 'AccretionCD' E. - https://github.com/AccretionCD
* Zogniton - https://github.com/Zogniton - Inventiry Overhaul initial creator
