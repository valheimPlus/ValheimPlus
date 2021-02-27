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

		private static List<KeyValuePair<GameObject, List<KeyValuePair<Renderer, Material[]>>>> objectAndMaterialArray = new List<KeyValuePair<GameObject, List<KeyValuePair<Renderer, Material[]>>>>();
		private static List<KeyValuePair<GameObject, Piece>> selectedPieces = new List<KeyValuePair<GameObject, Piece>>();

		private static Color highlightColor = new Color(178/255, 102/255, 255/255, 0.8f);

		private static Piece anchor;

		public static void SelectObject()
        {
            if (ExecuteRayCast())
            {
				
				if (!isSelected()) {
					AddHighlight();
				}
                else
				{
					RemoveHighlight();
				}
				
			}

		}


		public static void DeselectObject()
		{
			if (ExecuteRayCast())
				Debug.Log(isSelected());
		}

		private static Boolean isSelected()
        {
			foreach (KeyValuePair<GameObject, Piece> objectInArray in selectedPieces)
			{
				Piece targetPiece = objectInArray.Value;
				if (targetPiece == HitPiece)
				{
					return true;
				}
			}
			return false;
		}

		public static void AddHighlight()
		{
			List<KeyValuePair<Renderer, Material[]>> placementMaterials = new List<KeyValuePair<Renderer, Material[]>>();
			Renderer[] componentsInChildren = HitPiece.GetComponentsInChildren<Renderer>();

			placementMaterials = new List<KeyValuePair<Renderer, Material[]>>();
			foreach (Renderer renderer in componentsInChildren)
			{
				Material[] sharedMaterials = renderer.sharedMaterials;
				placementMaterials.Add(new KeyValuePair<Renderer, Material[]>(renderer, sharedMaterials));
			}

			// Add to render info arra
			objectAndMaterialArray.Add(new KeyValuePair<GameObject, List<KeyValuePair<Renderer, Material[]>>>(HitObject, placementMaterials));
			selectedPieces.Add(new KeyValuePair<GameObject, Piece>(HitObject, HitPiece));
			Debug.Log(objectAndMaterialArray.Count());
			Debug.Log(selectedPieces.Count());

			Renderer[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (Material material in array[i].materials)
				{
					if (material.HasProperty("_EmissionColor"))
					{
						material.SetColor("_EmissionColor", highlightColor * 0.7f);
					}
					//material.color = highlightColor;
				}
			}
			return;
		}

		public static void RemoveHighlight()
        {
			foreach (KeyValuePair<GameObject, List<KeyValuePair<Renderer, Material[]>>> objectEntry in objectAndMaterialArray)
			{
				// is the correct object in array
				if (HitObject == objectEntry.Key)
				{

					// remove from object array
					objectAndMaterialArray.Remove(objectEntry);

					// reset to the original material
					foreach (KeyValuePair<Renderer, Material[]> keyValuePair in objectEntry.Value)
					{
						if (keyValuePair.Key)
						{
							keyValuePair.Key.materials = keyValuePair.Value;
						}
					}

					//Remove from piece array
					foreach (KeyValuePair<GameObject, Piece> objectInArray in selectedPieces)
					{
						if (HitObject == objectInArray.Key)
						{
							selectedPieces.Remove(objectInArray);
							return;
						}
					}

					return;
				}
			}
		}

		private static void removeObjectFromSelection(GameObject target) { 
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
