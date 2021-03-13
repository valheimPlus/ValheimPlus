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
# NOTE: Edit the script only if you know what you're doing!

# Resolve base directory relative to this script
# Hopefully this resolves relative paths and links
a="/$0"; a=${a%/*}; a=${a#/}; a=${a:-.}; BASEDIR=$(cd "$a"; pwd -P)

exec="$BASEDIR/valheim.x86_64"

export DOORSTOP_ENABLE=TRUE
export DOORSTOP_INVOKE_DLL_PATH="$BASEDIR/BepInEx/core/BepInEx.Preloader.dll"
export DOORSTOP_CORLIB_OVERRIDE_PATH="$BASEDIR/unstripped_corlib"

# Allow to specify --doorstop-enable true|false
# Everything else is passed as-is to `exec`
while :; do
    case $1 in
        --doorstop-enable)
            if [ -n "$2" ]; then
                export DOORSTOP_ENABLE=$(echo "$2" | tr a-z A-Z)
                shift
            else
                echo "No --doorstop-enable value specified, using default!"
            fi
            ;;
        --doorstop-target)
            if [ -n "$2" ]; then
                export DOORSTOP_INVOKE_DLL_PATH="$2"
                shift
            else
                echo "No --doorstop-target value specified, using default!"
            fi
            ;;
        --doorstop-dll-search-override)
            if [ -n "$2" ]; then
                export DOORSTOP_CORLIB_OVERRIDE_PATH="$2"
            else
                echo "No --doorstop-dll-search-override value specified, using default!"
            fi
            ;;
        *)
            if [ -z "$1" ]; then
                break
            fi
            if [ -z "$launch" ]; then
                launch="$1"
            else
                rest="$rest $1"
            fi
            ;;
    esac
    shift
done


export LD_LIBRARY_PATH="$BASEDIR/doorstop_libs:$LD_LIBRARY_PATH"
export LD_PRELOAD="libdoorstop_x64.so:$LD_PRELOAD"


# Run the main executable
# Don't quote here since $exec may contain args passed by Steam
if [ -n "$launch" ]; then
    exec "$launch" $rest
else
    exec "$exec"
fi
