using UnityEngine;

namespace FX
{
    [RequireComponent(typeof(ScaleFX))]
    public class ScaleFXManager : MonoBehaviour
    {
        private FX.ScaleFX scaleFX;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => scaleFX = GetComponent<FX.ScaleFX>() as FX.ScaleFX;

        public void ScaleUp(Vector3 fromScale, Vector3 toScale)
        {
            scaleFX.StopAsync();

            scaleFX.StartAsync(new FX.ScaleFX.Config
            {
                fromScale = fromScale,
                toScale = toScale
            });
        }

        public void ScaleDown(Vector3 fromScale, Vector3 toScale)
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