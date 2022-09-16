using UnityEngine;

using UI.Panels;

namespace UI
{
    public class TerminalCanvasUIManager : DragbarCanvasUIManager<TerminalPanelUIManager>
    {
        private new Camera camera;
        private bool isLocked;
        private float lockY;

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
            // PointProjectorDatabase.PlotPoint($"{gameObject.name} Start", $"{gameObject.name} {Vector3.Distance(layer.transform.position, transform.position)}", PointProjector.Type.Yellow, transform.position);
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

        // Update is called once per frame
        // protected override void Update()
        // {
        //     base.Update();
        //     Debug.Log($"Position X : {transform.position.x}");
        //     Debug.Log($"Position Y : {transform.position.y}");
        //     Debug.Log($"Position Z : {transform.position.z}");
        // }

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
            Debug.Log($"{gameObject.name} UpdatePosition Override Impl");

            // PointProjectorDatabase.PlotPoint("Hit A", PointProjector.Type.White, hit.point, Vector3.one * 0.15f);

            if (!isLocked)
            {
                base.UpdatePosition(source, origin, direction, target, hit);
                return;
            }

            Vector3 relativeDirection = (hit.point - layer.transform.position).normalized;
            var point = layer.transform.position + relativeDirection * (layer.transform.localScale.z * 0.5f);

            GameObject p0 = new GameObject();
            p0.transform.position = layer.transform.position;
            p0.transform.rotation = layer.transform.rotation;
            p0.transform.LookAt(point);

            float angle = Vector3.SignedAngle(layer.transform.forward, transform.forward, transform.right);

            Vector3 tmp = p0.transform.localEulerAngles;
            tmp.x = angle;
            p0.transform.localEulerAngles = tmp;

            point = p0.transform.position + p0.transform.forward * (layer.transform.localScale.z * 0.5f);

            // hit.point = dragBar.transform.position;

            // hit.point = new Vector3(hit.point.x, lockY, hit.point.z);
            // Vector3 relativeDirection = (hit.point - layer.transform.position).normalized;
            // var point = layer.transform.position + relativeDirection * (layer.transform.localScale.z * 0.5f);
            transform.position = point;
            
#if false
            /* Stage 1 */
            Vector3 relativeDirection = (hit.point - layer.transform.position).normalized;
            var point = layer.transform.position + relativeDirection * (layer.transform.localScale.z * 0.5f);

            /* Stage 2 */
            Vector3 referencePoint = new Vector3(layer.transform.position.x, point.y, layer.transform.position.z);
            relativeDirection = (point - referencePoint).normalized;

            /* Stage 3 */
            referencePoint = new Vector3(layer.transform.position.x, lockY, layer.transform.position.z);
            var ray = new Ray(referencePoint + relativeDirection * layer.transform.lossyScale.z, -relativeDirection);
            LayerMask menuLayerMask = LayerMask.GetMask("Far Menu");
            bool hasHit = Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, menuLayerMask);

            if (hasHit)
            {
                /* Stage 4 */
                relativeDirection = (hit.point - layer.transform.position).normalized;
                point = layer.transform.position + relativeDirection * (layer.transform.localScale.z * 0.5f);
                // transform.position = point;

                Debug.Log($"Point X : {point.x}");
                Debug.Log($"Point Y : {point.y}");
                Debug.Log($"Point Z : {point.z}");
                
                PointProjectorDatabase.PlotPoint($"{gameObject.name} [2]", $"{gameObject.name} {Vector3.Distance(layer.transform.position, point)}", PointProjector.Type.Blue, point);
            }
#endif

#if false
            /* Stage 1 */
            Vector3 referencePoint = new Vector3(layer.transform.position.x, hit.point.y, layer.transform.position.z);
            var relativeDirection = (hit.point - referencePoint).normalized;

            /* Stage 2 */
            referencePoint = new Vector3(layer.transform.position.x, lockY, layer.transform.position.z);
            var ray = new Ray(referencePoint + relativeDirection * layer.transform.lossyScale.z, -relativeDirection);
            LayerMask menuLayerMask = LayerMask.GetMask("Far Menu");
            bool hasHit = Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, menuLayerMask);

            if (hasHit)
            {
                transform.position = hit.point;
                
                PointProjectorDatabase.PlotPoint($"{gameObject.name} [2]", $"{gameObject.name} {Vector3.Distance(layer.transform.position, transform.position)}", PointProjector.Type.Blue, transform.position);
            }
#endif

#if false
            // var point = new Vector3(hit.point.x, layer.transform.position.y, hit.point.z);
            // Vector3 direction = (point - layer.transform.position).normalized;
            Vector3 referencePoint = new Vector3(layer.transform.position.x, lockY, layer.transform.position.z);
            // PointProjectorDatabase.PlotPoint("Ref", PointProjector.Type.White, referencePoint, Vector3.one * 0.15f);

            Vector3 adjustedDirection = (new Vector3(point.x, lockY, point.z) - referencePoint).normalized;
            LayerMask menuLayerMask = LayerMask.GetMask("Far Menu");

            // var ray = new Ray(layer.transform.position + direction * layer.transform.lossyScale.z, -direction);
            var ray = new Ray(referencePoint + adjustedDirection * layer.transform.lossyScale.z, -adjustedDirection);
            bool hasHit = Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, menuLayerMask);

            if (hasHit)
            {
                relativeDirection = (hit.point - layer.transform.position).normalized;
                hit.point = layer.transform.position + relativeDirection * (layer.transform.localScale.z * 0.5f);
                // PointProjectorDatabase.PlotPoint("Hit B", PointProjector.Type.White, hit.point, Vector3.one * 0.15f);
                // Vector3 offset = panel.GetObject().transform.position - dragBar.transform.position;
                transform.position = hit.point/* + offset*/;
            }

            // PointProjectorDatabase.PlotPoint("Tracked", $"Tracked [{transform.position.x},{transform.position.y},{transform.position.z}] {Vector3.Distance(layer.transform.position, transform.position)}", PointProjector.Type.Blue, transform.position);
            PointProjectorDatabase.PlotPoint("Tracked", $"Tracked {Vector3.Distance(layer.transform.position, transform.position)}", PointProjector.Type.Blue, transform.position);
            // Debug.Log($"Tracked Distance {Vector3.Distance(layer.transform.position, transform.position)}");
#endif
        }
    }
}