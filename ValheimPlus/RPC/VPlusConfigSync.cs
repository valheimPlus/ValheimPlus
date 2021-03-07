using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IniParser.Model;
using ValheimPlus.Configurations;

namespace ValheimPlus.RPC
{
    public class VPlusConfigSync
    {
        public static void RPC_VPlusConfigSync(long sender, ZPackage configPkg)
        {
            if (ZNet.m_isServer) //Server
            {
                if (!Configuration.Current.Server.IsEnabled) return;
                if (!Configuration.Current.Server.serverSyncsConfig) return;

                ZPackage pkg = new ZPackage();
                List<string> cleanConfigData = new List<string>();
                // Get stored config
                IniData configdata = Configuration.Current.ConfigData;

                foreach (PropertyInfo prop in typeof(Configuration).GetProperties())
                {
                    var keyName = prop.Name;
                    // If current Configuration properties is a ServerSyncConfig, HasNeedsServerSync through TryGetBoolMethod will return 1
                    int hasNeedsServerSync = Helper.TryGetBoolMethod(prop, Configuration.Current, "HasNeedsServerSync");
                    Dictionary<string, Type> propertiesType = new Dictionary<string, Type>();
                    if (hasNeedsServerSync > 0)
                    {
                        cleanConfigData.Add($"[{keyName}]");
                        if (configdata[keyName] != null)
                        {
                            configdata[keyName].ClearComments();
                            IEnumerator<KeyData> iterator = configdata[keyName].GetEnumerator();
                            while (iterator.MoveNext())
                            {
                                KeyData keyData = iterator.Current;

                                // If attr is "enabled" or not a KeyCode, we add it to the config to be sent
                                if ((keyData.KeyName.Equals("enabled") && configdata[keyName].GetBool(keyData.KeyName)) || (prop.PropertyType.GetProperty(keyData.KeyName) != null && !prop.PropertyType.GetProperty(keyData.KeyName).PropertyType.Equals(typeof(UnityEngine.KeyCode))))
                                { 
                                    cleanConfigData.Add($"{keyData.KeyName}={keyData.Value}");
                                }
                            }
                        }
                    }
                }

                //Add number of clean lines to package
                pkg.Write(cleanConfigData.Count);

                //Add each line to the package
                foreach (string line in cleanConfigData)
                {
                    pkg.Write(line);

                    ZLog.Log("SENTCONFIG: " + line);
                }

                ZRoutedRpc.instance.InvokeRoutedRPC(sender, "VPlusConfigSync", new object[]
                {
                    pkg
                });

                ZLog.Log("VPlus configuration synced to peer #" + sender);
            }
            else //Client
            {
                if (configPkg != null && 
                    configPkg.Size() > 0 && 
                    sender == ZRoutedRpc.instance.GetServerPeerID()) //Validate the message is from the server and not another client.
                {
                    int numLines = configPkg.ReadInt();

                    if (numLines == 0)
                    {
                        ZLog.LogWarning("Got zero line config file from server. Cannot load.");
                        return;
                    }

                    using (MemoryStream memStream = new MemoryStream())
                    {
                        using (StreamWriter tmpWriter = new StreamWriter(memStream))
                        {
                            for (int i = 0; i < numLines; i++)
                            {
                                string line = configPkg.ReadString();

                                tmpWriter.WriteLine(line);

                                ZLog.Log("CONFIGDATA: " + line);
                            }

                            tmpWriter.Flush(); //Flush to memStream
                            memStream.Position = 0; //Rewind stream

                            // Now we have the full config, we will load to the current config the received one
                            ConfigurationExtra.LoadConfigurationFromStream(memStream);

                            // Needed to make sure client is using server configuration as dayLength is setup before
                            // TimeManipulation.SetupDayLength(); DEACTIVATED

                            ZLog.Log("Successfully synced VPlus configuration from server.");
                        }
                    }
                }
            }
        }
    }
}