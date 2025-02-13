using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    public class LocomotionTechnique : MonoBehaviour
    {
        public GameObject hmd;

        [Header("References")]
        [SerializeField] private Rigidbody playerRigidbody;
        [SerializeField] private Transform tmp;

        [Header("Managers")]
        [SerializeField] private Controller.ControllerManager controllerManager;
        [SerializeField] private SkiStick.SkiStickManager skiStickManager;


        private readonly float maxVelocity = 5f;
        private readonly float forwardDragFactor = 0.99f;
        private readonly float sidewaysDragFactor = 0.8f;
        private readonly float rotationDragFactor = 0.8f;


        // Game mechanism variables.
        public ParkourCounter parkourCounter;
        public string stage;
        public Interaction.ObjectInteraction objectInteraction;


        void FixedUpdate()
        {
            Vector3 currentVelocity = transform.InverseTransformDirection(playerRigidbody.linearVelocity);
            Vector3 updatedVelocity = new(currentVelocity.x * sidewaysDragFactor, 0, currentVelocity.z * forwardDragFactor);

            // Update player's velocity.
		    playerRigidbody.linearVelocity = transform.TransformDirection(updatedVelocity);

            // Update the player's angular velocity.
            playerRigidbody.AddTorque(-playerRigidbody.angularVelocity * rotationDragFactor);

            if ((currentVelocity.z < maxVelocity && skiStickManager.ForwardForce > 0) ||
                (currentVelocity.z > -maxVelocity && skiStickManager.ForwardForce < 0))
            {
                playerRigidbody.AddRelativeForce(Vector3.forward * skiStickManager.ForwardForce);
            }

            // Apply a rotation force on the player.
            playerRigidbody.AddRelativeTorque(Vector3.up * skiStickManager.RotationForce);
            
            // Weaken the forces created by the pushing of the ski sticks.
            skiStickManager.WeakenForces();
        }

        void Update()
        {
            controllerManager.isInTargetShooting = objectInteraction.isInTargetPart;
            controllerManager.isInObjectManipulation = objectInteraction.isInObjectPart;

            if (objectInteraction.isInTargetPart || objectInteraction.isInObjectPart)
            {
                if (objectInteraction.isInObjectPart && controllerManager.endedObjectManipulation)
                {
                    objectInteraction.CalculateManipulationError(
                        controllerManager.GetClosestSkiStickChildrenPositions(
                            objectInteraction.GetTragetSkiStickTransform()));
                }
            }
            else
            {
                Respawn();
            }   
        }

        void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case Constants.BannerTag:
                    HandleBannerTriggerEnter(other);
                    break;

                case Constants.ObjectInteractionTaskTag:
                    HandleObjectInteractionTaskTriggerEnter(other);
                    break;

                case Constants.CoinTag:
                    HandleCoinTriggerEnter(other);
                    break;
            }
        }

        void OnTriggerExit(Collider other)
        {
            switch (other.tag)
            {
                case Constants.ObjectInteractionTaskTag:
                    HandleObjectInteractionTaskTriggerExit(other);
                    break;
            }
        }


        private void Respawn()
        {
            if (OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four))
            {
                if (parkourCounter.parkourStart)
                {
                    transform.position = parkourCounter.currentRespawnPos - new Vector3(0, 0.5f, 0);
                }
                else
                {
                    transform.position = tmp.position;
                    stage = "StartBanner";
                    parkourCounter.isStageChange = true;
                }
            }
        }

        private void HandleBannerTriggerEnter(Collider other)
        {
            stage = other.gameObject.name;
            parkourCounter.isStageChange = true;
        }

        private void HandleObjectInteractionTaskTriggerEnter(Collider other)
        {
            controllerManager.isInObjectInteraction = true;
            objectInteraction.EnterObjectInteraction();
        }

        private void HandleObjectInteractionTaskTriggerExit(Collider other)
        {
            controllerManager.isInObjectInteraction = false;
            objectInteraction.ExitObjectInteraction();
        }

        private void HandleCoinTriggerEnter(Collider other)
        {
            parkourCounter.coinCount += 1;
            GetComponent<AudioSource>().Play();
            other.gameObject.SetActive(false);
        }
    }
}
