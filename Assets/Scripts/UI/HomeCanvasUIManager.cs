using UnityEngine;

using UI.Panels;

namespace UI
{
    public class HomeCanvasUIManager : DragbarCanvasUIManager<HomePanelUIManager>
    {
        public enum TrackingMode
        {
            Free,
            Restricted,
            Locked
        }

        [Header("Config")]
        [SerializeField] TrackingMode trackingMode;

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

        public override void Show()
        {
            layer.gameObject.SetActive(true);
            bool setRoot = false;

            switch (trackingMode)
            {
                case TrackingMode.Free:
                    setRoot = true;
                    break;

                case TrackingMode.Restricted:
                    var collider = layer.GetComponent<SphereCollider>() as SphereCollider;
                    var inBounds = (collider.bounds.Contains(camera.transform.position));
                    setRoot = (!inBounds);
                    break;
                
                case TrackingMode.Locked:
                    setRoot = false;
                    break;
            }

            if (setRoot)
            {
                root.transform.position = new Vector3(camera.transform.position.x, 0f, camera.transform.position.z);
            }

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
    }
}