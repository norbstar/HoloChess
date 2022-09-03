using UnityEngine;

public class NominatedRaycastProjector : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] GameObject pointerPrefab;
    [SerializeField] Vector3 pointerScale;

    [SerializeField] RaycastNotifier notifier;

    private GameObject instance;
    private GameObject source;

    void OnEnable() => notifier.EventReceived += OnRaycastEvent;

    void OnDisable() => notifier.EventReceived -= OnRaycastEvent;

    // Frame-rate independent call for physics calculations
    void FixedUpdate()
    {
        if (instance == null) return;
        
        instance.transform.LookAt(source.transform);
    }

    private void OnRaycastEvent(GameObject origin, GameObject source, Vector3 point)
    {
        this.source = source;

        if (instance == null)
        {
            instance = Instantiate(pointerPrefab, point, Quaternion.Euler(0f, 0f, 90f));
            instance.transform.localScale = pointerScale;
            instance.gameObject.name = "Pointer";
            instance.transform.parent = origin.transform;
        }
        else
        {
            instance.transform.position = point;
        }
    }
}