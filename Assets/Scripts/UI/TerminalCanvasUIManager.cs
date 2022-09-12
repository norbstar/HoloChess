using UnityEngine;

using UI.Panels;

namespace UI
{
    public class TerminalCanvasUIManager : DragbarCanvasUIManager<TerminalPanelUIManager>
    {
        private new Camera camera;
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

        protected override void UpdatePosition(RaycastHit hit)
        {
            var point = new Vector3(hit.point.x, layer.transform.position.y, hit.point.z);
            Vector3 direction = (point - layer.transform.position).normalized;
            LayerMask menuLayerMask = LayerMask.GetMask("Far Menu");

            var ray = new Ray(layer.transform.position + direction * (originalOffset * parentToChildMultiplier), -direction);
            bool hasHit = Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, menuLayerMask);

            if (hasHit)
            {
                Vector3 offset = panel.GetObject().transform.position - dragBar.transform.position;
                transform.position = hit.point + offset;
            }
        }
    }
}