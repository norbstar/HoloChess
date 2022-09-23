using UnityEngine;

namespace Tests
{
    public class StupidityTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // float a = 100 * 0.94f;
            // Debug.Log($"a : {a}");

            // int b = (int) a;
            // Debug.Log($"b : {b}");

            // float c = (float) 100 * 0.94f;
            // Debug.Log($"c : {c}");

            // int d = (int) (c);
            // Debug.Log($"d : {d}");

            // int e = (int) ((float) 100 * 0.94f);
            // Debug.Log($"e : {e}");

            // float value = 0.94f;
            // Debug.Log($"{value}");

            // float f = (float) 100 * value;
            // Debug.Log($"f : {f}");

            // int g = (int) ((float) 100 * value);
            // Debug.Log($"g : {g}");

            // int a = (int) ((float) 100 * 0.94f);
            // Debug.Log($"a : {a}");

            // Debug.Log($"1 : {100f * 0.94f}");

            // int b = (int) (100f * 0.94f);
            // Debug.Log($"b : {b}");

            float value = 0.94f;
            Debug.Log($"Value : {value}");

            // int c = (int) ((float) 100 * value);
            // Debug.Log($"c : {c}");

            // Debug.Log($"2 : {100f * value}");

            // int d = (int) (100f * value);
            // Debug.Log($"d : {d}");

            int a = (int) (100 * 0.94f);
            Debug.Log($"a : {a}");

            int b = (int) (100 * value);
            Debug.Log($"b : {b}");

            int c = System.Convert.ToInt32(100 * value);
            Debug.Log($"c : {c}");
        }
    }
}