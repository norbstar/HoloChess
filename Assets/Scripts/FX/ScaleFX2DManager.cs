using UnityEngine;

namespace FX
{
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

        private Vector3 ScaleRelativeToX(Vector3 fromScale, Vector3 toScale, bool hasRectTransform)
        {
            Debug.Log($"{gameObject.name} ScaleRelativeToX FromScale : {fromScale.ToPrecisionString()} ToScale : {toScale.ToPrecisionString()} HasRectTransform : {hasRectTransform}");

            float scaleFactor = toScale.x / fromScale.x;
            Debug.Log($"{gameObject.name} ScaleRelativeToX ScaleFactor : {scaleFactor}");

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

            Debug.Log($"{gameObject.name} ScaleRelativeToX YToXRatio : {yToXRatio}");

            float scaleY = fromScale.y + ((toScale.x - fromScale.x) / yToXRatio);
            toScale = new Vector3(scaleX, scaleY, fromScale.z);

            Debug.Log($"{gameObject.name} ScaleRelativeToX ToScale : {toScale.ToPrecisionString()}");
            
            return toScale;
        }

        private Vector3 ScaleRelativeToY(Vector3 fromScale, Vector3 toScale, bool hasRectTransform)
        {
            Debug.Log($"{gameObject.name} ScaleRelativeToY FromScale : {fromScale.ToPrecisionString()} ToScale : {toScale.ToPrecisionString()} HasRectTransform : {hasRectTransform}");

            float scaleFactor = toScale.y / fromScale.y;
            Debug.Log($"{gameObject.name} ScaleRelativeToY ScaleFactor : {scaleFactor}");

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

            Debug.Log($"{gameObject.name} ScaleRelativeToY XToYRatio : {xToYRatio}");
                
            float scaleX = fromScale.x + ((toScale.y - fromScale.y) / xToYRatio);
            toScale = new Vector3(scaleX, scaleY, fromScale.z);

            Debug.Log($"{gameObject.name} ScaleRelativeToY ToScale : {toScale.ToPrecisionString()}");

            return toScale;
        }

        public override Vector3 Scale(Vector3 fromScale, Vector3 toScale, ScaleType scaleType)
        {
            Debug.Log($"{gameObject.name} Scale FromScale : {fromScale.ToPrecisionString()} ToScale : {toScale.ToPrecisionString()} ScaleType : {scaleType}");

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
    }
}