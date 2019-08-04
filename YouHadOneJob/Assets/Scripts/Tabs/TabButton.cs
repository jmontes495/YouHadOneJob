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

        private const string textFormat = "{0} ({1})";

        private string mainText;

        protected override void Awake ()
        {
            base.Awake ();
            mainText = label.text;
        }

        protected override void HandleClick ()
        {
            tabsManager.FocusTab (tabType);
        }

        private void Update ()
        {
            tabsManager.Tick (tabType);
            label.text = string.Format (textFormat, mainText, tabsManager.GetTabText (tabType));
        }
    }
}