using UnityEngine;

namespace UI.Panels
{
    [RequireComponent(typeof(RootResolver))]
    public class SettingsPanelUIManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] DragBarUIManager dragBar;
        public DragBarUIManager DragBar { get { return dragBar; } }

        public void EnableDragBar(bool enable) => dragBar.gameObject.SetActive(enable);
    }
}