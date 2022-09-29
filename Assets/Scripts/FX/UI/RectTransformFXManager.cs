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
        public Vector2 OriginalScale { get { return rectTransformFX.OriginalScale; } }

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => rectTransformFX = GetComponent<RectTransformFX>() as RectTransformFX;

        private Vector3 ScaleRelativeToX(Vector3 fromScale, Vector3 toScale)
        {
            float scaleFactor = toScale.x / fromScale.x;
            float scaleX = toScale.x;
            float yToXRatio = fromScale.y / fromScale.x;
            float scaleY = fromScale.y + ((toScale.x - fromScale.x) / yToXRatio);
            toScale = new Vector3(scaleX, scaleY, fromScale.z);
            
            return toScale;
        }

        private Vector3 ScaleRelativeToY(Vector3 fromScale, Vector3 toScale)
        {
            float scaleFactor = toScale.y / fromScale.y;
            float scaleY = toScale.y;
            float xToYRatio = fromScale.x / fromScale.y;
            float scaleX = fromScale.x + ((toScale.y - fromScale.y) / xToYRatio);
            toScale = new Vector3(scaleX, scaleY, fromScale.z);

            return toScale;
        }

        public Vector3 Scale(Vector3 fromScale, Vector3 toScale, ScaleType scaleType)
        {
            switch (scaleType)
            {
                case ScaleType.RelativeToX:
                    toScale = ScaleRelativeToX(fromScale, toScale);
                    break;

                case ScaleType.RelativeToY:
                    toScale = ScaleRelativeToY(fromScale, toScale);
                    break;
            }

            return toScale;
        }

        public void ScaleTween(Vector3 fromScale, Vector3 tweenScale, Vector3 toScale, ScaleType scaleType = ScaleType.Proportional)
        {
            if (scaleType != ScaleType.Proportional)
            {
                toScale = Scale(fromScale, toScale, scaleType);
            }

            ScaleTween(fromScale, tweenScale, toScale);
        }

        private void ScaleTween(Vector3 fromScale, Vector3 tweenScale, Vector3 toScale)
        {
            rectTransformFX.StopAsync();

            rectTransformFX.StartAsync(new RectTransformFX.Config
            {
                fromScale = fromScale,
                startScale = tweenScale,
                toScale = toScale,
                endScale = toScale
            });
        }
    }
}