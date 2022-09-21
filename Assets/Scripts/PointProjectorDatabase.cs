using UnityEngine;

public class PointProjectorDatabase : MonoBehaviour
{
    private static PointProjectorManager manager;

    void Awake() => ResolveDependencies();

    private void ResolveDependencies() => manager = FindObjectOfType<PointProjectorManager>() as PointProjectorManager;

    public static void PlotPoint(string name, string label, PointProjector.Type type, Vector3 position, Vector3? overrideScale = null)
    {
        PlotPoint(name, label, type, new PointProjector.PointProperties
        {
            position = position
        }, overrideScale);
    }

    public static void PlotPoint(string name, string label, PointProjector.Type type, PointProjector.PointProperties point, Vector3? overrideScale = null)
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
    }

    public static bool DestroyPoint(string name) => manager.Remove(name);
}