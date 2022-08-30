using UnityEngine;

namespace UI
{
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

        private void OnButtonEvent(ButtonUIManager manager, ButtonUIManager.Event @event) { }
    }
}