using UnityEngine;
using System.Collections;

/// <summary>
/// HUD element that displays count of area keys.
/// </summary>
public class HUDKeyCounter : MonoBehaviour
{
    public TextMesh textMesh;
    public WorldController world;
    public int KeysValueCache;

    /// <summary>
    /// MonoBehaviour.Update()
    /// </summary>
    void Update()
    {
        if (world.GameStateManager.areaKeys[(int)world.Area] != KeysValueCache)
        {
            KeysValueCache = world.GameStateManager.areaKeys[(int)world.Area];
            string text = world.GameStateManager.areaKeys[(int)world.Area].ToString();
            if (text.Length > 2)
            {
                text = "99";
            }
            else if (text.Length == 1)
            {
                text = "0" + text;
            }
            textMesh.text = text;
        }
    }
}
