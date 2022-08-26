using UnityEngine;

namespace FX
{
    [RequireComponent(typeof(ScaleFX))]
    public class ScaleFXManager : MonoBehaviour
    {
        private FX.ScaleFX scaleFX;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => scaleFX = GetComponent<FX.ScaleFX>() as FX.ScaleFX;

        public void ScaleUp(float fromScale, float toScale)
        {
            scaleFX.StopAsync();

            scaleFX.StartAsync(new FX.ScaleFX.Config
            {
                fromScale = fromScale,
                toScale = toScale
            });
        }

        public void ScaleDown(float fromScale, float toScale)
        {
            scaleFX.StopAsync();

            scaleFX.StartAsync(new FX.ScaleFX.Config
            {
                fromScale = fromScale,
                toScale = toScale
            });
        }
    }
}