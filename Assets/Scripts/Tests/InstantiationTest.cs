using System;

using UnityEngine;

namespace Tests
{
    public class InstantiationTest : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] GameObject prefab;

        [Serializable]
        public class Scale
        {
            public bool shouldOverride = false;
            public Vector3 localScale = Vector3.one;
        }

        [SerializeField] Scale scale;

        private GameObject instance;
        public GameObject Instance { get { return instance; } }

        // Start is called before the first frame update
        void Start()
        {
            instance = Instantiate(prefab, transform.position, Quaternion.identity, transform);

            if (scale.shouldOverride)
            {
                instance.transform.localScale = scale.localScale;
            }

            var label = $"{prefab.name} Instance";
            instance.GetComponent<PointManager>().Text = label;
            instance.name = label;
        }
    }
}