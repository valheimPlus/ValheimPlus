using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus.RPC
{
    public class VPlusConfigSync
    {
        public static void RPC_VPlusConfigSync(long sender, ZPackage configPkg)
        {
            if (ZNet.m_isServer) //Server
            {
                if (Configuration.Current == null)
                {
                    Configuration.LoadConfiguration();
                }

                ZPackage pkg = new ZPackage();
                string data = Configuration.Current.GetSyncableSections();

                //Add number of clean lines to package
                pkg.Write(data);

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

                    Configuration receivedConfig = new Configuration();
                    Configuration.LoadFromIniString(receivedConfig, configPkg.ReadString());

                    Configuration.SetSyncableValues(receivedConfig);

                    ZLog.Log("Successfully synced VPlus configuration from server.");
                }
            }
        }
    }
}