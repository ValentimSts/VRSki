using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SkiStick
{
    public class SkiStick : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private SkiStickHandle handle;
        [SerializeField] private SkiStickTip tip;

        [Header("Settings")]
        [SerializeField] private Side side;

        [Header("Debugging")]
        [SerializeField] private bool debug;


        private bool hasBeenAttachedToController = false;


        public SkiStickHandle Handle => handle;
        public SkiStickTip Tip => tip;
        public Side Side => side;


        void Start()
        {
            hasBeenAttachedToController = false;
            gameObject.SetActive(false);
        } 
        

        public void AttachToController(Transform controller)
        {
            gameObject.SetActive(true);

            // Hacky solution, but it works. Essentially, the
            // ski sticks stay attached to the controllers
            // and only their active state changes, giving the
            // impression that they are being grabbed and released.
            if (!hasBeenAttachedToController)
            {
                hasBeenAttachedToController = true;
                rb.isKinematic = true;
                transform.SetParent(controller);
                transform.position += controller.position - handle.GrabPointTransform.position;
            }
        }

        public void DetachFromController()
        {
            gameObject.SetActive(false);
        }        

        public List<Vector3> GetChildrenPositions()
        {
            List<Vector3> childrenPositions = new();

            foreach (Transform child in transform)
            {
                childrenPositions.Add(child.position);
            }

            return childrenPositions;
        }
    }
} 

