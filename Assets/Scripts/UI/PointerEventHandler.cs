using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [AddComponentMenu("UI/Pointer Event Handler")]
    public class PointerEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [Header("Config")]
        [SerializeField] bool enableCallbacks = true;
        public bool EnableCallbacks { get { return enableCallbacks; } set { enableCallbacks = value; } }

        public enum Event
        {
            Enter,
            Down,
            Up,
            Exit
        }

        public delegate void OnPointerEvent(GameObject gameObject, Event @event, PointerEventData pointerEventData);
        public event OnPointerEvent EventReceived;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!enableCallbacks) return;

            // Debug.Log($"OnPointerEnter : {eventData.pointerEnter.gameObject.name}");
            EventReceived?.Invoke(gameObject, Event.Enter, eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!enableCallbacks) return;

            // Debug.Log($"OnPointerDown : {eventData.pointerEnter.gameObject.name}");
            EventReceived?.Invoke(gameObject, Event.Down, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!enableCallbacks) return;

            // Debug.Log($"OnPointerUp : {eventData.pointerEnter.gameObject.name}");
            EventReceived?.Invoke(gameObject, Event.Up, eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!enableCallbacks) return;
            
            // Debug.Log($"OnPointerExit : {eventData.pointerEnter.gameObject.name}");
            EventReceived?.Invoke(gameObject, Event.Exit, eventData);
        }
    }
}