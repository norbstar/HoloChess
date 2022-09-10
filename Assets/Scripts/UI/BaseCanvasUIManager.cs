using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(TrackedDeviceGraphicRaycaster))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(RootResolver))]
    public class BaseCanvasUIManager<T> : AnimatedCanvasUIManager where  T : IDragbarPanel
    {
        [Header("Components")]
        [SerializeField] T panel;
        public T Panel { get { return panel; } }
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
            var dragBar = panel.GetDragBar();
            
            if (!dragBar.IsPointerDown) return;

            Vector3 offset = panel.GetObject().transform.position - dragBar.transform.position;
            transform.position = hit.point + offset;
        }
    }
}