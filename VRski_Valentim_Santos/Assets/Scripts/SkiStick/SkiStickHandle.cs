using UnityEngine;

namespace Assets.Scripts.SkiStick
{
    public class SkiStickHandle : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform grabPointTransform;

        [Header("Debugging")]
        [SerializeField] private bool debug;


        public Transform GrabPointTransform => grabPointTransform;
    }
}