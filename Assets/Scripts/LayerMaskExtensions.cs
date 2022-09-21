using System.Collections.Generic;
using System.Text;

using UnityEngine;

public static class LayerMaskExtensions
{
    public static LayerMask CreateLayerMask(List<string> layers) => LayerMask.GetMask(layers.ToArray());

    public static string AsString(LayerMask layerMask)
    {
        var hasLayers = new bool[32];
 
        StringBuilder logBuilder = new StringBuilder();
        logBuilder.Append("[");

        StringBuilder nameBuilder = new StringBuilder();

        for (int i = 0; i < 32; i++)
        {
            if (layerMask == (layerMask | (1 << i)))
            {
                var name = LayerMask.LayerToName(i);
                nameBuilder.Append((nameBuilder.Length > 0)  ? $",{name}" : $"{name}");
            }
        }

        logBuilder.Append($"{nameBuilder.ToString()}");
        logBuilder.Append("]");
        
        return logBuilder.ToString();
    }

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