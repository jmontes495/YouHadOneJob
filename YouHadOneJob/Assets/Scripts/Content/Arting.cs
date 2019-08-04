using UnityEngine;
using UnityEngine.UI;

namespace YouHadOneJob
{
    public class Arting : TabContent
    {
        [SerializeField]
        private Button concept;
        [SerializeField]
        private PaintStep conceptStep;

        private PaintingState state;
        private float mistakeWaitTime = 8f;
        private float completeWaitTime = 5f;
        private float timeBetweenStrokes = 0.5f;
        private float timeToWait;


        private void Awake()
        {
            if (concept == null)
                return;

            concept.onClick.AddListener(Paint);
            conceptStep.SetStep(0);

            state = PaintingState.Painting;
        }

        protected override string GetInstructionsText ()
        {
            if (concept == null)
                return "Click to arts";

            switch (state)
            {
                case PaintingState.Painting:
                    return "Click to arts";
                case PaintingState.Completed:
                    return "Finished arting. Wait for " + Mathf.CeilToInt(timeToWait) + "s";
                case PaintingState.Cancelling:
                    return "Arted too fast. Wait for " + Mathf.CeilToInt(timeToWait) + "s";
            }

            return "Click to arts";
        }

        protected override string GetTabText ()
        {
            if (concept == null)
                return "Art";

            switch (state)
            {
                case PaintingState.Painting:
                    return conceptStep.GetStep() + "/12";
                case PaintingState.Completed:
                    return "Complete";
                case PaintingState.Cancelling:
                    return "Messed up";
            }

            return "Art";
        }

        protected override void Tick (bool isFocused)
        {
            if (concept == null)
                return;

            switch (state)
            {
                case PaintingState.Painting:
                    if (timeBetweenStrokes > 0.0f)
                        timeBetweenStrokes -= Time.deltaTime;

                    break;

                case PaintingState.Cancelling:
                case PaintingState.Completed:
                    if (timeToWait > 0.0f)
                        timeToWait -= Time.deltaTime;

                    if (timeToWait <= 0.0f)
                    {
                        conceptStep.SetStep(0);
                        state = PaintingState.Painting;
                    }

                    break;
            }
        }

        private void Paint()
        {
            if (state == PaintingState.Painting)
            {
                if(timeBetweenStrokes <= 0.0f)
                {
                    if (conceptStep.GetStep() == 11)
                    {
                        state = PaintingState.Completed;
                        timeToWait = completeWaitTime;
                    }
                    conceptStep.SetStep(conceptStep.GetStep() + 1);
                    timeBetweenStrokes = 0.5f;
                    return;
                }

                if(timeBetweenStrokes > 0.0f)
                {
                    conceptStep.SetStep(13);
                    timeBetweenStrokes = 0.5f;
                    state = PaintingState.Cancelling;
                    timeToWait = mistakeWaitTime;
                    return;
                }
                
            }
        }
    }
}