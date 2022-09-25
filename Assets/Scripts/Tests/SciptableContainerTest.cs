using System.Text;

using UnityEngine;

using Scriptables;

namespace Tests
{
    public class SciptableContainerTest : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] string hexString;
        [SerializeField] string unicodeString;
        [SerializeField] string value;
        // [SerializeField] BaseScriptable baseScriptable;
        // [SerializeField] ExtendedScriptable extendedScriptable;
        // [SerializeField] TypedKeyboardProfile<Binding> profile;

        void Awake()
        {
            Debug.Log($"Before : {hexString}");
            var character = System.Convert.ToInt32(hexString, 16);
            hexString = System.Char.ConvertFromUtf32(character);
            Debug.Log($"After : {hexString}");

            Debug.Log($"Before : {System.Text.RegularExpressions.Regex.Unescape(unicodeString)}");
            unicodeString = "Maths use \u03a0 (Pi) for calculations \u0041";
            Debug.Log($"After : {unicodeString}");

            Debug.Log($"Before : {value}");
            value = @"\u03a0";
            Debug.Log($"After : {value}");

            // You can convert a string into a byte array
            byte[] asciiBytes = Encoding.ASCII.GetBytes(unicodeString);

            // You can convert a byte array into a char array
            char[] asciiChars = Encoding.ASCII.GetChars(asciiBytes);
            string asciiString = new string(asciiChars);

            // The resulting string is different due to the unsupported character for ASCII encoding
            Debug.Log($"Unicode string: {unicodeString}");
            Debug.Log($"ASCII string: {asciiString}");
        }
    }
}