using UnityEngine;

using FX.UI;

namespace Tests
{
     [AddComponentMenu("Tests/Scale UI FX Component Test")]
    public class ScaleUIFXComponentTest : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] ScaleUIFXComponent component;

        // Start is called before the first frame update
        void Start()
        {
#if false
            Vector2 size = component.CalculateSize(new ScaleUIFXComponent.Config
            {
                flow = ScaleUIFXComponent.Flow.ToEnd
            });

            Debug.Log($"Size : {size}");
#endif

#if false
            string workings = component.ScaleUIFX.ShowWorkings(new ScaleUIFX.Config
            {
                size = Vector2.one * 1.1f,
                speed = 0.25f
            });

            Debug.Log($"Workings [x1.1] [0.25s]: {workings}");

            workings = component.ScaleUIFX.ShowWorkings(new ScaleUIFX.Config
            {
                size = Vector2.one * 1.1f,
                speed = 0.5f
            });

            Debug.Log($"Workings [x1.1] [0.5s]: {workings}");

            workings = component.ScaleUIFX.ShowWorkings(new ScaleUIFX.Config
            {
                size = Vector2.one * 1.5f,
                speed = 0.25f
            });

            Debug.Log($"Workings [x1.5] [0.25s]: {workings}");

            workings = component.ScaleUIFX.ShowWorkings(new ScaleUIFX.Config
            {
                size = Vector2.one * 1.5f,
                speed = 0.5f
            });

            Debug.Log($"Workings [x1.5] [0.5s]: {workings}");
#endif
        }
    }
}