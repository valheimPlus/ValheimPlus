using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Unity;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Runtime;
using IniParser;
using IniParser.Model;
using HarmonyLib;
using System.Globalization;
using Steamworks;
using ValheimPlus;
using UnityEngine.Rendering;

namespace ValheimPlus
{
    class AdvancedEditingMode
    {
        public static Boolean isInAEM = false;
        [HarmonyPatch(typeof(Player), "Update")]
        public static class AEM
        {

            public static RaycastHit raycastHit; // Hit Target

            public static Vector3 HitPoint;
            public static Vector3 HitNormal;
            public static Piece HitPiece;
            public static GameObject HitObject;
            public static Heightmap HitHeightmap;

            public static Boolean DelayedStop;

            static Boolean BlockRefresh = false;
            static Boolean controlFlag = false;
            static Boolean shiftFlag = false;
            static Boolean altFlag = false;

            static Quaternion InitialRotation;
            static Vector3 InitialPosition;

            static Vector3 exsistenceCheck;
            static Boolean HitExsists;
            private static void Postfix(Player __instance)
            {
                if (Settings.isEnabled("AdvancedEditingMode"))
                {

                    if (isInAEM && __instance.InPlaceMode())
                    {
                        DelayedStop = true;
                        HitPiece.transform.position = InitialPosition;
                        HitPiece.transform.rotation = InitialRotation;
                    }

                    if (Input.GetKeyDown(Settings.getHotkey("enterAdvancedEditingMode")) && !isInAEM && !__instance.InPlaceMode())
                    {
                        if (!AdvancedBuildingMode.isInABM)
                        {
                            int layerMask = __instance.m_placeRayMask;
                            Character PlayerCharacter = Helper.getPlayerCharacter();

                            if (Physics.Raycast(
                                GameCamera.instance.transform.position,
                                GameCamera.instance.transform.forward,
                                out raycastHit, 50f, layerMask
                                ) &&
                                raycastHit.collider &&
                                !raycastHit.collider.attachedRigidbody &&
                                Vector3.Distance(PlayerCharacter.m_eye.position, raycastHit.point) < __instance.m_maxPlaceDistance)
                            {
                                HitPoint = raycastHit.point;
                                HitNormal = raycastHit.normal;
                                HitPiece = raycastHit.collider.GetComponentInParent<Piece>();
                                HitObject = raycastHit.collider.gameObject;
                                HitHeightmap = raycastHit.collider.GetComponent<Heightmap>();
                                InitialRotation = HitPiece.transform.rotation;
                                InitialPosition = HitPiece.transform.position;
                            }
                            else
                            {
                                HitPoint = Vector3.zero;
                                HitNormal = Vector3.zero;
                                HitObject = null;
                                HitPiece = null;
                                HitHeightmap = null;
                            }


                            if (HitPiece != null)
                            {
                                Boolean hitValid = true;
                                if (HitPiece.m_onlyInTeleportArea && !EffectArea.IsPointInsideArea(HitPiece.transform.position, EffectArea.Type.Teleport, 0f))
                                {
                                    // Not in Teleport Area
                                    hitValid = false;
                                }
                                if (!HitPiece.m_allowedInDungeons && (HitPiece.transform.position.y > 3000f))
                                {
                                    // Not in dungeon
                                    hitValid = false;
                                }
                                if (Location.IsInsideNoBuildLocation(HitPiece.transform.position))
                                {
                                    // No build zone
                                    hitValid = false;
                                }
                                float radius = HitPiece.GetComponent<PrivateArea>() ? HitPiece.GetComponent<PrivateArea>().m_radius : 0f;
                                if (!PrivateArea.CheckAccess(HitPiece.transform.position, radius, true))
                                {
                                    // private zone
                                    hitValid = false;
                                }
                                // Initialize AEM
                                if (hitValid)
                                {
                                    isInAEM = true;
                                    HitExsists = true;
                                }
                            }
                        }
                    }

                    try
                    { // check to see if the hit object still exsists
                        if (isInAEM)
                        {
                            exsistenceCheck = HitPiece.transform.position;
                            HitExsists = true;
                        }
                    }
                    catch (Exception e)
                    {
                        HitExsists = false;
                    }

                    if (DelayedStop || (!HitExsists && isInAEM))
                    {
                        isInAEM = false;
                        HitPoint = Vector3.zero;
                        HitNormal = Vector3.zero;
                        HitObject = null;
                        HitPiece = null;
                        HitHeightmap = null;
                        DelayedStop = false;
                        InitialRotation = new Quaternion();
                        InitialPosition = new Vector3();
                    }

                    if (isInAEM && !AdvancedBuildingMode.isInABM)
                    {

                        // stop and reset object when entering building mode

                        float rX = 0;
                        float rZ = 0;
                        float rY = 0;

                        if (Input.GetKeyDown(Settings.getHotkey("resetAdvancedEditingMode")))
                        {
                            HitPiece.transform.position = InitialPosition;
                            HitPiece.transform.rotation = InitialRotation;
                        }

                        if (Input.GetKeyDown(Settings.getHotkey("abortAndExitAdvancedEditingMode")))
                        {
                            HitPiece.transform.position = InitialPosition;
                            HitPiece.transform.rotation = InitialRotation;
                            DelayedStop = true;
                        }

                        if (Input.GetKeyDown(Settings.getHotkey("confirmPlacementOfAdvancedEditingMode")))
                        {

                            GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(HitPiece.gameObject, HitPiece.transform.position, HitPiece.transform.rotation);
                            HitPiece.m_placeEffect.Create(HitPiece.transform.position, HitPiece.transform.rotation, gameObject2.transform, 1f);

                            ZNetScene.instance.Destroy(HitPiece.gameObject);

                            DelayedStop = true;
                        }


                        float distance = 2;
                        float scrollDistance = 2;

                        // TODO ADD INCREASE / DECREASE HOTKEYS
                        // TODO ADD HOTKEY TO SAVE / LOAD ROTATION

                        // CONTROL PRESSED
                        if (Input.GetKeyDown(KeyCode.LeftControl)) { controlFlag = true; }
                        if (Input.GetKeyUp(KeyCode.LeftControl)) { controlFlag = false; }


                        // SHIFT PRESSED
                        if (Input.GetKeyDown(KeyCode.LeftShift)) { shiftFlag = true; }
                        if (Input.GetKeyUp(KeyCode.LeftShift)) { shiftFlag = false; }
                        if (shiftFlag) { distance = 2 * 3; scrollDistance = 2 * 3; } else { distance = 2; scrollDistance = 2; }

                        // LEFT ALT PRESSED
                        if (Input.GetKeyDown(KeyCode.LeftAlt)) { altFlag = true; }
                        if (Input.GetKeyUp(KeyCode.LeftAlt)) { altFlag = false; }


                        Piece component = HitPiece;

                        // Maximum distance between player and placed piece
                        if (Vector3.Distance(__instance.transform.position, component.transform.position) > 25)
                        {
                            HitPiece.transform.position = InitialPosition;
                            HitPiece.transform.rotation = InitialRotation;
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

                    }
                }


            }

        }

        [HarmonyPatch(typeof(GameCamera), "UpdateCamera")]
        public static class BlockCameraScrollInAEM
        {

            private static void Prefix(GameCamera __instance)
            {
                if (isInAEM)
                {
                    __instance.m_maxDistance = __instance.m_distance;
                    __instance.m_minDistance = __instance.m_distance;
                }
                else
                {
                    __instance.m_maxDistance = 6;
                    __instance.m_minDistance = 1;
                }
            }
        }
    }
}
