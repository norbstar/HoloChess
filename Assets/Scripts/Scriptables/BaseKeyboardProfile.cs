namespace Scriptables
{
    public abstract class BaseKeyboardProfile<T> : TypedKeyboardProfile<T> where T : BaseKeyboardBinding
    {
        void OnEnable()
        {
            foreach (T binding in GetBindings())
            {
                if (binding == null) continue;

                char character = System.Convert.ToChar(System.Convert.ToUInt32($"0x{binding.code}", 16));
                
                if (binding.autoGenerateLabel)
                {
                    binding.label = character.ToString();
                }

                binding.character = character;
            }
        }
    }
}