using UnityEngine;

namespace FX
{
    [RequireComponent(typeof(ScaleFX))]
    public abstract class ScaleFXBaseManager : MonoBehaviour
    {
        public enum ScaleType
        {
            Proportional,
            RelativeToX,
            RelativeToY,
            RelativeToZ
        }

        private ScaleFX scaleFX;

        public virtual void Awake() => ResolveDependencies();

        private void ResolveDependencies() => scaleFX = GetComponent<ScaleFX>() as ScaleFX;

        public abstract Vector3 Scale(Vector3 fromScale, Vector3 toScale, ScaleType scaleType);

        public void ScaleTween(/*Vector3 fromScale, */Vector3 tweenScale, Vector3 toScale, ScaleType scaleType = ScaleType.Proportional)
        {
            // Debug.Log($"{gameObject.name} ScaleTween [1] FromScale : {fromScale.ToPrecisionString()} TweenScale : {tweenScale.ToPrecisionString()} ToScale : {toScale.ToPrecisionString()} ScaleType : {scaleType}");
            Debug.Log($"{gameObject.name} ScaleTween [1] TweenScale : {tweenScale.ToPrecisionString()} ToScale : {toScale.ToPrecisionString()} ScaleType : {scaleType}");

            if (scaleType != ScaleType.Proportional)
            {
                toScale = Scale(/*fromScale*/tweenScale, toScale, scaleType);
            }

            ScaleTween(/*fromScale, */tweenScale, toScale);
        }

        private void ScaleTween(/*Vector3 fromScale, */Vector3 tweenScale, Vector3 toScale)
        {
            // Debug.Log($"{gameObject.name} ScaleTween [2] FromScale : {fromScale.ToPrecisionString()} TweenScale : {tweenScale.ToPrecisionString()} ToScale : {toScale.ToPrecisionString()}");
            Debug.Log($"{gameObject.name} ScaleTween [2] TweenScale : {tweenScale.ToPrecisionString()} ToScale : {toScale.ToPrecisionString()}");

            scaleFX.StopAsync();

            scaleFX.StartAsync(new ScaleFX.Config
            {
                // fromScale = fromScale,
                startScale = tweenScale,
                toScale = toScale,
                // endScale = toScale
            });
        }
    }
}