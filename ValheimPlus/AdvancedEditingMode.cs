using UnityEngine;
using ValheimPlus.Configurations;
using ValheimPlus.Utility;

namespace ValheimPlus
{
    class AEM
    {
        // Status
        public static bool isActive;

        // Player Instance
        public static Player PlayerInstance;

        // Control Flags
        static bool controlFlag;
        static bool shiftFlag;
        static bool altFlag;

        // Hit Object Data
        public static Vector3 HitPoint;
        public static Vector3 HitNormal;
        public static Piece HitPiece;
        public static GameObject HitObject;
        public static Heightmap HitHeightmap;

        private static Quaternion InitialRotation;
        private static Vector3 InitialPosition;

        private static bool isInExistence;
        
        // Modification Speeds
        const float BASE_TRANSLATION_DISTANCE = (float) 0.1; // 1/10th of a 1m pole
        const float BASE_ROTATION_ANGLE_DEGREES = 3;
        
        static float currentModificationSpeed = 1;
        const float MIN_MODIFICATION_SPEED = 1;
        const float MAX_MODIFICATION_SPEED = 30;

        // Save and Load object rotation
        static Quaternion savedRotation;

        // Executing the raycast to find the object
        public static bool ExecuteRayCast(Player playerInstance)
        {
            int layerMask = playerInstance.m_placeRayMask;
            RaycastHit raycastHit;

            if (Physics.Raycast(
                GameCamera.instance.transform.position,
                GameCamera.instance.transform.forward,
                out raycastHit, 50f, layerMask
                ) &&
                raycastHit.collider &&
                !raycastHit.collider.attachedRigidbody &&
                Vector3.Distance(Helper.getPlayerCharacter(playerInstance).m_eye.position, raycastHit.point) < playerInstance.m_maxPlaceDistance)
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

            resetObjectInfo();
            return false;
        }

        // Exiting variables
        public static bool forceExitNextIteration;


        // Initializing class
        public static bool checkForObject()
        {
            if (PlayerInstance == null)
            {
                return false;
            }

            if (!ExecuteRayCast(PlayerInstance))
            {
                return false;
            }

            return true;
        }

        public static void run()
        {

            // ADD ZNET ERROR HANDLING AND REMOVE OBJECT IF

            // force exit
            if (forceExitNextIteration)
            {
                forceExitNextIteration = false;
                resetObjectInfo();
                isActive = false;
                return;
            }


            // CHECK FOR BUILD MODE
            if (isInBuildMode())
            {
                if (isActive)
                {
                    exitMode();
                    resetObjectTransform();
                }

                return;
            }

            // CHECK FOR ABM
            if (ABM.isActive)
            {
                if (isActive)
                {
                    exitMode();
                    resetObjectTransform();
                }

                return;
            }

            if (!isActive)
            {
                if (Input.GetKeyDown(Configuration.Current.AdvancedEditingMode.enterAdvancedEditingMode))
                {
                    if (checkForObject())
                        startMode();
                    return;
                }
            }

            if (Input.GetKeyDown(Configuration.Current.AdvancedEditingMode.abortAndExitAdvancedEditingMode))
            {
                exitMode();
                resetObjectTransform();
            }

            if (isActive)
            {
                // If object is not in existence anymore
                if (hitPieceStillExists())
                {
                    // Try to prevent znet error, relatively untested yet if this is any solution.
                    // ghetto solution, will be improved in future version if it proofs to be effective.
                    try
                    {
                        ZNetView component1 = HitPiece.GetComponent<ZNetView>();
                        if ((UnityEngine.Object)component1 == (UnityEngine.Object)null)
                        {
                            Debug.Log("AEM: Error, network object empty. Code: 2.");
                            exitMode();
                            return;
                        }
                    }
                    catch
                    {
                        Debug.Log("AEM: Error, network object empty. Code: 3.");
                        exitMode();
                    }

                    isRunning();
                    listenToHotKeysAndDoWork();
                }
                else
                {
                    exitMode();
                }
            }
        }

        private static void listenToHotKeysAndDoWork()
        {
            float rX = 0;
            float rZ = 0;
            float rY = 0;

            if (Input.GetKeyDown(Configuration.Current.AdvancedEditingMode.resetAdvancedEditingMode))
            {
                resetObjectTransform();
            }

            if (Input.GetKeyDown(Configuration.Current.AdvancedEditingMode.confirmPlacementOfAdvancedEditingMode))
            {
                if (isContainer())
                {
                    dropContainerContents();
                }

                // PLACE NEW
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(HitPiece.gameObject, HitPiece.transform.position, HitPiece.transform.rotation);
                HitPiece.m_placeEffect.Create(HitPiece.transform.position, HitPiece.transform.rotation, gameObject2.transform, 1f);

                // REMOVE OLD
                ZNetView component1 = HitPiece.GetComponent<ZNetView>();
                if ((UnityEngine.Object)component1 == (UnityEngine.Object)null) 
                {
                    Debug.Log("AEM: Error, network object empty.");

                    resetObjectTransform();
                    exitMode();
                    return;
                }

                component1.ClaimOwnership();
                ZNetScene.instance.Destroy(HitPiece.gameObject);
                Debug.Log("AEM: Executed.");

                exitMode();
                return;
            }

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

            if (Input.GetKeyUp(Configuration.Current.AdvancedEditingMode.copyObjectRotation))
            {
                savedRotation = HitPiece.transform.rotation;
            }
            if (Input.GetKeyUp(Configuration.Current.AdvancedEditingMode.pasteObjectRotation))
            {
                HitPiece.transform.rotation = savedRotation;
            }

            // Maximum distance between player and placed piece
            if (Vector3.Distance(PlayerInstance.transform.position, HitPiece.transform.position) > PlayerInstance.m_maxPlaceDistance)
            {
                resetObjectTransform();
                exitMode();
            }

            var currentRotationAngleDegrees = BASE_ROTATION_ANGLE_DEGREES * currentModificationSpeed;
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                Quaternion rotation;
                if (controlFlag)
                {
                    rX++;
                    rotation = Quaternion.Euler(HitPiece.transform.eulerAngles.x + (currentRotationAngleDegrees * rX), HitPiece.transform.eulerAngles.y, HitPiece.transform.eulerAngles.z); // forward to backwards
                }
                else if (altFlag)
                {
                    rZ++;
                    rotation = Quaternion.Euler(HitPiece.transform.eulerAngles.x, HitPiece.transform.eulerAngles.y, HitPiece.transform.eulerAngles.z + (currentRotationAngleDegrees * rZ)); // diagonal
                }
                else
                {
                    rY++;
                    rotation = Quaternion.Euler(HitPiece.transform.eulerAngles.x, HitPiece.transform.eulerAngles.y + (currentRotationAngleDegrees * rY), HitPiece.transform.eulerAngles.z); // left<->right
                }
                HitPiece.transform.rotation = rotation;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                Quaternion rotation;
                if (controlFlag)
                {
                    rX--;
                    rotation = Quaternion.Euler(HitPiece.transform.eulerAngles.x + (currentRotationAngleDegrees * rX), HitPiece.transform.eulerAngles.y, HitPiece.transform.eulerAngles.z); // forward to backwards
                }
                else if (altFlag)
                {
                    rZ--;
                    rotation = Quaternion.Euler(HitPiece.transform.eulerAngles.x, HitPiece.transform.eulerAngles.y, HitPiece.transform.eulerAngles.z + (currentRotationAngleDegrees * rZ)); // diagonal
                }
                else
                {
                    rY--;
                    rotation = Quaternion.Euler(HitPiece.transform.eulerAngles.x, HitPiece.transform.eulerAngles.y + (currentRotationAngleDegrees * rY), HitPiece.transform.eulerAngles.z); // left<->right
                }

                HitPiece.transform.rotation = rotation;
            }

            var currentTranslationDistance = BASE_TRANSLATION_DISTANCE * currentModificationSpeed;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (controlFlag)
                {
                    HitPiece.transform.Translate(Vector3.up * currentTranslationDistance);
                }
                else
                {
                    HitPiece.transform.Translate(Vector3.forward * currentTranslationDistance);
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (controlFlag)
                {
                    HitPiece.transform.Translate(Vector3.down * currentTranslationDistance);
                }
                else
                {
                    HitPiece.transform.Translate(Vector3.back * currentTranslationDistance);
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                HitPiece.transform.Translate(Vector3.left * currentTranslationDistance);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                HitPiece.transform.Translate(Vector3.right * currentTranslationDistance);
            }
        }

        // Hit Piece still is a valid target
        private static bool hitPieceStillExists()
        {
            try
            { // check to see if the hit object still exists
                if (isActive)
                {
                    isInExistence = true;
                }
            }
            catch
            {
                isInExistence = false;
            }

            return isInExistence;
        }

        // Check for access to object
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

        // Check if user is in build mode
        private static bool isInBuildMode()
        {
            return PlayerInstance.InPlaceMode();
        }

        private static void resetObjectTransform()
        {
            notifyUser("Object has been reset to initial position & rotation.");
            HitPiece.transform.position = InitialPosition;
            HitPiece.transform.rotation = InitialRotation;
        }

        private static void resetObjectInfo()
        {
            HitPoint = Vector3.zero;
            HitNormal = Vector3.zero;
            HitObject = null;
            HitPiece = null;
            HitHeightmap = null;
            InitialRotation = new Quaternion();
            InitialPosition = new Vector3();
        }

        private static void startMode()
        {
            notifyUser("Entering AEM");
            isActive = true;
        }

        private static void exitMode()
        {
            notifyUser("Exiting AEM");
            forceExitNextIteration = true;
        }

        private static void notifyUser(string Message, MessageHud.MessageType position = MessageHud.MessageType.TopLeft)
        {
            MessageHud.instance.ShowMessage(position, "AEM: " + Message);
        }

        private static void isRunning()
        {
            if (isActive)
            {
                MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "AEM is active");
            }
        }

        private static bool isContainer()
        {
            Container ContainerInstance = HitPiece.GetComponent<Container>();

            return ContainerInstance != null;
        }

        private static void dropContainerContents()
        {
            Container ContainerInstance = HitPiece.GetComponent<Container>();
            ContainerInstance.DropAllItems();
        }

        private static void changeModificationSpeed()
        {
            float speedDelta = 1;
            if (shiftFlag)
            {
                speedDelta = 10;
            }

            if (Input.GetKeyDown(Configuration.Current.AdvancedEditingMode.increaseScrollSpeed))
            {
                currentModificationSpeed = Mathf.Clamp(currentModificationSpeed + speedDelta, MIN_MODIFICATION_SPEED,
                    MAX_MODIFICATION_SPEED);

                notifyUser("Modification Speed: " + currentModificationSpeed);
            }

            if (Input.GetKeyDown(Configuration.Current.AdvancedEditingMode.decreaseScrollSpeed))
            {
                currentModificationSpeed = Mathf.Clamp(currentModificationSpeed - speedDelta, MIN_MODIFICATION_SPEED,
                    MAX_MODIFICATION_SPEED);

                notifyUser("Modification Speed: " + currentModificationSpeed);
            }
        }
    }
}
