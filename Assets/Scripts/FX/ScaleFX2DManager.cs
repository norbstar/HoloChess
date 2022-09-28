using UnityEngine;

namespace FX
{
    [RequireComponent(typeof(ScaleFX))]
    public class ScaleFX2DManager : MonoBehaviour
    {
        public enum ScaleType
        {
            Proportional,
            RelativeToX,
            RelativeToY
        }

        private RectTransform rectTransform;
        private ScaleFX scaleFX;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies()
        {
            rectTransform = GetComponent<RectTransform>() as RectTransform;
            scaleFX = GetComponent<ScaleFX>() as ScaleFX;
        }

        private Vector3 ScaleRelativeToX(Vector3 fromScale, Vector3 toScale, bool hasRectTransform)
        {
            float scaleFactor = toScale.x / fromScale.x;
            float scaleX = toScale.x;
            float yToXRatio;

            if (hasRectTransform)
            {
                yToXRatio = (rectTransform.rect.height * fromScale.y) / (rectTransform.rect.width * fromScale.x);
            }
            else
            {
                yToXRatio = fromScale.y / fromScale.x;
            }

            float scaleY = fromScale.y + ((toScale.x - fromScale.x) / yToXRatio);
            toScale = new Vector3(scaleX, scaleY, fromScale.z);
            
            return toScale;
        }

        private Vector3 ScaleRelativeToY(Vector3 fromScale, Vector3 toScale, bool hasRectTransform)
        {
            float scaleFactor = toScale.y / fromScale.y;
            float scaleY = toScale.y;
            float xToYRatio;

            if (hasRectTransform)
            {
                xToYRatio = (rectTransform.rect.width * fromScale.x) / (rectTransform.rect.height * fromScale.y);
            }
            else
            {
                xToYRatio = fromScale.x / fromScale.y;
            }
                
            float scaleX = fromScale.x + ((toScale.y - fromScale.y) / xToYRatio);
            toScale = new Vector3(scaleX, scaleY, fromScale.z);

            return toScale;
        }

        public Vector3 Scale(Vector3 fromScale, Vector3 toScale, ScaleType scaleType)
        {
            bool hasRectTransform = (rectTransform != null);

            switch (scaleType)
            {
                case ScaleType.RelativeToX:
                    toScale = ScaleRelativeToX(fromScale, toScale, hasRectTransform);
                    break;

                case ScaleType.RelativeToY:
                    toScale = ScaleRelativeToY(fromScale, toScale, hasRectTransform);
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
            scaleFX.StopAsync();

            scaleFX.StartAsync(new ScaleFX.Config
            {
                fromScale = fromScale,
                startScale = tweenScale,
                toScale = toScale,
                endScale = toScale
            });
        }
    }
}