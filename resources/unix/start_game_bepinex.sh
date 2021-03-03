#!/bin/sh
# BepInEx running script
#
# HOW TO USE:
# 1. Make this script executable with `chmod u+x ./start_game_bepinex.sh`
# 2. In Steam, go to game's preferences and change game's launch args to:
#    ./start_game_bepinex.sh %command%
# 3. Start the game via Steam
#
# Note that you won't get a console this way
#
# NOTE: Edit these values only if you know what you're doing!
#
#
# args:
# ARG1 - full path to the game executable to run. Defaults to valheim.x86_64
# ARG... - additional args to pass to the executable

exec="$PWD/valheim.x86_64"
if [ -n "$1" ]; then
    exec="$1"
fi

export DOORSTOP_ENABLE=TRUE
export DOORSTOP_INVOKE_DLL_PATH="$PWD/BepInEx/core/BepInEx.Preloader.dll"
export DOORSTOP_CORLIB_OVERRIDE_PATH="$PWD/unstripped_corlib"

tmplibpath=$LD_LIBRARY_PATH

export LD_LIBRARY_PATH="$PWD/doorstop_libs":$LD_LIBRARY_PATH
export LD_PRELOAD=libdoorstop_x64.so:$LD_PRELOAD

# Run the main executable
# Pass through any additional args
"$exec" ${@:2}

export LD_LIBRARY_PATH=$tmplibpath