using UnityEngine;

namespace Assets.Scripts.SkiStick
{
    public class SkiStickTip : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform playerTransform;

        [Header("Debugging")]
        [SerializeField] private bool debug;

        private readonly float forceMultiplier = 150f;
        private readonly float forceWeakeningFactor = 0.8f;
        private readonly float forceWeakeningThreshold = 0.5f;
    
        private bool isGrounded = false;
        private Vector3 lastGroundedPosition = Vector3.zero;
        private float force = 0f;


        public float Force
        {
            get { return force; }
        }


        void Start()
        {
            isGrounded = false;
            lastGroundedPosition = Vector3.zero;
            force = 0f;
        }

        void FixedUpdate()
        {
            if (isGrounded)
            {
                Vector3 currentGroundedPosition = playerTransform.InverseTransformPoint(transform.position);
                float offset = (lastGroundedPosition - currentGroundedPosition).z;
                lastGroundedPosition = playerTransform.InverseTransformPoint(transform.position);

                force += offset * forceMultiplier;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case Constants.GroundTag:
                    HandleGroundTriggerEnter();
                    break;
            }
        }

        void OnTriggerStay(Collider other)
        {
            switch (other.tag)
            {
                case Constants.GroundTag:
                    HandleGroundTriggerStay();
                    break;
            }
        }

        void OnTriggerExit(Collider other)
        {
            switch (other.tag)
            {
                case Constants.GroundTag:
                    HandleGroundTriggerExit();
                    break;
            }
        }


        private void HandleGroundTriggerEnter()
        {
            isGrounded = true;
            lastGroundedPosition = playerTransform.InverseTransformPoint(transform.position);
        }

        private void HandleGroundTriggerStay()
        {
            isGrounded = true;
            lastGroundedPosition = playerTransform.InverseTransformPoint(transform.position);
        }

        private void HandleGroundTriggerExit()
        {
            isGrounded = false;
            force = 0f;
        }

        public void WeakenForces()
        {
            force *= forceWeakeningFactor;
            if (Mathf.Abs(force) < forceWeakeningThreshold)
            {
                force = 0f;
            }
        }
    }
}