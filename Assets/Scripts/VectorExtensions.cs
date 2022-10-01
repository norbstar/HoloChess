using UnityEngine;

public static class VectorExtensions
{
    public static string ToPrecisionString(this Vector2 vector) => $"[{vector.x}, {vector.y}]";

    public static string ToPrecisionString(this Vector3 vector) => $"[{vector.x}, {vector.y}, {vector.z}]";
}