using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraGBFX : MonoBehaviour
{
    public UnityStandardAssets.ImageEffects.ScreenOverlay overlayEffect;
    private WindowedResolutionMultiplier res;

    void Start ()
    {
#if UNITY_EDITOR
        if (HardwareInterfaceManager.Instance == null) return; // bandaid over dumb editor bug related to script compilation
#endif
        res = HardwareInterfaceManager.Instance.resMulti;
        GenerateOverlayTexture();
    }

    void Update ()
    {
#if UNITY_EDITOR
        if (HardwareInterfaceManager.Instance == null) return; // bandaid over dumb editor bug related to script compilation
#endif
        if (res != HardwareInterfaceManager.Instance.resMulti)
        {
            res = HardwareInterfaceManager.Instance.resMulti;
            GenerateOverlayTexture();
        }
    }

    public void GenerateOverlayTexture ()
    {
        overlayEffect.enabled = true;
        Texture2D overlay = new Texture2D(HammerConstants.LogicalResolution_Horizontal * ((int)HardwareInterfaceManager.Instance.resMulti + 1) * 2,
            HammerConstants.LogicalResolution_Vertical * ((int)HardwareInterfaceManager.Instance.resMulti + 1) * 2, TextureFormat.RGBA32, false);
        int m = 2 * ((int)HardwareInterfaceManager.Instance.resMulti + 1);
        if (HardwareInterfaceManager.Instance.resMulti > WindowedResolutionMultiplier.x1)
        {
            for (int y = 0; y < overlay.height; y++)
            {
                for (int x = 0; x < overlay.width; x++)
                {
                    // every four/six/eight for 2x/3x/4x, etc.
                    if (x % m == 0 || y % m == 0)
                    {
                        overlay.SetPixel(x, y, Color.black);
                    } 
                    else
                    {
                        overlay.SetPixel(x, y, Color.clear);
                    }
                }
            }
            overlay.Apply();
            overlayEffect.texture = overlay;
        }
        else
        {
            overlayEffect.enabled = false; // can't fake LCD pixel spacing if we're mapping 1:1, duh
        }
    }
}
