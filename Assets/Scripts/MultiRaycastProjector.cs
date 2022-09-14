using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class MultiRaycastProjector : MonoBehaviour
{
    [Serializable]
    public class Pointer
    {
        public GameObject prefab;
        public Vector3 scale;
    }

    [Serializable]
    public class Projector
    {
        [Header("Pointer")]
        public Pointer pointer;

        [Header("Notifier")]
        public RaycastNotifier notifier;

        [Serializable]
        public class Info
        {
            public GameObject source;
            public Vector3 origin;
            public Vector3 direction;
            public GameObject target;
            public RaycastHit hit;
        }

        public GameObject Instance { get { return instance; } set { instance = value; } }
        public Info HitInfo { get { return hitInfo; } set { hitInfo = value; } }

        private Info hitInfo;
        private GameObject instance;
    }

    [Header("Config")]
    [SerializeField] List<Projector> projectors;

    void Awake()
    {
        projectors = GetProjectors();
    }

    protected virtual List<Projector> GetProjectors() => projectors;

    void OnEnable()
    {
        foreach (Projector projector in projectors)
        {
            projector.notifier.EventReceived += OnRaycastEvent;
        }
    }

    void OnDisable()
    {
        foreach (Projector projector in projectors)
        {
            projector.notifier.EventReceived -= OnRaycastEvent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (projectors.Count == 0) return;
        
        foreach (Projector projector in projectors)
        {
            if (projector.Instance != null)
            {
                // -projector.HitInfo.hit.normal
                Debug.Log($"{gameObject.name} LookAt : {projector.HitInfo.target.transform}");
                projector.Instance.transform.LookAt(/*projector.HitInfo.target.transform*/projector.HitInfo.hit.point + -projector.HitInfo.hit.normal);
            }
        }
    }

    private void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, GameObject target, RaycastHit hit)
    {
        Debug.Log($"{gameObject.name} {source.name} OnRaycastEvent : {hit.point}");
        Projector projector = projectors.FirstOrDefault(p => GameObject.ReferenceEquals(source, p.notifier.gameObject));

        if (projector == null) return;

        projector.HitInfo = new Projector.Info
        {
            source = source,
            origin = origin,
            direction = direction,
            target = target,
            hit = hit
        };

        Vector3 point = hit.point;
        
        if (projector.Instance == null)
        {
            projector.Instance = Instantiate(projector.pointer.prefab, point, Quaternion.identity);
            projector.Instance.transform.localScale = projector.pointer.scale;
            projector.Instance.gameObject.name = $"{source.name}-Pointer";
        }
        else
        {
            projector.Instance.transform.position = point;
        }
    }
}