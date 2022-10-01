using System;

using UnityEngine;

using TMPro;

namespace UI.Panels
{
    [AddComponentMenu("UI/Panels/Selected Button Group Panel UI Manager")]
    [RequireComponent(typeof(RootResolver))]
    public class StatsPanelUIManager : SelectedButtonGroupPanelUIManager, IDragbarPanel
    {
        [Header("Components")]
        [SerializeField] TextMeshProUGUI dateUI;
        public string Date { get { return dateUI.text; } }
        [SerializeField] TextMeshProUGUI timeUI;
        public string Time { get { return timeUI.text; } }
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

            var date = DateTime.Now.ToString("MM/dd/yyyy");
            dateUI.text = date;

            var time = DateTime.Now.ToString("HH:mm:ss");
            timeUI.text = time;
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