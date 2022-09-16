using System.Collections.Generic;

using UnityEngine;

public static class LayerMaskExtensions
{
    public static LayerMask CreateLayerMask(List<string> layers) => LayerMask.GetMask(layers.ToArray());

    // public static LayerMask CreateLayerMask(List<string> layers)
    // {
    //     LayerMask layerMask = new LayerMask();

    //     foreach (string layer in layers)
    //     {
    //         if (layer != null)
    //         {
    //             layerMask.value |= LayerMask.GetMask(layer);
    //         }
    //     }

    //     return layerMask;
    // }

    public static bool HasLayer(this LayerMask layerMask, int layer) => (layerMask == (layerMask | (1 << layer)));
 
    public static bool[] HasLayers(this LayerMask layerMask)
    {
        var hasLayers = new bool[32];
 
        for (int i = 0; i < 32; i++)
        {
            if (layerMask == (layerMask | (1 << i)))
            {
                hasLayers[i] = true;
            }
        }
 
        return hasLayers;
    }
}