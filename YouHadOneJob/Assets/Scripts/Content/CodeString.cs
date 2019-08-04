namespace YouHadOneJob
{
    public static class CodeString
    {
        public const string Value =
        "public class TabsManager : MonoBehaviour\n{\n\tprivate Dictionary<TabType, TabContent> tabsContent;\n\tprivate TabType openTabType;\n\tprivate TabContent openTabContent;\n\n\tprivate void Awake ()\n\t{\n\t\ttabsContent = new Dictionary<TabType, TabContent> ();\n\t\topenTabType = TabType.None;\n\n\t\tTabContent[] tabsContentArray = GetComponentsInChildren<TabContent> (true);\n\t\tforeach (TabContent tabContent in tabsContentArray)\n\t\t\ttabsContent.Add (tabContent.TabType, tabContent);\n\n\t\tFocusTab (TabType.Mail);\n\t}\n\n\tpublic void FocusTab (TabType tabType)\n\t{\n\t\tif (tabType == openTabType)\n\t\t\treturn;\n\t\tif (openTabType != TabType.None)\n\t\t\topenTabContent.Hide ();\n\n\t\topenTabType = tabType;\n\t\topenTabContent = tabsContent[tabType];\n\t\topenTabContent.Show ();\n\t}\n}"
        ;
    }
}