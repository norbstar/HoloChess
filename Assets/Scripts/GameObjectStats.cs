using UnityEngine;

public class GameObjectStats : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] Vector3 position;
    [SerializeField] Vector3 localPosition;
    
    void Awake()
    {
        position = transform.position;
        localPosition = transform.localPosition;
    }
}