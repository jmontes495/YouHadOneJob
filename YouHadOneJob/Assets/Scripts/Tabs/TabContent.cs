using UnityEngine;

namespace YouHadOneJob
{
    public class TabContent : MonoBehaviour
    {
        [SerializeField]
        private TabType tabType;

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
    }
}