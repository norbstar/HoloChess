using System;
using System.Collections.Generic;

using UnityEngine;

namespace Scriptables
{
    [Serializable]
    public class KeyboardBinding
    {
        public int id;
        public string unicode;
        public string character;

        [Header("Label")]
        public bool autoGenerateLabel = true;
        public string label = default(string);

        [Header("Sprite")]
        public bool displaySpriteInPlaceOfLabel = false;
        public Sprite sprite;

        [Header("Additional")]
        public List<string> additionalUnicodes;
    }
}