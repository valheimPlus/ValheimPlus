using HarmonyLib;
using System;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    class AdvancedEditingMode
    {
        [HarmonyPatch(typeof(Player), "Update")]
        public static class AdvancedEditingModeHook
        {
            private static void Postfix(Player __instance)
            {
                if (Configuration.Current.AdvancedEditingMode.IsEnabled)
                {
                    AEM.PlayerInstance = __instance;
                    AEM.run();
                }
            }
        }

        [HarmonyPatch(typeof(GameCamera), "UpdateCamera")]
        public static class BlockCameraScrollInAEM
        {

            private static void Prefix(GameCamera __instance)
            {;
                
                if (AEM.isActive)
                {
                    __instance.m_maxDistance = __instance.m_distance;
                    __instance.m_minDistance = __instance.m_distance;
                }
                else
                {
                    if(Configuration.Current.Camera.IsEnabled)
                    {
                        if(Configuration.Current.Camera.cameraMaximumZoomDistance >= 1 && Configuration.Current.Camera.cameraMaximumZoomDistance <= 100)
                            __instance.m_maxDistance = Configuration.Current.Camera.cameraMaximumZoomDistance;
                        if (Configuration.Current.Camera.cameraBoatMaximumZoomDistance >= 1 && Configuration.Current.Camera.cameraBoatMaximumZoomDistance <= 100)
                            __instance.m_maxDistanceBoat = Configuration.Current.Camera.cameraBoatMaximumZoomDistance;
                        if (Configuration.Current.Camera.cameraFOV >= 1 && Configuration.Current.Camera.cameraFOV <= 140)
                            __instance.m_fov = Configuration.Current.Camera.cameraFOV;

                        __instance.m_minDistance = 1;
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

    class AEM
    {
        // Status
        public static Boolean isActive = false;

        // Player Instance
        public static Player PlayerInstance;

        // Hit Object Data
        public static Vector3 HitPoint;
        public static Vector3 HitNormal;
        public static Piece HitPiece;
        public static GameObject HitObject;
        public static Heightmap HitHeightmap;

        private static Quaternion InitialRotation;
        private static Vector3 InitialPosition;

        private static Boolean isInExsistence;

        // Hotkey Flags
        private static Boolean controlFlag = false;
        private static Boolean shiftFlag = false;
        private static Boolean altFlag = false;

        // Modification Strenghts
        private static float gDistance = 2;
        private static float gScrollDistance = 2;

        // Executing the raycast to find the object
        public static Boolean ExecuteRayCast(Player playerInstance)
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
            else
            {
                resetObjectInfo();
                return false;
            }
        }

        // Exiting variables
        public static Boolean forceExitNextIteration = false;


        // Initializing class
        public static Boolean checkForObject()
        {
            if (PlayerInstance == null)
                return false;

            if (!ExecuteRayCast(PlayerInstance))
                return false;

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
                if (isActive) { 
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
                // If object is not in exsistence anymore
                if (hitPieceStillExsists())
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
                    catch (Exception e) {
                        Debug.Log("AEM: Error, network object empty. Code: 3.");
                        exitMode();
                    }

                    AEM.isRunning();
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
                    dropContainerContents();

                // PLACE NEW
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(HitPiece.gameObject, HitPiece.transform.position, HitPiece.transform.rotation);
                HitPiece.m_placeEffect.Create(HitPiece.transform.position, HitPiece.transform.rotation, gameObject2.transform, 1f);

                // REMOVE OLD
                ZNetView component1 = HitPiece.GetComponent<ZNetView>();
                if ((UnityEngine.Object)component1 == (UnityEngine.Object)null) {
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
            float distance = gDistance;
            float scrollDistance = gScrollDistance;
            if (Input.GetKeyDown(KeyCode.LeftShift)) { shiftFlag = true; }
            if (Input.GetKeyUp(KeyCode.LeftShift)) { shiftFlag = false; }
            changeModificationSpeeds(shiftFlag);
            if (shiftFlag) { distance = gDistance * 3; scrollDistance = gScrollDistance * 3; } else { distance = gDistance; scrollDistance = gScrollDistance; }

            // LEFT ALT PRESSED
            if (Input.GetKeyDown(KeyCode.LeftAlt)) { altFlag = true; }
            if (Input.GetKeyUp(KeyCode.LeftAlt)) { altFlag = false; }


            // Maximum distance between player and placed piece
            if (Vector3.Distance(PlayerInstance.transform.position, HitPiece.transform.position) > PlayerInstance.m_maxPlaceDistance)
            {
                resetObjectTransform();
                exitMode();
            }

            // SCROLL CONTROLS
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                Quaternion rotation;
                if (controlFlag)
                {
                    rX++;
                    rotation = Quaternion.Euler(HitPiece.transform.eulerAngles.x + (scrollDistance * (float)rX), HitPiece.transform.eulerAngles.y, HitPiece.transform.eulerAngles.z); // forward to backwards
                }
                else
                {
                    if (altFlag)
                    {
                        rZ++;
                        rotation = Quaternion.Euler(HitPiece.transform.eulerAngles.x, HitPiece.transform.eulerAngles.y, HitPiece.transform.eulerAngles.z + (scrollDistance * (float)rZ)); // diagonal
                    }
                    else
                    {
                        rY++;
                        rotation = Quaternion.Euler(HitPiece.transform.eulerAngles.x, HitPiece.transform.eulerAngles.y + (scrollDistance * (float)rY), HitPiece.transform.eulerAngles.z); // left<->right
                    }
                }
                HitPiece.transform.rotation = rotation;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                Quaternion rotation;
                if (controlFlag)
                {
                    rX--;
                    rotation = Quaternion.Euler(HitPiece.transform.eulerAngles.x + (scrollDistance * (float)rX), HitPiece.transform.eulerAngles.y, HitPiece.transform.eulerAngles.z); // forward to backwards
                }
                else
                {
                    if (altFlag)
                    {
                        rZ--;
                        rotation = Quaternion.Euler(HitPiece.transform.eulerAngles.x, HitPiece.transform.eulerAngles.y, HitPiece.transform.eulerAngles.z + (scrollDistance * (float)rZ)); // diagonal
                    }
                    else
                    {
                        rY--;
                        rotation = Quaternion.Euler(HitPiece.transform.eulerAngles.x, HitPiece.transform.eulerAngles.y + (scrollDistance * (float)rY), HitPiece.transform.eulerAngles.z); // left<->right
                    }
                }

                HitPiece.transform.rotation = rotation;
            }

            // NUMPAD CONTROL
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (controlFlag)
                {
                    HitPiece.transform.Translate(Vector3.up * distance * Time.deltaTime);
                }
                else
                {
                    HitPiece.transform.Translate(Vector3.forward * distance * Time.deltaTime);
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (controlFlag)
                {
                    HitPiece.transform.Translate(Vector3.down * distance * Time.deltaTime);
                }
                else
                {
                    HitPiece.transform.Translate(Vector3.back * distance * Time.deltaTime);
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                HitPiece.transform.Translate(Vector3.left * distance * Time.deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                HitPiece.transform.Translate(Vector3.right * distance * Time.deltaTime);
            }
        }

        // Hit Piece still is a valid target
        private static Boolean hitPieceStillExsists()
        {
            try
            { // check to see if the hit object still exsists
                if (isActive)
                {
                    Vector3 exsistenceCheck = HitPiece.transform.position;
                    isInExsistence = true;
                }
            }
            catch (Exception e)
            {
                isInExsistence = false;
            }
            return isInExsistence;
        }

        // Check for access to object
        private static Boolean isValidRayCastTarget()
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

            return hitValid;
        }

        // Check if user is in build mode
        private static Boolean isInBuildMode()
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

        private static Boolean isContainer()
        {
            Container ContainerInstance = HitPiece.GetComponent<Container>();

            if (ContainerInstance != null)
            {
                return true;
            }
            return false;
        }

        private static Boolean dropContainerContents()
        {
            Container ContainerInstance = HitPiece.GetComponent<Container>();
            ContainerInstance.DropAllItems();
            return true;
        }

        private static void changeModificationSpeeds(Boolean isShiftPressed)
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


    }
}
