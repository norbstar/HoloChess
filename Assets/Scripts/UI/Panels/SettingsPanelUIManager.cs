using UnityEngine;

namespace UI.Panels
{
    [RequireComponent(typeof(RootResolver))]
    public class SettingsPanelUIManager : ShortcutPanelUIManager
    {
        [Header("Components")]
        [SerializeField] DragBarUIManager dragBar;
        public DragBarUIManager DragBar { get { return dragBar; } }

        private static string CLOSE_PROGESS_BUTTON = "Close Progress Button";
        
        public delegate void OnCloseEvent();
        public event OnCloseEvent CloseEventReceived;

        public void EnableDragBar(bool enable) => dragBar.gameObject.SetActive(enable);

        protected override void OnSelectEvent(ButtonUIManager manager)
        {
            var name = manager.Button.name;
            // Debug.Log($"{Time.time} OnSelect {name}");

            if (name.Equals(CLOSE_PROGESS_BUTTON))
            {
                CloseEventReceived?.Invoke();
            }
        }
    }
}