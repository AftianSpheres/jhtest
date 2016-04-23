using UnityEngine;
using System.Collections;

/// <summary>
/// HUD element that only exists in debug builds.
/// Displays FPS counter, probably other things?
/// </summary>
public class HUDDebug : MonoBehaviour
{
    public TextMesh textMesh;
    float LastSecondTime = 0.0f;
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
        textMesh.text = "--";
    }

    /// <summary>
    /// MonoBehaviour.Update()
    /// </summary>
    void Update()
    {
        currentUpdateHit++;
        if (currentUpdateHit == 60)
        {
            float fps = Mathf.Round(120 - (60 * (Time.realtimeSinceStartup - LastSecondTime)));
            textMesh.text = fps.ToString();
            LastSecondTime = Time.realtimeSinceStartup;
            currentUpdateHit = 0;
        }
    }

}
