using UnityEngine;

namespace FX
{
    [AddComponentMenu("FX/Scale FX 2D Manager")]
    [RequireComponent(typeof(ScaleFX))]
    public class ScaleFX2DManager : ScaleFXBaseManager
    {
        private RectTransform rectTransform;

        public override void Awake()
        {
            base.Awake();
            ResolveDependencies();
        }

        private void ResolveDependencies() => rectTransform = GetComponent<RectTransform>() as RectTransform;

        private Vector3 ScaleRelativeToX(Vector3 startScale, Vector3 endScale, bool hasRectTransform)
        {
            // Debug.Log($"{gameObject.name} ScaleRelativeToX StartScale : {startScale.ToPrecisionString()} EndScale : {endScale.ToPrecisionString()} HasRectTransform : {hasRectTransform}");

            float scaleFactor = endScale.x / startScale.x;
            // Debug.Log($"{gameObject.name} ScaleRelativeToX ScaleFactor : {scaleFactor}");

            float scaleX = endScale.x;
            float scaleY;
            float yToXRatio;

            if (hasRectTransform)
            {
                yToXRatio = (rectTransform.rect.height * startScale.y) / (rectTransform.rect.width * startScale.x);
                scaleY = startScale.y + (((endScale.x - startScale.x) / yToXRatio) / yToXRatio);
            }
            else
            {
                yToXRatio = startScale.y / startScale.x;
                scaleY = startScale.y + ((endScale.x - startScale.x) / yToXRatio);
            }

            // Debug.Log($"{gameObject.name} ScaleRelativeToX YToXRatio : {yToXRatio}");

            endScale = new Vector3(scaleX, scaleY, startScale.z);

            // Debug.Log($"{gameObject.name} ScaleRelativeToX EndScale : {endScale.ToPrecisionString()}");
            
            return endScale;
        }

        private Vector3 ScaleRelativeToY(Vector3 startScale, Vector3 endScale, bool hasRectTransform)
        {
            // Debug.Log($"{gameObject.name} ScaleRelativeToY StartScale : {startScale.ToPrecisionString()} EndScale : {endScale.ToPrecisionString()} HasRectTransform : {hasRectTransform}");

            float scaleFactor = endScale.y / startScale.y;
            // Debug.Log($"{gameObject.name} ScaleRelativeToY ScaleFactor : {scaleFactor}");

            float scaleX;
            float scaleY = endScale.y;
            float xToYRatio;

            if (hasRectTransform)
            {
                xToYRatio = (rectTransform.rect.width * startScale.x) / (rectTransform.rect.height * startScale.y);
                scaleX = startScale.x + (((endScale.y - startScale.y) / xToYRatio) / xToYRatio);
            }
            else
            {
                xToYRatio = startScale.x / startScale.y;
                scaleX = startScale.x + ((endScale.y - startScale.y) / xToYRatio);
            }

            // Debug.Log($"{gameObject.name} ScaleRelativeToY XToYRatio : {xToYRatio}");
                
            endScale = new Vector3(scaleX, scaleY, startScale.z);

            // Debug.Log($"{gameObject.name} ScaleRelativeToY EndScale : {endScale.ToPrecisionString()}");

            return endScale;
        }

        public override Vector3 Scale(Vector3 startScale, Vector3 endScale, ScaleType scaleType)
        {
            // Debug.Log($"{gameObject.name} Scale StartScale : {startScale.ToPrecisionString()} EndScale : {endScale.ToPrecisionString()} ScaleType : {scaleType}");

            bool hasRectTransform = (rectTransform != null);

            switch (scaleType)
            {
                case ScaleType.RelativeToX:
                    endScale = ScaleRelativeToX(startScale, endScale, hasRectTransform);
                    break;

                case ScaleType.RelativeToY:
                    endScale = ScaleRelativeToY(startScale, endScale, hasRectTransform);
                    break;
            }

            return endScale;
        }
    }
}