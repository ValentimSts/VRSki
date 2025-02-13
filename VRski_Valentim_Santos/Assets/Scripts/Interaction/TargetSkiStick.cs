using UnityEngine;

namespace Assets.Scripts.Interaction
{
    public class TargetSkiStick : MonoBehaviour
    {
        private float minXValue;
        private float maxXValue;
        private float minYValue;
        private float maxYValue;
        private float minZValue;
        private float maxZValue;


        public Vector3 Position => transform.position;


        public void Init(float minXValue, float maxXValue, float minYValue,
            float maxYValue, float minZValue, float maxZValue)
        {
            this.minXValue = minXValue;
            this.maxXValue = maxXValue;
            this.minYValue = minYValue;
            this.maxYValue = maxYValue;
            this.minZValue = minZValue;
            this.maxZValue = maxZValue;

            gameObject.SetActive(false);
        }


        public void Spawn()
        {
            Vector3 randomPosition = new Vector3(Random.Range(minXValue, maxXValue),
                Random.Range(minYValue, maxYValue), Random.Range(minZValue, maxZValue));
            Quaternion randomRotation = GetRandomRotation();

            transform.SetPositionAndRotation(randomPosition, randomRotation);
            gameObject.SetActive(true);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public int GetChildCount()
        {
            return transform.childCount;
        }

        public Transform GetChild(int index)
        {
            return transform.GetChild(index);
        }


        private Quaternion GetRandomRotation()
        {
            return new Quaternion(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f),
                Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        }
    }
}