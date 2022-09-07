using System.Collections.Generic;

using UnityEngine;

public class RaycastNotifier : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] bool invertRay = false;
    [SerializeField] float invertOffset = 1f;
    [SerializeField] List<string> masks;

    public delegate void OnRaycastEvent(GameObject origin, RaycastHit hit);
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
        Vector3 origin = (invertRay) ? transform.position + transform.forward * invertOffset : transform.position;
        Vector3 direction = (invertRay) ? -transform.forward : transform.forward;
        
        var ray = new Ray(origin, direction);
        Debug.DrawRay(origin, direction, Color.green);
        
        bool hasHit = Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, mixedLayerMask);

        if (!hasHit) return;

        EventReceived?.Invoke(gameObject, hit);
    }
}