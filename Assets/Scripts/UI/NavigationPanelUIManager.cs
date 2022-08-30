using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(ShortcutPanelUIManager))]
    public class NavigationPanelUIManager : MonoBehaviour
    {
        private ShortcutPanelUIManager manager;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => manager = GetComponent<ShortcutPanelUIManager>() as ShortcutPanelUIManager;
        
        void OnEnable() => manager.ClickEventReceived += OnSelectEvent;

        void OnDisable() => manager.ClickEventReceived -= OnSelectEvent;

        private void OnSelectEvent(ShortcutPanelUIManager.Identity identity)
        {
            switch (identity)
            {
                case ShortcutPanelUIManager.Identity.Exit:
                    Application.Quit();
                    break;
            }
        }
    }
}