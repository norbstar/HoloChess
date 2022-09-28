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
            float scaleFactor, scaleX, scaleY;

            scaleFactor = toScale.x / fromScale.x;
            scaleX = toScale.x;
            float yToXRatio;

            if (hasRectTransform)
            {
                yToXRatio = (rectTransform.rect.height * fromScale.y) / (rectTransform.rect.width * fromScale.x);
            }
            else
            {
                yToXRatio = fromScale.y / fromScale.x;
            }

            scaleY = fromScale.y + ((toScale.x - fromScale.x) / yToXRatio);
            toScale = new Vector3(scaleX, scaleY, fromScale.z);
            Debug.Log($"ScaleRelativeToX From Scale : {fromScale.ToPrecisionString()} Scale Factor : {scaleFactor} Scale X : {scaleX} Scale Y : {scaleY} To Scale : {toScale.ToPrecisionString()}");
            
            return toScale;
        }

        private Vector3 ScaleRelativeToY(Vector3 fromScale, Vector3 toScale, bool hasRectTransform)
        {
            float scaleFactor, scaleX, scaleY;

            scaleFactor = toScale.y / fromScale.y;
            scaleY = toScale.y;
            float xToYRatio;

            if (hasRectTransform)
            {
                Debug.Log($"ScaleRelativeToY Width : {rectTransform.rect.width} FromScaleX : {fromScale.x} Height : {rectTransform.rect.height} FromScaleY : {fromScale.y}");
                xToYRatio = (rectTransform.rect.width * fromScale.x) / (rectTransform.rect.height * fromScale.y);
                Debug.Log($"ScaleRelativeToY XToYRatio : {xToYRatio}");
            }
            else
            {
                xToYRatio = fromScale.x / fromScale.y;
                Debug.Log($"ScaleRelativeToY XToYRatio [2] : {xToYRatio}");
            }
                
            scaleX = fromScale.x + ((toScale.y - fromScale.y) / xToYRatio);
            toScale = new Vector3(scaleX, scaleY, fromScale.z);
            Debug.Log($"ScaleRelativeToY From Scale : {fromScale.ToPrecisionString()} Scale Factor : {scaleFactor} Scale X : {scaleX} Scale Y : {scaleY} To Scale : {toScale.ToPrecisionString()}");

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

#if false
        public Vector3 Scale(Vector3 fromScale, Vector3 toScale, ScaleType scaleType)
        {
            float scaleFactor, scaleX, scaleY;
            bool hasSize = false;

            if (rectTransform != null)
            {
                hasSize = true;
            }

            switch (scaleType)
            {
                case ScaleType.RelativeToX:
                    scaleFactor = toScale.x / fromScale.x;
                    scaleX = fromScale.x * scaleFactor;
                    float yToXRatio;

                    if (hasSize)
                    {
                        // Accommodate the size as part of the calculation in addition to the scale (e.g. for a UI element)
                        yToXRatio = rectTransform.rect.height * fromScale.y / rectTransform.rect.width * fromScale.x;
                        scaleY = fromScale.y + (scaleFactor * yToXRatio);
                    }
                    else
                    {
                        // Use the scale alone (e.g. for a Sprite)
                        yToXRatio = fromScale.y / fromScale.x;
                        scaleY = fromScale.y + (scaleFactor * yToXRatio);
                    }
                    
                    toScale = new Vector3(scaleX, scaleY, fromScale.z);
                    break;

                case ScaleType.RelativeToY:
                    scaleFactor = toScale.y / fromScale.y;
                    scaleY = fromScale.y * scaleFactor;
                    float xToYRatio;

                    if (hasSize)
                    {
                        // Accommodate the size as part of the calculation in addition to the scale (e.g. for a UI element)
                        xToYRatio = rectTransform.rect.width * fromScale.x / rectTransform.rect.height * fromScale.y;
                        scaleX = fromScale.x + (scaleFactor * xToYRatio);
                    }
                    else
                    {
                        // Use the scale alone (e.g. for a Sprite)
                        xToYRatio = fromScale.x / fromScale.y;
                        scaleX = fromScale.x + (scaleFactor * xToYRatio);
                    }
                    
                    toScale = new Vector3(scaleX, scaleY, fromScale.z);
                    break;
            }

            Debug.Log($"Scale From Scale : {fromScale.ToPrecisionString()} Scale Factor : {scaleFactor} Scale X : {scaleX} Scale Y : {scaleY} To Scale : {toScale.ToPrecisionString()}");

            return toScale;
        }
#endif

        public void ScaleTween(Vector3 fromScale, Vector3 toScale, ScaleType scaleType)
        {
            Debug.Log($"ScaleTween From : {fromScale.ToPrecisionString()} To : {toScale.ToPrecisionString()} Scale Type : {scaleType}");

#if false
            switch (scaleType)
            {
                case ScaleType.Proportional:
                    ScaleTween(fromScale, toScale);
                    break;

                case ScaleType.RelativeToX:
                case ScaleType.RelativeToY:
                    toScale = Scale(fromScale, toScale, scaleType);
                    ScaleTween(fromScale, toScale);
                    break;
            }
#endif

            if (scaleType != ScaleType.Proportional)
            {
                toScale = Scale(fromScale, toScale, scaleType);
            }

            ScaleTween(fromScale, toScale);
        }

        public void ScaleTween(Vector3 fromScale, Vector3 toScale)
        {
            Debug.Log($"ScaleTween From : {fromScale.ToPrecisionString()} To : {toScale.ToPrecisionString()}");

            scaleFX.StopAsync();

            scaleFX.StartAsync(new ScaleFX.Config
            {
                fromScale = fromScale,
                startScale = transform.localScale,
                toScale = toScale,
                endScale = toScale
            });
        }

#if false
        public Vector3 Scale(Vector3 fromScale, ScaleType scaleType, float scaleFactor)
        {
            Vector3 toScale = fromScale;

            if (scaleType == ScaleType.Proportional)
            {
                toScale = new Vector3(fromScale.x * scaleFactor, fromScale.y * scaleFactor, fromScale.z);
            }
            else
            {
                float scaleX, scaleY;

                switch (scaleType)
                {
                    case ScaleType.RelativeToX:
                        scaleX = fromScale.x * scaleFactor;
                        scaleY = fromScale.y + (scaleX - fromScale.x);
                        toScale = new Vector3(scaleX, scaleY, fromScale.z);
                        break;

                    case ScaleType.RelativeToY:
                        scaleY = fromScale.y * scaleFactor;
                        scaleX = fromScale.x + (scaleY - fromScale.y);
                        toScale = new Vector3(scaleX, scaleY, fromScale.z);
                        break;
                }
            }

            return toScale;
        }

        public void ScaleCustom(Vector3 fromScale, ScaleType scaleType, float scaleFactor)
        {
            // Debug.Log($"ScaleCustom From : {fromScale.ToPrecisionString()} Scale Type : {scaleType} Scale Factor : {scaleFactor}");

            Vector3 toScale = Scale(fromScale, scaleType, scaleFactor);
            ScaleTween(fromScale, toScale);
        }

        public float CalculateInverseScaleFactor(Vector3 localScale, Vector3 originalScale, ScaleType scaleType)
        {
            // Debug.Log($"CalculateInverseScaleFactor Local Scale : {localScale.ToPrecisionString()} Original Scale : {originalScale.ToPrecisionString()} Scale Type : {scaleType}");

            // Default to proportional scaling
            float scaleFactor = ((localScale.x - originalScale.x) / localScale.x) * 10f;

            switch (scaleType)
            {
                case ScaleType.RelativeToX:
                    scaleFactor = ((localScale.x - originalScale.x) / localScale.x) * 10f;
                    break;
                
                case ScaleType.RelativeToY:
                    scaleFactor = ((localScale.y - originalScale.y) / localScale.y) * 10f;
                    break;
            }

            return scaleFactor;
        }
#endif
    }
}