using UnityEngine;
using System.Collections;

/// <summary>
/// HUD element that only exists in debug builds.
/// Displays FPS counter, probably other things?
/// </summary>
public class HUDDebug : MonoBehaviour
{
    public WorldController world;
    public TextMesh textMesh;
    int currentUpdateHit = 0;

    /// <summary>
    /// MonoBehaviour.Awake()
    /// </summary>
    void Awake()
    {
        if (Debug.isDebugBuild == false)
        {
            Destroy(gameObject);
        }
        textMesh.text = "60";
    }

    /// <summary>
    /// MonoBehaviour.Update()
    /// </summary>
    void Update()
    {
        if (world.StylisticHacksManager != null)
        {
            currentUpdateHit++;
            if (currentUpdateHit == 60)
            {
                textMesh.text = world.StylisticHacksManager.fps.ToString();
                currentUpdateHit = 0;
            }
        }
    }
}
