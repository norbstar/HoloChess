using UnityEngine;

namespace FX
{
    [AddComponentMenu("FX/Scale FX Manager>")]
    [RequireComponent(typeof(ScaleFX))]
    public class ScaleFXManager : ScaleFXBaseManager
    {
        private Vector3 ScaleRelativeToX(Vector3 startScale, Vector3 endScale)
        {
            Debug.Log($"{gameObject.name} ScaleRelativeToX StartScale : {startScale.ToPrecisionString()} EndScale : {endScale.ToPrecisionString()}");

            float scaleFactor = endScale.x / startScale.x;
            Debug.Log($"{gameObject.name} ScaleRelativeToX ScaleFactor : {scaleFactor}");

            float scaleX = endScale.x;
            float yToXRatio = startScale.y / startScale.x;
            float scaleY = startScale.y + ((endScale.x - startScale.x) / yToXRatio);
            float zToXRatio = startScale.z / startScale.x;
            float scaleZ = startScale.z + ((endScale.x - startScale.x) / zToXRatio);
            Debug.Log($"{gameObject.name} ScaleRelativeToX YToXRatio : {yToXRatio} ZToXRatio : {zToXRatio}");

            endScale = new Vector3(scaleX, scaleY, scaleZ);
            Debug.Log($"{gameObject.name} ScaleRelativeToX EndScale : {endScale.ToPrecisionString()}");

            return endScale;
        }

        private Vector3 ScaleRelativeToY(Vector3 startScale, Vector3 endScale)
        {
            Debug.Log($"{gameObject.name} ScaleRelativeToY StartScale : {startScale.ToPrecisionString()} EndScale : {endScale.ToPrecisionString()}");

            float scaleFactor = endScale.y / startScale.y;
             Debug.Log($"{gameObject.name} ScaleRelativeToY ScaleFactor : {scaleFactor}");

            float scaleY = endScale.y;
            float xToYRatio = startScale.x / startScale.y;
            float scaleX = startScale.x + ((endScale.y - startScale.y) / xToYRatio);
            float zToYRatio = startScale.z / startScale.y;
            float scaleZ = startScale.z + ((endScale.y - startScale.y) / zToYRatio);
            Debug.Log($"{gameObject.name} ScaleRelativeToY XToYRatio : {xToYRatio} ZToYRatio : {zToYRatio}");

            endScale = new Vector3(scaleX, scaleY, scaleZ);
            Debug.Log($"{gameObject.name} ScaleRelativeToY EndScale : {endScale.ToPrecisionString()}");

            return endScale;
        }

        private Vector3 ScaleRelativeToZ(Vector3 startScale, Vector3 endScale)
        {
            Debug.Log($"{gameObject.name} ScaleRelativeToZ StartScale : {startScale.ToPrecisionString()} EndScale : {endScale.ToPrecisionString()}");

            float scaleFactor = endScale.z / startScale.z;
            Debug.Log($"{gameObject.name} ScaleRelativeToX ScaleFactor : {scaleFactor}");

            float scaleZ = endScale.z;
            float xToZRatio = startScale.x / startScale.z;
            float scaleX = startScale.x + ((endScale.z - startScale.z) / xToZRatio);
            float yToZRatio = startScale.y / startScale.z;
            float scaleY = startScale.y + ((endScale.z - startScale.z) / yToZRatio);
            Debug.Log($"{gameObject.name} ScaleRelativeToY XToZRatio : {xToZRatio} YToZRatio : {yToZRatio}");

            endScale = new Vector3(scaleX, scaleY, scaleZ);
            Debug.Log($"{gameObject.name} ScaleRelativeToZ EndScale : {endScale.ToPrecisionString()}");

            return endScale;
        }

        public override Vector3 Scale(Vector3 startScale, Vector3 endScale, ScaleType scaleType)
        {
            Debug.Log($"{gameObject.name} Scale StartScale : {startScale.ToPrecisionString()} EndScale : {endScale.ToPrecisionString()} ScaleType : {scaleType}");

            switch (scaleType)
            {
                case ScaleType.RelativeToX:
                    endScale = ScaleRelativeToX(startScale, endScale);
                    break;

                case ScaleType.RelativeToY:
                    endScale = ScaleRelativeToY(startScale, endScale);
                    break;

                case ScaleType.RelativeToZ:
                    endScale = ScaleRelativeToZ(startScale, endScale);
                    break;
            }

            return endScale;
        }
    }
}