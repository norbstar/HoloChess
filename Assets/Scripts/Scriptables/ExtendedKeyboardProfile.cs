using System;

using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Extended Keyboard Profile", menuName = "Keyboard UI/Extended Profile", order = 2)]
    public class ExtendedKeyboardProfile : TypedKeyboardProfile<ExtendedKeyboardProfile.ExtendedBinding>
    {
        [Serializable]
        public class ExtendedBinding : BaseKeyboardBinding
        {
            public bool isMacro = false;
        }

        void OnEnable()
        {
            foreach (ExtendedBinding binding in GetBindings())
            {
                if (binding == null) continue;

                char character = System.Convert.ToChar(System.Convert.ToUInt32($"0x{binding.code}", 16));
                
                if (binding.autoGenerateLabel)
                {
                    binding.label = character.ToString();
                }

                binding.character = character;

                if (binding.isMacro)
                {
                    // TODO
                }
            }
        }
    }   
}