using UnityEngine;

namespace UI
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