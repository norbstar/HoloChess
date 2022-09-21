using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MenuLayer : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] bool enableGizmos = true;

    private new SphereCollider collider;
    public SphereCollider Collider { get { return collider; } }
    private List<GameObject> children;

    void Awake()
    {
        ResolveDependencies();
        children = new List<GameObject>();
    }

    private void ResolveDependencies() => collider = GetComponent<SphereCollider>() as SphereCollider;

    public bool IsShown { get { return gameObject.activeSelf; } }
    
    public void Attach(GameObject gameObject)
    {
        if (children.Contains(gameObject)) return;

        children.Add(gameObject);
    }

    public bool HasChildren { get { return children.Count > 0; } }

    public void Detach(GameObject gameObject)
    {
        if (!children.Contains(gameObject)) return;

        children.Remove(gameObject);
    }

    void OnDrawGizmos()
    {
        if (!enableGizmos)  return;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * (transform.localScale.z * 0.5f));
    }
}