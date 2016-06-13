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
    public int lineIndex;

	// Use this for initialization
	void Awake ()
    {
        textMesh.text = Resources.Load<TextAsset>(textResourcesPath + resourcePath).ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)[lineIndex];
	}
	
}
