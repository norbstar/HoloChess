using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererSettings : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] float startWidth = 0.0025f;
    [SerializeField] Color startColor = Color.red;
    [SerializeField] float endWidth = 0.0025f;
    [SerializeField] Color endColor = Color.blue;

    private LineRenderer lineRenderer;

    void Awake() => ResolveDependencies();

    private void ResolveDependencies() => lineRenderer = GetComponent<LineRenderer>() as LineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.startWidth = startWidth;
        lineRenderer.startColor = startColor;
        lineRenderer.endWidth = endWidth;
        lineRenderer.endColor = endColor;
    }
}