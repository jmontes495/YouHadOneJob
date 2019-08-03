using UnityEngine;

namespace YouHadOneJob
{
    public class TabButton : UIButtonSetter
    {
        [SerializeField]
        private TabType tabType;
        [SerializeField]
        private TabsManager tabsManager;

        protected override void HandleClick ()
        {
            tabsManager.FocusTab (tabType);
        }
    }
}