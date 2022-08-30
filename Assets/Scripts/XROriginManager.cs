using System.Linq;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XROriginManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
    foreach (ActionBasedController controller in GetComponentsInChildren<ActionBasedController>().ToList())
    {
        controller.gameObject.SetActive(false);
    }
#endif
    }
}