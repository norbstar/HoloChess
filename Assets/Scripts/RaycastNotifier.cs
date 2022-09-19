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

    public enum CastType
    {
        Outbound,
        Pulse
    }

    [Header("Config")]
    [SerializeField] CastMode castMode = CastMode.All;
    [SerializeField] CastType castType = CastType.Outbound;
    [SerializeField] float range = 1f;
    [SerializeField] List<string> compositeMask;

    [Serializable]
    public class HitInfo
    {
        public Vector3 origin;
        public Vector3 direction;
        public RaycastHit hit;
    }

    public delegate void OnRaycastEvent(GameObject source, List<HitInfo> hits);
    public event OnRaycastEvent EventReceived;

    private int layerMask;

    void Awake() => layerMask = LayerMaskExtensions.CreateLayerMask(compositeMask);

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"{gameObject.name} Listener Count : {EventReceived.GetInvocationList().Length}");
        
        if (EventReceived.GetInvocationList().Length == 0) return;

        List<HitInfo> hits = null;

        switch (castType)
        {
            case CastType.Outbound:
                RaycastOutbound(out hits);
                break;

            case CastType.Pulse:
                RaycastPulse(out hits);
                break;
        }


        System.Text.StringBuilder logBuilder = new System.Text.StringBuilder();
        logBuilder.Append($"Notifier Hit Summary");
        logBuilder.Append($"\n[Start]");
        logBuilder.Append($"\nHit Count : {hits.Count}");

        foreach (HitInfo hitInfo in hits)
        {
            var target = hitInfo.hit.transform.gameObject;
            logBuilder.Append($"\nHit Detection : {target.name}");
        }

        logBuilder.Append($"\n[End]");
        Debug.Log(logBuilder.ToString());
        
        if (hits.Count > 0)
        {
            EventReceived?.Invoke(gameObject, hits);
        }
    }

    private RaycastHit[] Cast(Ray ray)
    {
        RaycastHit[] hits = null;

        switch (castMode)
        {
            case CastMode.One:
                if (RaycastOne(ray, out RaycastHit hit))
                {
                    hits = new RaycastHit[] { hit };
                }
                break;
            
            case CastMode.All:
                RaycastAll(ray, out hits);
                break;
        }

        return hits;
    }

    private bool RaycastOutbound(out List<HitInfo> hits)
    {
        var origin = transform.position;
        var direction = transform.forward;
        var ray = new Ray(origin, direction);
        RaycastHit[] hitList = Cast(ray);

        hits = new List<HitInfo>();

        for (int itr = 0; itr < hitList.Length; itr++)
        {
            RaycastHit hit = hitList[itr];

            hits.Add(new HitInfo
            {
                origin = origin,
                direction = direction,
                hit = hit
            });
        }

        return hits.Count > 0;
    }

    private bool RaycastPulse(out List<HitInfo> hits)
    {
        RaycastOutbound(out List<HitInfo> outboundHits);

        hits = new List<HitInfo>();
        hits.AddRange(outboundHits);

        var origin = transform.position + transform.forward * range;
        var direction = -transform.forward;
        var ray = new Ray(origin, direction);
        RaycastHit[] hitList = Cast(ray);

        for (int itr = 0; itr < hitList.Length; itr++)
        {
            RaycastHit hit = hitList[itr];

            hits.Add(new HitInfo
            {
                origin = origin,
                direction = direction,
                hit = hit
            });
        }

        return hits.Count > 0;
    }

    private bool RaycastAll(Ray ray, out RaycastHit[] hits)
    {
        hits = (Physics.RaycastAll(ray.origin, ray.direction, range, layerMask));
        return hits.Length > 0;
    }

    private bool RaycastOne(Ray ray, out RaycastHit hit) => Physics.Raycast(ray.origin, ray.direction, out hit, range, layerMask);
}