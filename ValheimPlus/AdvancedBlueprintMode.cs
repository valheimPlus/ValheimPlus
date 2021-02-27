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

		private static List<KeyValuePair<Piece, List<KeyValuePair<Renderer, Material[]>>>> placementMaterialArray = new List<KeyValuePair<Piece, List<KeyValuePair<Renderer, Material[]>>>>();

		private static Color highlightColor = new Color(178/255, 102/255, 255/255, 0.8f);

		private static List<Piece> selectedObjects = new List<Piece>();

		public static void SelectObject()
        {
            if (ExecuteRayCast())
            {
				if (!isSelected())
					SetHighlight(true);
				else
					RemoveHighlight();
			}

		}

		public static void DeselectObject()
		{
			if (ExecuteRayCast())
				if (isSelected())
					return;
				
		}

		private static Boolean isSelected()
        {
			foreach (KeyValuePair<Piece, List<KeyValuePair<Renderer, Material[]>>> pieceEntry in placementMaterialArray)
			{
				if (pieceEntry.Key == HitPiece)
				{
					return true;
				}
			}
			return false;
		}

		public static void SetHighlight(bool enabled)
		{
			List<KeyValuePair<Renderer, Material[]>> placementMaterials = new List<KeyValuePair<Renderer, Material[]>>();
			Renderer[] componentsInChildren = HitPiece.GetComponentsInChildren<Renderer>();

			if (enabled)
			{
				placementMaterials = new List<KeyValuePair<Renderer, Material[]>>();
				foreach (Renderer renderer in componentsInChildren)
				{
					Material[] sharedMaterials = renderer.sharedMaterials;
					placementMaterials.Add(new KeyValuePair<Renderer, Material[]>(renderer, sharedMaterials));
				}

				// Add to render info arra
				placementMaterialArray.Add(new KeyValuePair<Piece, List<KeyValuePair<Renderer, Material[]>>>(HitPiece, placementMaterials));
				Debug.Log(placementMaterialArray.Count());

				Renderer[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					foreach (Material material in array[i].materials)
					{
						if (material.HasProperty("_EmissionColor")){
							material.SetColor("_EmissionColor", highlightColor * 0.7f);
						}
						//material.color = highlightColor;
					}
				}
				return;
			}
			
		}

		public static void RemoveHighlight()
        {
			foreach (KeyValuePair<Piece, List<KeyValuePair<Renderer, Material[]>>> pieceEntry in placementMaterialArray)
			{
				if (pieceEntry.Key == HitPiece)
				{
					foreach (KeyValuePair<Renderer, Material[]> keyValuePair in pieceEntry.Value)
					{
						if (keyValuePair.Key)
						{
							keyValuePair.Key.materials = keyValuePair.Value;
						}
					}

				}
			}
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
				return isValidRayCastTarget();
			}
			else
			{
				return false;
			}
		}

	}
}
