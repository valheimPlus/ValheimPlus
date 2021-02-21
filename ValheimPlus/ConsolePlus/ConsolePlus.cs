using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ValheimPlus.ConsolePlus
{ 
    public class ConsolePlus
    {
        private static ConsolePlus instance;

        public static string ExecuteCommandRPC => "ExecuteCommand";

        private Console valheimConsole;
        private Dictionary<string, IValheimPlusCommand> commandMap;
        public ConsolePlus(Console console)
        {
            valheimConsole = console;
        }

        public void ConsoleCommandExecuted(string command, params object[] args)
        {
            if(command.Equals("commands"))
            {
                foreach(var loadedCommand in commandMap.Values)
                {
                    LogFormat("{0} - {1} ({2})", loadedCommand.CommandName, loadedCommand.Description, loadedCommand.Arguments);
                }
            }
            else if (commandMap.TryGetValue(command, out var consoleCommand))
            {
                consoleCommand.Execute(args);
            }
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

            foreach (var command in commandMap.Values)
            {
                LogFormat("Loaded command: {0}", command.CommandName);
                command.Load();
            }
            LogFormat("Number of commands loaded: {0}", commandMap.Count);
        }
        private bool IsCommandAllowed(string commandName)
        {
            return !Settings.getString("ConsolePlus", "deniedCommands").Contains(commandName);
        }
        public void RPC_ExecuteCommand(ZRpc rpc, string commandBase64)
        {
            var client = rpc.GetSocket().GetHostName();
            ZPackage pkg = new ZPackage(commandBase64);
            var command = pkg.ReadString();
            LogFormat("Client: {0} is attempting to execute command: {1}", client, command);
            if (commandMap.TryGetValue(command, out var consoleCommand) && IsCommandAllowed(command))
            {
                int numParams = pkg.ReadInt();
                var commandParams = new string[numParams];
                for (int i = 0; i < numParams; i++)
                {
                    commandParams[i] = pkg.ReadString();
                }
                LogFormat("Executing command {0} with params {1}", command, string.Join(",", commandParams.Select(p => p.ToString())));
                consoleCommand.Execute(rpc.GetSocket().GetHostName(), commandParams);
            }
            else
            {
                ZNet.instance.RemotePrint(rpc, "Server does not undestand command: " + command);
            }
        }

        public static void LogFormat(string formatString, params object[] args)
        {
            Log(string.Format(formatString, args));
        }
        public static void Log(object message)
        {
            string outputMessage = string.Format("[ConsolePlus]: {0}", message);
            if (instance.valheimConsole == null)
            {
                //Server mode debugging
                Debug.Log(outputMessage);
            }
            else
            {
                instance.valheimConsole.AddString(outputMessage);
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
                    rpc.Register<string>(ExecuteCommandRPC, instance.RPC_ExecuteCommand);
                }
            }
        }
        [HarmonyPatch(typeof(ZNet), "Awake")]
        public static class ZNetAwakePatch
        {
            private static void Postfix(ZNet __instance)
            {
                //Load commands into server as well
                if (__instance.IsServer() && Settings.getBool("ConsolePlus", "serverAllowCommands"))
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
            private static void Postfix(Console __instance)
            {
                var input = __instance.m_input.text.Split(' ');
                var command = input.FirstOrDefault();

                if(input.Equals("help"))
                {
                    Log("Type commands for a list of all ConsolePlus commands");
                }

                var args = input.Skip(1).ToArray();
                instance.ConsoleCommandExecuted(command, args);
            }
        }
    }
}
