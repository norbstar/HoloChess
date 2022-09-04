using System.Collections.Generic;

using UnityEngine;

public class LeftHandRaycastProjector : MultiRaycastProjector
{
    [Header("Config")]
    public MultiRaycastProjector.Pointer pointer;

    protected override List<MultiRaycastProjector.Projector> GetProjectors()
    {
        List<MultiRaycastProjector.Projector> projectors = new List<MultiRaycastProjector.Projector>();

        if (TryGet.TryGetControllerWithCharacteristics(HandController.LeftHandCharacteristics, out HandController controller))
        {
            projectors.Add(new MultiRaycastProjector.Projector
            {
                pointer = pointer,
                notifier = controller.Notifier
            });
        }

        return projectors;
    }
}