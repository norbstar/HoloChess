using UnityEngine;

using UnityEngine.UI;

using TMPro;

namespace UI.Panels
{
    public class TerminalPanelUIManager : ShortcutPanelUIManager
    {
        [Header("Components")]
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] TextMeshProUGUI textUI;

        protected override void OnSelectEvent(ButtonUIManager manager)
        {
            var name = manager.Button.name;
            Debug.Log($"{Time.time} OnSelect {name}");

            if (name.Equals("Top Button"))
            {
                scrollRect.ScrollToTop();
            }
            else if (name.Equals("Bottom Button"))
            {
                scrollRect.ScrollToBottom();
            }
            else if (name.Equals("Clear Button"))
            {
                // TODO
            }
        }
    }
}