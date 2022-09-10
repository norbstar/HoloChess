using UnityEngine;

// https://docs.unity3d.com/Manual/ExecutionOrder.html

namespace Tests
{
    public class EventExecutionOrderTest : MonoBehaviour
    {
        void Awake() => Debug.Log($"{gameObject} : Awake");

        void OnEnable() => Debug.Log($"{gameObject} : OnEnable");

        void Start() => Debug.Log($"{gameObject} : Start");

        void Update() => Debug.Log($"{gameObject} : Update");

        void FixedUpdate() => Debug.Log($"{gameObject} : FixedUpdate");

        void LateUpdate() => Debug.Log($"{gameObject} : LateUpdate");

        void OnDisable() => Debug.Log($"{gameObject} : OnDisable");
    }
}