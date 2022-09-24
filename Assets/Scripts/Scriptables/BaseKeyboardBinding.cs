using System;

namespace Scriptables
{
    [Serializable]
    public class BaseKeyboardBinding
    {
        public int index;
        public string code;
        public string label = default(string);
        public char character;
        public bool autoGenerateLabel = true;
    }
}