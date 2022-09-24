using UnityEngine;

using Scriptables;

namespace Tests
{
    public class SciptableContainerTest : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] BaseScriptable baseScriptable;
        [SerializeField] ExtendedScriptable extendedScriptable;
    }
}