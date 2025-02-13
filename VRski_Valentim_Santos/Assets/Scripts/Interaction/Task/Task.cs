using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interaction.Task
{
    public class Task : MonoBehaviour
    {   
        [Header("Prefabs")]
        [SerializeField] private GameObject targetPrefab;

        [Header("Settings")]
        [SerializeField] private int targetCount;


        private List<Target> currentTargets;


        public int TargetCount => targetCount;


        public void StartNewTask(Transform playerTransform)
        {
            float minRadius = 2f;
            float maxRadius = 5f;

            float minNegativeXPos = (playerTransform.position + playerTransform.right * -1 * minRadius).x;
            float maxNegativeXPos = (playerTransform.position + playerTransform.right * -1 * maxRadius).x;
            float minPositiveXPos = (playerTransform.position + playerTransform.right * minRadius).x;
            float maxPositiveXPos = (playerTransform.position + playerTransform.right * maxRadius).x;

            float minYPos = playerTransform.up.y;
            float maxYPos = (playerTransform.position + playerTransform.up * maxRadius).y;

            float minNegativeZPos = (playerTransform.position + playerTransform.forward * -1 * minRadius).z;
            float maxNegativeZPos = (playerTransform.position + playerTransform.forward * -1 * maxRadius).z;
            float minPositiveZPos = (playerTransform.position + playerTransform.forward * minRadius).z;
            float maxPositiveZPos = (playerTransform.position + playerTransform.forward * maxRadius).z;

            List<Vector3> targetPositions = new();

            // Generate targetSpeeds.Length DIFFERENT random positions for the targets.
            for (int i = 0; i < targetCount; i++)
            {
                float negativeXPos = Random.Range(minNegativeXPos, maxNegativeXPos);
                float positiveXPos = Random.Range(minPositiveXPos, maxPositiveXPos);
                // Pick either the negative or positive x position.
                float xPos = Random.Range(0, 2) == 0 ? negativeXPos : positiveXPos;

                float yPos = Random.Range(minYPos, maxYPos);

                float negativeZPos = Random.Range(minNegativeZPos, maxNegativeZPos);
                float positiveZPos = Random.Range(minPositiveZPos, maxPositiveZPos);
                // Pick either the negative or positive z position.
                float zPos = Random.Range(0, 2) == 0 ? negativeZPos : positiveZPos;


                if (targetPositions.Count != 0)
                {
                    // Make sure the target positions are not too close to each other.
                    for (int j = 0; j < targetPositions.Count; j++)
                    {
                        while (Vector3.Distance(new Vector3(xPos, yPos, zPos), targetPositions[j]) < 3f)
                        {
                            negativeXPos = Random.Range(minNegativeXPos, maxNegativeXPos);
                            positiveXPos = Random.Range(minPositiveXPos, maxPositiveXPos);
                            xPos = Random.Range(0, 2) == 0 ? negativeXPos : positiveXPos;

                            yPos = Random.Range(minYPos, maxYPos);

                            negativeZPos = Random.Range(minNegativeZPos, maxNegativeZPos);
                            positiveZPos = Random.Range(minPositiveZPos, maxPositiveZPos);
                            zPos = Random.Range(0, 2) == 0 ? negativeZPos : positiveZPos;
                        }
                    }
                }

                targetPositions.Add(new Vector3(xPos, yPos, zPos));
            }

            currentTargets = new();
            for (int i = 0; i < targetCount; i++)
            {
                Target newTarget = CreateNewTarget(targetPositions[i], playerTransform);
                currentTargets.Add(newTarget);
            }

            foreach (Target target in currentTargets)
            {
                target.Activate();
            }
        }

        public bool HasTaskEnded()
        {
            foreach (Target target in currentTargets)
            {
                if (!target.HasBeenHit)
                {
                    return false;
                }
            }

            // Destroy all targets and return true.
            foreach (Target target in currentTargets)
            {
                target.Destroy();
            }

            return true;
        }


        private Target CreateNewTarget(Vector3 position, Transform playerTransform)
        {
            GameObject targetObject = Instantiate(targetPrefab.gameObject);
            targetObject.SetActive(false);
            targetObject.transform.SetParent(transform);

            Target target = targetObject.GetComponent<Target>();    
            target.Init(position, playerTransform);

            return target;
        }
    }
}