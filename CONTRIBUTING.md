# Contributing to ValheimPlus

## How to setup your environment for V+ development
How to setup the development enviroment to compile ValheimPlus yourself.

1. Download the [BepInEx for Valheim package](https://valheim.thunderstore.io/package/download/denikson/BepInExPack_Valheim/5.4.701/).
   - Unpack into your Valheim root folder and overwrite every file when asked.
   - This package sets up your Valheim game with BepInEx configurations specifically for mod devs. Created by [BepInEx](https://github.com/BepInEx).
1. Download the [AssemblyPublicizer package](https://mega.nz/file/oQxEjCJI#_XPXEjwLfv9zpcF2HRakYzepMwaUXflA9txxhx4tACA).
   - This package has a tool that you'll use to create publicized versions of the `assembly_*.dll` files for your local development.
   - Repo: https://github.com/MrPurple6411/Bepinex-Tools/releases/tag/1.0.0-Publicizer by [MrPurple6411](https://github.com/MrPurple6411).
1. Drag and drop all `assembly_*.dll` files from "\Valheim\valheim_Data\Managed\" folder onto "AssemblyPublicizer.exe". This will create a new folder called "/publicized_assemblies/".
1. Add all `.dll`s (Except `mscorlib.dll`, `System.Configuration.dll`, `System.Core.dll`, `System.dll`, and `System.Xml.dll`) of "\Valheim\valheim_Data\Managed\" and "publicized_assemblies" folder as references to the project.
1. Add all BepInEx `.dll`s (Except `0Harmony.dll` and `0Harmony20.dll`) as references to the project.
1. Use Nuget to add "Ini-parser" and "HarmonyX" references.
1. Define Enviroment Variable `VALHEIM_INSTALL` with path to Valheim Install Directory  
   - example: `setx VALHEIM_INSTALL "C:\Program Files\Steam\steamapps\common\Valheim" /M`

## V+ Conventions
### C#
1. Please add all `Patch`ed methods to the file named for the type being patched. So if we patch a class type named `Humanoid`, add that patch to the `GameClasses/Humanoid.cs` file.
1. Patch naming convention update
   - Any new classes for patches will follow the following naming convention. Please make sure future classes are named appropriately. The objective is to simplify and avoid clashes of method patching and a potential refactor in the future. 

   ```csharp
   File of patch == GameClass.cs file
 
   Class structure:
   [HarmonayPatch(typeof(GameClass), "MethodName")]
   {modifiers} class GameClass_MethodName_Patch
   {
     bool Prefix...
     void Postfix...
     // Etc...
   }
   ```
   - If you are planning to do a Transpiler, replace "Patch" with "Transpiler"
1. Please add a `///<summary>` above every patched method and add comments to parts of code that is more that just a simple assignment
1. Try to avoid using "magic strings/numbers" if possible, use class-defined `const`s especially if using the value more than once
1. Always try avoid returning `false` in a `Prefix()` method if possible. This will not only skip the original method but also other `Prefix`s and breaks compatibility with other mods. If you need to return `false` please include your reasoning as a comment in the code.

### Configuration .cfg
1. Always add new configurations to the .cfg file as well as the configuration class and set the value to the default value from the game.
   - It is easy to forget test values in your code when you make a PR. Please double check your default values when creating a PR
   - It is recommended when testing to test value inline with your code instead of altering the values on the .cfg/class to prevent this issue. If an inline hardcoded value gets pushed, it's easier to spot this mistake than a inaccurate cfg setting.
1. Try to find an appropriate existing configuration section before adding a new one. Generally, if your configuration changes something related to the player, for example, then add it to the `[Player]` section. Look at it from the perspective of a user who's looking for a specific configuration instead of the perspective of someone who's coding and looking for a class.
1. Check out our helpers in the /Utilities/ folder and use them where appropriate
1. For configuration modifiers that add a modifcation to the base value, always use a percentage configuration and use the `applyModifier()` utility.

## Making a Pull Request
1. Only make a pull request for finished work. Otherwise, if we pull the work down to test it and it doesn't work, we don't know if it's because it's unfinished or if there's an unintentional bug.
   - If you'd like a review on your work before something it's finished, send us a link to a compare via Discord or make a "Draft" PR.
1. If you want credit, add your credit to the `README.md` in your pull request if the work is more than a documentation update. We will not be able to track this ourselves and rely on you to add your preferred way of being credited.
1. After you have made a GitHub contribution, reach out to one of the V+ devs on Discord if you'd like the "GitHub contributor" role.

## Pull Request labels
1. pending merge - This work has been reviewed and accepted and will be merged after the coming release
1. pending close - This work has not been updated for a while and will be closed if no response/update is received in 2 days
1. needs updating - This work has received feedback and needs to be updated before being accepted
1. merge conflicts - This pr has merge conflicts that need to be resolved
1. question - The author of the PR needs to answer a question before the PR can move forward
