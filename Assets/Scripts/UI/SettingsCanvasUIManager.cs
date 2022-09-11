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
            if ((!target.Equals(layer.gameObject)) || (!panel.DragBar.IsPointerDown)) return;

            Vector3 offset = panel.transform.position - panel.DragBar.transform.position;
            transform.position = hit.point + offset;
        }
    }
}