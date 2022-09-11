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
        [SerializeField] MenuLayer layer;

        private HomeCanvasUIManager homeCanvasUIManager;

        protected override void Awake()
        {
            base.Awake();
            ResolveDependencies();
        }

        private void ResolveDependencies() => homeCanvasUIManager = FindObjectOfType<HomeCanvasUIManager>() as HomeCanvasUIManager;

        private void LookAtRoot()
        {
            Vector3 offset = transform.position - layer.transform.position;
            transform.LookAt(transform.position + offset);
        }

        protected override void OnUpdate()
        {
            if (isShown)
            {
                LookAtRoot();
            }

            panel.EnableDragBar(homeCanvasUIManager.Layer.IsShown);
        }

        public override void Show()
        {
            layer.gameObject.SetActive(true);
            layer.Attach(gameObject);

            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
            
            layer.Detach(gameObject);

            if (!layer.HasChildren)
            {
                layer.gameObject.SetActive(false);
            }
        }

        protected override void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, GameObject target, RaycastHit hit)
        {
            var dragBar = panel.GetDragBar();
            
            if ((!target.Equals(layer.gameObject)) || (!dragBar.IsPointerDown)) return;

            Vector3 offset = panel.GetObject().transform.position - dragBar.transform.position;
            transform.position = hit.point + offset;
        }
    }
}