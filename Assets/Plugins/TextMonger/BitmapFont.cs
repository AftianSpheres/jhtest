using UnityEngine;
using System.Collections.Generic;

public class BitmapFont : ScriptableObject
{
    [SerializeField] private Dictionary<char, Sprite> characters;
    public int spaceLength { get; private set; }
    public int characterHeight { get; private set; }

    public Sprite GetCharacter (char c)
    {
        Sprite s;
        if (characters.ContainsKey(c)) s = characters[c];
        else s = default(Sprite);
        return s;
    }
}
