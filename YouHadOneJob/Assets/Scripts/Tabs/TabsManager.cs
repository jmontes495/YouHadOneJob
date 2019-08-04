using System.Collections.Generic;
using UnityEngine;

namespace YouHadOneJob
{
    public class TabsManager : MonoBehaviour
    {
        private Dictionary<TabType, TabContent> tabsContent;
        private TabType openTabType;
        private TabContent openTabContent;

        private void Awake ()
        {
            tabsContent = new Dictionary<TabType, TabContent> ();
            openTabType = TabType.None;

            TabContent[] tabsContentArray = GetComponentsInChildren<TabContent> (true);
            foreach (TabContent tabContent in tabsContentArray)
                tabsContent.Add (tabContent.TabType, tabContent);

            FocusTab (TabType.Mail);
        }

        public void FocusTab (TabType tabType)
        {
            if (tabType == openTabType)
                return;
            if (openTabType != TabType.None)
                openTabContent.Hide ();

            openTabType = tabType;
            openTabContent = tabsContent[tabType];
            openTabContent.Show ();
        }

        public void Tick (TabType tabType)
        {
            tabsContent[tabType].PublicTick (tabType == openTabType);
        }

        public string GetTabText (TabType tabType)
        {
            return tabsContent[tabType].PublicGetTabText ();
        }
    }
}