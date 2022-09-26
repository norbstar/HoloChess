using UnityEngine;

namespace FX
{
    [RequireComponent(typeof(ScaleFX))]
    public class ScaleFX2DManager : MonoBehaviour
    {
        private ScaleFX scaleFX;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => scaleFX = GetComponent<ScaleFX>() as ScaleFX;

        public void ScaleFromTo(Vector3 fromScale, Vector3 toScale)
        {
            scaleFX.StopAsync();

            scaleFX.StartAsync(new ScaleFX.Config
            {
                fromScale = fromScale,
                toScale = toScale
            });
        }
    }
}