using System;

using UnityEngine;

public class NominatedRaycastProjector : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] GameObject pointerPrefab;
    [SerializeField] Vector3 pointerScale;

    [SerializeField] RaycastNotifier notifier;

    [Serializable]
    public class Info
    {
        public GameObject source;
        public Vector3 origin;
        public Vector3 direction;
        public GameObject target;
        public RaycastHit hit;
    }
    
    private Info hitInfo;
    private GameObject instance;

    void OnEnable() => notifier.EventReceived += OnRaycastEvent;

    void OnDisable() => notifier.EventReceived -= OnRaycastEvent;

    // Update is called once per frame
    void Update()
    {
        if (instance == null) return;
        
        instance.transform.LookAt(hitInfo.target.transform);
    }

    private void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, GameObject target, RaycastHit hit)
    {
        Vector3 point = hit.point;

        this.hitInfo = new Info
        {
            source = source,
            origin = origin,
            direction = direction,
            target = target,
            hit = hit
        };

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