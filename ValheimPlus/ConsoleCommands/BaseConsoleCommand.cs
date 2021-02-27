// ValheimPlus

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace ValheimPlus.ConsoleCommands
{
    public class BaseConsoleCommand
    {
        public string HelpText { get; internal set; }
        public string CommandName { get; internal set; }

        public virtual bool ParseCommand(ref string input, bool silent = false)
        {
            return false;
        }

        internal static List<BaseConsoleCommand> consoleCommandInstances = new List<BaseConsoleCommand>();

        public static void InitializeCommand<T>() where T : BaseConsoleCommand, new()
        {
            if (!consoleCommandInstances.Any(x => x is T))
            {
                consoleCommandInstances.Add(new T());
            }
        }

        public static bool TryExecuteCommand(ref string input, bool silent = false)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            string[] split = input.Split(' ');
            BaseConsoleCommand command = consoleCommandInstances.FirstOrDefault(x => String.Equals(x.CommandName, split[0], StringComparison.CurrentCultureIgnoreCase));

            if (command != null)
            {
                return command.ParseCommand(ref input, silent);
            }

            return false;
        }

        protected static float GetFloat(string input)
        {
            if (!float.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out var val))
            {
                return float.NaN;
            }

            return val;
        }

        protected static int GetInt(string input)
        {
            if (!int.TryParse(input, out var val))
            {
                return int.MinValue;
            }

            return val;
        }

        protected static bool GetBool(string input)
        {
            return new[] { "true", "y", "yes" }.Contains(input.ToLower());
        }

        protected static KeyCode GetKeyCode(string input)
        {
            if (System.Enum.TryParse<KeyCode>(input, out var result))
            {
                return result;
            }

            return KeyCode.None;
        }
    }

    [HarmonyPatch(typeof(Console), "InputText")]
    public static class HookConsoleInput
    {
        public static void Postfix()
        {
            string temp = Console.instance.m_input.text;

            // if help is issued, add list of our commands here
            if (string.Equals(temp.Trim(), "help", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.instance.AddString("");
                Console.instance.AddString("Valheim+ console commands:");
                foreach (var cmd in BaseConsoleCommand.consoleCommandInstances)
                {
                    Console.instance.AddString(cmd.HelpText);
                }
            }

            if (!BaseConsoleCommand.TryExecuteCommand(ref temp))
            {
                // Output something if command could not execute? not at this time.
            }
        }
    }
}