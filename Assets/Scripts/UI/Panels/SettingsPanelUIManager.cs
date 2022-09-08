using UnityEngine;

namespace UI.Panels
{
    [RequireComponent(typeof(RootResolver))]
    public class SettingsPanelUIManager : ShortcutPanelUIManager
    {
        [Header("Components")]
        [SerializeField] DragBarUIManager dragBar;
        public DragBarUIManager DragBar { get { return dragBar; } }

        public delegate void OnCloseEvent();
        public event OnCloseEvent CloseEventReceived;

        public void EnableDragBar(bool enable) => dragBar.gameObject.SetActive(enable);

        protected override void OnSelectEvent(ButtonUIManager manager)
        {
            var name = manager.Button.name;
            Debug.Log($"{Time.time} OnSelect {name}");

            if (name.Equals("Close Button"))
            {
                CloseEventReceived?.Invoke();
            }
        }
    }
}