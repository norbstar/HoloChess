using System.Collections.Generic;

using UnityEngine;

public class RaycastNotifier : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] bool invertRay = false;
    [SerializeField] float invertOffset = 1f;
    [SerializeField] List<string> masks;

    public delegate void OnRaycastEvent(GameObject source, Vector3 point);
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

    // Frame-rate independent call for physics calculations
    void FixedUpdate()
    {
        Vector3 origin = (invertRay) ? transform.position + transform.forward * invertOffset : transform.position;
        Vector3 direction = (invertRay) ? -transform.forward : transform.forward;
        
        var ray = new Ray(origin, direction);
        Debug.DrawRay(origin, direction, Color.green);
        
        bool hasHit = Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, mixedLayerMask);
        Debug.Log($"Raycast Handler Has Hit : {hasHit}");

        if (hasHit)
        {
            Debug.Log($"Raycast Handler Source : {gameObject.name} Target : {hit.collider.gameObject.name} Point : {hit.point} Origin : {ray.origin} Direction : {ray.direction} Distance : {hit.distance}");
            EventReceived?.Invoke(hit.transform.gameObject, hit.point);
        }
    }
}