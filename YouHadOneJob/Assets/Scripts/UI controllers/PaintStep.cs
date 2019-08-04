using UnityEngine;
using UnityEngine.UI;

namespace YouHadOneJob
{
    public class PaintStep : MonoBehaviour
    {
        [SerializeField, Range(0, 13)]
        private int step;
        [SerializeField]
        private Image painter;
        [SerializeField]
        private Sprite[] spriteCollection;

        private int currentStep;

        private void Start()
        {
            step = 0;
            currentStep = 0;
            painter.sprite = spriteCollection[currentStep];
        }

        private void Update()
        {
            if (currentStep != step)
            {
                currentStep = step;
                painter.sprite = spriteCollection[currentStep];
            }
        }
        
        public void SetStep(int stp)
        {
            step = stp;
        }

        public int GetStep()
        {
            return currentStep;
        }
    }
}