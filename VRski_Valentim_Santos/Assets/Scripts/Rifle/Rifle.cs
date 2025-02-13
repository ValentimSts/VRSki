using System;
using Assets.Scripts.Interaction.Task;
using UnityEngine;

namespace Assets.Scripts.Rifle
{
    public class Rifle : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject bulletPrefab;

        [Header("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform grabPointTransform;
        [SerializeField] private Transform bulletSpawnPoint;


        private float bulletSpeed = 40f;
        private float fireRate = 1f;
        

        private bool hasStoredRotation = false;



        void Start()
        {
            hasStoredRotation = false;
            gameObject.SetActive(false);
        }

        public void AttachToController(Transform controller)
        {
            gameObject.SetActive(true);

            rb.isKinematic = true;
            transform.rotation = controller.rotation * Quaternion.Euler(-90f, 90f, 0f);
            transform.SetParent(controller);
            transform.position += controller.position - grabPointTransform.position;
        }

        public void DetachFromController()
        {
            transform.rotation = Quaternion.identity;
            gameObject.SetActive(false);
        }  

        public void Shoot()
        {
            Bullet bullet = CreateNewBullet();
            bullet.Shoot(bulletSpeed);
        }


        private Bullet CreateNewBullet()
        {
            GameObject bulletObject = Instantiate(bulletPrefab, bulletSpawnPoint.position, transform.rotation * Quaternion.Euler(0f, -90f, 0f));
            bulletObject.SetActive(false);
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            return bullet;
        }
    }
}