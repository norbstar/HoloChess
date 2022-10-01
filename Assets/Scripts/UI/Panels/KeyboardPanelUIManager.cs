using UnityEngine;

namespace UI.Panels
{
    [AddComponentMenu("UI/Panels/Keyboard Panel UI Manager")]
    [RequireComponent(typeof(RootResolver))]
    public class KeyboardPanelUIManager : SelectedButtonGroupPanelUIManager, IDragbarPanel
    {
        [Header("Components")]
        [SerializeField] DragBarUIManager dragBar;
        public DragBarUIManager DragBar { get { return dragBar; } }

        private static string CLOSE_PROGRESS_BUTTON = "Close Progress Button";
        
        public delegate void OnCloseEvent();
        public event OnCloseEvent CloseEventReceived;

        public GameObject GetObject() => gameObject;

        public DragBarUIManager GetDragBar() => dragBar;

        public void EnableDragBar(bool enable) => dragBar.gameObject.SetActive(enable);

        // Update is called once per frame
        void Update()
        {
            // if (!isShown) return;
        }
        
        protected override void OnSelectEvent(ButtonUIManager manager)
        {
            var name = manager.Button.name;

            if (name.Equals(CLOSE_PROGRESS_BUTTON))
            {
                CloseEventReceived?.Invoke();
            }
        }
    }
}