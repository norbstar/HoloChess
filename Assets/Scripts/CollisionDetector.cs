using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] bool detectOnStay = false;
    [SerializeField] bool invertRay = false;

    public enum Event
    {
        OnCollisonEnter,
        OnTriggerEnter,
        OnCollisonStay,
        OnTriggerStay,
        OnCollisonExit,
        OnTriggerExit
    }

    public delegate void OnCollisionEvent(GameObject source, Event @event, Vector3? nearestPoint);
    public event OnCollisionEvent EventReceived;

    public void OnCollisonEnter(Collision collision) => EventReceived?.Invoke(collision.gameObject, Event.OnCollisonEnter, collision.contacts[0].point);

    public void OnTriggerEnter(Collider collider) => EventReceived?.Invoke(collider.gameObject, Event.OnTriggerEnter, null);

    public void OnCollisonStay(Collision collision) => EventReceived?.Invoke(collision.gameObject, Event.OnCollisonStay, collision.contacts[0].point);

    public void OnTriggerStay(Collider collider) => EventReceived?.Invoke(collider.gameObject, Event.OnTriggerStay, null);

    public void OnCollisonExit(Collision collision) => EventReceived?.Invoke(collision.gameObject, Event.OnCollisonExit, collision.contacts[0].point);
 
    public void OnTriggerExit(Collider collider) => EventReceived?.Invoke(collider.gameObject, Event.OnTriggerExit, null);
}