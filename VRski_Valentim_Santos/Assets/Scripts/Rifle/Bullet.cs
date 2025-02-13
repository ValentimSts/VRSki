using UnityEngine;

namespace Assets.Scripts.Interaction.Task
{
    public class Bullet : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody rb;
        

        void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case Constants.GroundTag:
                    Destroy();
                    break;

                default:
                    break;
            }
        }
        

        public void Shoot(float speed)
        {
            gameObject.SetActive(true);
            rb.isKinematic = false;

            rb.AddForce(transform.forward * speed, ForceMode.Impulse);
            Destroy(gameObject, 5f);
        }


        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}