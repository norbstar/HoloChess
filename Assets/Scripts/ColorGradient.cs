using UnityEngine;

public class ColorGradient : MonoBehaviour
{
    [SerializeField] Gradient gradient;

    public Color Value(float time) => gradient.Evaluate(time);
}