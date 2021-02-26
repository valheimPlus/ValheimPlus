// ValheimPlus

using System;
using System.Linq;
using System.Reflection;
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
            AdminOnly = true;
        }


        public override bool ParseCommand(string input)
        {
            string[] split = input.Split(' ');
            if (split.Length != 3)
            {
                // TODO: Write error message to console
                // Explanation   Setvalue Section.Property value
                return false;
            }

            string sectionAndPropertyName = split[1];
            string[] split2 = sectionAndPropertyName.Split('.');
            if (split2.Length != 2)
            {
                // Also write error message to console again
                return false;
            }

            PropertyInfo sectionProperty = typeof(Configuration).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(x => String.Equals(x.Name, split2[0], StringComparison.CurrentCultureIgnoreCase));
            if (sectionProperty == null)
            {
                // TODO: Write error message that the section does not exist
                return false;
            }

            var section = sectionProperty.GetValue(Configuration.Current, null);

            PropertyInfo valueProperty = sectionProperty.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(x => String.Equals(x.Name, split2[1], StringComparison.CurrentCultureIgnoreCase));

            if (valueProperty == null)
            {
                // TODO: Errormessage value does not exist
                return false;
            }

            // Switch for value type
            
            if (valueProperty.PropertyType == typeof(float))
            {
                float newValue = BaseConsoleCommand.GetFloat(split[2]);
                float oldValue = (float) valueProperty.GetValue(section, null);
                // TODO: Write old -> new
                valueProperty.SetValue(section, newValue, null);
                // TODO: notify for resync

                return true;
            }
            if (valueProperty.PropertyType == typeof(int))
            {
                int newValue = BaseConsoleCommand.GetInt(split[2]);
                int oldvalue = (int) valueProperty.GetValue(section, null);
                // TODO: Write old -> new
                valueProperty.SetValue(section, newValue, null);
                // TODO: notify for resync
                
                return true;
            }
            if (valueProperty.PropertyType == typeof(KeyCode))
            {
                KeyCode newValue = BaseConsoleCommand.GetKeyCode(split[2]);
                KeyCode oldValue = (KeyCode)valueProperty.GetValue(section, null);
                // TODO: Write old -> new
                valueProperty.SetValue(section, newValue, null);
                // TODO: notify for resync
                
                return true;
            }

            // If it got here, we done something weird in the configuration files
            // All types should be int,float or KeyCode

            return false;
        }
    }
}