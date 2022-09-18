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

        if (TryGet.XR.TryGetControllers(out List<HandController> controllers))
        {
            foreach (HandController controller in controllers)
            {
                switch (controller.WhichHand)
                {
                    case HandController.Hand.Left:
                        projectors.Add(new MultiRaycastProjector.Projector
                        {
                            pointer = leftHandPointer,
                            notifier = controller.Notifier
                        });
                        break;

                    case HandController.Hand.Right:
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