using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{

    class AdvancedBuildingMode
    {
        public static Boolean isInABM = false;

        static Boolean DelayedStop = false;
        static Boolean BlockRefresh = false;
        static Boolean controlFlag = false;
        static Boolean shiftFlag = false;
        static Boolean altFlag = false;

        [HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
        public static class ModifyPlacingRestrictionOfGhost
        {
            private static Boolean Prefix(Player __instance, bool flashGuardStone)
            {
                Player PlayerInstance = __instance;
                if (PlayerInstance == null)
                { return true; }

                if (Configuration.Current.AdvancedBuildingMode != null || !__instance.InPlaceMode())
                {
                    return true;
                }

                KeyCode enter = Configuration.Current.AdvancedBuildingMode.EnterAdvancedBuildingMode;
                KeyCode exit = Configuration.Current.AdvancedBuildingMode.ExitAdvancedBuildingMode;

                // Error Handling/Detection for Hoe & Terrain Tool
                if (PlayerInstance.m_buildPieces != null)
                {
                    GameObject selectedPrefab = PlayerInstance.m_buildPieces.GetSelectedPrefab();
                    if (selectedPrefab == null || IsHoeOrTerrainTool(selectedPrefab))
                    {
                        return true;
                    }
                }

                // Prevent cross usage of ABM & ABE
                if (AdvancedEditingMode.isInAEM)
                {
                    return true;
                }


                // Delayed function stop to place the object at the right location (if we would immediatly stop, it would be placed at cursor location)
                if (DelayedStop)
                {
                    isInABM = false;
                    DelayedStop = false;
                    BlockRefresh = false;
                    return true;
                }

                // Error Handling and removal of left over placement marker
                if (!PlayerInstance.InPlaceMode() || PlayerInstance.m_placementGhost == null || PlayerInstance.m_buildPieces == null)
                {
                    DelayedStop = false;
                    BlockRefresh = false;
                    if (PlayerInstance.m_placementMarkerInstance != null)
                    {
                        PlayerInstance.m_placementMarkerInstance.SetActive(false);
                    }
                    return true;
                }


                float rX = 0;
                float rZ = 0;
                float rY = 0;

                if (Input.GetKeyDown(enter))
                {
                    isInABM = true;
                    DelayedStop = false;
                    BlockRefresh = true;
                }
                if (Input.GetKeyDown(exit))
                {
                    DelayedStop = true;
                    PlayerInstance.m_placeRotation = 0;
                }

                float distance = 2;
                float scrollDistance = 2;

                // TODO ADD INCREASE / DECREASE HOTKEYS
                // TODO ADD HOTKEY TO SAVE / LOAD ROTATION

                // CONTROL PRESSED
                if (Input.GetKeyDown(KeyCode.LeftControl)) { controlFlag = true; }
                if (Input.GetKeyUp(KeyCode.LeftControl)) { controlFlag = false; }


                // Detect hotkey presses for hotbar to reduce potential issues
                if (Input.GetKeyDown(KeyCode.Alpha1)) { DelayedStop = true; }
                if (Input.GetKeyDown(KeyCode.Alpha2)) { DelayedStop = true; }
                if (Input.GetKeyDown(KeyCode.Alpha3)) { DelayedStop = true; }
                if (Input.GetKeyDown(KeyCode.Alpha4)) { DelayedStop = true; }
                if (Input.GetKeyDown(KeyCode.Alpha5)) { DelayedStop = true; }
                if (Input.GetKeyDown(KeyCode.Alpha6)) { DelayedStop = true; }
                if (Input.GetKeyDown(KeyCode.Alpha7)) { DelayedStop = true; }
                if (Input.GetKeyDown(KeyCode.Alpha8)) { DelayedStop = true; }
                if (Input.GetKeyDown(KeyCode.Alpha9)) { DelayedStop = true; }
                if (Input.GetKeyDown(KeyCode.Alpha0)) { DelayedStop = true; }


                // SHIFT PRESSED
                if (Input.GetKeyDown(KeyCode.LeftShift)) { shiftFlag = true; }
                if (Input.GetKeyUp(KeyCode.LeftShift)) { shiftFlag = false; }
                if (shiftFlag) { distance = 2 * 3; scrollDistance = 2 * 3; } else { distance = 2; scrollDistance = 2; }

                // LEFT ALT PRESSED
                if (Input.GetKeyDown(KeyCode.LeftAlt)) { altFlag = true; }
                if (Input.GetKeyUp(KeyCode.LeftAlt)) { altFlag = false; }


                Piece component = PlayerInstance.m_placementGhost.GetComponent<Piece>();

                // Maximum distance between player and placed piece
                if (Vector3.Distance(__instance.transform.position, component.transform.position) > 25)
                {
                    DelayedStop = true;
                }

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
                if (!PrivateArea.CheckAccess(PlayerInstance.m_placementGhost.transform.position, radius, flashGuardStone))
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

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    // Stop with the next iteration of the function and place the piece
                    DelayedStop = true;
                }



                return !BlockRefresh;

            }

            // ToDo hook equipped item to determine what tool is active
            private static void Postfix(ref Player __instance)
            {
                if (DelayedStop)
                {
                    if (__instance.m_placementMarkerInstance)
                    {
                        __instance.m_placementMarkerInstance.SetActive(false);
                    }
                }
                if (Configuration.Current.Building != null && Configuration.Current.Building.NoInvalidPlacementRestriction)
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
}
