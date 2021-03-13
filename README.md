![ValheimPlus Logo](https://raw.githubusercontent.com/nxPublic/ValheimPlus/master/logo.png)

# ValheimPlus
A HarmonyX Mod aimed at improving the gameplay quality of Valheim. The mod includes several different main features that allow users to modify the stats of players, buildings and entities. V+ also offers players the ability to build and place objects with very high precision through a sophisticated system, as well as tweaking and modifying already placed objects with equal precision. The goal is to provide V+ as a base modification for Valheim to increase quality of life, tweak the game's difficulty, and in general, improve the player's experience. V+ also comes with a version and configuration control system for servers and users, enabling server owners to ensure that only players with the same configuration are able to join the server.

# ValheimPlus Server Hosting
Below you can find a list of hosting companies that are supporting ValheimPlus with very competitive pricing and ease of use.

[![GPortal](https://imgur.com/L46GT7q.png)](https://www.g-portal.com/valheim)
[![ZapHosting](https://i.imgur.com/4ZB1xHU.jpg)](https://zap-hosting.com/valheimplus)
[![GFXHosting](https://www.gtxgaming.co.uk/wp-content/uploads/2021/02/valheim_plus_banner-3.png)](https://www.gtxgaming.co.uk/clientarea/aff.php?aff=2096)





### All features can be enabled and tweaked through the V+ config file.



# Player

### Gameplay
* Modify stamina consumption and regeneration.
* Modify stamina consumption when using tools and weaponry.
* Modify food duration.
* Disable food degradation over time (maintain full benefit for the whole duration).
* Modify carry load.
* Modify auto-pickup range.
* Modify unarmed damage.
* Modify base amount of unarmed damage that is multiplied by your unarmed skill level.
* Modify each skill's experience gain separately by percent.
* Remove screen shakes.
* Tweak/disable death penalty.
* Tweak Rested bonus duration per comfort level.
* Disable the use of portals.
* Modify the Guardian buff duration and cooldown


### Player Hud
* Show skill experience gains in the top left corner.
* Show amount of items in the player's inventory when crafting or building an object.
* Show numerical stamina value below stamina bar.
* Disable red screen flash on receiving damage.
* Display a warning message when attempting to place different crops too close to each other.
* Added hotkey options for forward and backward roll.

**Map**
* Force all players in the server to display their map position.
* Allow all players to share all map exploration with every other player in the server, as long as their map position is displayed (currently players only receive map exploration from other players while they are online).

**Camera**
* Change the player's Field of View.
* Change the maximum zoom-out distance.
* Change the maximum zoom-out distance when on a boat.
* Switch between first person and third person on button press.
* Hotkeys for changing FOV in first person.

**Gathering**
* Modify the amount of resources dropped on destruction of objects (this includes chitin, stone, all types of wood, and minerals).
* Modify the drop chance of resources from destroyed objects.

**Wagon**
* Modify the weight contribution of items placed inside a wagon.
* Modify the base weight of the wagon.

**Fire sources**
* Fire sources retain max fuel once the fuel is added.
* Alternatively, only torches retain max fuel, while other fire sources will continue to consume it.

**Game Difficulty**
* Modify the difficulty multipliers applied to health and damage of enemies based on the number of connected players.
* Modify the range at which the game considers other players to be nearby.
* Add a number of players to the player count for the purpose of difficulty calculation.
* Set the difficulty calculation to a specific player count.


### Inventory
* Modify the inventory behavior so that items will be placed in the first slot rather than the last available slot.
* Modify the size of player inventory and all types of containers, including vehicles:
  * Player inventory can be extended up to 20 x 8.
  * Wood chest inventory can be extended up to 10 x 8.
  * Iron chest inventory can be extended up to 20 x 8.
  * Personal chest inventory can be extended up to 20 x 8.
  * Cart/Wagon chest inventory can be extended up to 30 x 8.
  * Karve chest inventory can be extended up to 30 x 8.
  * Longboat chest inventory can be extended up to 30 x 8.


**Note: As of 0.9.4, player inventory slot configuration is not compatible with Equipment and Quick slots mod**


### Items
* Modify the durability of each item type separately.
* Modify the amount of armor granted by armor pieces.
* Modify the amount of damage blocked by all shields.
* Remove teleport prevention from all items.
* Reduce the weight of all items by percent.
* Modify maximum item stack size by percent.



# Crafting and Production

### Workbench
* Modify Workbench radius.
* Modify the radius at which Workbench attachments can be placed.
* Allow crafting stations to automatically repair all appropriate items in the player's inventory on interaction.

### Kiln and Furnace
* Modify Kiln and Furnace processing speed.
* Modify kiln and Furnace maximum capacity.
* Allow items produced by Kiln and Furnace to be automatically placed in the closest container.

### Beehive 
* Modify Beehive honey production speed.
* Modify Beehive capacity.
* Display time left until next production.
* Allow items produced by Beehive to be automatically placed in the closest container.

### Fermenter
* Modify Fermenter speed.
* Modify Fermenter output amount.
* Display time left until next production.


# Building
* Disable "Invalid Placement" restriction while building.
* Disable deterioration of placeables from weather exposure. 
* Disable deterioration of placeables from water exposure.
* Added a free rotation mode for the default Building Mode.
* Added Advanced Building Mode.
* Added Advanced Editing Mode.
* Modify the structural integrity of placeables.
* Modify the maximum distance you can place objects at.
* Added the ability for the hammer tool to repair all placeables in an area instead of just the targeted placeable.
* Added option for placeables destroyed/dismantled by players to always drop their full material cost, even if built by another player.
* Modify effective radius of comfort placeables.
* Modify Ward structure protection radius.


### Free Rotation Mode for the default Building Mode
* **Video demo: https://imgur.com/xMH7STj.mp4**
* This modifies the default build mode. How it works (all mentioned hotkeys can be modified):
   * Players can rotate the object selected in any direction while in the usual building mode by pressing certain hotkeys. The location of the object can be manipulated with the mouse:
      * ScrollWheel + LeftAlt to rotate by 1 degree on the Y-axis.
      * ScrollWheel + C to rotate by 1 degree on the X-axis.
      * ScrollWheel + Z to rotate by 1 degree on the Z-axis.
   * Use the copy rotation hotkeys to copy the current rotation or apply the same rotation to the next object that is being built.
   * Build the object by clicking.

### Advanced Building Mode
* **Video demo: https://i.imgur.com/ddQCzPy.mp4**
* How it works (all mentioned hotkeys can be modified):
   * Players can freeze the item by pressing the configured key (F1 by default).
   * Players can modify the item position and rotation with the following key combinations:
      * Arrow Up/Down/Left/Right to move the building object in the respective direction.
      * Arrow Up/Down + Control to move the building object up and down.
      * ScrollWheel to rotate the building object on the Y-axis.
      * ScrollWheel + Control to rotate the building object on the X-axis.
      * ScrollWheel + left Alt to rotate the building object on the Z-axis.
      * Numpad plus/minus to either increase or decrease speed, holding SHIFT to raise/lower by 10 instead of 1 (Pressing Shift at any moment in time increases the distance/rotation angle 3 times)
   * Build the object by clicking.
  
**NOTE:**
* *Objects built with this system are not excempt from the structure/support system. Dungeons and other no-build areas are still restricted.*

### Advanced Editing Mode
* **Video demo: https://imgur.com/DMb4ZUv.mp4**
* You cannot be in Build mode (hammer, hoe or terrain tool). How it works: 
   * Players can select the item with the configured key (Numpad0 is default).
   * Players can modify the item position and rotation with the following key combinations:
      * Arrow Up/Down/Left/Right to move the building object in the respective direction.
      * Arrow Up/Down + Control to move the building object up and down.
      * ScrollWheel = rotates the building object on the Y-axis.
      * ScrollWheel + Control to rotate the building object on the X-axis.
      * ScrollWheel + left Alt to rotate the building object on the Z-axis.
      * resetAdvancedEditingMode HotKey resets the position and rotation to the initial values.
      * Numpad plus/minus to either increase or decrease speed, holding SHIFT to raise/lower by 10 instead of 1 (Pressing Shift at any moment in time increases the distance/rotation angle 3 times)
   * Press the confirmPlacementOfAdvancedEditingMode Hotkey to confirm the changes. (press abortAndExitAdvancedEditingMode HotKey to abort editing mode and reset the object).

**NOTE:**
* *Other players will not be able to see the item being moved until the player building the item confirms the placement. Dungeons and other no-build areas are still restricted.* 

### Structural Integrity
* Apply a modifier to the structural integrity of the following materials:
  * Wood
  * Stone
  * Iron
  * Hardwood
* Disable structural integrity entirely (this will cause objects placed mid-air to not break and fall).
* Make anything built by players immune to all damage.
* Make boats invincible to all damage.



# Server
* Remove password requirement for the server.
* Modify the maximum amount of players on a server.
* Modify server data rate in kilobytes.
* Modify number of seconds it takes for items to despawn after being dropped on the ground. (default is 3600 seconds).
   * *Note: Items on ground will retain base game functionality which ensures that drops don't disappear if a player is nearby or there is a "player base" nearby*
* Automatically sync V+ configuration of players joining a server to match the server's configuration.


### Time and Day Manipulation
*This feature is currently disabled*
* Modify total time of day/night cycle.
* Modify speed of time passing at night.


# Installation Instructions
We supply 4 different versions of V+ with every release since version 0.8. You can find detailed instructions on how to install these variants below.

**ATTENTION FOR MULTIPLAYER**: Both the game and the server should have this mod installed to prevent all kinds of different issues. If you have the mod installed and then have friends join over steam they should have the mod as well.


## **Windows**
### Game(Steam)

1. Download the [latest package called WindowsClient.zip over this link](https://github.com/nxPublic/ValheimPlus/releases/latest/). *(Scroll down and click "assets")*
2. Locate your game folder manually or start Steam client and right-click the Valheim game in your Steam library and select Manage -> browse local files for Steam to open your game folder. 
3. Extract the contents of the .zip file into the game folder.
4. Open valheim_plus.cfg under BepInEx\config with any text editor and configure the mod to your needs.
   
**Please read the section about Server Config & Version Control (About Version Enforcement) below.**


### **Server**

[Easy to set up and ready to use ValheimPlus servers can be rented here at ZAP-Hosting.com!](https://zap-hosting.com/valheimplus)

This guide does not cover how to create a dedicated server. These are the steps to install the mod:

1. Download the [latest package called WindowsServer.zip over this link](https://github.com/valheimPlus/ValheimPlus/releases/latest/). *(Scroll down and click "assets")*
2. Extract the contents of the .zip file into your root server folder.
3. Open valheim_plus.cfg under BepInEx\config with any text editor and configure the mod to your needs.

**Please read the section about Server Config & Version Control (About Version Enforcement) below.**

## **Linux**

### DIY Valheim and Valheim+ Dedicated Server Installer:
The Njord installer provided by ZeroBandwidth and team allows for you to easily set up your own Dedicated Valheim Server built on Ubuntu Linux. This menu system gives you the option to play Valheim in vanilla mode or with the all modding power of Valheim+.
* All Njord Menu related questions and support should be directed to his community [Njord Menu Support](https://github.com/Nimdy/Dedicated_Valheim_Server_Script/issues)
* [Njord Menu Github Page](https://github.com/Nimdy/Dedicated_Valheim_Server_Script) 

### **Game[Unix]**

1. Download the [latest package called UnixServer.zip over this link](https://github.com/valheimPlus/ValheimPlus/releases/latest/). *(Scroll down and click "assets")*
2. Locate your game folder manually or start Steam client and right-click the Valheim game in your Steam library and select Manage -> browse local files for Steam to open your game folder. 
3. Extract the contents of the archive into the game folder.
4. Make sure to run 'chmod u+x start_game_bepinex.sh' to make the start script executable.
5. Right-click the Valheim game in your Steam library.
6. Go to "Properties..." -> "Launch options" and input "./start_game_bepinex.sh %command%".


### **Server[Unix]**

[Easy to set up and ready to use ValheimPlus servers can be rented here at ZAP-Hosting.com !](https://zap-hosting.com/valheimplus)

This guide does not cover how to create a dedicated server. These are the steps to install the mod:

1. Download the [latest package called UnixServer.zip over this link](https://github.com/valheimPlus/ValheimPlus/releases/latest/). *(Scroll down and click "assets")*
2. Extract the contents of the .zip file into your root server folder.
3. Make sure to run 'chmod u+x start_server_bepinex.sh' to make the start script executable.
4. Make sure that all uploaded files belong to the owner and group who owns and starts the Valheim server, e.g 'chown -R steam:steam ./valheim/
5. Configure server startup parameters:
    * If you intend to run the script directly, open it and edit the variables at the top (description included in the file).
    * If you want to define the parameters elsewhere, you can pass them to start_server_bepinex.sh as you would to the valheim server executable (description included in the file). This is recommended over the first approach, as updates will overwrite the start script and you would have to configure it again.
    * If you are using [LGSM](https://linuxgsm.com), go inside your instance config file and change executable to "./start_server_bepinex.sh" to load the mod and your server arguments. More on LGSM config files can be found on [their homepage](https://docs.linuxgsm.com/configuration/linuxgsm-config).
6. Make sure to start the server via start_server_bepinex.sh, else the mod will not be loaded.

**Uses libc6-dev**

**Most server hosters do not allow you to upload script files or make your own scripts executable (for very good reasons). You will have to wait until your hoster adapts V+ for his servers.**

**Please read the section about Server Config & Version Control (About Version Enforcement) below.**


# What if the game updates?
Game updates are unlikely to do more than partially break ValheimPlus at worst. In case you encounter any issues, use Steam's verify integrity feature and wait for it to download/update all files.
Then simply unpack the *valheim_Data* folder from the downloaded last available v+ version package into your game folder again.
This should resolve any issues related. If you continue to have issues, contact the help channel in our discord server.

# Server Config & Version Control (About Version Enforcement)
* If you have enforceMod enabled, only players with the same mod version can join your server and you can only join servers with the same mod version.

**This system is working reliably and is issue-free. Any issues encountered are likely derived from a faulty configuration set up, or the server/client not running v+.**

# Join the Discord
We have several different channels including a showcase channel and alpha testing system, allowing players to always get the newest versions available to test out.

![ValheimPlus Icon](https://raw.githubusercontent.com/nxPublic/ValheimPlus/master/ico.png)
https://discord.gg/AmH6Va97GT

# Twitter
Is used to post about releases of ValheimPlus and to potentially reply to issues.
https://twitter.com/ValheimPlus

# Support on Patreon
Supporting this Project on Patreon will allow me to dedicate more of my free time to this project. It will also pay for server costs of our new domains and our upcoming discord bot.

https://www.patreon.com/valheimPlus


# Configuration File

The Config file name is supposed to be "valheim_plus.cfg" and it needs to be placed in "BepInEx\config".

You can turn off and on every feature of V+ via the config file, by default all settings are turned off.

When hosting a server, the server configuration file overwrites the client's configuration file on connect. 

Only the server configuration file (located in the server files) needs to be set up when hosting a server with V+.

When hosting for other players over steam, every player will need v+ and they will receive the local settings from the host's game folder.


# Contributing to ValheimPlus
Please see [CONTRIBUTING.md](/CONTRIBUTING.md) for details on compiling V+ for development and contributing to the project.


# Official Development Team

* Kevin 'nx#8830' J.- https://github.com/nxPublic
* Greg 'Zedle' G. - https://github.com/zedle
* Paige 'radmint' N. - https://github.com/radmint
* Miguel 'Mixone' T. - https://github.com/Mixone-FinallyHere
* Chris 'Xenofell' S. - https://github.com/cstamford

# Credits
* TheTerrasque - https://github.com/TheTerrasque
* Bruno Vasconcelos - https://github.com/Drakeny
* GaelicGamer - https://github.com/GaelicGamer
* Doudou 'xiaodoudou' - https://github.com/xiaodoudou
* MrPurple6411#0415 - BepInEx Valheim version, AssemblyPublicizer
* Mehdi 'AccretionCD' E. - https://github.com/AccretionCD
* Zogniton - https://github.com/Zogniton - Inventory Overhaul initial creator
* Jules - https://github.com/sirskunkalot
