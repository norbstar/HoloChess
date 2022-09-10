using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

using UI.Panels;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(TrackedDeviceGraphicRaycaster))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(RootResolver))]
    public class SettingsCanvasUIManager : AnimatedCanvasUIManager
    {
        [Header("Components")]
        [SerializeField] SettingsPanelUIManager panel;
        public SettingsPanelUIManager Panel { get { return panel; } }
        [SerializeField] GameObject sphere;

        private void LookAtRoot()
        {
            Vector3 offset = transform.position - sphere.transform.position;
            transform.LookAt(transform.position + offset);
        }

        protected override void OnUpdate()
        {
            if (isShown)
            {
                LookAtRoot();
            }

            panel.EnableDragBar(sphere.activeSelf);
        }

        protected override void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, RaycastHit hit)
        {
            if (!panel.DragBar.IsPointerDown) return;

            Vector3 offset = panel.transform.position - panel.DragBar.transform.position;
            // Debug.Log($"Offset : {offset} Distance : {Vector3.Distance(panel.transform.position, panel.DragBar.transform.position)}");
            transform.position = hit.point + offset;
        }
    }
}