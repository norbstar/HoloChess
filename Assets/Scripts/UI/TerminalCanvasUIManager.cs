using UnityEngine;

using UI.Panels;

namespace UI
{
    public class TerminalCanvasUIManager : DragbarCanvasUIManager<TerminalPanelUIManager>
    {
        private new Camera camera;
        private float originalOffset;
        private float parentToChildMultiplier;
        private bool isLocked;
        private float lockY;

        protected override void Awake()
        {
            base.Awake();
            ResolveDependencies();

            originalOffset = transform.localPosition.z;
            parentToChildMultiplier = panel.transform.localScale.x / transform.localScale.x;
        }

        private void ResolveDependencies() => camera = Camera.main;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            isLocked = panel.IsLocked;

            if (isLocked)
            {
                lockY = transform.position.y;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            panel.LockedEventReceived += OnLockSwapEvent;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            panel.LockedEventReceived -= OnLockSwapEvent;
        }

        private void OnLockSwapEvent(bool isLocked)
        {
            this.isLocked = isLocked;

            if (isLocked)
            {
                lockY = transform.position.y;
            }
        }

        protected override void UpdatePosition(GameObject source, Vector3 origin, Vector3 direction, GameObject target, RaycastHit hit)
        {
            // PointProjectorDatabase.PlotPoint("Hit A", PointProjector.Type.White, hit.point, Vector3.one * 0.15f);

            if (!isLocked)
            {
                base.UpdatePosition(source, origin, direction, target, hit);
                return;
            }

            // var point = new Vector3(hit.point.x, layer.transform.position.y, hit.point.z);
            // Vector3 direction = (point - layer.transform.position).normalized;
            Vector3 referencePoint = new Vector3(layer.transform.position.x, lockY, layer.transform.position.z);
            // PointProjectorDatabase.PlotPoint("Ref", PointProjector.Type.White, referencePoint, Vector3.one * 0.15f);

            Vector3 adjustedDirection = (new Vector3(hit.point.x, lockY, hit.point.z) - referencePoint).normalized;
            LayerMask menuLayerMask = LayerMask.GetMask("Far Menu");

            // var ray = new Ray(layer.transform.position + direction * (originalOffset * parentToChildMultiplier), -direction);
            var ray = new Ray(referencePoint + adjustedDirection * (originalOffset * parentToChildMultiplier), -adjustedDirection);
            bool hasHit = Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, menuLayerMask);

            if (hasHit)
            {
                // PointProjectorDatabase.PlotPoint("Hit B", PointProjector.Type.White, hit.point, Vector3.one * 0.15f);
                // Vector3 offset = panel.GetObject().transform.position - dragBar.transform.position;
                transform.position = hit.point;
            }
        }
    }
}