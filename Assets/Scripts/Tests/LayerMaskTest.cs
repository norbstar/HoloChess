using System.Collections.Generic;

using UnityEngine;

public class LayerMaskTest : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] List<string> compositeMask;

    private int layerMask;

    void Awake() => layerMask = LayerMaskExtensions.CreateLayerMask(compositeMask);

    // Start is called before the first frame update
    void Start()
    {
        bool inLayerMask = LayerMaskExtensions.HasLayer(layerMask, LayerMask.NameToLayer("Floor"));
        Debug.Log($"In Layer Mask : {inLayerMask}");
    }
}
