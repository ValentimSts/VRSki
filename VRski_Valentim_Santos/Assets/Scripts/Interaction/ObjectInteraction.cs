using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace Assets.Scripts.Interaction
{
    public class ObjectInteraction : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private TargetSkiStick targetSkiStickPrefab;

        [Header("References")]
        [SerializeField] private Transform centerEyeAnchor;
        [SerializeField] private TaskUI.TaskUI taskUI;
        [SerializeField] private Task.Task task;
        [SerializeField] private ParkourCounter parkourCounter;
        [SerializeField] private DataRecording dataRecording;

        
        private TargetSkiStick currentTargetSkiStick;


        public TMP_Text scoreText;
        public float partSumTime;
        public float partSumErr;


        // First part of the object interaction, where
        // the user has to hit the targets.
        public bool isInTargetPart;
        // Second part of the object interaction, where
        // the user has to manipulate an object. to match
        // the target object.
        public bool isInObjectPart;
        private int completeTaskCount;
        private List<float> targetShootTimes;
        private Vector3 manipulationError;


        void Start()
        {
            isInTargetPart = false;
            isInObjectPart = false;
            completeTaskCount = 0;
            manipulationError = Vector3.zero;
            targetShootTimes = new();
        }

        void Update()
        {
            if (isInTargetPart && taskUI.HasTaskEnded)
            {
                completeTaskCount++;

                if (completeTaskCount == task.TargetCount)
                {
                    LogTargetTaskData();
                    isInTargetPart = false;
                    isInObjectPart = true;
                    completeTaskCount = 0;

                    StartObjectManipulation();
                    taskUI.EndTask();
                }
                else if (completeTaskCount == task.TargetCount - 1)
                {
                    LogTargetTaskData();
                    taskUI.StartLastTask();
                }
                else
                {
                    LogTargetTaskData();
                    taskUI.StartIntermidiateTask();
                }
            }
        }


        public void EnterObjectInteraction()
        {
            isInTargetPart = true;
            isInObjectPart = false;
            completeTaskCount = 0;
            manipulationError = Vector3.zero;
            targetShootTimes.Clear();

            taskUI.StartFirstTask();
        }

        public void ExitObjectInteraction()
        {
            isInTargetPart = false;
            isInObjectPart = false;
            completeTaskCount = 0;
            manipulationError = Vector3.zero;
            targetShootTimes.Clear();
            scoreText.text = "";

            taskUI.EndTask();
        }

        public Transform GetTragetSkiStickTransform()
        {
            return currentTargetSkiStick.transform;
        }   

        public void CalculateManipulationError(List<Vector3> skiStickChildPositions)
        {
            manipulationError = Vector3.zero;

            for (int i = 0; i < currentTargetSkiStick.GetChildCount(); i++)
            {
                manipulationError += currentTargetSkiStick.GetChild(i).transform.position - skiStickChildPositions[i];
            }

            isInObjectPart = false;

            LogObjectManipulationTaskData();
        }


        private void LogTargetTaskData()
        {
            targetShootTimes.Add(taskUI.LastTaskTimer);

            if (scoreText.text == null)
            {
                scoreText.text += "#" + completeTaskCount + " Time: " + taskUI.LastTaskTimer.ToString("F1");
            }
            else
            {
                scoreText.text += "\n" + "#" + completeTaskCount + " Time: " + taskUI.LastTaskTimer.ToString("F1");
            }
        }

        private void LogObjectManipulationTaskData()
        {
            scoreText.text += "\nFinal Task Time: " + taskUI.LastTaskTimer.ToString("F1") + "Error: " + manipulationError.magnitude.ToString("F2");

            float taskTime = 0f;
            foreach (float time in targetShootTimes)
            {
                taskTime += time;
            }

            partSumErr += manipulationError.magnitude;
            partSumTime += taskTime;

            dataRecording.AddOneData(parkourCounter.locomotionTech.stage.ToString(), completeTaskCount, taskTime, manipulationError);
            currentTargetSkiStick.Destroy();
        }

        private void StartObjectManipulation()
        {
            isInTargetPart = false;
            isInObjectPart = true;

            Vector3 playerPosition = centerEyeAnchor.transform.position;
            Vector3 forwardOffset = centerEyeAnchor.transform.forward;
            forwardOffset.y = 0;

            Vector3 tmpPos = playerPosition + forwardOffset.normalized * 0.5f;

            float minXValue = tmpPos.x - 1f;
            float maxXValue = tmpPos.x + 1f;
            float minYValue = tmpPos.y;
            float maxYValue = tmpPos.y + 1f;
            // We choose for the target ski stick's min and max z values
            // to be the same so that it will always be static on the z-axis.
            float minZValue = tmpPos.z;
            float maxZValue = tmpPos.z;

            currentTargetSkiStick = CreateNewTargetSkiStick(minXValue, maxXValue, minYValue, maxYValue, minZValue, maxZValue);
            currentTargetSkiStick.Spawn();
        }

        private TargetSkiStick CreateNewTargetSkiStick(float minXValue, float maxXValue,
            float minYValue, float maxYValue, float minZValue, float maxZValue)
        {
            GameObject targetSkiStickGameObject = Instantiate(targetSkiStickPrefab.gameObject);
            targetSkiStickGameObject.SetActive(false);
            targetSkiStickGameObject.transform.SetParent(transform);

            TargetSkiStick targetSkiStick = targetSkiStickGameObject.GetComponent<TargetSkiStick>();
            targetSkiStick.Init(minXValue, maxXValue, minYValue, maxYValue, minZValue, maxZValue);

            return targetSkiStick;
        }
    }
}