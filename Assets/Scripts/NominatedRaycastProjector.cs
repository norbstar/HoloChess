using System;
using System.Collections.Generic;

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

    private void OnRaycastEvent(GameObject source, List<RaycastNotifier.HitInfo> hits)
    {
        foreach (RaycastNotifier.HitInfo hitInfo in hits)
        {
            Vector3 point = hitInfo.hit.point;
            var target = hitInfo.hit.transform.gameObject;
            
            this.hitInfo = new Info
            {
                source = source,
                origin = hitInfo.origin,
                direction = hitInfo.direction,
                target = target,
                hit = hitInfo.hit
            };

            if (instance == null)
            {
                instance = Instantiate(pointerPrefab, point, Quaternion.identity);
                instance.transform.localScale = pointerScale;
                instance.gameObject.name = $"{source.name}-Pointer";
            }
            else
            {
                instance.transform.position = point;
            }
        }
    }
}