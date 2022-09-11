using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MenuLayer : MonoBehaviour
{
    private List<GameObject> children;

    void Awake() => children = new List<GameObject>();

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
}