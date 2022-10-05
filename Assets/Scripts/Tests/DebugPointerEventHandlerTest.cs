using UnityEngine;
using UnityEngine.EventSystems;

using UI;

namespace Tests
{
    [AddComponentMenu("Debug Pointer Event Handler Test")]
    [RequireComponent(typeof(PointerEventHandler))]
    public class DebugPointerEventHandlerTest : MonoBehaviour
    {
        void OnEnable()
        {
            var eventHandler = GetComponent<PointerEventHandler>() as PointerEventHandler;
            eventHandler.EventReceived += OnPointerEvent;
        }

        void OnDisable()
        {
            var eventHandler = GetComponent<PointerEventHandler>() as PointerEventHandler;
            eventHandler.EventReceived -= OnPointerEvent;
        }

        private void OnPointerEvent(GameObject gameObject, PointerEventHandler.Event @event, PointerEventData eventData)
        {
            // Debug.Log($"OnPointerEvent : {@event}");

            switch (@event)
            {
                case PointerEventHandler.Event.Enter:
                    OnPointerEnter(gameObject, eventData);
                    break;

                case PointerEventHandler.Event.Down:
                    OnPointerDown(gameObject, eventData);
                    break;

                case PointerEventHandler.Event.Up:
                    OnPointerUp(gameObject, eventData);
                    break;

                case PointerEventHandler.Event.Exit:
                    OnPointerExit(gameObject, eventData);
                    break;
            }
        }

        private void OnPointerEnter(GameObject gameObject, PointerEventData eventData) => Debug.Log($"OnPointerEnter");

        private void OnPointerDown(GameObject gameObject, PointerEventData eventData)  => Debug.Log($"OnPointerDown");

        private void OnPointerUp(GameObject gameObject, PointerEventData eventData)  => Debug.Log($"OnPointerUp");

        private void OnPointerExit(GameObject gameObject, PointerEventData eventData)  => Debug.Log($"OnPointerExit");
    }
}