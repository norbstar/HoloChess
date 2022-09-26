using UnityEngine;

public static class VectorExtensions
{
    public static string ToPrecisionString(this Vector3 vector) => $"[{vector.x}, {vector.y}, {vector.z}]";
}