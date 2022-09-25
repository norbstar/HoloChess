using System;

using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Extended Keyboard Profile", menuName = "Keyboard UI/Extended Profile", order = 2)]
    public class ExtendedKeyboardProfile : TypedKeyboardProfile<ExtendedKeyboardProfile.ExtendedKeyboardBinding>
    {
        [Serializable]
        public class ExtendedKeyboardBinding : KeyboardBinding
        {
            public bool isMacro = false;
        }
        
        public override void MapBinding(ExtendedKeyboardBinding binding)
        {
            var text = System.Text.RegularExpressions.Regex.Unescape(binding.unicode).ToString();

            if (binding.autoGenerateLabel)
            {
                binding.label = text;
            }

            binding.character = text;
            Debug.Log(binding.character);

            if (binding.isMacro)
            {
                // TODO
            }
        }
    }   
}