using System;
using System.Collections.Generic;

using UnityEngine;

public class RaycastNotifier : MonoBehaviour
{
    public enum CastMode
    {
        One,
        All
    }

    [Header("Config")]
    [SerializeField] CastMode castMode = CastMode.All;
    [SerializeField] bool invertRay = false;
    [SerializeField] float invertOffset = 1f;
    [SerializeField] List<string> compositeMask;

    [Serializable]
    public class HitInfo
    {
        public GameObject target;
        public RaycastHit hit;
    }

    public delegate void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, RaycastHit[] hits);
    public event OnRaycastEvent EventReceived;

    private int layerMask;

    void Awake() => layerMask = LayerMaskExtensions.CreateLayerMask(compositeMask);

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"{gameObject.name} Update Listeners : {EventReceived.GetInvocationList().Length}");
        
        if (EventReceived.GetInvocationList().Length == 0) return;

        Vector3 origin = (invertRay) ? transform.position + transform.forward * invertOffset : transform.position;
        Vector3 direction = (invertRay) ? -transform.forward : transform.forward;
        
        var ray = new Ray(origin, direction);
        
        switch (castMode)
        {
            case CastMode.One:
                RayCastOne(ray);
                break;
            
            case CastMode.All:
                RayCastAll(ray);
                break;
        }
    }

    private void RayCastAll(Ray ray)
    {
        Debug.Log($"{gameObject.name} RayCastAll");

        RaycastHit[] hits = (Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, layerMask));
        EventReceived?.Invoke(gameObject, ray.origin, ray.direction, hits);
    }

    private void RayCastOne(Ray ray)
    {
        Debug.Log($"{gameObject.name} RayCastOne");

        bool hasHit = Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, layerMask);

        if (hasHit)
        {
            EventReceived?.Invoke(gameObject, ray.origin, ray.direction, new RaycastHit[] { hit });
        }
    }
}