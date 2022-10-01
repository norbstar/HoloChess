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

        public abstract Vector3 Scale(Vector3 startScale, Vector3 endScale, ScaleType scaleType);

        public void Tween(Vector3 fromScale, Vector3 toScale, Vector3 tweenScale, Vector3 endScale, ScaleType scaleType = ScaleType.Proportional)
        {
            // Debug.Log($"{gameObject.name} Tween FromScale {fromScale.ToPrecisionString()} ToScale {toScale.ToPrecisionString()} TweenScale : {tweenScale.ToPrecisionString()} ToScale : {endScale.ToPrecisionString()} ScaleType : {scaleType}");

            if (scaleType != ScaleType.Proportional)
            {
                endScale = Scale(tweenScale, endScale, scaleType);
            }

            DoTween(fromScale, toScale, tweenScale, endScale);
        }

        private void DoTween(Vector3 fromScale, Vector3 toScale, Vector3 tweenScale, Vector3 endScale)
        {
            // Debug.Log($"{gameObject.name} DoTween FromScale {fromScale.ToPrecisionString()} ToScale {toScale.ToPrecisionString()} TweenScale : {tweenScale.ToPrecisionString()} ToScale : {endScale.ToPrecisionString()}");

            scaleFX.StopAsync();

            scaleFX.StartAsync(new ScaleFX.Config
            {
                fromScale = fromScale,
                toScale = toScale,
                startScale = tweenScale,
                endScale = endScale
            });
        }
    }
}