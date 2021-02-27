// ValheimPlus

using System;
using System.Collections.Generic;
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


        public override bool ParseCommand(ref string input, bool silent)
        {
            string inputCopy = input;
            List<string> parts = input.Replace("  "," ").Split(' ').ToList();
            string command = parts.Count >= 1 ? parts[0] : null;
            string sectionPropPart = parts.Count >= 2 ? parts[1] : null;
            string valuePart = parts.Count == 3 ? parts[2] : null;

            List<string> configParts = new List<string>();
            string sectionName = null;
            string valueName = null;
            if (!string.IsNullOrEmpty(sectionPropPart))
            {
                configParts.AddRange(sectionPropPart.Split('.'));
                sectionName = configParts.Count >= 1 ? configParts[0] : null;
                valueName = configParts.Count == 2 ? configParts[1] : null;
            }

            // Set input to nothing, so it won't be added again after our messages
            input = "";
            if (string.IsNullOrEmpty(valueName) && string.Equals(sectionName, "help", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!silent)
                {
                    Console.instance.AddString("Usage: SetValue <SectionName>.<ValueName> <value>");
                    Console.instance.AddString("Example to set a value: SetValue Kiln.productionSpeed 15.7");
                    Console.instance.AddString("To get a list of sections: SetValue");
                    Console.instance.AddString("To get a list of values of a section: SetValue <SectionName>");
                    Console.instance.AddString("Info about a value: SetValue <SectionName>.<ValueName>");
                }

                return false;
            }

            if (string.IsNullOrEmpty(sectionName))
            {
                Console.instance.AddString("List of configuration sections:");
                foreach (var property in typeof(Configuration).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    bool syncable = typeof(ISyncableSection).IsAssignableFrom(property.PropertyType);
                    Console.instance.AddString($"{property.Name} \t\t{(syncable ? "Admin only" : "")}");
                }
            }


            if (!string.IsNullOrEmpty(sectionName) && string.IsNullOrEmpty(valuePart))
            {

                if (string.IsNullOrEmpty(valueName))
                {
                    // write info about section

                    var sectProperty = typeof(Configuration).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .FirstOrDefault(x => string.Equals(x.Name, sectionName, StringComparison.CurrentCultureIgnoreCase));
                    if (sectProperty == null)
                    {
                        Console.instance.AddString($"No section {sectionName} available.");
                        return false;
                    }
                    Console.instance.AddString($"Section {sectProperty.Name}");
                    Console.instance.AddString("Properties:");

                    var sectValue = sectProperty.GetValue(Configuration.Current, null);

                    foreach (var prop in sectProperty.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.Name != "NeedsServerSync"))
                    {
                        var val = prop.GetValue(sectValue, null);
                        Console.instance.AddString($"{prop.Name} {prop.PropertyType.Name} ({val})");
                    }
                }
                else 
                {
                    var sectProperty = typeof(Configuration).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .FirstOrDefault(x => string.Equals(x.Name, sectionName, StringComparison.CurrentCultureIgnoreCase));
                    var valueProp = sectProperty.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .FirstOrDefault(x => string.Equals(x.Name, valueName, StringComparison.CurrentCultureIgnoreCase));
                    ConfigurationAttribute ca = (ConfigurationAttribute)valueProp.GetCustomAttributes(false).FirstOrDefault(x => x is ConfigurationAttribute);
                    string valueComment = "";
                    if (ca != null)
                    {
                        valueComment = ca.Comment;
                    }
                    Console.instance.AddString($"Section {sectProperty.Name} value {valueProp.Name}, type {valueProp.PropertyType.Name}");

                    if (!string.IsNullOrEmpty(valueComment))
                    {
                        Console.instance.AddString(valueComment);
                    }

                }

                return false;
            }

            if (parts.Count != 3)
            {
                // TODO: Write error message to console
                // Explanation   Setvalue Section.Property value
                if (!silent)
                {
                    Console.instance.AddString("Usage: SetValue <SectionName>.<ValueName> <value>");
                    Console.instance.AddString("Example: SetValue Kiln.productionSpeed 15.7");
                }

                return false;
            }


            var sectionProperty = typeof(Configuration).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(x => string.Equals(x.Name, sectionName, StringComparison.CurrentCultureIgnoreCase));
            if (sectionProperty == null)
            {
                if (!silent)
                {
                    Console.instance.AddString($"Section '{sectionName}' does not exist.");
                }
                return false;
            }

            var section = sectionProperty.GetValue(Configuration.Current, null);

            var needsSync = typeof(ISyncableSection).IsAssignableFrom(sectionProperty.PropertyType);

            var valueProperty = sectionProperty.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(x => string.Equals(x.Name, valueName, StringComparison.CurrentCultureIgnoreCase));

            if (valueProperty == null)
            {
                if (!silent)
                {
                    Console.instance.AddString($"Value '{valueName}' does not exist in section '{sectionName}'");
                }
                return false;
            }

            // Switch for value type
            if (valueProperty.PropertyType == typeof(float))
            {
                var newValue = GetFloat(valuePart);
                var oldValue = (float)valueProperty.GetValue(section, null);


                if (!silent)
                {
                    Console.instance.AddString($"Setting {sectionName}.{valueName} to {newValue} (old: {oldValue})");
                }

                if (needsSync && !silent)
                {
                    SyncToClients(inputCopy);
                }
                else if ((needsSync && silent) || (!silent && !needsSync))
                {
                    valueProperty.SetValue(section, newValue, null);
                }

                return true;
            }

            if (valueProperty.PropertyType == typeof(int))
            {
                var newValue = GetInt(valuePart);
                var oldValue = (int)valueProperty.GetValue(section, null);

                if (!silent)
                {
                    Console.instance.AddString($"Setting {sectionName}.{valueName} to {newValue} (old: {oldValue})");
                }

                if (needsSync && !silent)
                {
                    SyncToClients(inputCopy);
                }
                else if ((needsSync && silent) || (!silent && !needsSync))
                {
                    valueProperty.SetValue(section, newValue, null);
                }

                return true;
            }

            if (valueProperty.PropertyType == typeof(KeyCode))
            {
                var newValue = GetKeyCode(valuePart);
                var oldValue = (KeyCode)valueProperty.GetValue(section, null);

                if (!silent)
                {
                    Console.instance.AddString($"Setting {sectionName}.{valueName} to {newValue} (old: {oldValue})");
                }

                if (needsSync && !silent)
                {
                    SyncToClients(inputCopy);
                }
                else if ((needsSync && silent) || (!silent && !needsSync))
                {
                    valueProperty.SetValue(section, newValue, null);
                }

                return true;
            }

            if (valueProperty.PropertyType == typeof(bool))
            {
                var newValue = BaseConsoleCommand.GetBool(valuePart);
                var oldValue = (bool)valueProperty.GetValue(section, null);
                if (!silent)
                {
                    Console.instance.AddString($"Setting {sectionName}.{valueName} to {newValue} (old: {oldValue})");
                }

                if (needsSync && !silent)
                {
                    SyncToClients(inputCopy);
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

        private static void SyncToClients(string inputCopy)
        {
            ZPackage zPgk = new ZPackage();
            zPgk.Write(inputCopy);
            ZRoutedRpc.instance.InvokeRoutedRPC("SetConfigurationValue", zPgk);
        }
    }
}