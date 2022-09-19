using System;
using System.Collections.Generic;

using UnityEngine;

public class ElementToLayerMap<T> : MonoBehaviour
{
    [Serializable]
    public class Map
    {
        public T defaultElement;
        public List<string> defaultCompositeMask;

        [Serializable]
        public class CustomConfig
        {
            public T element;
            public List<string> compositeMask;
        }

        public List<CustomConfig> customConfigs;
    }

    [SerializeField] Map config;

    public class Filter
    {
        public T element;
        public LayerMask layerMask;
    }

    private LayerMask layerMask;
    public LayerMask LayerMask { get { return layerMask; } }
    private List<Filter> filters;
    public List<Filter> Filters { get { return filters; }  }

    void Awake() => PopulateMap();

    private void PopulateMap()
    {
        filters = new List<Filter>();

        var layerMask = LayerMaskExtensions.CreateLayerMask(config.defaultCompositeMask);

        filters.Add(new Filter
        {
            element = config.defaultElement,
            layerMask = layerMask
        });

        this.layerMask |= layerMask;

        foreach (Map.CustomConfig config in config.customConfigs)
        {
            layerMask = LayerMaskExtensions.CreateLayerMask(config.compositeMask);

            filters.Add(new Filter
            {
                element = config.element,
                layerMask =  layerMask
            });

            this.layerMask |= layerMask;
        }
    }
    
    public bool TryGetMapItem(int layer, out T element)
    {
        foreach (Filter mapItem in filters)
        {
            if (LayerMaskExtensions.HasLayer(mapItem.layerMask, layer))
            {
                element = mapItem.element;
                return true;
            }
        }

        element = default(T);
        return false;
    }
}