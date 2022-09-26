using UnityEngine;

public class PointProjectorDatabase : MonoBehaviour
{
    private static PointProjectorManager manager;

    void Awake() => ResolveDependencies();

    private void ResolveDependencies() => manager = FindObjectOfType<PointProjectorManager>() as PointProjectorManager;

    public static PointProjector PlotPoint(string name, string label, PointProjector.Type type, Vector3 position, Quaternion? rotation = null, Vector3? scale = null)
    {
        var properties = new PointProjector.PointProperties
        {
            position = position
        };

        if (rotation.HasValue)
        {
            properties.rotation = rotation.Value;
        }

        if (scale.HasValue)
        {
            properties.scale = scale.Value;
        }

        return PlotPoint(name, label, type, properties);
    }

    private static PointProjector PlotPoint(string name, string label, PointProjector.Type type, PointProjector.PointProperties point)
    {
        if (manager.TryGet(name, out PointProjector projector))
        {
            projector.Point = point;
        }
        else
        {
            projector = manager.Add(type, name, label);
            projector.Point = point;
        }

        projector.Label = label;
        return projector;
    }

    public static bool DestroyPoint(string name) => manager.Remove(name);
}