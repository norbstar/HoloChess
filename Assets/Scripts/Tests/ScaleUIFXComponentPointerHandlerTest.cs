using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

using UI;
using FX.UI;

namespace Tests
{
    [AddComponentMenu("Tests/Scale UI FX Component Pointer Handler Test")]
    [RequireComponent(typeof(PointerEventHandler))]
    public class ScaleUIFXComponentPointerHandlerTest : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] ScaleUIFXComponent component;

        [Header("Config")]
        [SerializeField] bool scaleContent = false;

        private PointerEventHandler eventHandler;
        private bool isPointerEnter;

        public virtual void Awake()
        {
            ResolveDependencies();
        }

        private void ResolveDependencies() => eventHandler = GetComponent<PointerEventHandler>() as PointerEventHandler;

        void OnEnable() => eventHandler.EventReceived += OnPointerEvent;

        void OnDisable() => eventHandler.EventReceived -= OnPointerEvent;

        private void OnPointerEvent(GameObject gameObject, PointerEventHandler.Event @event, PointerEventData eventData)
        {
            // Debug.Log($"OnPointerEvent : {@event} GameObject : {gameObject.name}");

            switch (@event)
            {
                case PointerEventHandler.Event.Enter:
                    OnPointerEnter(gameObject, eventData);
                    break;

                case PointerEventHandler.Event.Exit:
                    OnPointerExit(gameObject, eventData);
                    break;
            }
        }

        private void OnPointerEnter(GameObject gameObject, PointerEventData eventData)
        {
            // Debug.Log($"OnPointerEnter : {gameObject.name}");

            isPointerEnter = true;
            // StartCoroutine(OnPointerEnterCoroutine(eventData, eventData.pointerEnter));

            component.Scale(new ScaleUIFXComponent.Config
            {
                flow = ScaleUIFXComponent.Flow.ToEnd,
                flags = (scaleContent) ? ScaleUIFXComponent.Flags.ScaleContent : 0
            });
        }

        // private IEnumerator OnPointerEnterCoroutine(PointerEventData eventData, GameObject gameObject)
        // {
        //     component.Scale(new ScaleUIFXComponent.Config
        //     {
        //         flow = ScaleUIFXComponent.Flow.ToEnd
        //     });

        //     yield return null;
        // }

        private void OnPointerExit(GameObject gameObject, PointerEventData eventData)
        {
            // Debug.Log($"OnPointerExit : {gameObject.name}");

            isPointerEnter = false;
            // StartCoroutine(OnPointerExitCoroutine(eventData, eventData.pointerEnter));

            component.Scale(new ScaleUIFXComponent.Config
            {
                flow = ScaleUIFXComponent.Flow.ToStart,
                flags = (scaleContent) ? ScaleUIFXComponent.Flags.ScaleContent : 0
            });
        }

        // private IEnumerator OnPointerExitCoroutine(PointerEventData eventData, GameObject gameObject)
        // {
        //     component.Scale(new ScaleUIFXComponent.Config
        //     {
        //         flow = ScaleUIFXComponent.Flow.ToStart
        //     });

        //     yield return null;
        // }
    }
}