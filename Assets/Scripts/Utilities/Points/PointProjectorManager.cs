using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Utilities.Points
{
    public class PointProjectorManager : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] GameObject prefab;

        private List<PointProjector> projectors;

        public PointProjector Add(PointProjector.Type type, string name, string label)
        {
            if (projectors == null)
            {
                projectors = new List<PointProjector>();
            }

            if (projectors.FirstOrDefault(p => p.name.Equals(name))) return null;

            var instance = Instantiate(prefab, Vector3.zero, Quaternion.identity, gameObject.transform);
            instance.name = name;

            var projector = instance.GetComponent<PointProjector>();
            projector.Build(type, label);
            projectors.Add(projector);

            return projector;
        }

        public bool Remove(string name)
        {
            if (projectors == null) return false;

            var instance = projectors.FirstOrDefault(p => p.name.Equals(name));
            
            if (instance != null)
            {
                projectors.Remove(instance);
                Destroy(instance.gameObject);
                return true;
            }

            return false;
        }
        
        public bool TryGet(string name, out PointProjector projector)
        {
            if (projectors == null)
            {
                projector = default(PointProjector);
                return false;
            }

            projector = projectors.FirstOrDefault(p => p.name.Equals(name));
            return (projector != null);
        }
    }
}