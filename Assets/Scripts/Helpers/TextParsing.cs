using UnityEngine;
using System.Collections.Generic;

enum EscCode
{
    None,
    blockEnd,
    btn_DPDown,
    btn_DPUp,
    btn_DPLeft,
    btn_DPRight,
    btn_snesB,
    btn_snesA,
    btn_snesX,
    btn_snesY,
    btn_start,
    btn_select,
    lstick_neutral,
    lstick_down,
    lstick_up,
    lstick_left,
    lstick_right,
    rstick_neutral,
    rstick_down,
    rstick_up,
    rstick_left,
    rstick_right,
    btn_l1,
    btn_l2,
    btn_l3,
    btn_r1,
    btn_r2,
    btn_r3
}

public struct TextBlock
{
    string[] lines;
}


/// <summary>
/// Old attempt to do a text-parsing class that wouldn't involve writing my own text rendering system, but - well.
/// This will be deleted! It's just here so I can grab some of the data out of it when I implement parsing in the new system, lol
/// </summary>
public static class TextParsing
{
    static readonly string[] newlineChars = {"\r", "\n", "\r\n" };
    static readonly string[] escCodes =
    {
        "[!sp| ]",
        "[!sp|end]",
        "[!sp|ddown]",
        "[!sp|dup]",
        "[!sp|dleft]",
        "[!sp|dright]",
        "[!sp|snesB]",
        "[!sp|snesA]",
        "[!sp|snesX]",
        "[!sp|snesY]",
        "[!sp|start]",
        "[!sp|select]",
        "[!sp|lstick]",
        "[!sp|lsdown]",
        "[!sp|lsup]",
        "[!sp|lsleft]",
        "[!sp|lsright]",
        "[!sp|rstick]",
        "[!sp|rsdown]",
        "[!sp|rsup]",
        "[!sp|rsleft]",
        "[!sp|rsright]",
        "[!sp|l1]",
        "[!sp|l2]",
        "[!sp|l3]",
        "[!sp|r1]",
        "[!sp|r2]",
        "[!sp|r3]"
    };

    static readonly string[] escReplacements =
    {
        "",
        "æ¿çµ",
        "æ¡  ",
        "æ²  ",
        "æ³  ",
        "æ¤  ",
        "æ€  ",
        "æ¼  ",
        "æ½  ",
        "æ¾  ",
        "æ‘     ",
        "æ’     ",
        "æ¥  ",
        "æ×  ",
        "æä  ",
        "æå  ",
        "æé  ",
        "æ®  ",
        "æþ  ",
        "æü  ",
        "æú  ",
        "æí  ",
        "æó  ",
        "æö  ",
        "æ«  ",
        "æ»  ",
        "æ¬  ",
        "æá  "
    };

    // line/block system - export a set of blocks

    // replace esccodes on unsplit / split by allowable line length/ split those by newlines / split the resulting list of lines by block breaks
    // gonna have to write my own text rendering code. fuck.
    // well, do the new parser now and worry about that later

    // replace escCodes in place.

    static string ReplaceEscCodes (string s)
    {
        for (int i = 0; i < escCodes.Length; i++)
        {
            if (s.Contains(escCodes[i])) s.Replace(escCodes[i], escReplacements[i]);
        }
        return s;
    }

    static string[] SplitByLineLength (List<char> cl, int pxLineLength, Font font, int spacing)
    {
        List<char> working = new List<char>();
        List<string> output = new List<string>();
        CharacterInfo characterInfo;
        int lineLen = 0;
        while (cl.Count > 0)
        {
            if (font.GetCharacterInfo(cl[0], out characterInfo))
            {
                lineLen += characterInfo.advance + spacing;
                if (lineLen > pxLineLength)
                {
                    output.Add(new string(working.ToArray()));
                    output = new List<string>();
                    lineLen = 0;
                }
                else
                {

                }
            }

        }

        return output.ToArray();
    }


}
