using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ValheimPlus
{
    class ACM
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

		private static List<KeyValuePair<GameObject, Piece>> placementGhosts = new List<KeyValuePair<GameObject, Piece>>();

		public static GameObject groupAnchor; 

		// States
		public static Boolean isPlacedForAdjustment = false;



		// Hotkey Flags
		private static bool controlFlag = false;
		private static bool shiftFlag = false;
		private static bool altFlag = false;

		// Modification Strenghts
		private static float gDistance = 2;
		private static float gScrollDistance = 2;

		public static void run()
        {
			//Debug.Log("run");
			if (Input.GetKeyDown(KeyCode.Keypad1))
			{
				SelectObject();
			}
			if (Input.GetKeyDown(KeyCode.Keypad2))
			{
				DeselectObject();
			}
			if (Input.GetKeyDown(KeyCode.Keypad3))
			{
				createPlacementGhost();
			}

			if (Input.GetKeyDown(KeyCode.Keypad6))
			{

				if (isPlacedForAdjustment)
					Debug.Log(Vector3.Distance(groupAnchor.transform.position, PlayerInstance.transform.position));
			}

			if (isPlacedForAdjustment)
			{
				runlistenToHotKeysAndDoWork();
			}
		}

		private static void runlistenToHotKeysAndDoWork()
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

			// LEFT ALT PRESSED
			if (Input.GetKeyDown(KeyCode.LeftAlt)) { altFlag = true; }
			if (Input.GetKeyUp(KeyCode.LeftAlt)) { altFlag = false; }

			
			// SCROLL CONTROLS
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				Quaternion rotation;
				if (controlFlag)
				{
					rX++;
					rotation = Quaternion.Euler(groupAnchor.transform.eulerAngles.x + (scrollDistance * (float)rX), groupAnchor.transform.eulerAngles.y, groupAnchor.transform.eulerAngles.z); // forward to backwards
				}
				else
				{
					if (altFlag)
					{
						rZ++;
						rotation = Quaternion.Euler(groupAnchor.transform.eulerAngles.x, groupAnchor.transform.eulerAngles.y, groupAnchor.transform.eulerAngles.z + (scrollDistance * (float)rZ)); // diagonal
					}
					else
					{
						rY++;
						rotation = Quaternion.Euler(groupAnchor.transform.eulerAngles.x, groupAnchor.transform.eulerAngles.y + (scrollDistance * (float)rY), groupAnchor.transform.eulerAngles.z); // left<->right
					}
				}
				groupAnchor.transform.rotation = rotation;
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				Quaternion rotation;
				if (controlFlag)
				{
					rX--;
					rotation = Quaternion.Euler(groupAnchor.transform.eulerAngles.x + (scrollDistance * (float)rX), groupAnchor.transform.eulerAngles.y, groupAnchor.transform.eulerAngles.z); // forward to backwards
				}
				else
				{
					if (altFlag)
					{
						rZ--;
						rotation = Quaternion.Euler(groupAnchor.transform.eulerAngles.x, groupAnchor.transform.eulerAngles.y, groupAnchor.transform.eulerAngles.z + (scrollDistance * (float)rZ)); // diagonal
					}
					else
					{
						rY--;
						rotation = Quaternion.Euler(groupAnchor.transform.eulerAngles.x, groupAnchor.transform.eulerAngles.y + (scrollDistance * (float)rY), groupAnchor.transform.eulerAngles.z); // left<->right
					}
				}

				groupAnchor.transform.rotation = rotation;
			}

			// NUMPAD CONTROL
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				if (controlFlag)
				{
					groupAnchor.transform.Translate(Vector3.up * distance * Time.deltaTime);
				}
				else
				{
					groupAnchor.transform.Translate(Vector3.forward * distance * Time.deltaTime);
				}
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				if (controlFlag)
				{
					groupAnchor.transform.Translate(Vector3.down * distance * Time.deltaTime);
				}
				else
				{
					groupAnchor.transform.Translate(Vector3.back * distance * Time.deltaTime);
				}
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				groupAnchor.transform.Translate(Vector3.left * distance * Time.deltaTime);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				groupAnchor.transform.Translate(Vector3.right * distance * Time.deltaTime);
			}


		}

		private static void changeModificationSpeeds(bool isShiftPressed)
		{
			float incValue = 1;
			if (shiftFlag)
				incValue = 10;

			if (Input.GetKeyDown(KeyCode.KeypadPlus))
			{

				if ((gScrollDistance - incValue) < 360)
					gScrollDistance += incValue;

				if ((gDistance - incValue) < 360)
					gDistance += incValue;
				notifyUser("Modification Speed: " + gDistance);
				Debug.Log("Modification Speed: " + gDistance);
			}
			if (Input.GetKeyDown(KeyCode.KeypadMinus))
			{

				if ((gScrollDistance - incValue) > 0)
					gScrollDistance = gScrollDistance - incValue;

				if ((gDistance - incValue) > 0)
					gDistance = gDistance - incValue;

				notifyUser("Modification Speed: " + gDistance);
				Debug.Log("Modification Speed: " + gDistance);
			}
		}

		private static void notifyUser(string Message, MessageHud.MessageType position = MessageHud.MessageType.TopLeft)
		{
			MessageHud.instance.ShowMessage(position, "ACM: " + Message);
		}

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


		public static void createPlacementGhost()
        {
			Vector3 vector;
			Vector3 up;
			Piece piece;
			Heightmap heightmap;
			Collider x;

			RemoveAllHighlights();
			groupAnchor = GameObject.Instantiate(new GameObject("groupAnchor"));
			Piece anchorPiece = null;
			if (PieceRayTest(out vector, out up, out piece, out heightmap, out x, true))
            {
				foreach (KeyValuePair<GameObject, Piece> piecesToPlace in selectedPieces)
				{
					if (anchorPiece == null) { 
						anchorPiece = piecesToPlace.Value;
					}
					GameObject m_placementMarkerInstance = UnityEngine.Object.Instantiate<GameObject>(piecesToPlace.Value.gameObject, piecesToPlace.Value.transform.position, piecesToPlace.Value.transform.rotation);

					Vector3 distanceToAnchor = anchorPiece.transform.position - piecesToPlace.Value.transform.position;
					
					m_placementMarkerInstance.SetActive(true);
					m_placementMarkerInstance.transform.position = vector - distanceToAnchor;
					m_placementMarkerInstance.gameObject.transform.SetParent(groupAnchor.transform);
		
					groupAnchor.transform.position = HitPoint;

					isPlacedForAdjustment = true;

				}
			}


			clearSelection();
		}

		private static bool PieceRayTest(out Vector3 point, out Vector3 normal, out Piece piece, out Heightmap heightmap, out Collider waterSurface, bool water)
		{
			int layerMask = PlayerInstance.m_placeRayMask;
			if (water)
			{
				layerMask = PlayerInstance.m_placeWaterRayMask;
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(GameCamera.instance.transform.position, GameCamera.instance.transform.forward, out raycastHit, 50f, layerMask) && raycastHit.collider && !raycastHit.collider.attachedRigidbody && Vector3.Distance(PlayerInstance.m_eye.position, raycastHit.point) < PlayerInstance.m_maxPlaceDistance)
			{
				point = raycastHit.point;
				normal = raycastHit.normal;
				piece = raycastHit.collider.GetComponentInParent<Piece>();
				heightmap = raycastHit.collider.GetComponent<Heightmap>();
				if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Water"))
				{
					waterSurface = raycastHit.collider;
				}
				else
				{
					waterSurface = null;
				}
				return true;
			}
			point = Vector3.zero;
			normal = Vector3.zero;
			piece = null;
			heightmap = null;
			waterSurface = null;
			return false;
		}


		public static void changePlacementOfPlacementGhosts()
		{
			foreach (KeyValuePair<GameObject, Piece> piecesToPlace in selectedPieces)
			{
				
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

		public static void RemoveAllHighlights()
		{
			foreach (KeyValuePair<GameObject, List<KeyValuePair<Renderer, Material[]>>> objectEntry in objectAndMaterialArray)
			{
				foreach (KeyValuePair<Renderer, Material[]> keyValuePair in objectEntry.Value)
				{
					if (keyValuePair.Key)
					{
						keyValuePair.Key.materials = keyValuePair.Value;
					}
				}
			}
		}
		public static void clearSelection()
        {
			selectedPieces = new List<KeyValuePair<GameObject, Piece>>();
			objectAndMaterialArray = new List<KeyValuePair<GameObject, List<KeyValuePair<Renderer, Material[]>>>>();
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
