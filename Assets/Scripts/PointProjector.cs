using System;

using UnityEngine;

// [DisallowMultipleComponent]
public class PointProjector : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] GameObject bluePrefab;
    [SerializeField] GameObject greenPrefab;
    [SerializeField] GameObject redPrefab;
    [SerializeField] GameObject yellowPrefab;
    [SerializeField] GameObject whitePrefab;

    public enum Type
    {
        Blue,
        Green,
        Red,
        White,
        Yellow
    }

    private GameObject instance;
    private Vector3 point;
    private string label;

    public Vector3 Point { get { return point; } set { point = value; } }
    public string Label { get { return label; } set { label = value; } } 

    public void Build(Type type, string label, Vector3? overrideScale = null)
    {
        switch (type)
        {
            case Type.Blue:
                instance = Instantiate(bluePrefab, point, Quaternion.identity);
                break;

            case Type.Green:
                instance = Instantiate(greenPrefab, point, Quaternion.identity);
                break;

            case Type.Red:
                instance = Instantiate(redPrefab, point, Quaternion.identity);
                break;

            case Type.Yellow:
                instance = Instantiate(yellowPrefab, point, Quaternion.identity);
                break;

            case Type.White:
                instance = Instantiate(whitePrefab, point, Quaternion.identity);
                break;
        }

        if (overrideScale.HasValue)
        {
            instance.transform.localScale = overrideScale.Value;
        }

        instance.GetComponent<PointManager>().Text = label;
        this.label = name = label;
    }

    // Update is called once per frame
    void Update() => instance.transform.position = point;
}