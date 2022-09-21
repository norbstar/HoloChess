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
            isLocked = panel.IsLocked;

            if (isLocked)
            {
                lockY = transform.position.y;
            }

            // PointProjectorDatabase.PlotPoint($"p0", $"p0", PointProjector.Type.White, transform.position);
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

        // void FixedUpdate()
        // {
        //     if (p0 == null) return;

        //     Vector3[] positions = new Vector3[2];
        //     positions[0] = transform.position;
        //     PointProjectorDatabase.PlotPoint($"p0", $"p0", PointProjector.Type.Red, positions[0]);
        //     positions[1] = transform.position + (transform.forward * 10f);
        //     PointProjectorDatabase.PlotPoint($"p1", $"p1", PointProjector.Type.Blue, positions[1]);
        //     lineRenderer.positionCount = positions.Length;
        //     lineRenderer.SetPositions(positions);
        // }

        // Update is called once per frame
        // void LateUpdate()
        // {
        //     PointProjectorDatabase.PlotPoint($"Canvas", $"Canvas {transform.position}", PointProjector.Type.Red, transform.position);
            
            // if (p0 != null)
            // {
            //     PointProjectorDatabase.PlotPoint($"P0", $"P0 {p0.transform.position}", PointProjector.Type.Green, p0.transform.position);

            //     Vector3[] positions = new Vector3[2];
            //     positions[0] = p0.transform.position;
            //     positions[1] = p0.transform.position + (p0.transform.forward * 10f);
            //     lineRendererA.positionCount = positions.Length;
            //     lineRendererA.SetPositions(positions);
            // }

            // if (p1 != null)
            // {
            //     PointProjectorDatabase.PlotPoint($"P1", $"P1 {p1.transform.position}", PointProjector.Type.Blue, p1.transform.position);

            //     Vector3[] positions = new Vector3[2];
            //     positions[0] = p1.transform.position;
            //     positions[1] = p1.transform.position + (p1.transform.forward * 10f);
            //     lineRendererB.positionCount = positions.Length;
            //     lineRendererB.SetPositions(positions);
            // }

            // if (point != null)
            // {
            //     PointProjectorDatabase.PlotPoint($"Point", $"Point {point}", PointProjector.Type.Yellow, point);
            // }
        // }

        private Transform centerPoint;

        // Update is called once per frame
        // void LateUpdate()
        // {
        //     if (centerPoint == null)
        //     {
        //         centerPoint = new GameObject().transform;
        //         centerPoint.parent = root.transform;
        //         centerPoint.name = "Center Point";
        //         centerPoint.transform.position = layer.transform.position;
        //     }

        //     centerPoint.transform.LookAt(transform.position);
        //     float angle = Vector3.SignedAngle(centerPoint.transform.up, layer.transform.up, Vector3.right);
        //     Vector3 point = centerPoint.transform.position + centerPoint.transform.forward * (layer.transform.localScale.z * 0.5f);
        //     PointProjectorDatabase.PlotPoint($"p0", $"p0", PointProjector.Type.Green, point);
        //     Debug.Log($"Angle : {angle}");

        //     Vector3[] positions = new Vector3[2];
        //     positions[0] = centerPoint.transform.position;
        //     positions[1] = point;
        //     lineRendererA.positionCount = positions.Length;
        //     lineRendererA.SetPositions(positions);
        // }

        private void OnLockSwapEvent(bool isLocked)
        {
            this.isLocked = isLocked;

            if (isLocked)
            {
                lockY = transform.position.y;
            }
        }

#if true // WORKING EXAMPLE
        Transform tran1, tran2, tran3;
        Vector3 point;

        public void OnRandomize()
        {
            // Step 1
            if (tran1 == null)
            {
                tran1 = new GameObject().transform;
                tran1.parent = root.transform;
                tran1.name = "tran1";
                tran1.transform.position = layer.transform.position;
                PointProjectorDatabase.PlotPoint($"tran1", $"tran1", PointProjector.Type.Red, tran1.transform.position);
            }

            // Step 2
            tran1.transform.LookAt(transform.position);
            float tran1Angle = Vector3.SignedAngle(tran1.transform.forward, layer.transform.forward, Vector3.right);
            Debug.Log($"tran1 Position : {tran1.transform.position} Rotation : {tran1.transform.rotation} Angle : {tran1Angle} [Red] -> [Green]");

            // Step 3
            point = tran1.transform.position + tran1.transform.forward * (layer.transform.localScale.z * 0.5f);
            PointProjectorDatabase.PlotPoint($"tran1-point", $"tran1 [point]", PointProjector.Type.Green, point);

            Vector3[] positions = new Vector3[2];
            positions[0] = tran1.transform.position;
            positions[1] = point;
            lineRendererA.positionCount = positions.Length;
            lineRendererA.SetPositions(positions);

            // Step 4
            // Transform t_Reference = new GameObject().transform;
            // t_Reference.eulerAngles = new Vector3(0f, tran1.eulerAngles.y, 0f);
            // Vector3 v3_Dir = tran1.position - layer.transform.position;
            // float f_AngleBetween = Vector3.Angle(t_Reference.forward, v3_Dir);
            // Debug.Log($"{f_AngleBetween}");

            

            // Step 5
            if (tran2 == null)
            {
                tran2 = new GameObject().transform;
                tran2.parent = root.transform;
                tran2.name = "tran2";
                tran2.transform.position = layer.transform.position;
                PointProjectorDatabase.PlotPoint($"tran2", $"tran2", PointProjector.Type.Blue, tran2.transform.position);
            }

            // Step 6
            tran2.transform.rotation = Random.rotation;
            float tran2Angle = Vector3.SignedAngle(tran2.transform.forward, layer.transform.forward, Vector3.right);
            Debug.Log($"tran2 Position : {tran2.transform.position} Rotation : {tran2.transform.rotation}  Angle : {tran2Angle} [Blue] -> [Yellow]");
            
            // Step 7
            point = tran2.transform.position + tran2.transform.forward * (layer.transform.localScale.z * 0.5f);
            PointProjectorDatabase.PlotPoint($"tran2-point", $"tran2 [point]", PointProjector.Type.Yellow, point);

            positions = new Vector3[2];
            positions[0] = tran2.transform.position;
            positions[1] = point;
            lineRendererB.positionCount = positions.Length;
            lineRendererB.SetPositions(positions);

            // Step 8
            if (tran3 == null)
            {
                tran3 = new GameObject().transform;
                tran3.parent = root.transform;
                tran3.name = "tran3";
                tran3.transform.position = layer.transform.position;
                PointProjectorDatabase.PlotPoint($"tran3", $"tran3", PointProjector.Type.Orange, tran3.transform.position);
            }

            // Step 9
            Vector3 tmp = tran2.transform.eulerAngles;
            tmp.x = tran1Angle;
            tran3.transform.eulerAngles = tmp;
            
            float tran3Angle = Vector3.SignedAngle(tran3.transform.forward, layer.transform.forward, Vector3.right);
            Debug.Log($"tran3 Position : {tran3.transform.position} Rotation : {tran3.transform.rotation}  Angle : {tran3Angle} [Orange] -> [White]");
            
            // Step 10
            point = tran3.transform.position + tran3.transform.forward * (layer.transform.localScale.z * 0.5f);
            PointProjectorDatabase.PlotPoint($"tran3-point", $"tran3 [point]", PointProjector.Type.White, point);

            positions = new Vector3[2];
            positions[0] = tran3.transform.position;
            positions[1] = point;
            lineRendererC.positionCount = positions.Length;
            lineRendererC.SetPositions(positions);
#if false
            // Part  A
            if (p0 == null)
            {
                p0 = new GameObject();
                p0.name = "p0";
                p0.transform.position = layer.transform.position;
            }
            
            p0.transform.rotation = Random.rotation;
            PointProjectorDatabase.PlotPoint($"P0", $"P0", PointProjector.Type.Red, new PointProjector.PointProperties
            {
                position = p0.transform.position,
                rotation = p0.transform.rotation
            });

            Vector3 point = p0.transform.position + p0.transform.forward * (layer.transform.localScale.z * 0.5f);
            PointProjectorDatabase.PlotPoint($"P1", $"P1", PointProjector.Type.Green, point);
            
            Vector3[] positions = new Vector3[2];
            positions[0] = p0.transform.position;
            positions[1] = point;
            lineRendererA.positionCount = positions.Length;
            lineRendererA.SetPositions(positions);

            // Part B
            if (p1 == null)
            {
                p1 = new GameObject();
                p1.name = "p1";
                p1.transform.position = layer.transform.position;
            }

            p1.transform.LookAt(transform.position);
            point = p1.transform.position + p1.transform.forward * (layer.transform.localScale.z * 0.5f);
            PointProjectorDatabase.PlotPoint($"P2", $"P2", PointProjector.Type.Blue, point);

            positions = new Vector3[2];
            positions[0] = p1.transform.position;
            positions[1] = point;
            lineRendererB.positionCount = positions.Length;
            lineRendererB.SetPositions(positions);
#endif
        }
#endif

        public void OnApply()
        {
            if (point != null)
            {
                transform.position = point;
            }
        }

        Transform lookCanvas, lookHitPoint, lookAdjustedHitPoint;

        protected override void ProcessRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, GameObject target, RaycastHit hit)
        {
            if (!isLocked)
            {
                base.ProcessRaycastEvent(source, origin, direction, target, hit);
                return;
            }

            if (lookCanvas == null)
            {
                lookCanvas = new GameObject().transform;
                lookCanvas.parent = root.transform;
                lookCanvas.name = "lookCanvas";
                lookCanvas.transform.position = layer.transform.position;
            }

            lookCanvas.transform.LookAt(transform.position);
            float angle = Vector3.SignedAngle(lookCanvas.transform.forward, layer.transform.forward, Vector3.right);

            if (lookHitPoint == null)
            {
                lookHitPoint = new GameObject().transform;
                lookHitPoint.parent = root.transform;
                lookHitPoint.name = "lookHitPoint";
                lookHitPoint.transform.position = layer.transform.position;
            }

            Vector3 relativeDirection = (hit.point - layer.transform.position).normalized;
            Vector3 point = layer.transform.position + relativeDirection * (layer.transform.localScale.z * 0.5f);
            lookHitPoint.transform.LookAt(point);

            if (lookAdjustedHitPoint == null)
            {
                lookAdjustedHitPoint = new GameObject().transform;
                lookAdjustedHitPoint.parent = root.transform;
                lookAdjustedHitPoint.name = "lookAdjustedHitPoint";
                lookAdjustedHitPoint.transform.position = lookHitPoint.transform.position;
            }

            Vector3 tmp = lookHitPoint.transform.eulerAngles;
            tmp.x = -angle;
            lookAdjustedHitPoint.transform.eulerAngles = tmp;

            point = lookAdjustedHitPoint.transform.position + lookAdjustedHitPoint.transform.forward * (layer.transform.localScale.z * 0.5f);
            transform.position = point;

            // Vector3 relativeDirection = (hit.point - layer.transform.position).normalized;
            // point = layer.transform.position + relativeDirection * (layer.transform.localScale.z * 0.5f);

            // p0 = new GameObject();
            // p0.transform.position = layer.transform.position;
            // p0.transform.LookAt(point);

            // Vector3 tmp = p0.transform.eulerAngles;
            // float angle = Vector3.SignedAngle(p0.transform.forward, transform.forward, Vector3.right);
            // tmp.x += angle;
            // p1.transform.position = p0.transform.position;
            // p1.transform.eulerAngles = tmp;

            // point = p0.transform.position + p0.transform.forward * (layer.transform.localScale.z * 0.5f);
            // transform.position = point;
        }
    }
}