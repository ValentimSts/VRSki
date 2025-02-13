using Oculus.Interaction.Samples;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class Controller : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private OVRInput.Controller controller;
        [SerializeField] private Collider controllerCollider;
        [SerializeField] private Collider rifleCheckCollider;

        [Header("Settings")]
        [SerializeField] private Side side;


        private readonly float buttonPressThreshold = 0.95f;
        private bool isOtherControllerInCollider;


        private ControllerAction respawnAction;
        private ControllerAction shootRifleAction;
        private ControllerAction endObjectInteractionAction;

        private bool isRespawnButtonPressed;
        private bool isShootRifleButtonPressed;
        private bool isEndObjectInteractionButtonPressed;

        private bool hasPressedButton;


        public Side Side => side;
        public bool IsOtherControllerInCollider => isOtherControllerInCollider;

        public bool IsRespawnButtonPressed => isRespawnButtonPressed;
        public bool IsShootRifleButtonPressed => isShootRifleButtonPressed;
        public bool IsEndObjectInteractionButtonPressed => isEndObjectInteractionButtonPressed;


        void Start()
        {
            controllerCollider.enabled = false;
            rifleCheckCollider.enabled = false;
            isOtherControllerInCollider = false;

            hasPressedButton = false;

            isRespawnButtonPressed = false;
            isShootRifleButtonPressed = false;
            isEndObjectInteractionButtonPressed = false;

            respawnAction = new ControllerAction(Action.Respawn, side);
            shootRifleAction = new ControllerAction(Action.ShootRifle, side);
            endObjectInteractionAction = new ControllerAction(Action.EndObjectInteraction, side);
        }

        void Update()
        {
            // Shooting only counts if the player presses the button and then releases it.
            if (!hasPressedButton)
            {
                hasPressedButton = IsButtonPressed(shootRifleAction);
                if (hasPressedButton)
                {
                    isShootRifleButtonPressed = true;
                }
            }
            else if (hasPressedButton && isShootRifleButtonPressed)
            {
                isShootRifleButtonPressed = false;
            }
            
            if (hasPressedButton && !isShootRifleButtonPressed)
            {
                hasPressedButton = !IsButtonUp(shootRifleAction);
            }

            isRespawnButtonPressed = IsButtonPressed(respawnAction);
            isEndObjectInteractionButtonPressed = IsButtonPressed(endObjectInteractionAction);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.ControllerTag))
            {
                isOtherControllerInCollider = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Constants.ControllerTag))
            {
                isOtherControllerInCollider = false;
            }
        }


        public void SetAsClosestToPlayer()
        {
            controllerCollider.enabled = false;
            rifleCheckCollider.enabled = true;
        }

        public void SetAsFurthestFromPlayer()
        {
            controllerCollider.enabled = true;
            rifleCheckCollider.enabled = false;
        }


        private bool IsButtonPressed(ControllerAction action)
        {
            return action.ButtonType switch
            {
                ButtonType.Button => OVRInput.Get((OVRInput.Button)action.ButtonValue, controller),
                ButtonType.Trigger => OVRInput.Get((OVRInput.Axis1D)action.ButtonValue, controller) > buttonPressThreshold,

                // This should never happen.
                _ => false,
            };            
        }

        private bool IsButtonUp(ControllerAction action)
        {
            return action.ButtonType switch
            {
                ButtonType.Button => OVRInput.GetUp((OVRInput.Button)action.ButtonValue, controller),
                ButtonType.Trigger => OVRInput.Get((OVRInput.Axis1D)action.ButtonValue, controller) <= buttonPressThreshold,

                // This should never happen.
                _ => false,
            };
        }
    }
}
