using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class OptionsMenuResLabel : MonoBehaviour
{
    public TextMesh textMesh;
    private HardwareInterfaceManager hardwareInterfaceManager;
    private bool fullscreenBoolBuffer;
    private WindowedResolutionMultiplier windowedResBuffer;
    private Resolution fullscreenResBuffer;
    private string[] lines;

    void Start ()
    {
        GameObject hwInterfaceManagerObj = GameObject.Find("Universe/HardwareInterfaceManager");
        if (hwInterfaceManagerObj != null)
        {
            hardwareInterfaceManager = hwInterfaceManagerObj.GetComponent<HardwareInterfaceManager>();
        }
        string s = Resources.Load<TextAsset>(GlobalStaticResourcePaths.p_windowed_resolutions).ToString();
        lines = s.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (hardwareInterfaceManager == null)
        {
            GameObject hwInterfaceManagerObj = GameObject.Find("Universe/HardwareInterfaceManager");
            if (hwInterfaceManagerObj != null)
            {
                hardwareInterfaceManager = hwInterfaceManagerObj.GetComponent<HardwareInterfaceManager>();
            }
        }
        else
        {
            if (Screen.fullScreen != fullscreenBoolBuffer || hardwareInterfaceManager.fullscreenRes.width != fullscreenResBuffer.width || hardwareInterfaceManager.fullscreenRes.height != fullscreenResBuffer.height ||
                hardwareInterfaceManager.WindowedRes != windowedResBuffer)
            {
                fullscreenBoolBuffer = Screen.fullScreen;
                windowedResBuffer = hardwareInterfaceManager.WindowedRes;
                fullscreenResBuffer = hardwareInterfaceManager.fullscreenRes;
                if (Screen.fullScreen == true)
                {
                    textMesh.text = Screen.currentResolution.width.ToString() + "x" + Screen.currentResolution.height.ToString();
                }
                else
                {
                    textMesh.text = lines[(int)hardwareInterfaceManager.WindowedRes];
                }
            }
        }
	}
}
