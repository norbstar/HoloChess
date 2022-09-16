using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PlayerController : MonoBehaviour
    {
        [field : SerializeField] public Material Detected { get; private set; }
        [field : SerializeField] public Material Undetected { get; private set; }

        private new MeshRenderer renderer;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => renderer = GetComponent<MeshRenderer>() as MeshRenderer;

        public void SetDetected(bool detected) => renderer.material = (detected) ? Detected : Undetected;
    }
}