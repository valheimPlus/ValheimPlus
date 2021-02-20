using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ValheimPlus.ConsolePlus
{
    public delegate bool ConsoleCommandExecuted(string command, params object[] args);
    public class ConsolePlus
    {
        private static ConsolePlus instance;

        public event ConsoleCommandExecuted ConsoleCommandExecuted;

        private Console valheimConsole;
        private Dictionary<string, IValheimPlusCommand> commandMap;
        public ConsolePlus(Console console)
        {
            console.AddString("ConsolePlus loaded");
            valheimConsole = console;

            ConsoleCommandExecuted += OnConsoleCommandExecuted;
        }

        private bool OnConsoleCommandExecuted(string command, params object[] args)
        {
            LogFormat("Executing command: {0} with args: {1}", command, string.Join(",", args.Select(a => a.ToString())));
            return true;
        }
        private IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public void LoadCommands()
        {
            var commandTypes = from type in GetLoadableTypes(typeof(ConsolePlus).Assembly)
                               where typeof(IValheimPlusCommand).IsAssignableFrom(type) && !type.IsAbstract
                               select type;
            commandMap = commandTypes
                .Select(t => Activator.CreateInstance(t) as IValheimPlusCommand)
                .ToDictionary(command => command.CommandName, command => command);
            LogFormat("Number of commands loaded: {0}", commandMap.Count);
            foreach(var commandPair in commandMap)
            {
                LogFormat("Command {0} expects args {1}", commandPair.Key, string.Join(",", commandPair.Value.Arguments));
            }
        }
        public void LogFormat(string formatString, params object[] args)
        {
            //Nice
            Log(string.Format("[ConsolePlus]: {0}", string.Format(formatString, args)));
        }
        public void Log(object message)
        {
            string outputMessage = string.Format("[ConsolePlus]: {0}", message);
            if (valheimConsole == null)
            {
                //Server mode debugging
                Debug.Log(outputMessage);
            }
            else
            {
                valheimConsole.AddString(outputMessage);
            }
        }

        [HarmonyPatch(typeof(ZNet), "RPC_PeerInfo")]
        public static class ZNetRPC_PeerInfoPatch
        {
            private static void Postfix(ZNet __instance, ZRpc rpc, ZPackage pkg)
            {
                if (__instance.IsServer())
                {
                    __instance.RemotePrint(rpc, "Server is running ConsolePlus");
                }
            }
        }
        [HarmonyPatch(typeof(ZNet), "Awake")]
        public static class ZNetAwakePatch
        {
            private static void Prefix(ZNet __instance)
            {
                //Load commands into server as well
                if (__instance.IsServer())
                {
                    instance = new ConsolePlus(null);
                    instance.LoadCommands();
                }
            }
        }


        [HarmonyPatch(typeof(Console), "Awake")]
        public static class ConsoleAwakePatch
        {
            private static void Postfix(Console __instance)
            {
                instance = new ConsolePlus(__instance);
                instance.LoadCommands();
            }
        }

        [HarmonyPatch(typeof(Console), "InputText")]
        public static class InputTextPatch
        {
            private static bool Prefix(Console __instance)
            {
                var input = __instance.m_input.text.Split(' ');
                var command = input.FirstOrDefault();

                if (string.IsNullOrEmpty(command))
                {
                    __instance.AddString("[ConsolePlus]: Invalid command");
                    return true;
                }

                var args = input.Skip(1).ToArray();
                var didExecute = instance.ConsoleCommandExecuted?.Invoke(command, args);
                if (!didExecute.HasValue)
                    return false;
                return didExecute.HasValue && didExecute.Value;
            }
        }
    }
}
