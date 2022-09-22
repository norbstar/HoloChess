using UnityEngine;

using UI.Panels;

namespace UI
{
    public class TerminalCanvasUIManager : DragbarCanvasUIManager<TerminalPanelUIManager>
    {
        private new Camera camera;
        private bool isLocked;
        private float layerRadius;

        protected override void Awake()
        {
            base.Awake();
            ResolveDependencies();

            layerRadius = layer.transform.localScale.z * 0.5f;
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

#if false
        Transform randomTransform, vAlignedTransform;
        Vector3 point;

        public void OnRandomize()
        {
            PointProjectorDatabase.PlotPoint($"Layer Transform", $"Layer Transform", PointProjector.Type.White, layer.transform.position);
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
            PointProjectorDatabase.PlotPoint($"V Aligned Transform", $"V Aligned Transform", PointProjector.Type.Green, vAlignedTransform.position);
            
            // Step 3
            float height = Mathf.Abs((layer.transform.localPosition.y - layerRadius) - transform.localPosition.y);
            Debug.Log($"Relative Height : {height}");

            float capRadius = Mathf.Sqrt(Mathf.Pow(layerRadius, 2) - Mathf.Pow(layerRadius - height, 2));
            Debug.Log($"Cap Radius : {capRadius}");

            Vector3 heightAdjustedLayerPoint = new Vector3(layer.transform.position.x, vAlignedTransform.position.y, layer.transform.position.z);
            PointProjectorDatabase.PlotPoint($"Height Adjusted Layer Transform", $"Height Adjusted Layer Transform", PointProjector.Type.Blue, heightAdjustedLayerPoint);
            
            Vector3 direction = (vAlignedTransform.transform.position - heightAdjustedLayerPoint).normalized;
            point = heightAdjustedLayerPoint + direction * capRadius;
            PointProjectorDatabase.PlotPoint($"Height Adjusted Transform", $"Height Adjusted Transform", PointProjector.Type.Yellow, point);
            PointProjectorDatabase.PlotPoint($"Target Transform", $"Target Transform", PointProjector.Type.Orange, point);
        }

        public void OnApply()
        {
            if (point != null)
            {
                base.transform.position = point;
            }
        }
#endif

        protected override void ProcessRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, GameObject target, RaycastHit hit)
        {
            if (!isLocked)
            {
                base.ProcessRaycastEvent(source, origin, direction, target, hit);
                return;
            }

            // Step 1
            Vector3 vAlignedPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            PointProjectorDatabase.PlotPoint($"V Aligned Point", $"V Aligned Point", PointProjector.Type.Green, vAlignedPoint);
            
            // Step 2
            float height = Mathf.Abs((layer.transform.localPosition.y - layerRadius) - transform.localPosition.y);
            float capRadius = Mathf.Sqrt(Mathf.Pow(layerRadius, 2) - Mathf.Pow(layerRadius - height, 2));

            // Step 3
            Vector3 heightAdjustedLayerPoint = new Vector3(layer.transform.position.x, vAlignedPoint.y, layer.transform.position.z);
            PointProjectorDatabase.PlotPoint($"Height Adjusted Layer Point", $"Height Adjusted Layer Point", PointProjector.Type.Blue, heightAdjustedLayerPoint);
            
            // Step 4
            Vector3 relativeDirection = (vAlignedPoint - heightAdjustedLayerPoint).normalized;
            Vector3 point = heightAdjustedLayerPoint + relativeDirection * capRadius;
            transform.position = point;
            
            PointProjectorDatabase.PlotPoint($"Constrained Target Point", $"Constrained Target Point", PointProjector.Type.Orange, transform.position);
            float distance = Vector3.Distance(layer.transform.position, transform.position);
            Debug.Log($"<color=orange>Constrained Target Point</color> : {transform.position} {distance}");
        }
    }
}