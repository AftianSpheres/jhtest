using UnityEngine;

public class CameraBlocker : MonoBehaviour
{
    new public Camera camera;
    private Resolution resBuffer;
    private bool fullscreenBuffer;

    void Awake ()
    {
        ReBlock();
    }

    void Update ()
    {
        if (Screen.currentResolution.height != resBuffer.height || Screen.currentResolution.width != resBuffer.width || Screen.fullScreen != fullscreenBuffer)
        {
            ReBlock();
        }
    }

    void ReBlock()
    {
        resBuffer = Screen.currentResolution;
        fullscreenBuffer = Screen.fullScreen;
        if (Screen.fullScreen == true)
        {
            float ratio = 1.0f;
            float border = 1.0f;
            if (Screen.currentResolution.width > Screen.currentResolution.height)
            {
                if (Screen.currentResolution.height % HammerConstants.LogicalResolution_Vertical != 0)
                {
                    border = ((float)Screen.currentResolution.height - (Screen.currentResolution.height % (float)HammerConstants.LogicalResolution_Vertical)) / (float)Screen.currentResolution.height;
                }
                ratio = ((Screen.currentResolution.height * border) / (float)HammerConstants.LogicalResolution_Vertical) / (Screen.currentResolution.width / (float)HammerConstants.LogicalResolution_Horizontal);
                camera.rect = new Rect((1 - ratio) * .5f, (1 - border) * .5f, 1.0f * ratio, 1.0f * border);
            }
            else
            {
                if (Screen.currentResolution.width % HammerConstants.LogicalResolution_Horizontal != 0)
                {
                    border = ((float)Screen.currentResolution.width - (Screen.currentResolution.width % (float)HammerConstants.LogicalResolution_Horizontal)) / (float)Screen.currentResolution.width;
                }
                ratio = ((Screen.currentResolution.width * border) / (float)HammerConstants.LogicalResolution_Horizontal) / (Screen.currentResolution.height / (float)HammerConstants.LogicalResolution_Vertical);
                camera.rect = new Rect((1 - border) * .5f, (1 - ratio) * .5f, 1.0f * border, 1.0f * ratio);
            }
        }
        else
        {
            camera.rect = new Rect(0, 0, 1, 1);
        }
    }
}
