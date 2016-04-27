using UnityEngine;
using System.Collections;

/// <summary>
/// HUD element that displays level.
/// </summary>
public class HUDLevelCounter : MonoBehaviour
{
    public TextMesh textMesh;
    public WorldController world;
    public int LevelValueCache;

    /// <summary>
    /// MonoBehaviour.Update()
    /// </summary>
    void Update()
    {
        if (world.player.energy.Level != LevelValueCache)
        {
            LevelValueCache = world.player.energy.Level;
            string text = world.player.energy.Level.ToString();
            textMesh.text = text;
        }
    }
}
