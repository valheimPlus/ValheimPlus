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

# Credits
nx#8830 - All (as of now)
