// ValheimPlus

using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus.ConsoleCommands
{
    public class SetConfigurationValue : BaseConsoleCommand
    {
        public SetConfigurationValue()
        {
            CommandName = "SetValue";
            HelpText = "Set configuration values - SetValue";

            // TODO: Get all section names (syncables) and show list of options
            
        }


        public override bool ParseCommand(string input, bool silent)
        {
            var split = input.Split(' ');
            if (split.Length != 3)
            {
                // TODO: Write error message to console
                // Explanation   Setvalue Section.Property value
                if (!silent)
                {
                    Console.instance.AddString("Usage: SetValue <SectionName>.<ValueName> <value>");
                    Console.instance.AddString("Exmample: SetValue Kiln.productionSpeed " + 15.7f);
                }

                return false;
            }

            var sectionAndPropertyName = split[1];
            var split2 = sectionAndPropertyName.Split('.');
            if (split2.Length != 2)
            {
                // Also write error message to console again
                return false;
            }

            var sectionProperty = typeof(Configuration).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(x => string.Equals(x.Name, split2[0], StringComparison.CurrentCultureIgnoreCase));
            if (sectionProperty == null)
            {
                if (!silent)
                {
                    Console.instance.AddString($"Section '{split2[0]}' does not exist.");
                }
                return false;
            }

            var section = sectionProperty.GetValue(Configuration.Current, null);

            var needsSync = typeof(ISyncableSection).IsAssignableFrom(sectionProperty.PropertyType);

            var valueProperty = sectionProperty.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(x => string.Equals(x.Name, split2[1], StringComparison.CurrentCultureIgnoreCase));

            if (valueProperty == null)
            {
                if (!silent)
                {
                    Console.instance.AddString($"Value '{split2[1]}' does not exist in section '{split[0]}'");
                }
                return false;
            }

            // Switch for value type

            if (valueProperty.PropertyType == typeof(float))
            {
                var newValue = GetFloat(split[2]);
                var oldValue = (float)valueProperty.GetValue(section, null);


                if (!silent)
                {
                    Console.instance.AddString($"Setting {split[1]} to {newValue} (old: {oldValue})");
                }

                if (needsSync && !silent)
                {
                    ZPackage zPgk = new ZPackage();
                    zPgk.Write(input);
                    ZRoutedRpc.instance.InvokeRoutedRPC("SetConfigurationValue", zPgk);
                }
                else if ((needsSync && silent) || (!silent && !needsSync))
                {
                    valueProperty.SetValue(section, newValue, null);
                }

                return true;
            }

            if (valueProperty.PropertyType == typeof(int))
            {
                var newValue = GetInt(split[2]);
                var oldValue = (int)valueProperty.GetValue(section, null);

                if (!silent)
                {
                    Console.instance.AddString($"Setting {split[1]} to {newValue} (old: {oldValue})");
                }

                if (needsSync && !silent)
                {
                    ZPackage zPgk = new ZPackage();
                    zPgk.Write(input);
                    ZRoutedRpc.instance.InvokeRoutedRPC("SetConfigurationValue", zPgk);
                }
                else if ((needsSync && silent) || (!silent && !needsSync))
                {
                    valueProperty.SetValue(section, newValue, null);
                }

                return true;
            }

            if (valueProperty.PropertyType == typeof(KeyCode))
            {
                var newValue = GetKeyCode(split[2]);
                var oldValue = (KeyCode)valueProperty.GetValue(section, null);

                if (!silent)
                {
                    Console.instance.AddString($"Setting {split[1]} to {newValue} (old: {oldValue})");
                }

                if (needsSync && !silent)
                {
                    ZPackage zPgk = new ZPackage();
                    zPgk.Write(input);
                    ZRoutedRpc.instance.InvokeRoutedRPC("SetConfigurationValue", zPgk);
                }
                else if ((needsSync && silent) || (!silent && !needsSync))
                {
                    valueProperty.SetValue(section, newValue, null);
                }

                return true;
            }

            if (valueProperty.PropertyType == typeof(bool))
            {
                var newValue = BaseConsoleCommand.GetBool(split[2]);
                var oldValue = (bool)valueProperty.GetValue(section, null);
                if (!silent)
                {
                    Console.instance.AddString($"Setting {split[1]} to {newValue} (old: {oldValue})");
                }

                if (needsSync && !silent)
                {
                    ZPackage zPgk = new ZPackage();
                    zPgk.Write(input);
                    ZRoutedRpc.instance.InvokeRoutedRPC("SetConfigurationValue", zPgk);
                }
                else if ((needsSync && silent) || (!silent && !needsSync))
                {
                    valueProperty.SetValue(section, newValue, null);
                }

                return true;
            }

            // If it got here, we done something weird in the configuration files
            // All types should be int,float, bool or KeyCode

            return false;
        }
    }
}