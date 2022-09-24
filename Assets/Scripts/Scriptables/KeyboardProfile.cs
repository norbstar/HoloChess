using System;
using System.Text;

using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Keyboard Profile", menuName = "Keyboard UI/Profile", order = 1)]
    public class KeyboardProfile : TypedKeyboardProfile<KeyboardProfile.Binding>
    {
        [Serializable]
        public class Binding
        {
            public string unicode;
            public string label = default(string);
            public string character;
            public bool autoGenerateLabel = true;
        }

        void OnEnable()
        {
            foreach (Binding binding in GetBindings())
            {
                if (binding == null) continue;
                
                var character = Convert.ToInt32(binding.unicode , 16);
                var text = Char.ConvertFromUtf32(character);

                if (binding.autoGenerateLabel)
                {
                    binding.label = text;
                }
                
                binding.character = text;
                Debug.Log(binding.character);
            }
        }
    }
}