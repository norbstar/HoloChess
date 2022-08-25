using UnityEngine;

using Mutator;

namespace Test
{
    [RequireComponent(typeof(PositionMutator))]
    public class PositionMutatorTest : MonoBehaviour
    {
        [SerializeField] Vector3 target;

        void Awake() => ResolveDependencies();

        private PositionMutator mutator;

        private void ResolveDependencies() => mutator = GetComponent<PositionMutator>() as PositionMutator;

        // Start is called before the first frame update
        void Start() => mutator.MoveTo(target);
    }
}