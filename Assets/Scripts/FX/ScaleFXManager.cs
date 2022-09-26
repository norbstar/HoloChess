using UnityEngine;

namespace FX
{
    [RequireComponent(typeof(ScaleFX))]
    public class ScaleFXManager : MonoBehaviour
    {
        public enum ScaleType
        {
            Proportional,
            RelativeToX,
            RelativeToY,
            RelativeToZ
        }

        private FX.ScaleFX scaleFX;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => scaleFX = GetComponent<FX.ScaleFX>() as FX.ScaleFX;

        public Vector3 Scale(Vector3 fromScale, ScaleType scaleType, float scaleFactor)
        {
            Vector3 toScale = fromScale;

            if (scaleType == ScaleType.Proportional)
            {
                toScale = fromScale * scaleFactor;
            }
            else
            {
                float scaleX, scaleY, scaleZ;

                switch (scaleType)
                {
                    case ScaleType.RelativeToX:
                        scaleX = fromScale.x * scaleFactor;
                        scaleY = fromScale.y + (scaleX - fromScale.x);
                        scaleZ = fromScale.z + (scaleX - fromScale.x);
                        toScale = new Vector3(scaleX, scaleY, scaleZ);
                        break;

                    case ScaleType.RelativeToY:
                        scaleY = fromScale.y * scaleFactor;
                        scaleX = fromScale.x + (scaleY - fromScale.y);
                        scaleZ = fromScale.z + (scaleY - fromScale.y);
                        toScale = new Vector3(scaleX, scaleY, scaleZ);
                        break;

                    case ScaleType.RelativeToZ:
                        scaleZ = fromScale.z * scaleFactor;
                        scaleX = fromScale.x + (scaleZ - fromScale.z);
                        scaleY = fromScale.y + (scaleZ - fromScale.z);
                        toScale = new Vector3(scaleX, scaleY, scaleZ);
                        break;
                }
            }

            return toScale;
        }

        public void ScaleTween(Vector3 fromScale, Vector3 toScale)
        {
            Debug.Log($"ScaleTween From : {fromScale} To : {toScale}");

            scaleFX.StopAsync();

            scaleFX.StartAsync(new FX.ScaleFX.Config
            {
                fromScale = fromScale,
                toScale = toScale
            });
        }

        public void ScaleCustom(Vector3 fromScale, ScaleType scaleType, float scaleFactor)
        {
            Debug.Log($"ScaleCustom From : {fromScale} Scale Type : {scaleType} Scale Factor : {scaleFactor}");

            Vector3 toScale = Scale(fromScale, scaleType, scaleFactor);
            ScaleTween(fromScale, toScale);
        }
    }
}