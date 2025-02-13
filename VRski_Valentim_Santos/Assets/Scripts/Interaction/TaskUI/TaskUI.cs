using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Interaction.TaskUI
{
    public class TaskUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Panel startPanel;
        [SerializeField] private Panel donePanel;
        [SerializeField] private GameObject hmd;
        [SerializeField] private Task.Task task;


        private readonly float countdownTimer = 3f;

        private float lastTaskTimer;
        private float currTaskTimer;
        private bool hasTaskStarted;
        private bool hasTaskEnded;
        private bool isCountdown;


        public bool HasTaskEnded => hasTaskEnded;
        public float LastTaskTimer => lastTaskTimer;


        void Start()
        {
            startPanel.Deactivate();
            donePanel.Deactivate();

            lastTaskTimer = 0f;
            currTaskTimer = 0f;
            hasTaskStarted = false;
            hasTaskEnded = false;
            isCountdown = false;
        }

        void Update()
        {
            if (startPanel.HasBeenHit && !hasTaskStarted)
            {
                startPanel.DeactivateButKeepHit();
                task.StartNewTask(hmd.transform);
                hasTaskStarted = true;
            }
            else if (startPanel.HasBeenHit && task.HasTaskEnded())
            {
                startPanel.Deactivate();
                donePanel.Activate();

                // Start object manipulation

                hasTaskStarted = false;
            }
            else if (donePanel.HasBeenHit)
            {
                donePanel.Deactivate();
                lastTaskTimer = currTaskTimer;
                hasTaskEnded = true;
            }
            else if (isCountdown)
            {
                startPanel.SetText((countdownTimer - currTaskTimer).ToString("F1"));
            }

            if (hasTaskEnded)
            {
                startPanel.Deactivate();
                donePanel.Deactivate();
            }
            else 
            {
                currTaskTimer += Time.deltaTime;
            }
        }


        public void StartFirstTask()
        {
            lastTaskTimer = 0f;
            currTaskTimer = 0f;
            hasTaskEnded = false;
            isCountdown = false;
            SpawnTaskUI();
        }

        public void StartIntermidiateTask()
        {
            lastTaskTimer = 0f;
            currTaskTimer = 0f;
            hasTaskEnded = false;
            isCountdown = false;
            SpawnTaskUI();
            StartCoroutine(Countdown(countdownTimer));
        }

        public void StartLastTask()
        {
            lastTaskTimer = 0f;
            currTaskTimer = 0f;
            hasTaskEnded = false;
            isCountdown = false;
            SpawnTaskUI();
            StartCoroutine(Countdown(countdownTimer));
        }

        public void EndTask()
        {
            lastTaskTimer = currTaskTimer;
            currTaskTimer = 0f;
            hasTaskEnded = true;
            isCountdown = false;
            startPanel.Deactivate();
            donePanel.Deactivate();
        }


        private void SpawnTaskUI()
        {
            Vector3 playerPosition = hmd.transform.position;

            Vector3 forwardOffset = hmd.transform.forward;
            forwardOffset.y = 0;

            Vector3 tmpPos = playerPosition + forwardOffset.normalized * 1.5f;

            startPanel.transform.position = tmpPos + Vector3.left * .5f;
            startPanel.transform.LookAt(playerPosition);
            startPanel.transform.Rotate(new Vector3(0, 180f, 0));

            donePanel.transform.position = tmpPos + Vector3.right * .5f;
            donePanel.transform.LookAt(playerPosition);
            donePanel.transform.Rotate(new Vector3(0, 180f, 0));

            startPanel.Activate();
        }

        private IEnumerator Countdown(float t)
        {
            isCountdown = true;
            startPanel.IsCountdown = true;
            currTaskTimer = 0f;

            yield return new WaitForSeconds(t);

            isCountdown = false;
            startPanel.SetText("start");
            startPanel.IsCountdown = false;
            
            isCountdown = false;
            yield return 0;
        }
    }
}