using UnityEngine;

namespace UI.Panels
{
    [AddComponentMenu("UI/Panels/Scene Panel UI Manager")]
    [RequireComponent(typeof(RootResolver))]
    public class ScenePanelUIManager : SelectedButtonGroupPanelUIManager, IDragbarPanel
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