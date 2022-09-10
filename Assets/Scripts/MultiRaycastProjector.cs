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

        public GameObject Instance { get { return instance; } set { instance = value; } }
        public GameObject Source { get { return source; } set { source = value; } }

        private GameObject source;
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
                projector.Instance.transform.LookAt(projector.Source.transform);
            }
        }
    }

    private void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, RaycastHit hit)
    {
        // GameObject source = hit.transform.gameObject;
        Vector3 point = hit.point;

        Projector projector = projectors.FirstOrDefault(p => GameObject.ReferenceEquals(origin, p.notifier.gameObject));

        if (projector == null) return;

        projector.Source = source;

        if (projector.Instance == null)
        {
            projector.Instance = Instantiate(projector.pointer.prefab, point, Quaternion.Euler(0f, 0f, 90f));
            projector.Instance.transform.localScale = projector.pointer.scale;
            projector.Instance.gameObject.name = $"{source.name}-Pointer";
        }
        else
        {
            projector.Instance.transform.position = point;
        }
    }
}