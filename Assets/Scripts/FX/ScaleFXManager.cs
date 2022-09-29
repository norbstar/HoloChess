using UnityEngine;

namespace FX
{
    [RequireComponent(typeof(ScaleFX))]
    public class ScaleFXManager : ScaleFXBaseManager
    {
        private Vector3 ScaleRelativeToX(Vector3 fromScale, Vector3 toScale)
        {
            float scaleFactor = toScale.x / fromScale.x;
            float scaleX = toScale.x;
            float yToXRatio = fromScale.y / fromScale.x;
            float scaleY = fromScale.y + ((toScale.x - fromScale.x) / yToXRatio);
            float zToXRatio = fromScale.z / fromScale.x;
            float scaleZ = fromScale.z + ((toScale.x - fromScale.x) / zToXRatio);
            toScale = new Vector3(scaleX, scaleY, scaleZ);
            
            return toScale;
        }

        private Vector3 ScaleRelativeToY(Vector3 fromScale, Vector3 toScale)
        {
            float scaleFactor = toScale.y / fromScale.y;
            float scaleY = toScale.y;
            float xToYRatio = fromScale.x / fromScale.y;
            float scaleX = fromScale.x + ((toScale.y - fromScale.y) / xToYRatio);
            float zToYRatio = fromScale.z / fromScale.y;
            float scaleZ = fromScale.z + ((toScale.y - fromScale.y) / zToYRatio);
            toScale = new Vector3(scaleX, scaleY, scaleZ);

            return toScale;
        }

        private Vector3 ScaleRelativeToZ(Vector3 fromScale, Vector3 toScale)
        {
            float scaleFactor = toScale.z / fromScale.z;
            float scaleZ = toScale.z;
            float xToZRatio = fromScale.x / fromScale.z;
            float scaleX = fromScale.x + ((toScale.z - fromScale.z) / xToZRatio);
            float yToZRatio = fromScale.y / fromScale.z;
            float scaleY = fromScale.y + ((toScale.z - fromScale.z) / yToZRatio);
            toScale = new Vector3(scaleX, scaleY, scaleZ);

            return toScale;
        }

        public override Vector3 Scale(Vector3 fromScale, Vector3 toScale, ScaleType scaleType)
        {
            switch (scaleType)
            {
                case ScaleType.RelativeToX:
                    toScale = ScaleRelativeToX(fromScale, toScale);
                    break;

                case ScaleType.RelativeToY:
                    toScale = ScaleRelativeToY(fromScale, toScale);
                    break;

                case ScaleType.RelativeToZ:
                    toScale = ScaleRelativeToZ(fromScale, toScale);
                    break;
            }

            return toScale;
        }
    }
}