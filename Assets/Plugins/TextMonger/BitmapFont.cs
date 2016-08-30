using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct BitmapFontCharacter
{
    public Sprite sprite
    {
        get
        {
            return _sprite;
        }
        internal set
        {
            _sprite = value;
            spriteRect = _sprite.rect;
        }
    }
    public int xOffset { get; internal set; }
    public int yOffset { get; internal set; }
    public int advance { get; internal set; }

    private Sprite _sprite;

    [SerializeField] private Rect spriteRect; // we can't hold onto a Sprite reference through deserialization, so we keep the rect and recalculate the sprite

    public void RestoreSprite(Texture2D tex)
    {
        _sprite = Sprite.Create(tex, spriteRect, Vector2.zero);
    }

}

/// <summary>
/// 
/// </summary>

[Serializable]
[ExecuteInEditMode]
public class BitmapFont : ScriptableObject, ISerializationCallbackReceiver
{
    private Dictionary<char, BitmapFontCharacter> characters;
    public int spaceLength { get; private set; }
    public int spaceHeight { get; private set; }
    [SerializeField] private Texture2D tex;
    [SerializeField] private string assetName;
    [SerializeField] private int xmlHash;
    [SerializeField] private string assetPath; // WE USE THIS WITH ASSETDATABASE CALLS. If those aren't behind an #if UNITY_EDITOR block the build will break. Extract all the data you care about to the on-disk .asset file!
    [SerializeField] private char[] dictKeys;
    [SerializeField] private BitmapFontCharacter[] dictValues;

    public BitmapFontCharacter GetCharacter (char c)
    {
        BitmapFontCharacter s;
        if (characters.ContainsKey(c)) s = characters[c];
        else s = default(BitmapFontCharacter);
        return s;
    }

    public void OnBeforeSerialize()
    {
        dictKeys = new char[characters.Count];
        characters.Keys.CopyTo(dictKeys, 0);
        dictValues = new BitmapFontCharacter[characters.Count];
        characters.Values.CopyTo(dictValues, 0);
    }

    public void OnAfterDeserialize ()
    {
        characters = new Dictionary<char, BitmapFontCharacter>(dictKeys.Length);
        for (int i = 0; i < dictKeys.Length; i++)
        {
            dictValues[i].RestoreSprite(tex);
            characters.Add(dictKeys[i], dictValues[i]);
        }
    }

#if UNITY_EDITOR

    void LoadIn()
    {
        BmFont.FontFile fontData = BmFont.FontLoader.Load(AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath + ".xml"));
        xmlHash = Animator.StringToHash(AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath + ".xml").text);
        tex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath + ".png");

        for (int i = 0; i < fontData.Chars.Count; i++)
        {
            BitmapFontCharacter c = new BitmapFontCharacter();
            c.xOffset = fontData.Chars[i].XOffset;
            c.yOffset = fontData.Chars[i].YOffset;
            c.advance = fontData.Chars[i].XAdvance;
            c.sprite = Sprite.Create(tex, new Rect(fontData.Chars[i].X, fontData.Chars[i].Y, fontData.Chars[i].Width, fontData.Chars[i].Height), Vector2.zero);
            characters.Add(char.ConvertFromUtf32(fontData.Chars[i].ID)[0], c);
        }
    }

    void OnEnable ()
    {
        if (assetName != string.Empty && AssetDatabase.Contains(this))
        {
            if (Animator.StringToHash(AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath + ".xml").text) != xmlHash)
            {
                LoadIn();
            }
        }

        else
        {
            string _name = "liberation-sans";
            assetPath = "Assets/Plugins/TextMonger/Fonts/" + _name;
            characters = new Dictionary<char, BitmapFontCharacter>();
            TextAsset xml = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath + ".xml");
            Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath + ".png");
            if (xml != null && tex != null)
            {
                assetName = _name;
                LoadIn();
                SaveToDisk();
            }
        }
    }

    void SaveToDisk()
    {
        if (!AssetDatabase.Contains(this))
        {
            AssetDatabase.CreateAsset(this, "Assets/Plugins/TextMonger/Fonts/" + assetName + ".asset");
        }
        AssetDatabase.SaveAssets();
    }
#endif

}
