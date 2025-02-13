using UnityEngine;
using TMPro;
using Meta.XR.ImmersiveDebugger.UserInterface;

namespace Assets.Scripts.Interaction.TaskUI
{
    public class Panel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text text;


        private bool hasBeenHit;
        private bool isCountdown;


        public bool HasBeenHit => hasBeenHit;
        public bool IsCountdown 
        {
            set => isCountdown = value;
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.SkiStickTipTag))
            {
                if (!isCountdown)
                {
                    hasBeenHit = true;
                }
            }
        }


        public void Activate()
        {
            hasBeenHit = false;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            hasBeenHit = false;
            gameObject.SetActive(false);
        }

        public void DeactivateButKeepHit()
        {
            gameObject.SetActive(false);
        }

        public void SetText(string newText)
        {
            text.text = newText;
        }
    }
}