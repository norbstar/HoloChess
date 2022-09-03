using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class MultiRaycastProjector : MonoBehaviour
{
    [Serializable]
    public class Projector
    {
        [Header("Prefab")]
        public GameObject pointerPrefab;
        public Vector3 pointerScale;

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

    // Frame-rate independent call for physics calculations
    void FixedUpdate()
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

    private void OnRaycastEvent(GameObject origin, GameObject source, Vector3 point)
    {
        Projector projector = projectors.FirstOrDefault(p => GameObject.ReferenceEquals(origin, p.notifier.gameObject));

        if (projector == null) return;

        projector.Source = source;

        if (projector.Instance == null)
        {
            projector.Instance = Instantiate(projector.pointerPrefab, point, Quaternion.Euler(0f, 0f, 90f));
            projector.Instance.transform.localScale = projector.pointerScale;
            projector.Instance.gameObject.name = $"{origin.name}-Pointer";
            projector.Instance.transform.parent = origin.transform;
        }
        else
        {
            projector.Instance.transform.position = point;
        }
    }
}