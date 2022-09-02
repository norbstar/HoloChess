using UnityEngine;

public class LeftHandRaycastProjector : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] GameObject referencePrefab;

    private HandController leftHandController;
    private GameObject reference;

    void Awake()
    {
        if (TryGet.TryGetControllerWithCharacteristics(HandController.LeftHandCharacteristics, out HandController controller))
        {
            leftHandController = controller;
        }
    }

    void OnEnable()
    {
        if (leftHandController != null)
        {
            leftHandController.RaycastEventReceived += OnRaycastEvent;
        }
    }

    void OnDisable()
    {
        if (leftHandController != null)
        {
            leftHandController.RaycastEventReceived -= OnRaycastEvent;
        }
    }

    private void OnRaycastEvent(HandController controller, GameObject source, Vector3 point)
    {
        if (reference == null)
        {
            reference = Instantiate(referencePrefab, point, Quaternion.identity);
            reference.gameObject.name = "Reference";
        }
        else
        {
            reference.transform.position = point;
        }
    }
}