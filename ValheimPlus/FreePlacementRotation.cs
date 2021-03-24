using HarmonyLib;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Rotates placementGhost by 1 degree, if pressed key, or reset to x22.5f degrees usual rotation.
    /// Attaches to nearly placed objects as usual placement
    /// </summary>
    public class FreePlacementRotation
    {
        private class PlayerData
        {
            public Vector3 PlaceRotation = Vector3.zero;
            public bool Opposite;
            public Piece LastPiece;
            public KeyCode LastKeyCode;
            
        }
        
        private static readonly Dictionary<Player, PlayerData> PlayersData = new Dictionary<Player, PlayerData>();

        [HarmonyPatch(typeof(Player), "UpdatePlacement")]
        public static class ModifyPUpdatePlacement
        {
            private static void Postfix(Player __instance, bool takeInput, float dt)
            {
                if (!Configuration.Current.FreePlacementRotation.IsEnabled)
                    return;
                
                if (ABM.isActive)
                    return;

                if (!__instance.InPlaceMode())
                    return;

                if (!takeInput)
                    return;

                if (Hud.IsPieceSelectionVisible())
                    return;

                if (!PlayersData.ContainsKey(__instance))
                    PlayersData[__instance] = new PlayerData();

                RotateWithWheel(__instance);
                SyncRotationWithTargetInFront(__instance, Configuration.Current.FreePlacementRotation.copyRotationParallel, false);
                SyncRotationWithTargetInFront(__instance, Configuration.Current.FreePlacementRotation.copyRotationPerpendicular, true);
            }

            private static void RotateWithWheel(Player __instance)
            {
                var wheel = Input.GetAxis("Mouse ScrollWheel");

                var playerData = PlayersData[__instance];
                
                if (!wheel.Equals(0f) || ZInput.GetButton("JoyRotate"))
                {
                    if (Input.GetKey(Configuration.Current.FreePlacementRotation.rotateY))
                    {
                        playerData.PlaceRotation += Vector3.up * Mathf.Sign(wheel);
                        __instance.m_placeRotation = (int) (playerData.PlaceRotation.y / 22.5f);
                    }
                    else if (Input.GetKey(Configuration.Current.FreePlacementRotation.rotateX))
                    {
                        playerData.PlaceRotation += Vector3.right * Mathf.Sign(wheel);
                    }
                    else if (Input.GetKey(Configuration.Current.FreePlacementRotation.rotateZ))
                    {
                        playerData.PlaceRotation += Vector3.forward * Mathf.Sign(wheel);
                    }
                    else
                    {
                        __instance.m_placeRotation = ClampPlaceRotation(__instance.m_placeRotation);
                        playerData.PlaceRotation = new Vector3(0, __instance.m_placeRotation * 22.5f, 0);
                    }

                    playerData.PlaceRotation = ClampAngles(playerData.PlaceRotation);

                    Debug.Log("Angle " + playerData.PlaceRotation);
                }
            }
            
            private static void SyncRotationWithTargetInFront(Player __instance, KeyCode keyCode, bool perpendicular)
            {
                if (__instance.m_placementGhost == null)
                    return;

                if (Input.GetKeyUp(keyCode))
                {
                    Vector3 point;
                    Vector3 normal;
                    Piece piece;
                    Heightmap heightmap;
                    Collider waterSurface;
                    if (__instance.PieceRayTest(out point, out normal, out piece, out heightmap, out waterSurface,
                        false) && piece != null)
                    {
                        var playerData = PlayersData[__instance];
                        
                        var rotation = piece.transform.rotation;
                        if (perpendicular)
                            rotation *= Quaternion.Euler(0, 90, 0);

                        if (playerData.LastKeyCode != keyCode || playerData.LastPiece != piece)
                            playerData.Opposite = false;
                        
                        playerData.LastKeyCode = keyCode;
                        playerData.LastPiece = piece;
                        
                        if (playerData.Opposite)
                            rotation *= Quaternion.Euler(0, 180, 0);
                        
                        playerData.Opposite = !playerData.Opposite;
                        
                        playerData.PlaceRotation = rotation.eulerAngles;
                        Debug.Log("Sync Angle " + playerData.PlaceRotation);
                    }
                }
            }
        }

        private static Vector3 ClampAngles(Vector3 angles)
        {
            return new Vector3(ClampAngle(angles.x), ClampAngle(angles.y), ClampAngle(angles.z));
        }
        
        private static int ClampPlaceRotation(int index)
        {
            const int MaxIndex = 16; // 360/22.5f
            
            if (index < 0)
                index = MaxIndex + index;
            else if (index >= MaxIndex)
                index -= MaxIndex;
            return index;
        }
        
        private static float ClampAngle(float angle)
        {
            if (angle < 0)
                angle = 360 + angle;
            else if (angle >= 360)
                angle -= 360;
            return angle;
        }

        [HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
        public static class ModifyPlacingRestrictionOfGhost
        {
            private static void Postfix(Player __instance, bool flashGuardStone)
            {
                if (!Configuration.Current.FreePlacementRotation.IsEnabled)
                    return;
                
                if (ABM.isActive)
                    return;
                
                UpdatePlacementGhost(__instance, flashGuardStone);
            }

            // almost copy of original UpdatePlacementGhost with modified calculation of Quaternion quaternion = Quaternion.Euler(rotation);
            // need to be re-calculated in Postfix for correct work of auto-attachment of placementGhost after change rotation
            private static void UpdatePlacementGhost(Player __instance, bool flashGuardStone)
            {
                if ((UnityEngine.Object) __instance.m_placementGhost == (UnityEngine.Object) null)
                {
                    if (!(bool) (UnityEngine.Object) __instance.m_placementMarkerInstance)
                        return;
                    __instance.m_placementMarkerInstance.SetActive(false);
                }
                else
                {
                    bool flag = ZInput.GetButton("AltPlace") || ZInput.GetButton("JoyAltPlace");
                    Piece component1 = __instance.m_placementGhost.GetComponent<Piece>();
                    bool water = component1.m_waterPiece || component1.m_noInWater;
                    Vector3 point;
                    Vector3 normal;
                    Piece piece;
                    Heightmap heightmap;
                    Collider waterSurface;
                    if (__instance.PieceRayTest(out point, out normal, out piece, out heightmap, out waterSurface, water))
                    {
                        __instance.m_placementStatus = Player.PlacementStatus.Valid;
                        if ((UnityEngine.Object) __instance.m_placementMarkerInstance == (UnityEngine.Object) null)
                            __instance.m_placementMarkerInstance =
                                UnityEngine.Object.Instantiate<GameObject>(__instance.m_placeMarker, point,
                                    Quaternion.identity);
                        __instance.m_placementMarkerInstance.SetActive(true);
                        __instance.m_placementMarkerInstance.transform.position = point;
                        __instance.m_placementMarkerInstance.transform.rotation = Quaternion.LookRotation(normal);
                        if (component1.m_groundOnly || component1.m_groundPiece || component1.m_cultivatedGroundOnly)
                            __instance.m_placementMarkerInstance.SetActive(false);
                        WearNTear wearNtear = (UnityEngine.Object) piece != (UnityEngine.Object) null
                            ? piece.GetComponent<WearNTear>()
                            : (WearNTear) null;
                        StationExtension component2 = component1.GetComponent<StationExtension>();
                        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
                        {
                            CraftingStation closestStationInRange = component2.FindClosestStationInRange(point);
                            if ((bool) (UnityEngine.Object) closestStationInRange)
                            {
                                component2.StartConnectionEffect(closestStationInRange);
                            }
                            else
                            {
                                component2.StopConnectionEffect();
                                __instance.m_placementStatus = Player.PlacementStatus.ExtensionMissingStation;
                            }

                            if (component2.OtherExtensionInRange(component1.m_spaceRequirement))
                                __instance.m_placementStatus = Player.PlacementStatus.MoreSpace;
                        }

                        if ((bool) (UnityEngine.Object) wearNtear && !wearNtear.m_supports)
                            __instance.m_placementStatus = Player.PlacementStatus.Invalid;
                        if (component1.m_waterPiece && (UnityEngine.Object) waterSurface == (UnityEngine.Object) null &&
                            !flag)
                            __instance.m_placementStatus = Player.PlacementStatus.Invalid;
                        if (component1.m_noInWater && (UnityEngine.Object) waterSurface != (UnityEngine.Object) null)
                            __instance.m_placementStatus = Player.PlacementStatus.Invalid;
                        if (component1.m_groundPiece && (UnityEngine.Object) heightmap == (UnityEngine.Object) null)
                        {
                            __instance.m_placementGhost.SetActive(false);
                            __instance.m_placementStatus = Player.PlacementStatus.Invalid;
                            return;
                        }

                        if (component1.m_groundOnly && (UnityEngine.Object) heightmap == (UnityEngine.Object) null)
                            __instance.m_placementStatus = Player.PlacementStatus.Invalid;
                        if (component1.m_cultivatedGroundOnly &&
                            ((UnityEngine.Object) heightmap == (UnityEngine.Object) null ||
                             !heightmap.IsCultivated(point)))
                            __instance.m_placementStatus = Player.PlacementStatus.NeedCultivated;
                        if (component1.m_notOnWood && (bool) (UnityEngine.Object) piece &&
                            (bool) (UnityEngine.Object) wearNtear &&
                            (wearNtear.m_materialType == WearNTear.MaterialType.Wood ||
                             wearNtear.m_materialType == WearNTear.MaterialType.HardWood))
                            __instance.m_placementStatus = Player.PlacementStatus.Invalid;
                        if (component1.m_notOnTiltingSurface && (double) normal.y < 0.800000011920929)
                            __instance.m_placementStatus = Player.PlacementStatus.Invalid;
                        if (component1.m_inCeilingOnly && (double) normal.y > -0.5)
                            __instance.m_placementStatus = Player.PlacementStatus.Invalid;
                        if (component1.m_notOnFloor && (double) normal.y > 0.100000001490116)
                            __instance.m_placementStatus = Player.PlacementStatus.Invalid;
                        if (component1.m_onlyInTeleportArea &&
                            !(bool) (UnityEngine.Object) EffectArea.IsPointInsideArea(point, EffectArea.Type.Teleport,
                                0.0f))
                            __instance.m_placementStatus = Player.PlacementStatus.NoTeleportArea;
                        if (!component1.m_allowedInDungeons && __instance.InInterior())
                            __instance.m_placementStatus = Player.PlacementStatus.NotInDungeon;
                        if ((bool) (UnityEngine.Object) heightmap)
                            normal = Vector3.up;
                        __instance.m_placementGhost.SetActive(true);

                        var rotation = PlayersData.ContainsKey(__instance)
                            ? PlayersData[__instance].PlaceRotation
                            : __instance.m_placeRotation * 22.5f * Vector3.up;

                        Quaternion quaternion = Quaternion.Euler(rotation);
                        
                        if ((component1.m_groundPiece || component1.m_clipGround) &&
                            (bool) (UnityEngine.Object) heightmap || component1.m_clipEverything)
                        {
                            if ((bool) (UnityEngine.Object) __instance.m_buildPieces.GetSelectedPrefab()
                                    .GetComponent<TerrainModifier>() && component1.m_allowAltGroundPlacement &&
                                (component1.m_groundPiece && !ZInput.GetButton("AltPlace")) &&
                                !ZInput.GetButton("JoyAltPlace"))
                            {
                                float groundHeight = ZoneSystem.instance.GetGroundHeight(__instance.transform.position);
                                point.y = groundHeight;
                            }

                            __instance.m_placementGhost.transform.position = point;
                            __instance.m_placementGhost.transform.rotation = quaternion;
                        }
                        else
                        {
                            Collider[] componentsInChildren = __instance.m_placementGhost.GetComponentsInChildren<Collider>();
                            if (componentsInChildren.Length != 0)
                            {
                                __instance.m_placementGhost.transform.position = point + normal * 50f;
                                __instance.m_placementGhost.transform.rotation = quaternion;
                                Vector3 vector3_1 = Vector3.zero;
                                float num1 = 999999f;
                                foreach (Collider collider in componentsInChildren)
                                {
                                    if (!collider.isTrigger && collider.enabled)
                                    {
                                        MeshCollider meshCollider = collider as MeshCollider;
                                        if (!((UnityEngine.Object) meshCollider != (UnityEngine.Object) null) ||
                                            meshCollider.convex)
                                        {
                                            Vector3 a = collider.ClosestPoint(point);
                                            float num2 = Vector3.Distance(a, point);
                                            if ((double) num2 < (double) num1)
                                            {
                                                vector3_1 = a;
                                                num1 = num2;
                                            }
                                        }
                                    }
                                }

                                Vector3 vector3_2 = __instance.m_placementGhost.transform.position - vector3_1;
                                if (component1.m_waterPiece)
                                    vector3_2.y = 3f;
                                __instance.m_placementGhost.transform.position = point + vector3_2;
                                __instance.m_placementGhost.transform.rotation = quaternion;
                            }
                        }

                        if (!flag)
                        {
                            __instance.m_tempPieces.Clear();
                            Transform a;
                            Transform b;
                            if (__instance.FindClosestSnapPoints(__instance.m_placementGhost.transform, 0.5f, out a, out b,
                                __instance.m_tempPieces))
                            {
                                Vector3 position = b.parent.position;
                                Vector3 p = b.position - (a.position - __instance.m_placementGhost.transform.position);
                                if (!__instance.IsOverlapingOtherPiece(p, __instance.m_placementGhost.name, __instance.m_tempPieces))
                                    __instance.m_placementGhost.transform.position = p;
                            }
                        }

                        if (Location.IsInsideNoBuildLocation(__instance.m_placementGhost.transform.position))
                            __instance.m_placementStatus = Player.PlacementStatus.NoBuildZone;
                        if (!PrivateArea.CheckAccess(__instance.m_placementGhost.transform.position,
                            (bool) (UnityEngine.Object) component1.GetComponent<PrivateArea>()
                                ? component1.GetComponent<PrivateArea>().m_radius
                                : 0.0f, flashGuardStone))
                            __instance.m_placementStatus = Player.PlacementStatus.PrivateZone;
                        if (__instance.CheckPlacementGhostVSPlayers())
                            __instance.m_placementStatus = Player.PlacementStatus.BlockedbyPlayer;
                        if (component1.m_onlyInBiome != Heightmap.Biome.None &&
                            (Heightmap.FindBiome(__instance.m_placementGhost.transform.position) &
                             component1.m_onlyInBiome) == Heightmap.Biome.None)
                            __instance.m_placementStatus = Player.PlacementStatus.WrongBiome;
                        if (component1.m_noClipping && __instance.TestGhostClipping(__instance.m_placementGhost, 0.2f))
                            __instance.m_placementStatus = Player.PlacementStatus.Invalid;
                    }
                    else
                    {
                        if ((bool) (UnityEngine.Object) __instance.m_placementMarkerInstance)
                            __instance.m_placementMarkerInstance.SetActive(false);
                        __instance.m_placementGhost.SetActive(false);
                        __instance.m_placementStatus = Player.PlacementStatus.Invalid;
                    }

                    __instance.SetPlacementGhostValid(__instance.m_placementStatus == Player.PlacementStatus.Valid);
                }
            }
        }
    }
}