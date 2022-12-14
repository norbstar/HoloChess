using UnityEngine;

namespace Utilities.Points
{
    [AddComponentMenu("Utilities/Points/Point Projector")]
    public class PointProjector : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] GameObject bluePrefab;
        [SerializeField] GameObject greenPrefab;
        [SerializeField] GameObject redPrefab;
        [SerializeField] GameObject yellowPrefab;
        [SerializeField] GameObject purplePrefab;
        [SerializeField] GameObject orangePrefab;
        [SerializeField] GameObject whitePrefab;
        [SerializeField] GameObject customPrefab;

        public enum Type
        {
            Blue,
            Green,
            Red,
            Yellow,
            Purple,
            Orange,
            White,
            Custom
        }

        private GameObject instance;
        private PointManager manager;
        private string label;

        public class PointProperties
        {
            public Vector3? position = Vector3.zero;
            public Quaternion? rotation = Quaternion.identity;
            public Vector3? scale = Vector3.zero;
        }

        private PointProperties point;

        public PointProperties Point
        {
            get
            {
                return point;
            }
            
            set
            {
                if (value.position.HasValue)
                {
                    point.position = value.position.Value;
                }

                if (value.rotation.HasValue)
                {
                    point.rotation = value.rotation.Value;
                }

                if (value.scale.HasValue)
                {
                    point.scale = value.scale.Value;
                }
            }
        }

        void Awake() => point = new PointProperties();
        
        public string Label
        {
            get
            {
                return label;
            }
            
            set
            {
                label = value;
                manager.Text = label;
            }
        } 

        public void Build(Type type, string label)
        {
            switch (type)
            {
                case Type.Blue:
                    instance = Instantiate(bluePrefab, point.position.Value, point.rotation.Value);
                    break;

                case Type.Green:
                    instance = Instantiate(greenPrefab, point.position.Value, point.rotation.Value);
                    break;

                case Type.Red:
                    instance = Instantiate(redPrefab, point.position.Value, point.rotation.Value);
                    break;

                case Type.Yellow:
                    instance = Instantiate(yellowPrefab, point.position.Value, point.rotation.Value);
                    break;

                case Type.Purple:
                    instance = Instantiate(purplePrefab, point.position.Value, point.rotation.Value);
                    break;

                case Type.Orange:
                    instance = Instantiate(orangePrefab, point.position.Value, point.rotation.Value);
                    break;

                case Type.White:
                    instance = Instantiate(whitePrefab, point.position.Value, point.rotation.Value);
                    break;

                case Type.Custom:
                    if (customPrefab == null) return;
                    instance = Instantiate(customPrefab, point.position.Value, point.rotation.Value);
                    break;
            }

            instance.transform.parent = transform;
            
            manager = instance.GetComponent<PointManager>() as PointManager;
            this.label = manager.Text = label;
        }

        // Update is called once per frame
        void Update()
        {
            instance.transform.position = point.position.Value;
            instance.transform.rotation = point.rotation.Value;
            instance.transform.localScale = point.scale.Value;
        }
    }
}