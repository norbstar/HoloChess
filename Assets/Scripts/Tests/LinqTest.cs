using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace Tests
{
    public class LinqTest : MonoBehaviour
    {
        private List<string> labels;

        void Awake() => labels = new List<string>();

        // Start is called before the first frame update
        void Start()
        {
            Setup();

            TestA();
            TestB();
        }

        private void Setup()
        {
            // labels.Add("Origin");
            labels.Add("Canvas");
        }

        private void TestA()
        {
            var label = labels.FirstOrDefault(r => r.Equals("Origin"));
            Debug.Log($"TestA IsNull : {label == null} Result : {label}");
        }

        private void TestB()
        {
            var label = labels.Where(r => r.Equals("Origin")).FirstOrDefault();
            Debug.Log($"TestB IsNull : {label == null} Result : {label}");
        }
    }
}