// ValheimPlus

using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace ValheimPlus.ConsoleCommands
{
    public class BaseConsoleCommand
    {
        public string HelpText { get; internal set; }
        public string CommandName { get; internal set; }

        private static bool isAdmin = false;

        public virtual bool ParseCommand(string input, bool silent = false)
        {
            return false;
        }

        private static List<BaseConsoleCommand> consoleCommandInstances = new List<BaseConsoleCommand>();

        public static void InitializeCommand<T>() where T : BaseConsoleCommand, new()
        {
            if (!consoleCommandInstances.Any(x => x is T))
            {
                consoleCommandInstances.Add(new T());
            }
        }

        public static bool TryExecuteCommand(string input, bool silent = false)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            string[] split = input.Split(' ');
            BaseConsoleCommand command = consoleCommandInstances.FirstOrDefault(x => String.Equals(x.CommandName, split[0], StringComparison.CurrentCultureIgnoreCase));

            if (command != null)
            {
                return command.ParseCommand(input, silent);
            }

            return false;
        }

        protected static float GetFloat(string input)
        {
            if (!float.TryParse(input, out var val))
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
        public static bool Prefix()
        {
            Console.instance.AddString("Hook in place!");
            if (BaseConsoleCommand.TryExecuteCommand(Console.instance.m_input.text))
            {
                return false;
            }

            return true;
        }
    }

}