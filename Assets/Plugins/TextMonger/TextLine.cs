using UnityEngine;
using System.Collections.Generic;


namespace TextMonger
{
    public class TextLine
    {
        public Color lineStartColor;
        public string text;
        public Dictionary<int, Sprite> spriteElements;
        public Dictionary<int, Color> colorChanges;
    }
}