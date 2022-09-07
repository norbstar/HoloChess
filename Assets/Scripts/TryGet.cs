using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TryGet
{
    public class XR
    {
        public static bool TryGetControllers(out List<HandController> controllers)
        {
            controllers = (GameObject.FindObjectsOfType(typeof(HandController)) as HandController[]).ToList<HandController>();
            return (controllers.Count > 0);
        }

        public static bool TryGetIdentifyController(GameObject interactor, out HandController controller)
        {
            if (interactor != null && interactor.CompareTag("Hand"))
            {
                if (interactor.TryGetComponent<HandController>(out HandController handController))
                {
                    controller = handController;
                    return true;
                }
            }

            controller = default(HandController);
            return false;
        }

        public static bool TryGetControllerWithCharacteristics(InputDeviceCharacteristics characteristics, out HandController controller)
        {
            if (TryGetControllers(out List<HandController> controllers))
            {
                controller = controllers.FirstOrDefault(c => (int) characteristics == (int) c.Characteristics);
                return (controller != null);
            }

            controller = default(HandController);
            return false;
        }

        public static bool TryGetOpposingController(HandController controller, out HandController opposingController)
        {
            opposingController = null;

            if (TryGetControllers(out List<HandController> controllers))
            {
                var characteristics = controller.Characteristics;

                if ((int) characteristics == (int) HandController.LeftHandCharacteristics)
                {
                    var rightController = (HandController) controllers.FirstOrDefault(hc => (int) hc.Characteristics == (int) HandController.RightHandCharacteristics);
                    opposingController = (rightController != null) ? rightController : null;
                }
                else if ((int) characteristics == (int) HandController.RightHandCharacteristics)
                {
                    var leftController = (HandController) controllers.FirstOrDefault(hc => (int) hc.Characteristics == (int) HandController.LeftHandCharacteristics);
                    opposingController = (leftController != null) ? leftController : null;
                }
            }
        
            return (opposingController != null);
        }

        public static bool TryGetXRController(GameObject gameObject, out XRController xrController)
        {
            xrController = gameObject.GetComponent<XRController>() as XRController;
            return (xrController != null);
        }
    }

    public static bool TryGetRootResolver(GameObject gameObject, out GameObject rootGameObject)
    {
        if (gameObject.TryGetComponent<RootResolver>(out RootResolver rootResolver))
        {
            rootGameObject = rootResolver.Root;
            return true;
        }

        rootGameObject = gameObject;
        return true;
    }
}