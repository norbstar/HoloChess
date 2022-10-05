using System;

using UnityEngine;

using ScaleType = FX.UI.ScaleUIFX.ScaleType;

namespace FX.UI
{
    [AddComponentMenu("FX/UI/Scale UI FX Component")]
    [RequireComponent(typeof(ScaleUIFX))]
    public class ScaleUIFXComponent : MonoBehaviour
    {
        [Flags]
        public enum Flags
        {
            ScaleContent = 1
        }

        public enum Flow
        {
            ToStart,
            ToEnd
        }

        public class Config
        {
            public Flow flow;
            public Flags flags;
        }

        [SerializeField] ScaleType type = ScaleType.Unified;
        [SerializeField] float speed = 1f;
        [SerializeField] float scaleFactor = 1.1f;       
        [SerializeField] Vector2 scaler = Vector2.one * 1.1f;

        private ScaleUIFX scaleUIFX;
        public ScaleUIFX ScaleUIFX { get { return scaleUIFX; } }

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => scaleUIFX = GetComponent<ScaleUIFX>() as ScaleUIFX;

        public Vector2 CalculateSize(Config config)
        {
            Vector2 originalSize = scaleUIFX.OriginalSize;
            float xDelta, xSize, yDelta, ySize;
            Vector2 size = originalSize;

            if (config.flow == Flow.ToEnd)
            {
                switch (type)
                {
                    case ScaleType.Unified:
                        size = originalSize * scaleFactor;
                        break;

                    case ScaleType.XConstrained:
                        yDelta = (originalSize.y * scaleFactor) - originalSize.y;
                        xSize = Mathf.Clamp(originalSize.x * scaleFactor, originalSize.x, originalSize.x + yDelta);
                        size = new Vector2(xSize, originalSize.y * scaleFactor);
                        break;

                    case ScaleType.XEqual:
                        yDelta = (originalSize.y * scaleFactor) - originalSize.y;
                        xSize = originalSize.x + yDelta;
                        size = new Vector2(xSize, originalSize.y * scaleFactor);
                        break;

                    case ScaleType.YConstrained:
                        xDelta = (originalSize.x * scaleFactor) - originalSize.x;
                        ySize = Mathf.Clamp(originalSize.y * scaleFactor, originalSize.y, originalSize.y + xDelta);
                        size = new Vector2(originalSize.x * scaleFactor, ySize);
                        break;

                    case ScaleType.YEqual:
                        xDelta = (originalSize.x * scaleFactor) - originalSize.x;
                        ySize = originalSize.y + xDelta;
                        size = new Vector2(originalSize.x * scaleFactor, ySize);
                        break;

                    case ScaleType.Custom:
                        size = new Vector2(originalSize.x * scaler.x, originalSize.y * scaler.y);
                        break;
                }
            }

            return size;
        }

        public Vector2 CalculateScale(Vector2 size, Config config)
        {
            Vector2 originalSize = scaleUIFX.OriginalSize;
            
            if ((size.x == originalSize.x) || (size.y == originalSize.y)) return scaleUIFX.OriginalScale;

            float xDelta = Mathf.Abs(size.x - originalSize.x);
            float yDelta = Mathf.Abs(size.y - originalSize.y);
            float scaleFactor = (xDelta < yDelta) ? size.x / originalSize.x : size.y / originalSize.y;
            return Vector2.one * scaleFactor;
        }

        public void Scale(Config config)
        {
            if (config == null) return;

            // Debug.Log($"Scale Type : {type} Speed : {speed} Flow {config.flow} Flags : {config.flags}");
            scaleUIFX.StopAsync();

            Vector2 size = CalculateSize(config);
            Vector2 scale = scaleUIFX.OriginalScale;

            bool scaleContent = (config.flags.HasFlag(Flags.ScaleContent));
            // Debug.Log($"[Pre] Size : {size.ToPrecisionString()} Scale : {scale.ToPrecisionString()} ScaleContent : {scaleContent}");
            
            if (scaleContent)
            {
                scale = CalculateScale(size, config);
                // size = scaleUIFX.OriginalSize;
                size /= scale;
            }

            // Debug.Log($"[Post] Size : {size.ToPrecisionString()} Scale : {scale.ToPrecisionString()}");

            scaleUIFX.StartAsync(new ScaleUIFX.Config
            {
                size = size,
                scale = scale,
                speed = speed,
                flags = config.flags
            });
        }
    }
}