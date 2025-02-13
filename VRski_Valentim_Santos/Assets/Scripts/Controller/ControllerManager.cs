using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class ControllerManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Controller leftController;
        [SerializeField] private Controller rightController;
        [SerializeField] private SkiStick.SkiStick leftSkiStick;
        [SerializeField] private SkiStick.SkiStick rightSkiStick;
        [SerializeField] private Rifle.Rifle rifle;


        private OVRInput.InputDeviceShowState defaultControllerShowState;
        private bool areSkiSticksGrabbed = false;
        private bool isRifleGrabbed = false;
        private Controller closestController;
        private Controller furthestController;

    
        public bool isInObjectInteraction = false;
        public bool isInTargetShooting = false;
        public bool isInObjectManipulation = false;

        public bool shotRifle = false;
        public bool endedObjectManipulation = false;


        void Start()
        {
            // Check if the ski sticks are on the correct side   
            // (left or right) and if the controllers are the.
            if (leftSkiStick.Side != SkiStick.Side.Left)
            {
                Debug.LogError("ERROR - [ControllerManager] The left ski stick is not setup on the left side.");
            }

            if (rightSkiStick.Side != SkiStick.Side.Right)
            {
                Debug.LogError("ERROR - [ControllerManager] The right ski stick is not setup on the right side.");
            }

            defaultControllerShowState = leftController.GetComponentInChildren<OVRControllerHelper>().m_showState;
            areSkiSticksGrabbed = false;
            isRifleGrabbed = false;
            closestController = null;
            furthestController = null;

            shotRifle = false;
            endedObjectManipulation = false;
            isInObjectInteraction = false;
            isInObjectManipulation = false;

            GrabSkiSticks();
        }

        void Update()
        {
            closestController = GetClosestControllerToPlayer();
            furthestController = closestController.Side == Side.Left ? rightController : leftController;

            closestController.SetAsClosestToPlayer();
            furthestController.SetAsFurthestFromPlayer();

            if (isInObjectInteraction)
            {
                if (isInObjectManipulation)
                {
                    if (areSkiSticksGrabbed)
                    {
                        endedObjectManipulation = closestController.IsEndObjectInteractionButtonPressed ||
                            furthestController.IsEndObjectInteractionButtonPressed;
                    }
                    else
                    {
                        GrabSkiSticks();
                    }
                }
                else if (isInTargetShooting)
                {
                    if (areSkiSticksGrabbed) TryToGrabRifle();
                    if (isRifleGrabbed) TryToGrabSkiSticks();

                    if (isRifleGrabbed)
                    {
                        shotRifle = closestController.IsShootRifleButtonPressed;
                        
                        if (shotRifle)
                        {
                            rifle.Shoot();
                        }
                    }
                }
            }
            else
            {
                GrabSkiSticks();
            }
        }


        public List<Vector3> GetClosestSkiStickChildrenPositions(Transform transform)
        {
            float leftSkiStickDistance = Vector3.Distance(transform.position, leftSkiStick.transform.position);
            float rightSkiStickDistance = Vector3.Distance(transform.position, rightSkiStick.transform.position);

            return leftSkiStickDistance < rightSkiStickDistance ? 
                leftSkiStick.GetChildrenPositions() : 
                rightSkiStick.GetChildrenPositions();
        }


        private void TryToGrabRifle()
        {
            if (closestController == null || furthestController == null)
            {
                return;
            }

            if (closestController.IsOtherControllerInCollider)
            {
                GrabRifle(closestController);
            }
        }

        private void TryToGrabSkiSticks()
        {
            if (closestController == null)
            {
                return;
            } 

            if (!closestController.IsOtherControllerInCollider)
            {
                GrabSkiSticks();
            }
        }


        private void GrabSkiSticks()
        {
            if (areSkiSticksGrabbed)
            {
                return;
            }

            if (isRifleGrabbed)
            {
                ReleaseRifle();
            }

            HideControllerVisuals(leftController.transform);
            HideControllerVisuals(rightController.transform);

            leftSkiStick.AttachToController(leftController.transform);
            rightSkiStick.AttachToController(rightController.transform);

            areSkiSticksGrabbed = true;
        }

        private void ReleaseSkiSticks()
        {
            if (!areSkiSticksGrabbed)
            {
                return;
            }

            ShowControllerVisuals(leftController.transform);
            ShowControllerVisuals(rightController.transform);

            leftSkiStick.DetachFromController();
            rightSkiStick.DetachFromController();

            areSkiSticksGrabbed = false;            
        }

        private void GrabRifle(Controller controller)
        {
            if (isRifleGrabbed)
            {
                return;
            }

            if (areSkiSticksGrabbed)
            {
                ReleaseSkiSticks();
            }

            HideControllerVisuals(leftController.transform);
            HideControllerVisuals(rightController.transform);

            rifle.AttachToController(controller.transform);

            isRifleGrabbed = true;
        }

        private void ReleaseRifle()
        {
            if (!isRifleGrabbed)
            {
                return;
            }

            ShowControllerVisuals(leftController.transform);
            ShowControllerVisuals(rightController.transform);

            rifle.DetachFromController();

            isRifleGrabbed = false;
        }


        private Controller GetClosestControllerToPlayer()
        {
            float leftControllerDistance = Vector3.Distance(playerTransform.position, leftController.transform.position);
            float rightControllerDistance = Vector3.Distance(playerTransform.position, rightController.transform.position);

            return leftControllerDistance < rightControllerDistance ? leftController : rightController;
        }


        private void HideControllerVisuals(Transform controller)
        {
            controller.GetComponentInChildren<OVRControllerHelper>().m_showState =
                OVRInput.InputDeviceShowState.ControllerNotInHand;
        }

        private void ShowControllerVisuals(Transform controller)
        {
            controller.GetComponentInChildren<OVRControllerHelper>().m_showState =
                defaultControllerShowState;
        }
    }
}