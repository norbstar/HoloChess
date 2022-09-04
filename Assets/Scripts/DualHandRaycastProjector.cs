using System.Collections.Generic;

using UnityEngine;

public class DualHandRaycastProjector : MultiRaycastProjector
{
    [Header("Config")]
    public MultiRaycastProjector.Pointer leftHandPointer;
    public MultiRaycastProjector.Pointer rightHandPointer;

    protected override List<MultiRaycastProjector.Projector> GetProjectors()
    {
        List<MultiRaycastProjector.Projector> projectors = new List<MultiRaycastProjector.Projector>();

        if (TryGet.TryGetControllers(out List<HandController> controllers))
        {
            // Debug.Log($"Dual Hand Raycast Projector Resolved {controllers.Count} controllers");

            foreach (HandController controller in controllers)
            {
                switch (controller.WhichHand)
                {
                    case HandController.Hand.Left:
                        // Debug.Log($"Dual Hand Raycast Projector Resolved Left Hand Controller");
                        projectors.Add(new MultiRaycastProjector.Projector
                        {
                            pointer = leftHandPointer,
                            notifier = controller.Notifier
                        });
                        break;

                    case HandController.Hand.Right:
                        // Debug.Log($"Dual Hand Raycast Projector Resolved Right Hand Controller");
                        projectors.Add(new MultiRaycastProjector.Projector
                        {
                            pointer = rightHandPointer,
                            notifier = controller.Notifier
                        });
                        break;
                }
            }
        }

        return projectors;
    }
}