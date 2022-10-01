using UnityEngine;
using UnityEngine.UI;

namespace Tests
{
    [AddComponentMenu("Tests/Require sImage Component")]
    [RequireComponent(typeof(Image))]
    public abstract class RequireImageComponent : MonoBehaviour { }
}