using UnityEngine;

public class PointProjectorDatabase : MonoBehaviour
{
    private static PointProjectorManager manager;

    void Awake() => ResolveDependencies();

    private void ResolveDependencies() => manager = FindObjectOfType<PointProjectorManager>() as PointProjectorManager;

    public static PointProjector PlotPoint(string name, string label, PointProjector.Type type, Vector3 position, Vector3? overrideScale = null)
    {
        var properties = new PointProjector.PointProperties
        {
            position = position
        };

        return PlotPoint(name, label, type, properties, overrideScale);
    }

    public static PointProjector PlotPoint(string name, string label, PointProjector.Type type, PointProjector.PointProperties point, Vector3? overrideScale = null)
    {
        if (manager.TryGet(name, out PointProjector projector))
        {
            projector.Point = point;
        }
        else
        {
            projector = manager.Add(type, name, label, overrideScale);
            projector.Point = point;
        }

        projector.Label = label;
        return projector;
    }

    public static bool DestroyPoint(string name) => manager.Remove(name);
}