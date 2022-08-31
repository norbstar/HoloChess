using UnityEngine;

namespace UI.Panels
{
    public class NavigationPanelUIManager : ShortcutPanelUIManager
    {
        protected override void OnSelectEvent(ButtonUIManager manager)
        {
            var name = manager.Button.name;

            if (name.Equals("Exit Button"))
            {
                Application.Quit();
            }
        }
    }
}