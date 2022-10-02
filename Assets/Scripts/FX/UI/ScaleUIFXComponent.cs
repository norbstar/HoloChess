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
            public ScaleType? type;
            public float? timeline;
            public Flow flow;
            public Flags flags;
        }

        [field : SerializeField] public ScaleType Type { get; private set; } = ScaleType.Proportional;
        [field : SerializeField] public float Timeline { get; private set; } = 1f;
        [field : SerializeField] public float ScaleFactor { get; private set; } = 1.1f;       
        [field : SerializeField] public Vector2 CustomScaling { get; private set; } = Vector2.one * 1.1f;

        private ScaleUIFX scaleUIFX;
        public ScaleUIFX ScaleUIFX { get { return scaleUIFX; } }

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => scaleUIFX = GetComponent<ScaleUIFX>() as ScaleUIFX;

        public bool Scale(Config config)
        {
            if (config == null) return false;

            Debug.Log($"{gameObject.name} DoScale Type : {config.type} Timeline : {config.timeline} Flow {config.flow} Flags : {config.flags}");

            var type = (config.type.HasValue) ? config.type.Value : Type;
            var timeline = (config.timeline.HasValue) ? config.timeline.Value : Timeline;
            Vector2 size = scaleUIFX.TweenSize;
            float xDelta, xSize, yDelta, ySize;
            Vector2? endSize = null;

            if (config.flow == Flow.ToEnd)
            {
                switch (type)
                {
                    case ScaleType.Proportional:
                        endSize = size * ScaleFactor;
                        break;

                    case ScaleType.XConstrained:
                        xDelta = (size.x * ScaleFactor) - size.x;
                        ySize = Mathf.Clamp(size.y * ScaleFactor, size.y, size.y + xDelta);
                        endSize = new Vector2(size.x * ScaleFactor, ySize);
                        break;

                    case ScaleType.XEqual:
                        xDelta = (size.x * ScaleFactor) - size.x;
                        ySize = size.y + xDelta;
                        endSize = new Vector2(size.x * ScaleFactor, ySize);
                        break;

                    case ScaleType.YConstrained:
                        yDelta = (size.y * ScaleFactor) - size.y;
                        xSize = Mathf.Clamp(size.x * ScaleFactor, size.x, size.x + yDelta);
                        endSize = new Vector2(xSize, size.y * ScaleFactor);
                        break;

                    case ScaleType.YEqual:
                        yDelta = (size.y * ScaleFactor) - size.y;
                        xSize = size.x + yDelta;
                        endSize = new Vector2(xSize, size.y * ScaleFactor);
                        break;

                    case ScaleType.Custom:
                        endSize = new Vector2(size.x * CustomScaling.x, size.y * CustomScaling.y);
                        break;
                }
            }
            else
            {
                endSize = size;
            }

            bool success = false;
            
            if (endSize.HasValue)
            {
                DoScale(type, timeline, size, size * ScaleFactor, scaleUIFX.TweenSize, endSize.Value, config.flags);
                success = true;
            }

            return success;
        }

        private void DoScale(ScaleType type, float timeline, Vector2 fromSize, Vector2 toSize, Vector2 tweenSize, Vector2 endSize, Flags flags)
        {
            Debug.Log($"{gameObject.name} DoScale Type : {type} Timeline : {timeline} FromSize {fromSize.ToPrecisionString()} ToSize {toSize.ToPrecisionString()} TweenSize : {tweenSize.ToPrecisionString()} ToSize : {endSize.ToPrecisionString()} Flags : {flags}");

            scaleUIFX.StopAsync();

            bool scaleContent = (flags.HasFlag(Flags.ScaleContent));

            scaleUIFX.StartAsync(new ScaleUIFX.Config
            {
                timeline = timeline,
                fromSize = fromSize,
                toSize = toSize,
                startSize = tweenSize,
                endSize = endSize,
                scaleContent = scaleContent
            });
        }
    }
}