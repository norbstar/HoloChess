using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class PointProjectorManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] GameObject prefab;

    private List<PointProjector> projectors;

    public PointProjector Add(PointProjector.Type type, string label, Vector3? overrideScale = null)
    {
        if (projectors == null)
        {
            projectors = new List<PointProjector>();
        }

        if (projectors.FirstOrDefault(p => p.Label.Equals(label))) return null;

        var instance = Instantiate(prefab, Vector3.zero, Quaternion.identity, gameObject.transform);
        var projector = instance.GetComponent<PointProjector>();
        projector.Build(type, label, overrideScale);
        projectors.Add(projector);

        return projector;
    }

    public bool Remove(string label)
    {
        if (projectors == null) return false;

        var instance = projectors.FirstOrDefault(p => p.Label.Equals(label));
        
        if (instance != null)
        {
            projectors.Remove(instance);
            Destroy(instance.gameObject);
            return true;
        }

        return false;
    }
    
    public bool TryGet(string label, out PointProjector projector)
    {
        if (projectors == null)
        {
            projector = default(PointProjector);
            return false;
        }

        projector = projectors.FirstOrDefault(p => p.Label.Equals(label));
        return (projector != null);
    }
}