using UnityEngine;

namespace Assets.Scripts.SkiStick
{
    public class SkiStickManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SkiStick leftSkiStick;
        [SerializeField] private SkiStick rightSkiStick;

        [Header("Debugging")]
        [SerializeField] private bool debug;

        private readonly float forwardForceClamp = 40f;
        private readonly float rotationForceClamp = 10f;
        private readonly float rotationDampingFactor = 0.2f;

        private float forwardForce;
        private float rotationForce;


        public float ForwardForce
        {
            get { return forwardForce; }
        }

        public float RotationForce
        {
            get { return rotationForce; }
        }


        void Start()
        {
            forwardForce = 0f;
            rotationForce = 0f;
        }

        void FixedUpdate()
        {
            forwardForce = Mathf.Min(leftSkiStick.Tip.Force + rightSkiStick.Tip.Force, forwardForceClamp);
            rotationForce = Mathf.Min((leftSkiStick.Tip.Force - rightSkiStick.Tip.Force) * rotationDampingFactor, rotationForceClamp);
        }


        public void WeakenForces()
        {
            leftSkiStick.Tip.WeakenForces();
            rightSkiStick.Tip.WeakenForces();
        }
    }
}