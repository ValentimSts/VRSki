using UnityEngine;

namespace Assets.Scripts.Interaction.Task
{
    public class Target : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject brokenTargetPrefab;


        private bool hasBeenHit = false;


        public bool HasBeenHit => hasBeenHit;


        void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case Constants.BulletTag:
                    hasBeenHit = true;
                    gameObject.SetActive(false);
                    break;

                default:
                    break;
            }
        }


        public void Init(Vector3 position, Transform playerTransform)
        {
            transform.position = position;
            transform.LookAt(playerTransform);
            transform.Rotate(new Vector3(0f, 180f, 0f));
            gameObject.SetActive(false);
        } 

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Break()
        {
            // Instantiate(brokenBoxPrefab, transform.position, transform.rotation);
            Destroy();
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}