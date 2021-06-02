using System.Linq;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    class ABM
    {
        // Status
        public static bool isActive;

        // Player Instance
        private static Player PlayerInstance;

        // Control Flags
        static bool controlFlag;
        static bool shiftFlag;
        static bool altFlag;

        // Exit flags
        public static bool exitOnNextIteration;
        static bool blockDefaultFunction;

        private static Piece component;

        // Modification Speeds
        const float BASE_TRANSLATION_DISTANCE = (float) 0.1; // 1/10th of a 1m pole
        const float BASE_ROTATION_ANGLE_DEGREES = 3;
        
        static float currentModificationSpeed = 1;
        const float MIN_MODIFICATION_SPEED = 1;
        const float MAX_MODIFICATION_SPEED = 30;

        // Save and Load object rotation
        static Quaternion savedRotation;

        public static void Run(ref Player __instance)
        {
            PlayerInstance = __instance;

            if (AEM.isActive)
            {
                if (isActive)
                {
                    exitMode();
                }

                return;
            }

            if (Input.GetKeyDown(Configuration.Current.AdvancedBuildingMode.exitAdvancedBuildingMode))
            {
                if (isActive)
                {
                    exitMode();
                }

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
                exitMode();

                return;
            }

            // Check if prefab selected (build pieces) & ghost is ready
            if (selectedPrefab() == null || PlayerInstance.m_placementGhost == null)
            {
                if (isActive) 
                { 
                    exitMode();
                }

                return;
            }

            // Check if Build Mode && Correct build mode
            if (isInBuildMode() && IsHoeOrTerrainTool(selectedPrefab()))
            {
                if (isActive)
                {
                    exitMode();
                }

                return;
            }

            if (isActive)
            {
                // Maximum distance between player and placed piece
                if (Vector3.Distance(PlayerInstance.transform.position, component.transform.position) > PlayerInstance.m_maxPlaceDistance)
                {
                    exitMode();
                }

                isRunning();

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
            if (Input.GetKeyDown(KeyCode.LeftShift)) { shiftFlag = true; }
            if (Input.GetKeyUp(KeyCode.LeftShift)) { shiftFlag = false; }
            // LEFT ALT PRESSED
            if (Input.GetKeyDown(KeyCode.LeftAlt)) { altFlag = true; }
            if (Input.GetKeyUp(KeyCode.LeftAlt)) { altFlag = false; }
            
            changeModificationSpeed();

            if (Input.GetKeyUp(Configuration.Current.AdvancedBuildingMode.copyObjectRotation))
            {
                savedRotation = component.transform.rotation;
            }
            if (Input.GetKeyUp(Configuration.Current.AdvancedBuildingMode.pasteObjectRotation))
            {
                component.transform.rotation = savedRotation;
            }

            var currentRotationAngleDegrees = BASE_ROTATION_ANGLE_DEGREES * currentModificationSpeed;
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                Quaternion rotation;
                if (controlFlag)
                {
                    rX++;
                    rotation = Quaternion.Euler(component.transform.eulerAngles.x + (currentRotationAngleDegrees * rX), component.transform.eulerAngles.y, component.transform.eulerAngles.z); // forward to backwards
                }
                else if (altFlag)
                {
                    rZ++;
                    rotation = Quaternion.Euler(component.transform.eulerAngles.x, component.transform.eulerAngles.y, component.transform.eulerAngles.z + (currentRotationAngleDegrees * rZ)); // diagonal
                }
                else
                {
                    rY++;
                    rotation = Quaternion.Euler(component.transform.eulerAngles.x, component.transform.eulerAngles.y + (currentRotationAngleDegrees * rY), component.transform.eulerAngles.z); // left<->right
                }
                component.transform.rotation = rotation;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                Quaternion rotation;
                if (controlFlag)
                {
                    rX--;
                    rotation = Quaternion.Euler(component.transform.eulerAngles.x + (currentRotationAngleDegrees * rX), component.transform.eulerAngles.y, component.transform.eulerAngles.z); // forward to backwards
                }
                else if (altFlag)
                {
                    rZ--;
                    rotation = Quaternion.Euler(component.transform.eulerAngles.x, component.transform.eulerAngles.y, component.transform.eulerAngles.z + (currentRotationAngleDegrees * rZ)); // diagonal
                }
                else
                {
                    rY--;
                    rotation = Quaternion.Euler(component.transform.eulerAngles.x, component.transform.eulerAngles.y + (currentRotationAngleDegrees * rY), component.transform.eulerAngles.z); // left<->right
                }

                component.transform.rotation = rotation;
            }

            var currentTranslationDistance = BASE_TRANSLATION_DISTANCE * currentModificationSpeed;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (controlFlag)
                {
                    component.transform.Translate(Vector3.up * currentTranslationDistance);
                }
                else
                {
                    component.transform.Translate(Vector3.forward * currentTranslationDistance);
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (controlFlag)
                {
                    component.transform.Translate(Vector3.down * currentTranslationDistance);
                }
                else
                {
                    component.transform.Translate(Vector3.back * currentTranslationDistance);
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                component.transform.Translate(Vector3.left * currentTranslationDistance);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                component.transform.Translate(Vector3.right * currentTranslationDistance);
            }

            try
            {
                isValidPlacement();
            }
            catch
            {
            }
        }

        private static void isValidPlacement()
        {
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

            if (component.m_onlyInTeleportArea && !EffectArea.IsPointInsideArea(component.transform.position, EffectArea.Type.Teleport))
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

            if (!PrivateArea.CheckAccess(PlayerInstance.m_placementGhost.transform.position, radius))
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

        private static bool isInBuildMode()
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
                catch
                {
                    return null;
                }
            }

            return null;
        }

        // Is Hoe or Terrain Tool in build mode?
        private static bool IsHoeOrTerrainTool(GameObject selectedPrefab)
        {
            string[] hoePrefabs = { "paved_road", "mud_road", "raise", "path" };
            string[] terrainToolPrefabs = { "cultivate", "replant" };

            if (selectedPrefab.name.ToLower().Contains("sapling"))
            {
                return true;
            }

            if (hoePrefabs.Contains(selectedPrefab.name) || terrainToolPrefabs.Contains(selectedPrefab.name))
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

        private static void changeModificationSpeed()
        {
            float speedDelta = 1;
            if (shiftFlag)
            {
                speedDelta = 10;
            }

            if (Input.GetKeyDown(Configuration.Current.AdvancedBuildingMode.increaseScrollSpeed))
            {
                currentModificationSpeed = Mathf.Clamp(currentModificationSpeed + speedDelta, MIN_MODIFICATION_SPEED,
                    MAX_MODIFICATION_SPEED);

                notifyUser("Modification Speed: " + currentModificationSpeed);
            }

            if (Input.GetKeyDown(Configuration.Current.AdvancedBuildingMode.decreaseScrollSpeed))
            {
                currentModificationSpeed = Mathf.Clamp(currentModificationSpeed - speedDelta, MIN_MODIFICATION_SPEED,
                    MAX_MODIFICATION_SPEED);

                notifyUser("Modification Speed: " + currentModificationSpeed);
            }
        }
    }
}
