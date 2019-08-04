using UnityEngine;

namespace YouHadOneJob
{
    public class GaugePointer : MonoBehaviour
    {
        [SerializeField, Range(0f, 100f)]
        private float sanity;
        [SerializeField]
        private float minRotationAngle;
        [SerializeField]
        private float maxRotationAngle;
        [SerializeField]
        private GameObject pointer;

        private float mySanity;

        private void Start()
        {
            sanity = 100.0f;
            mySanity = 100.0f;
        }

        private void Update()
        {
            if(sanity != mySanity)
            {
                mySanity = sanity;
                float rotation = CalculateRotation();
                Vector3 rotationVector = transform.rotation.eulerAngles;
                rotationVector.z = rotation;
                pointer.transform.rotation = Quaternion.Euler(rotationVector);
            }
        }

        private float CalculateRotation()
        {
            float rotation = 0.0f;
            if (mySanity.Equals(50.0f))
                return rotation;

            if (mySanity < 50.0f)
                rotation = Mathf.Clamp((minRotationAngle * (50.0f - mySanity)) / 50.0f, 0.0f, minRotationAngle);
            if (mySanity > 50.0f)
                rotation = Mathf.Clamp((maxRotationAngle * (mySanity - 50.0f)) / 50.0f, maxRotationAngle, 0.0f);

            return rotation;
        }

        public void SetSanity(float san)
        {
            sanity = san;
        }
    }
}