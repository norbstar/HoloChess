using UnityEngine;

using Mutator;

namespace Test
{
    [RequireComponent(typeof(AlphaMutator))]
    public class AlphaMutatorTest : MonoBehaviour
    {
        void Awake() => ResolveDependencies();

        private AlphaMutator mutator;

        private void ResolveDependencies() => mutator = GetComponent<AlphaMutator>() as AlphaMutator;

        // Start is called before the first frame update
        void Start() => mutator.FadeOut();
    }
}