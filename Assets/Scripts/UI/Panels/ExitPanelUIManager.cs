using UnityEngine;

namespace UI.Panels
{
    [AddComponentMenu("UI/Exit Panel UI Manager")]
    public class ExitPanelUIManager : MonoBehaviour
    {
        [SerializeField] ProgressButtonUIManager manager;

        void OnEnable()
        {
            manager.EventReceived += OnButtonEvent;
        }

        void OnDisable()
        {
            manager.EventReceived -= OnButtonEvent;
        }

        private void OnButtonEvent(ButtonUIManager manager, ButtonUIManager.Event @event)
        {
            if (@event != ButtonUIManager.Event.OnSelect) return;
            
            Application.Quit();
        }
    }
}