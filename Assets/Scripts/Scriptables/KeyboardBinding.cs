using System;
using System.Collections.Generic;

namespace Scriptables
{
    [Serializable]
    public class KeyboardBinding
    {
        public int id;
        public string unicode;
        public string label = default(string);
        public bool autoGenerateLabel = true;
        public List<string> additionalUnicodes;
        public string character;
    }
}