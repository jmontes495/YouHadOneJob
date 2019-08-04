using UnityEngine;
using UnityEngine.UI;

namespace YouHadOneJob
{
    public abstract class TabContent : MonoBehaviour
    {
        [SerializeField]
        private TabType tabType;
        [SerializeField]
        private Text instructionsLabel;

        public TabType TabType
        {
            get { return tabType; }
        }

        public void Show ()
        {
            gameObject.SetActive (true);
        }

        public void Hide ()
        {
            gameObject.SetActive (false);
        }

        public void PublicTick (bool isFocused)
        {
            Tick (isFocused);
            instructionsLabel.text = GetInstructionsText ();
        }

        public string PublicGetTabText ()
        {
            return GetTabText ();
        }

        protected abstract void Tick (bool isFocused);

        protected abstract string GetTabText ();

        protected abstract string GetInstructionsText ();
    }
}