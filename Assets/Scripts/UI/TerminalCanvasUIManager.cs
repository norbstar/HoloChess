using UnityEngine;

using UI.Panels;

namespace UI
{
    public class TerminalCanvasUIManager : DragbarCanvasUIManager<TerminalPanelUIManager>
    {
        [Header("Config")]
        [SerializeField] LineRenderer lineRendererA;
        [SerializeField] LineRenderer lineRendererB;
        [SerializeField] LineRenderer lineRendererC;

        private new Camera camera;
        private bool isLocked;

        protected override void Awake()
        {
            base.Awake();
            ResolveDependencies();
        }

        private void ResolveDependencies() => camera = Camera.main;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            isLocked = panel.IsLocked;
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

        private void OnLockSwapEvent(bool isLocked) => this.isLocked = isLocked;

        Transform randomTransform, vAlignedTransform;
        Vector3 point;

        public void OnRandomize()
        {
            PointProjectorDatabase.PlotPoint($"Canvas Transform", $"Canvas Transform", PointProjector.Type.White, transform.position);

            // Step 1
            if (randomTransform == null)
            {
                randomTransform = new GameObject().transform;
                randomTransform.parent = root.transform;
                randomTransform.name = "Random Transform";
            }

            randomTransform.transform.position = layer.transform.position;
            randomTransform.transform.rotation = Random.rotation;
            point = randomTransform.position + randomTransform.forward * (layer.transform.localScale.z * 0.5f);
            PointProjectorDatabase.PlotPoint($"Rnd Transform", $"Rnd Transform", PointProjector.Type.Red, point);

            // Step 2
            if (vAlignedTransform == null)
            {
                vAlignedTransform = new GameObject().transform;
                vAlignedTransform.parent = root.transform;
                vAlignedTransform.name = "Vertically Aligned Transform";
            }

            vAlignedTransform.transform.position = new Vector3(point.x, transform.position.y, point.z);
            PointProjectorDatabase.PlotPoint($"V Aligned Transform", $"V Aligned Transform", PointProjector.Type.Yellow, vAlignedTransform.position);
            
            // Step 3
            Vector3 direction = (vAlignedTransform.transform.position - layer.transform.position).normalized;
            point = layer.transform.position + direction * (layer.transform.localScale.z * 0.5f);
            PointProjectorDatabase.PlotPoint($"Target Transform", $"Target Transform", PointProjector.Type.Blue, point);
        }

        public void OnApply()
        {
            if (point != null)
            {
                base.transform.position = point;
            }
        }

        protected override void ProcessRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, GameObject target, RaycastHit hit)
        {
            if (!isLocked)
            {
                base.ProcessRaycastEvent(source, origin, direction, target, hit);
                return;
            }

            // TODO
        }
    }
}