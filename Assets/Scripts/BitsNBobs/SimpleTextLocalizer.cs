using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Simple runtime localizer for fixed-content text meshes.
/// Use this for single-lines; nothing bigger!
/// WARNING: quick and nasty, probably inefficient as hell
/// </summary>
public class SimpleTextLocalizer : MonoBehaviour
{
    private static string textResourcesPath = "Text/" + HammerConstants.LocalizationPrefix + "/";
    public TextMesh textMesh;
    public string resourcePath;
    public int[] linesIndex;

	// Use this for initialization
	void Awake ()
    {
        string[] res = Resources.Load<TextAsset>(textResourcesPath + resourcePath).ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        textMesh.text = "";
        Debug.Log(res);
        for (int i = 0; i < linesIndex.Length; i++)
        {
            string s;
            if (i < linesIndex.Length - 1)
            {
                s = res[linesIndex[i]] + "\n";
            }
            else
            {
                s = res[linesIndex[i]];
            }
            textMesh.text = textMesh.text.Insert(textMesh.text.Length, s);
        }
	}
	
}
