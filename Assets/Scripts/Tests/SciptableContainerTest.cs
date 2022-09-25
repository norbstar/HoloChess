using System.Collections.Generic;
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

        [SerializeField] BaseScriptable baseScriptable;
        [SerializeField] ExtendedScriptable extendedScriptable;
        [SerializeField] ScriptableObject sciprtable;
        [SerializeField] KeyboardProfileScriptable keyboardProfile;

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

            System.Type type = sciprtable.GetType();

            if (type == typeof(BaseScriptable))
            {
                Debug.Log($"Profile is BaseScriptable");
            }
            else if (type == typeof(ExtendedScriptable))
            {
                Debug.Log($"Profile is ExtendedScriptable");
            }

            type = keyboardProfile.GetType();

            if (type == typeof(KeyboardProfile))
            {
                Debug.Log($"Profile2 is KeyboardProfile");

                KeyboardProfile profile = (KeyboardProfile) keyboardProfile;
                List<KeyboardBinding> bindings = profile.GetBindings();

                foreach (KeyboardBinding binding in bindings)
                {
                    Debug.Log($"Id : {binding.id} Character : {binding.character}");
                }
            }
            else if (type == typeof(ExtendedKeyboardProfile))
            {
                Debug.Log($"Profile2 is ExtendedKeyboardProfile");

                ExtendedKeyboardProfile profile = (ExtendedKeyboardProfile) keyboardProfile;
                List<ExtendedKeyboardProfile.ExtendedKeyboardBinding> bindings = profile.GetBindings();
                
                foreach (ExtendedKeyboardProfile.ExtendedKeyboardBinding binding in bindings)
                {
                    Debug.Log($"Id : {binding.id} Character : {binding.character} isMacro {binding.isMacro}");
                }
            }
            else
            {
                throw new System.Exception("Profile 2 is of an non TypedKeyboardProfile type!");
            }
        }
    }
}