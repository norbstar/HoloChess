using System;

namespace Scriptables
{
    [Serializable]
    public class KeyboardBinding
    {
        public int id;
        public string unicode;
        public string label = default(string);
        public bool autoGenerateLabel = true;
        public string character;
    }
}