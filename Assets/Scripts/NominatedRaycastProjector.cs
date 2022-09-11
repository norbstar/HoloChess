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

    // Update is called once per frame
    void Update()
    {
        if (instance == null) return;
        
        instance.transform.LookAt(source.transform);
    }

    private void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, GameObject target, RaycastHit hit)
    {
        // GameObject source = hit.transform.gameObject;
        Vector3 point = hit.point;

        this.source = source;

        if (instance == null)
        {
            instance = Instantiate(pointerPrefab, point, Quaternion.Euler(0f, 0f, 90f));
            instance.transform.localScale = pointerScale;
            instance.gameObject.name = $"{source.name}-Pointer";
        }
        else
        {
            instance.transform.position = point;
        }
    }
}