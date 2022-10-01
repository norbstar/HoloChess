using UnityEngine;

using UI.Panels;

namespace UI
{
    [AddComponentMenu("UI/Termial Canvas UI Manager")]
    public class TerminalCanvasUIManager : DragbarCanvasUIManager<TerminalPanelUIManager>
    {
        private bool isLocked;

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

        protected override void ProcessRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, GameObject target, RaycastHit hit)
        {
            if (!isLocked)
            {
                base.ProcessRaycastEvent(source, origin, direction, target, hit);
                return;
            }

            // Step 1 - Vertically align the hit point to the height of the canvas
            Vector3 vAlignedPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            
            // Step 2 - Calculate the relative height of the aligned point with respect to the layer 
            float height = Mathf.Abs((layer.transform.localPosition.y - layerRadius) - transform.localPosition.y);

            // Step 3 - Calculate the cap radius of the layer at the adjusted height
            float capRadius = Mathf.Sqrt(Mathf.Pow(layerRadius, 2) - Mathf.Pow(layerRadius - height, 2));

            // Step 4 - Create a reference point at the center of the layer, but adjusted with respect to height
            Vector3 heightAdjustedLayerPoint = new Vector3(layer.transform.position.x, vAlignedPoint.y, layer.transform.position.z);

            // Step 5 - Calculate the relative direction from the height adjusted hit point to the reference point
            Vector3 relativeDirection = (vAlignedPoint - heightAdjustedLayerPoint).normalized;
            
            // Step 6 - Re-project the point in the relative direction at a distance of the cap's radius
            Vector3 point = heightAdjustedLayerPoint + relativeDirection * capRadius;
            
            transform.position = point;
        }
    }
}