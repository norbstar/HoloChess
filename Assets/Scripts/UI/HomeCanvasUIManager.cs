using System;

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
    public class HomeCanvasUIManager : AnimatedCanvasUIManager
    {
        [Header("Components")]
        [SerializeField] HomePanelUIManager panel;
        public HomePanelUIManager Panel { get { return panel; } }
        [SerializeField] MenuLayer layer;
        public MenuLayer Layer { get { return layer; } }

        [Header("Config")]
        [SerializeField] GameObject referencePrefab;

        [Serializable]
        public class Reference
        {
            public string name;
            public GameObject gameObject;
        }

        private new Camera camera;
        public Camera Camera { get { return camera; } }
        private float originalOffset;
        private float parentToChildMultiplier;

        protected override void Awake()
        {
            base.Awake();

            ResolveDependencies();
            originalOffset = transform.localPosition.z;
            parentToChildMultiplier = panel.transform.localScale.x / transform.localScale.x;
        }

        private void ResolveDependencies() => camera = Camera.main;

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
        }

        public override void Show()
        {
            root.transform.position = new Vector3(camera.transform.position.x, 0f, camera.transform.position.z);
            layer.gameObject.SetActive(true);

            LayerMask menuLayerMask = LayerMask.GetMask("Near Menu");

            var ray = new Ray(camera.transform.position + camera.transform.forward * (originalOffset * parentToChildMultiplier), -camera.transform.forward);
            bool hasHit = Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, menuLayerMask);

            Vector3? spawnPoint = null;
            
            if (hasHit)
            {
                spawnPoint = hit.point;
                transform.position = spawnPoint.Value;
            }

            LookAtRoot();
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
            layer.gameObject.SetActive(false);
        }

        protected override void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, GameObject target, RaycastHit hit)
        {
            if ((!target.Equals(layer.gameObject)) || (!panel.DragBar.IsPointerDown)) return;

            Vector3 offset = panel.transform.position - panel.DragBar.transform.position;
            transform.position = hit.point + offset;
        }
    }
}