using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ValheimPlus
{
    class AdvancedBlueprintMode
    {

		public static Player PlayerInstance;

		// Hit Object Data
		public static Vector3 HitPoint;
		public static Vector3 HitNormal;
		public static Piece HitPiece;
		public static GameObject HitObject;
		public static Heightmap HitHeightmap;

		private static Quaternion InitialRotation;
		private static Vector3 InitialPosition;

		private static List<KeyValuePair<Renderer, Material[]>> placementMaterials;

		private static Color highlightColor = new Color(178/255, 102/255, 255/255, 0.8f);

		private static List<Piece> selectedObjects = new List<Piece>();

		public static void SelectObject()
        {
			SetHighlight(true);
		}

		public static void DeselectObject()
		{
			SetHighlight(false);
			
		}

		public static void SetHighlight(bool enabled)
		{
			Renderer[] componentsInChildren = HitPiece.GetComponentsInChildren<Renderer>();
			if (enabled)
			{
				placementMaterials = new List<KeyValuePair<Renderer, Material[]>>();
				foreach (Renderer renderer in componentsInChildren)
				{
					Material[] sharedMaterials = renderer.sharedMaterials;
					placementMaterials.Add(new KeyValuePair<Renderer, Material[]>(renderer, sharedMaterials));
				}
				Renderer[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					foreach (Material material in array[i].materials)
					{
						if (material.HasProperty("_EmissionColor"))
						{
							material.SetColor("_EmissionColor", highlightColor * 0.7f);
						}
						material.color = highlightColor;
					}
				}
				return;
			}
			foreach (KeyValuePair<Renderer, Material[]> keyValuePair in placementMaterials)
			{
				if (keyValuePair.Key)
				{
					keyValuePair.Key.materials = keyValuePair.Value;
				}
			}
			placementMaterials = null;
		}

		private static bool isValidRayCastTarget()
		{
			bool hitValid = true;

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

			return hitValid;
		}

		public static bool ExecuteRayCast()
		{
			int layerMask = PlayerInstance.m_placeRayMask;
			RaycastHit raycastHit;

			if (Physics.Raycast(
				GameCamera.instance.transform.position,
				GameCamera.instance.transform.forward,
				out raycastHit, 50f, layerMask
				) &&
				raycastHit.collider &&
				!raycastHit.collider.attachedRigidbody &&
				Vector3.Distance(Helper.getPlayerCharacter(PlayerInstance).m_eye.position, raycastHit.point) < PlayerInstance.m_maxPlaceDistance)
			{
				HitPoint = raycastHit.point;
				HitNormal = raycastHit.normal;
				HitPiece = raycastHit.collider.GetComponentInParent<Piece>();
				HitObject = raycastHit.collider.gameObject;
				HitHeightmap = raycastHit.collider.GetComponent<Heightmap>();
				InitialRotation = HitPiece.transform.rotation;
				InitialPosition = HitPiece.transform.position;
				Debug.Log(HitPiece.m_name);
				return true;
			}
			else
			{
				return false;
			}
		}

	}
}
