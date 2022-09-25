using System;

using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Keyboard Profile", menuName = "Keyboard UI/Profile", order = 1)]
    public class KeyboardProfile : TypedKeyboardProfile<KeyboardBinding>
    {
        public override void MapBinding(KeyboardBinding binding)
        {
            var text = System.Text.RegularExpressions.Regex.Unescape(binding.unicode).ToString();

            if (binding.autoGenerateLabel)
            {
                binding.label = text;
            }
            
            binding.character = text;
            Debug.Log(binding.character);
        }
    }
}