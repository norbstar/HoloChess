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

        private ScaleFX scaleFX;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => scaleFX = GetComponent<ScaleFX>() as ScaleFX;

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

        public void ScaleTween(Vector3 fromScale, Vector3 toScale)
        {
            // Debug.Log($"ScaleTween From : {fromScale.ToPrecisionString()} To : {toScale.ToPrecisionString()}");

            scaleFX.StopAsync();

            scaleFX.StartAsync(new ScaleFX.Config
            {
                fromScale = fromScale,
                toScale = toScale
            });
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
    }
}