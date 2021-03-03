using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using IniParser;
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
                if (!Configuration.Current.Server.serverSyncsConfig) return;

                ZPackage pkg = new ZPackage();

                string[] rawConfigData = File.ReadAllLines(ConfigurationExtra.ConfigIniPath);
                List<string> cleanConfigData = new List<string>();

                FileIniDataParser parser = new FileIniDataParser();
                IniData configdata = parser.ReadFile(ConfigurationExtra.ConfigIniPath);


                foreach (var prop in typeof(Configuration).GetProperties())
                {
                    var keyName = prop.Name;
                    var method = prop.PropertyType.GetMethod("HasNeedsServerSync", BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

                    if (method != null)
                    {
                        var instance = prop.GetValue(Configuration.Current, null);
                        bool HasNeedsServerSync = (bool)method.Invoke(instance, new object[] { });
                        if (HasNeedsServerSync)
                        {
                            cleanConfigData.Add($"[{keyName}]");
                            bool hasEnableProperty = false;
                            if (configdata[keyName] != null)
                            {
                                configdata[keyName].ClearComments();
                                IEnumerator<KeyData> iterator = configdata[keyName].GetEnumerator();
                                while (iterator.MoveNext())
                                {
                                    KeyData keyData = iterator.Current;
                                    if (keyData.KeyName.Equals("enabled"))
                                    {
                                        hasEnableProperty = true;
                                    }
                                    cleanConfigData.Add($"{keyData.KeyName}={keyData.Value}");
                                }
                            } 
                            if (!hasEnableProperty)
                            {
                                cleanConfigData.Add("enabled = false");
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

                            ConfigurationExtra.LoadConfigurationFromStream(memStream);

                            // Needed to make sure client is using server configuration as dayLength is setup before
                            TimeManipulation.SetupDayLength();

                            ZLog.Log("Successfully synced VPlus configuration from server.");
                        }
                    }
                }
            }
        }
    }
}