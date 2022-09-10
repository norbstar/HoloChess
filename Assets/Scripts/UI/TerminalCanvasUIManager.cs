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
    public class TerminalCanvasUIManager : AnimatedCanvasUIManager
    {
        [Header("Components")]
        [SerializeField] TerminalPanelUIManager panel;
        public TerminalPanelUIManager Panel { get { return panel; } }
        [SerializeField] GameObject sphere;

        [Header("Config")]
        [SerializeField] float projectedRadius = 2.5f;

        private PointProjectorManager pointProjectorManager;
        private PointProjector hPoint, p1Point, p2Point, p3Point, p4Point, p5Point, p6Point, p7Point, tPoint;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            pointProjectorManager = FindObjectOfType<PointProjectorManager>() as PointProjectorManager;
            // hPoint = pointProjectorManager.Add("Hit Point");
            p1Point = pointProjectorManager.Add(PointProjector.Type.White, "p1");
            p2Point = pointProjectorManager.Add(PointProjector.Type.Red, "p2");
            p3Point = pointProjectorManager.Add(PointProjector.Type.Green, "p3");
            p4Point = pointProjectorManager.Add(PointProjector.Type.Blue, "p4");
            p5Point = pointProjectorManager.Add(PointProjector.Type.Yellow, "p5");
            p6Point = pointProjectorManager.Add(PointProjector.Type.Yellow, "p6");
            p7Point = pointProjectorManager.Add(PointProjector.Type.Yellow, "p7");
            // tPoint = pointProjectorManager.Add("Transform Point");

            // if (pointProjectorManager.TryGet("Hit Point", out PointProjector projector))
            // {
            //     projector.Point = hit.point;
            // }
        }

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

            panel.EnableDragBar(sphere.activeSelf);
        }

        public override void Show()
        {
            // float distance = Vector3.Distance(FindObjectOfType<HomeCanvasUIManager>().transform.position, sphere.transform.position);
            Vector3 direction = (FindObjectOfType<HomeCanvasUIManager>().transform.position - sphere.transform.position).normalized;
            Vector3 projectedPoint = sphere.transform.position + (direction * projectedRadius);
            // transform.position = sphere.transform.position;
            // transform.Translate(direction * projectedRadius);
            // this.p1Point.Point = projectedPoint;
            // Debug.Log($"Projected Point[1] : {this.p1Point.Point}");

            // this.p1Point.Point = sphere.transform.position;
            // this.p2Point.Point = sphere.transform.position + direction;
            // this.p3Point.Point = sphere.transform.position + direction * distance;
            // this.p4Point.Point = sphere.transform.position + direction * projectedRadius;
            
            transform.position = projectedPoint;
            LookAtRoot();

            base.Show();
        }

        protected override void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, RaycastHit hit)
        {
            if (!panel.DragBar.IsPointerDown) return;

            // float distance = Vector3.Distance(hit.point, sphere.transform.position);
            // Vector3 direction = (hit.point - sphere.transform.position).normalized;
            this.p5Point.Point = hit.point;
            Vector3 projectedPoint = source.transform.position + (-direction * projectedRadius);
            this.p6Point.Point = projectedPoint;
            // transform.position = hit.point;
            // transform.Translate(direction * projectedRadius);

            Vector3 offset = panel.transform.position - panel.DragBar.transform.position;

            // this.hitPoint.Point = hit.point;
            // Debug.Log($"Hit Point : {this.hPoint.Point}");

            // this.p5Point.Point = sphere.transform.position + direction;
            // this.p6Point.Point = sphere.transform.position + direction * distance;
            // this.p7Point.Point = sphere.transform.position + direction * projectedRadius;

           // Debug.Log($"Projected Point[1] : {this.p1Point.Point}");
            
            // this.transformPoint.Point = projectedPoint + offset;
            // Debug.Log($"Transform Point : {this.transformPoint.Point}");

            // transform.position = projectedPoint + offset;
        }
    }
}