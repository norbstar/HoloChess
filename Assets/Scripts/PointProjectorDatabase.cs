using UnityEngine;

public class PointProjectorDatabase : MonoBehaviour
{
    private static PointProjectorManager manager;

    void Awake() => ResolveDependencies();

    private void ResolveDependencies() => manager = FindObjectOfType<PointProjectorManager>() as PointProjectorManager;

    public static void PlotPoint(string label, PointProjector.Type type, Vector3 position, Vector3? overrideScale = null)
    {
        if (manager.TryGet(label, out PointProjector projector))
        {
            projector.Point = position;           
        }
        else
        {
            var instance = manager.Add(type, label, overrideScale);
            instance.Point = position;
        }
    }

    public static bool DestroyPoint(string label) => manager.Remove(label);
}