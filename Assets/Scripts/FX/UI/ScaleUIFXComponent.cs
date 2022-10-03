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

        public void Scale(Config config)
        {
            if (config == null) return;

            Debug.Log($"{gameObject.name} Scale Type : {type} Speed : {speed} Flow {config.flow} Flags : {config.flags}");
            
            Vector2 size = CalculateSize(config);
            Vector2 scale = CalculateScale(config);
            DoScale(type, speed, size, scale, config.flags);
        }

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

        public Vector2 CalculateScale(Config config)
        {
            Vector2 originalScale = scaleUIFX.OriginalScale;
            Vector2 scale = originalScale;

            bool scaleContent = (config.flags.HasFlag(Flags.ScaleContent));

            if (scaleContent)
            {
                // TODO
            }

            return scale;
        }

        private void DoScale(ScaleType type, float speed, Vector2 size, Vector2 scale, Flags flags)
        {
            Debug.Log($"{gameObject.name} DoScale Type : {type} Speed : {speed} Size : {size.ToPrecisionString()} Scale : {scale.ToPrecisionString()} Flags : {flags}");

            scaleUIFX.StopAsync();

            scaleUIFX.StartAsync(new ScaleUIFX.Config
            {
                size = size,
                scale = scale,
                speed = speed,
                flags = flags
            });
        }
    }
}