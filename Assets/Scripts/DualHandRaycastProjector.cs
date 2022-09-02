using System.Collections.Generic;

using UnityEngine;

public class DualHandRaycastProjector : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] GameObject leftHandReferencePrefab;
    [SerializeField] GameObject rightHandReferencePrefab;

    private HandController leftHandController, rightHandController;
    private GameObject leftHandReference, rightHandReference;

    void Awake()
    {
        if (TryGet.TryGetControllers(out List<HandController> controllers))
        {
            Debug.Log($"Dual Hand Raycast Projector Resolved {controllers.Count} controllers");

            foreach (HandController controller in controllers)
            {
                switch (controller.WhichHand)
                {
                    case HandController.Hand.Left:
                        Debug.Log($"Dual Hand Raycast Projector Resolved Left Hand Controller");
                        leftHandController = controller;
                        break;

                    case HandController.Hand.Right:
                        Debug.Log($"Dual Hand Raycast Projector Resolved Right Hand Controller");
                        rightHandController = controller;
                        break;
                }
            }
        }
    }

    void OnEnable()
    {
        if (leftHandController != null)
        {
            Debug.Log($"Dual Hand Raycast Projector Registering Left Hand Controller");
            leftHandController.RaycastEventReceived += OnRaycastEvent;
        }

        if (rightHandController != null)
        {
            Debug.Log($"Dual Hand Raycast Projector Registering Right Hand Controller");
            rightHandController.RaycastEventReceived += OnRaycastEvent;
        }
    }

    void OnDisable()
    {
        if (leftHandController != null)
        {
            Debug.Log($"Dual Hand Raycast Projector Unregistering Left Hand Controller");
            leftHandController.RaycastEventReceived -= OnRaycastEvent;
        }

        if (rightHandController != null)
        {
            Debug.Log($"Dual Hand Raycast Projector Unregistering Right Hand Controller");
            rightHandController.RaycastEventReceived -= OnRaycastEvent;
        }
    }

    // Frame-rate independent call for physics calculations
    // void FixedUpdate()
    // {
    //     Vector3 offset;
    //     // offset = transform.position - source.transform.position;
    //     // transform.LookAt(transform.position + offset);

    //     if (leftHandReference != null)
    //     {
    //         leftHandReference.transform.LookAt(source.transform.position, transform.up);
    //     }
    // }

    private void OnRaycastEvent(HandController controller, GameObject source, Vector3 point)
    {
        Debug.Log($"Dual Hand Raycast Projector Controller : {controller.name} Source : {source.name} Point : {point}");
        Quaternion rotation;
        Vector3 offset;

        switch (controller.WhichHand)
        {
            case HandController.Hand.Left:
                if (leftHandReference == null)
                {
                    leftHandReference = Instantiate(leftHandReferencePrefab, point, Quaternion.Euler(0f, 0f, 90f));
                    leftHandReference.gameObject.name = "Left Hand Reference";
                }
                else
                {
                    leftHandReference.transform.position = point;
                }

                // rotation = Quaternion.Euler(source.transform.position - leftHandReference.transform.position);
                // leftHandReference.transform.rotation = rotation;
                // leftHandReference.transform.LookAt(source.transform.position, Vector3.forward);
                // leftHandReference.transform.LookAt(source.transform.position);
                leftHandReference.transform.LookAt(point);
                break;

            case HandController.Hand.Right:
                if (rightHandReference == null)
                {
                    rightHandReference = Instantiate(rightHandReferencePrefab, point, Quaternion.Euler(0f, 0f, 90f));
                    rightHandReference.gameObject.name = "Right Hand Reference";
                }
                else
                {
                    rightHandReference.transform.position = point;
                }

                // rotation = Quaternion.Euler(rightHandReference.transform.position - source.transform.position);
                // rightHandReference.transform.rotation = rotation;
                // rightHandReference.transform.LookAt(source.transform.position, Vector3.back);
                //  rightHandReference.transform.LookAt(source.transform.position);
                rightHandReference.transform.LookAt(point);
                break;
        }
    }
}