using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus.FirstPerson
{
	[HarmonyPatch]
	public static class VPlusFirstPerson
	{
		static VPlusFirstPerson()
		{
		}
        #region Data Structures

        // Struct to hold the dynamic values
        public struct DynamicPerson
        {
			// Are we in first person or not
			public static bool isFirstPerson = false;
			// Holder for old m_3rdOffset value
			public static Vector3 noVPFP_3rdOffset = Vector3.zero;
			// Holder for old m_fpsOffset value
			public static Vector3 noVPFP_fpsOffset = Vector3.zero;
		};

		// Struct to hold Camera constants
		public struct CameraConstants
        {
			// Valheim zoom thingy value
			public static float zoomSens = 10f;
			// Min and max distance of camera
			public static float minDistance = 1f;
			public static float maxDistance = 8f;
			// Near Clip Plane max and min
			public static float nearClipPlaneMax = 0.02f;
			public static float nearClipPlaneMin = 0.01f;
		};

		#endregion

        #region GameClasses

        #region Character
        
		/// <summary>
        /// Hooks visibility LODs of player
        /// </summary>
        [HarmonyPatch(typeof(Character), "SetVisible")]
		public static class Character_SetVisible_Patch
		{
			private static bool Prefix(ref Character __instance, bool visible)
			{
				if (Configuration.Current.FirstPerson.IsEnabled)
				{
					if (__instance.m_lodGroup == null)
					{
						return false;
					}
					if (__instance.m_lodVisible == visible)
					{
						return false;
					}
					if (__instance.IsPlayer() && !visible)
					{
						return false;
					}
					__instance.m_lodVisible = visible;
					if (__instance.m_lodVisible)
					{
						__instance.m_lodGroup.localReferencePoint = __instance.m_originalLocalRef;
						return false;
					}
					__instance.m_lodGroup.localReferencePoint = new Vector3(999999f, 999999f, 999999f);
					return false;
				}
				return true;
			}
		}

        #endregion

        #region Player

        /// <summary>
        /// Ignore ghost clipping
        /// </summary>
        [HarmonyPatch(typeof(Player), "TestGhostClipping")]
		public static class Player_TestGhostClipping_Patch
		{
			// This function is basically testing whether camera is penetrating player
			// So yeeting it like there is no tomorrow
			// The game function is literally called ComputePenetration, decent
			private static bool Prefix()
			{
				if(Configuration.Current.FirstPerson.IsEnabled)
					return false;
				return true;
			}
		}

        #endregion

        #region GameCamera

        /// <summary>
        /// Grab a hold of our camera constants in awake
        /// </summary>
        [HarmonyPatch(typeof(GameCamera), "Awake")]
		public static class GameCamera_Awake_Patch
		{
			private static void Postfix(ref GameCamera __instance)
			{
				if (Configuration.Current.FirstPerson.IsEnabled)
				{
					CameraConstants.zoomSens = __instance.m_zoomSens;
					CameraConstants.minDistance = __instance.m_minDistance;
					CameraConstants.maxDistance = __instance.m_maxDistance;
				}
			}
		}

		/// <summary>
		/// Update camera for First Person
		/// 
		/// </summary>
		[HarmonyPatch(typeof(GameCamera), "UpdateCamera")]
		public static class GameCamera_Update_Patch
		{
			// Put outside just to clean up
			private static void SetupFP(ref GameCamera __instance, ref Player localPlayer)
            {
				// Save old offsets and then use our own
				DynamicPerson.noVPFP_3rdOffset = __instance.m_3rdOffset;
				DynamicPerson.noVPFP_fpsOffset = __instance.m_fpsOffset;
				__instance.m_3rdOffset = Vector3.zero;
				__instance.m_fpsOffset = Vector3.zero;
				// Set the camera stuff to 0 or our new value
				__instance.m_minDistance = 0;
				__instance.m_maxDistance = 0;
				__instance.m_zoomSens = 0;
				__instance.m_nearClipPlaneMax = CameraConstants.nearClipPlaneMax;
				__instance.m_nearClipPlaneMin = CameraConstants.nearClipPlaneMin;
				// What Field Of View value, default is 65
				__instance.m_fov = Configuration.Current.FirstPerson.defaultFOV;
				// Make head stuff have no size, same method as mounting legs disappear
				localPlayer.m_head.localScale = Vector3.zero;
				localPlayer.m_eye.localScale = Vector3.zero;
			}
			private static void Postfix(ref GameCamera __instance, float dt)
			{
				if (Configuration.Current.FirstPerson.IsEnabled)
				{
					// This is from game code, not sure if need it though
					if (__instance.m_freeFly)
					{
						__instance.UpdateFreeFly(dt);
						__instance.UpdateCameraShake(dt);
						return;
					}
					Player localPlayer = Player.m_localPlayer;
					if (Input.GetKeyDown(Configuration.Current.FirstPerson.hotkey))
					{
						DynamicPerson.isFirstPerson = !DynamicPerson.isFirstPerson;
						if (DynamicPerson.isFirstPerson)
						{
							SetupFP(ref __instance, ref localPlayer);
						}
						else
						{
							// We are not in First Person any more, reload old values
							__instance.m_3rdOffset = DynamicPerson.noVPFP_3rdOffset;
							__instance.m_fpsOffset = DynamicPerson.noVPFP_fpsOffset;
							// Set the camera stuff to our constant stored values
							__instance.m_minDistance = CameraConstants.minDistance;
							__instance.m_maxDistance = CameraConstants.maxDistance;
							__instance.m_zoomSens = CameraConstants.zoomSens;
							// Default Field Of View value
							__instance.m_fov = 65f;
							// Make head stuff be normal again
							localPlayer.m_head.localScale = Vector3.one;
							localPlayer.m_eye.localScale = Vector3.one;
						}
					}
					// FOV of cameras needs to be reset too
					__instance.m_camera.fieldOfView = __instance.m_fov;
					__instance.m_skyCamera.fieldOfView = __instance.m_fov;
					if (localPlayer)
					{
						// Same game check
						if ((!Chat.instance || !Chat.instance.HasFocus()) &&
							!Console.IsVisible() && !InventoryGui.IsVisible() &&
							!StoreGui.IsVisible() && !Menu.IsVisible() &&
							!Minimap.IsOpen() && !localPlayer.InCutscene() &&
							!localPlayer.InPlaceMode())
						{
							if (DynamicPerson.isFirstPerson)
							{
								// I dont think either of these two ifs needs explain....
								if (Input.GetKeyDown(Configuration.Current.FirstPerson.raiseFOVHotkey))
								{
									__instance.m_fov += 1f;
									Console.instance.AddString($"Changed fov to: {__instance.m_fov}");
								}
								else if (Input.GetKeyDown(Configuration.Current.FirstPerson.lowerFOVHotkey))
								{
									__instance.m_fov -= 1f;
									Console.instance.AddString($"Changed fov to: {__instance.m_fov}");
								}
							}
							else
							{
								// Not first camera, just do main game thing
								float minDistance = __instance.m_minDistance;
								float axis = Input.GetAxis("Mouse ScrollWheel");
								__instance.m_distance -= axis * __instance.m_zoomSens;
								float max = (localPlayer.GetControlledShip() != null) ? __instance.m_maxDistanceBoat : __instance.m_maxDistance;
								__instance.m_distance = Mathf.Clamp(__instance.m_distance, 0f, max);
							}
						}
						if (localPlayer.IsDead() && localPlayer.GetRagdoll())
						{
							// THe weird death cam actually:
							//  - Breaks body into ragdoll
							//  - Gives momentum to pieces
							//  - Then finds the average point of them all to make camera look at
							__instance.transform.LookAt(localPlayer.GetRagdoll().GetAverageBodyPosition());
						}
						else if (DynamicPerson.isFirstPerson)
						{
							// Place camera at head level, plus a tiny bit more
							__instance.transform.position = localPlayer.m_head.position + new Vector3(0, 0.2f, 0);
						}
						else
						{
							// Not even sure why we need to reset the base transform
							// But game does it so...
							// ¯\_(ツ)_/¯
							Vector3 position;
							Quaternion rotation;
							__instance.GetCameraPosition(dt, out position, out rotation);
							__instance.transform.position = position;
							__instance.transform.rotation = rotation;
						}
						__instance.UpdateCameraShake(dt);
					}
				}
			}
		}

        #endregion

		#endregion
    }
}
