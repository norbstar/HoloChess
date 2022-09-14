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
    [SerializeField] List<string> masks;

    public delegate void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, GameObject target, RaycastHit hit);
    public event OnRaycastEvent EventReceived;

    private int mixedLayerMask;

    void Awake()
    {
        foreach (string mask in masks)
        {
            if (mask != null)
            {
                mixedLayerMask |= LayerMask.GetMask(mask);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EventReceived.GetInvocationList().Length == 0) return;
        
        Debug.Log($"{gameObject.name} Update Listeners : {EventReceived.GetInvocationList().Length}");

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
        RaycastHit [] hits = (Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, mixedLayerMask));

        foreach (RaycastHit hit in hits)
        {
            EventReceived?.Invoke(gameObject, ray.origin, ray.direction, hit.transform.gameObject, hit);
        }
    }

    private void RayCastOne(Ray ray)
    {
        bool hasHit = Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, mixedLayerMask);

        if (!hasHit) return;

        EventReceived?.Invoke(gameObject, ray.origin, ray.direction, hit.transform.gameObject, hit);
    }
}