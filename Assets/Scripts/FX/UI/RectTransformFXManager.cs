using UnityEngine;

namespace FX.UI
{
    [RequireComponent(typeof(RectTransformFX))]
    public class RectTransformFXManager : MonoBehaviour
    {
        public enum ScaleType
        {
            Proportional,
            RelativeToX,
            RelativeToY,
            RelativeToZ
        }

        private RectTransformFX rectTransformFX;
        public RectTransformFX RectTransformFX { get { return rectTransformFX; } }
        public Vector2 OriginalSize { get { return rectTransformFX.OriginalSize; } }

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => rectTransformFX = GetComponent<RectTransformFX>() as RectTransformFX;

        private Vector2 ScaleRelativeToX(Vector2 startSize, Vector2 endSize)
        {
            // Debug.Log($"{gameObject.name} ScaleRelativeToX StartSize : {startSize.ToPrecisionString()} EndSize : {endSize.ToPrecisionString()}");

            float sizeX = endSize.x;
            float sizeY = startSize.y + (endSize.x - startSize.x);
            endSize = new Vector2(sizeX, sizeY);
            // Debug.Log($"{gameObject.name} ScaleRelativeToX EndSize : {endSize.ToPrecisionString()}");
            
            return endSize;
        }

        private Vector2 ScaleRelativeToY(Vector2 startSize, Vector2 endSize)
        {
            // Debug.Log($"{gameObject.name} ScaleRelativeToY StartSize : {startSize.ToPrecisionString()} EndSize : {endSize.ToPrecisionString()}");

            float sizeY = endSize.y;
            float sizeX = startSize.x + (endSize.y - startSize.y);
            endSize = new Vector2(sizeX, sizeY);
            // Debug.Log($"{gameObject.name} ScaleRelativeToY EndSize : {endSize.ToPrecisionString()}");

            return endSize;
        }

        public Vector3 Scale(Vector2 startSize, Vector2 endSize, ScaleType scaleType)
        {
            // Debug.Log($"{gameObject.name} Scale StartSize : {startSize.ToPrecisionString()} EndSize : {endSize.ToPrecisionString()} ScaleType : {scaleType}");

            switch (scaleType)
            {
                case ScaleType.RelativeToX:
                    endSize = ScaleRelativeToX(startSize, endSize);
                    break;

                case ScaleType.RelativeToY:
                    endSize = ScaleRelativeToY(startSize, endSize);
                    break;
            }

            return endSize;
        }

        public void Tween(Vector2 fromSize, Vector2 toSize, Vector2 tweenSize, Vector2 endSize, ScaleType scaleType = ScaleType.Proportional)
        {
            // Debug.Log($"{gameObject.name} Tween FromSize {fromSize.ToPrecisionString()} ToSize {toSize.ToPrecisionString()} TweenSize : {tweenSize.ToPrecisionString()} ToSize : {endSize.ToPrecisionString()} ScaleType : {scaleType}");

            if (scaleType != ScaleType.Proportional)
            {
                endSize = Scale(tweenSize, endSize, scaleType);
            }

            DoTween(fromSize, toSize, tweenSize, endSize);
        }

        private void DoTween(Vector2 fromSize, Vector2 toSize, Vector2 tweenSize, Vector2 endSize)
        {
            // Debug.Log($"{gameObject.name} DoTween FromSize {fromSize.ToPrecisionString()} ToSize {toSize.ToPrecisionString()} TweenSize : {tweenSize.ToPrecisionString()} ToSize : {endSize.ToPrecisionString()}");

            rectTransformFX.StopAsync();

            rectTransformFX.StartAsync(new RectTransformFX.Config
            {
                fromSize = fromSize,
                toSize = toSize,
                startSize = tweenSize,
                endSize = endSize
            });
        }
    }
}