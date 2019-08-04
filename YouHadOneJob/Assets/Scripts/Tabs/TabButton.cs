using UnityEngine;
using UnityEngine.UI;

namespace YouHadOneJob
{
    public class TabButton : UIButtonSetter
    {
        [SerializeField]
        private TabType tabType;
        [SerializeField]
        private Text label;
        [SerializeField]
        private TabsManager tabsManager;

        protected override void HandleClick ()
        {
            tabsManager.FocusTab (tabType);
        }

        private void Update ()
        {
            tabsManager.Tick (tabType);
            label.text = "(" + tabsManager.GetTabText (tabType) + ")";
        }
    }
}