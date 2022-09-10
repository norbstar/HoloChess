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
        [SerializeField] GameObject sphere;

        [Header("Audio")]
        [SerializeField] AudioClip onRevealClip;

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
            Vector3 offset = transform.position - sphere.transform.position;
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
            sphere.SetActive(true);

            LayerMask menuLayerMask = LayerMask.GetMask("Menu");

            var ray = new Ray(camera.transform.position + camera.transform.forward * (originalOffset * parentToChildMultiplier), -camera.transform.forward);
            bool hasHit = Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, menuLayerMask);

            Vector3? spawnPoint = null;
            
            if (hasHit)
            {
                spawnPoint = hit.point;
                transform.position = spawnPoint.Value;
            }

            LookAtRoot();

            if (onRevealClip != null)
            {
                AudioSource.PlayClipAtPoint(onRevealClip, Vector3.zero, 1.0f);
            }

            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
            sphere.SetActive(false);
        }

        protected override void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, RaycastHit hit)
        {
            Debug.Log($"OnRaycastEvent [1] {source.name} Origin : {origin} Hit : {hit.point}");

            if (!panel.DragBar.IsPointerDown) return;

            Debug.Log($"OnRaycastEvent [2]");

            Vector3 offset = panel.transform.position - panel.DragBar.transform.position;
            // Debug.Log($"Offset : {offset} Distance : {Vector3.Distance(panel.transform.position, panel.DragBar.transform.position)}");
            transform.position = hit.point + offset;
        }
    }
}