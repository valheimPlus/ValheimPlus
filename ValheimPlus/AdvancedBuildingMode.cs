using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Runtime;
using IniParser;
using IniParser.Model;
using System.Globalization;
using Steamworks;
using ValheimPlus;
using UnityEngine.Rendering;
using ValheimPlus.Configurations;

namespace ValheimPlus
{

    class AdvancedBuildingMode
    {
        [HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
        public static class ModifyPlacingRestrictionOfGhost
        {
            private static Boolean Prefix(Player __instance, bool flashGuardStone)
            {
                if (Configuration.Current.AdvancedBuildingMode.IsEnabled)
                {
                    ABM.PlayerInstance = __instance;
                    ABM.run();
                }
                if (ABM.isActive)
                    return false;
                return true;
            }


            private static void Postfix(ref Player __instance)
            {
                if (ABM.exitOnNextIteration)
                {
                    try
                    {
                        if (__instance.m_placementMarkerInstance)
                        {
                            __instance.m_placementMarkerInstance.SetActive(false);
                        }
                    }catch(Exception e)
                    {

                    }
                }
                if (Configuration.Current.AdvancedBuildingMode.IsEnabled && Configuration.Current.Building.noInvalidPlacementRestriction)
                {
                    if (__instance.m_placementStatus == Player.PlacementStatus.Invalid)
                    {
                        __instance.m_placementStatus = Player.PlacementStatus.Valid;
                        __instance.m_placementGhost.GetComponent<Piece>().SetInvalidPlacementHeightlight(false);
                    }
                }
            }


        }

        public static bool IsHoeOrTerrainTool(GameObject selectedPrefab)
        {
            string[] HoePrefabs = { "paved_road", "mud_road", "raise", "path" };
            string[] TerrainToolPrefabs = { "cultivate", "replant" };

            if (selectedPrefab.name.ToLower().Contains("sapling"))
            {
                return true;
            }
            if (HoePrefabs.Contains(selectedPrefab.name) || TerrainToolPrefabs.Contains(selectedPrefab.name))
            {
                return true;
            }
            return false;
        }
    }


    class ABM
    {
        // Status
        public static Boolean isActive = false;

        // Player Instance
        public static Player PlayerInstance;

        // Control Flags
        static Boolean controlFlag = false;
        static Boolean shiftFlag = false;
        static Boolean altFlag = false;

        // Exit flags
        public static Boolean exitOnNextIteration = false;
        static Boolean blockDefaultFunction = false;

        static private Piece component;

        //Modification Speeds
        static float gDistance = 2;
        static float gScrollDistance = 2;


        public static void run()
        {

            if (AEM.isActive)
            {
                if (isActive)
                    exitMode();
                return;
            }

            if (Input.GetKeyDown(Configuration.Current.AdvancedBuildingMode.exitAdvancedBuildingMode))
            {
                if (isActive)
                    exitMode();
                return;
            }

            if (exitOnNextIteration)
            {
                isActive = false;
                blockDefaultFunction = false;
                exitOnNextIteration = false;
                component = null;
            }

            if (isActive && component == null)
            {
                if (isActive)
                    exitMode();
                return;
            }

            // Check if prefab selected (build pieces) & ghost is ready
            if (selectedPrefab() == null || PlayerInstance.m_placementGhost == null || PlayerInstance.m_buildPieces == null)
            {
                if (isActive)
                    exitMode();
                return;
            }

            // Check if Build Mode && Correct build mode
            if (isInBuildMode() && IsHoeOrTerrainTool(selectedPrefab()))
            {
                if(isActive)
                    exitMode();
                return;
            }

            

            if (isActive)
            {
                // Maximum distance between player and placed piece
                if (Vector3.Distance(PlayerInstance.transform.position, component.transform.position) > PlayerInstance.m_maxPlaceDistance)
                {
                    exitMode();
                }
                ABM.isRunning();
                // DO WORK WHEN ALREADY STARTED
                listenToHotKeysAndDoWork();
            }
            else
            {
                if (Input.GetKeyDown(Configuration.Current.AdvancedBuildingMode.enterAdvancedBuildingMode))
                {
                    startMode();
                }
            }

        }

        private static void listenToHotKeysAndDoWork()
        {

            float rX = 0;
            float rZ = 0;
            float rY = 0;


            // CONTROL PRESSED
            if (Input.GetKeyDown(KeyCode.LeftControl)) { controlFlag = true; }
            if (Input.GetKeyUp(KeyCode.LeftControl)) { controlFlag = false; }


            // SHIFT PRESSED
            float distance = gDistance;
            float scrollDistance = gScrollDistance;
            if (Input.GetKeyDown(KeyCode.LeftShift)) { shiftFlag = true; }
            if (Input.GetKeyUp(KeyCode.LeftShift)) { shiftFlag = false; }
            changeModificationSpeeds(shiftFlag);
            if (shiftFlag) { distance = gDistance * 3; scrollDistance = gScrollDistance * 3; } else { distance = gDistance; scrollDistance = gScrollDistance; }

            // LEFT ALT PRESSED
            if (Input.GetKeyDown(KeyCode.LeftAlt)) { altFlag = true; }
            if (Input.GetKeyUp(KeyCode.LeftAlt)) { altFlag = false; }



            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                Quaternion rotation;
                if (controlFlag)
                {
                    rX++;
                    rotation = Quaternion.Euler(component.transform.eulerAngles.x + (scrollDistance * (float)rX), component.transform.eulerAngles.y, component.transform.eulerAngles.z); // forward to backwards
                }
                else
                {
                    if (altFlag)
                    {
                        rZ++;
                        rotation = Quaternion.Euler(component.transform.eulerAngles.x, component.transform.eulerAngles.y, component.transform.eulerAngles.z + (scrollDistance * (float)rZ)); // diagonal
                    }
                    else
                    {
                        rY++;
                        rotation = Quaternion.Euler(component.transform.eulerAngles.x, component.transform.eulerAngles.y + (scrollDistance * (float)rY), component.transform.eulerAngles.z); // left<->right
                    }
                }
                component.transform.rotation = rotation;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                Quaternion rotation;
                if (controlFlag)
                {
                    rX--;
                    rotation = Quaternion.Euler(component.transform.eulerAngles.x + (scrollDistance * (float)rX), component.transform.eulerAngles.y, component.transform.eulerAngles.z); // forward to backwards
                }
                else
                {
                    if (altFlag)
                    {
                        rZ--;
                        rotation = Quaternion.Euler(component.transform.eulerAngles.x, component.transform.eulerAngles.y, component.transform.eulerAngles.z + (scrollDistance * (float)rZ)); // diagonal
                    }
                    else
                    {
                        rY--;
                        rotation = Quaternion.Euler(component.transform.eulerAngles.x, component.transform.eulerAngles.y + (scrollDistance * (float)rY), component.transform.eulerAngles.z); // left<->right
                    }
                }

                component.transform.rotation = rotation;
            }


            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (controlFlag)
                {
                    component.transform.Translate(Vector3.up * distance * Time.deltaTime);
                }
                else
                {
                    component.transform.Translate(Vector3.forward * distance * Time.deltaTime);
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (controlFlag)
                {
                    component.transform.Translate(Vector3.down * distance * Time.deltaTime);
                }
                else
                {
                    component.transform.Translate(Vector3.back * distance * Time.deltaTime);
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                component.transform.Translate(Vector3.left * distance * Time.deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                component.transform.Translate(Vector3.right * distance * Time.deltaTime);
            }

            try
            {
                isValidPlacement();
            }catch(Exception e) { }
        }

        private static Boolean isValidPlacement()
        {
            bool water = component.m_waterPiece || component.m_noInWater;

            PlayerInstance.m_placementStatus = 0;

            if (component.m_groundOnly || component.m_groundPiece || component.m_cultivatedGroundOnly)
            {
                PlayerInstance.m_placementMarkerInstance.SetActive(false);
            }

            StationExtension component2 = component.GetComponent<StationExtension>();

            if (component2 != null)
            {
                CraftingStation craftingStation = component2.FindClosestStationInRange(component.transform.position);
                if (craftingStation)
                {
                    component2.StartConnectionEffect(craftingStation);
                }
                else
                {
                    component2.StopConnectionEffect();
                    PlayerInstance.m_placementStatus = Player.PlacementStatus.ExtensionMissingStation; // Missing Station
                }
                if (component2.OtherExtensionInRange(component.m_spaceRequirement))
                {
                    PlayerInstance.m_placementStatus = Player.PlacementStatus.MoreSpace; // More Space
                }
            }

            if (component.m_onlyInTeleportArea && !EffectArea.IsPointInsideArea(component.transform.position, EffectArea.Type.Teleport, 0f))
            {
                PlayerInstance.m_placementStatus = Player.PlacementStatus.NoTeleportArea;
            }
            if (!component.m_allowedInDungeons && (component.transform.position.y > 3000f))
            {
                PlayerInstance.m_placementStatus = Player.PlacementStatus.NotInDungeon;
            }
            if (Location.IsInsideNoBuildLocation(PlayerInstance.m_placementGhost.transform.position))
            {
                PlayerInstance.m_placementStatus = Player.PlacementStatus.NoBuildZone;
            }
            float radius = component.GetComponent<PrivateArea>() ? component.GetComponent<PrivateArea>().m_radius : 0f;
            if (!PrivateArea.CheckAccess(PlayerInstance.m_placementGhost.transform.position, radius, true))
            {
                PlayerInstance.m_placementStatus = Player.PlacementStatus.PrivateZone;
            }

            if (PlayerInstance.m_placementStatus != 0)
            {
                component.SetInvalidPlacementHeightlight(true);
            }
            else
            {
                component.SetInvalidPlacementHeightlight(false);
            }
            return true;
        }

        private static void startMode()
        {
            notifyUser("Starting ABM");
            isActive = true;
            blockDefaultFunction = true;
            component = PlayerInstance.m_placementGhost.GetComponent<Piece>();
        }

        private static void exitMode()
        {
            notifyUser("Exiting ABM");
            exitOnNextIteration = true;
            isActive = false;
            component = null;
        }

        private static Boolean isInBuildMode()
        {
            return PlayerInstance.InPlaceMode();
        }

        private static GameObject selectedPrefab()
        {
            if (PlayerInstance.m_buildPieces != null)
            {
                GameObject selectedPrefab;
                try
                {
                    selectedPrefab = PlayerInstance.m_buildPieces.GetSelectedPrefab();
                    return selectedPrefab;
                }
                catch(Exception e)
                {
                    return null;
                }
            }
            return null;
        }

        // Is Hoe or Terrain Tool in build mode?
        private static Boolean IsHoeOrTerrainTool(GameObject selectedPrefab)
        {
            string[] HoePrefabs = { "paved_road", "mud_road", "raise", "path" };
            string[] TerrainToolPrefabs = { "cultivate", "replant" };

            if (selectedPrefab.name.ToLower().Contains("sapling"))
            {
                return true;
            }
            if (HoePrefabs.Contains(selectedPrefab.name) || TerrainToolPrefabs.Contains(selectedPrefab.name))
            {
                return true;
            }
            return false;
        }

        private static void notifyUser(string Message, MessageHud.MessageType position = MessageHud.MessageType.TopLeft)
        {
            MessageHud.instance.ShowMessage(position, "ABM: " + Message);
        }

        private static void isRunning()
        {
            if (isActive)
            {
                MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "ABM is active");
            }
        }

        private static void changeModificationSpeeds(Boolean isShiftPressed)
        {
            float incValue = 1;
            if (shiftFlag)
                incValue = 10;

            if (Input.GetKeyDown(KeyCode.KeypadPlus)) {

                if ((gScrollDistance - incValue) < 360)
                    gScrollDistance += incValue;

                if ((gDistance - incValue) < 360)
                    gDistance += incValue;
                notifyUser("Modification Speed: " + gDistance);
                Debug.Log("Modification Speed: " + gDistance);
            }
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {

                if((gScrollDistance-incValue) > 0)
                    gScrollDistance = gScrollDistance - incValue;

                if ((gDistance - incValue) > 0)
                    gDistance = gDistance - incValue;

                notifyUser("Modification Speed: " + gDistance);
                Debug.Log("Modification Speed: " + gDistance);
            }
        }


    }
}
