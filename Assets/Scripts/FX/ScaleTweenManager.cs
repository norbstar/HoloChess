using UnityEngine;

namespace FX
{
    [RequireComponent(typeof(ScaleFX))]
    public abstract class ScaleTweenManager : MonoBehaviour
    {
        public enum ScaleType
        {
            Proportional,
            RelativeToX,
            RelativeToY
        }

        private ScaleFX scaleFX;

        public virtual void Awake() => ResolveDependencies();

        private void ResolveDependencies() => scaleFX = GetComponent<ScaleFX>() as ScaleFX;

        public abstract Vector3 Scale(Vector3 fromScale, Vector3 toScale, ScaleType scaleType);

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