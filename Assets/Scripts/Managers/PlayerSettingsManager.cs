using UnityEngine;
using System;
using System.Xml;
using System.Collections;

public class PlayerSettingsManager : Manager<PlayerSettingsManager>
{
    private static string configFilePath = "hammer.cfg";
    public static float VolumeMin = 0.0f;
    public static float VolumeMax = 1.0f;
    public float MusicVolume = 1f;
    public float SFXVolume = 1f;
    public ControlPrefs controlPrefs;
    private HardwareInterfaceManager hardwareInterfaceManager;

	// Use this for initialization
	void Awake ()
    {
        controlPrefs = new ControlPrefs(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void TextFileToManager ()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(configFilePath);
        if (doc.SelectSingleNode("PlayerSettings/MusicVolume") != null) 
        {
            if (float.TryParse(doc.SelectSingleNode("PlayerSettings/MusicVolume").Value, out MusicVolume) == false)
            {
                MusicVolume = 1f;
            }
            else if (MusicVolume < VolumeMin)
            {
                MusicVolume = VolumeMin;
            }
            else if (MusicVolume > VolumeMax)
            {
                MusicVolume = VolumeMax;
            }
        }
        else
        {
            MusicVolume = 1f;
        }
        if (doc.SelectSingleNode("PlayerSettings/SFXVolume") != null)
        {
            if (float.TryParse(doc.SelectSingleNode("PlayerSettings/SFXVolume").Value, out SFXVolume) == false)
            {
                SFXVolume = 1f;
            }
            else if (SFXVolume < VolumeMin)
            {
                SFXVolume = VolumeMin;
            }
            else if (SFXVolume > VolumeMax)
            {
                SFXVolume = VolumeMax;
            }
        }
        else
        {
            MusicVolume = 1f;
        }
        bool foundGoodResolution = false;
        if (doc.SelectSingleNode("PlayerSettings/isFullscreen") != null)
        {
            if (doc.SelectSingleNode("PlayerSettings/fullscreenRes/height") != null && doc.SelectSingleNode("PlayerSettings/fullscreenRes/width") != null)
            {
                int x;
                int y;
                if (int.TryParse(doc.SelectSingleNode("PlayerSettings/fullscreenRes/height").Value, out y) == true && int.TryParse(doc.SelectSingleNode("PlayerSettings/fullscreenRes/width").Value, out x) == true)
                {
                    for (int i = 0; i < Screen.resolutions.Length; i++)
                    {
                        if (Screen.resolutions[i].refreshRate % 60 == 0 && Screen.resolutions[i].height == y && Screen.resolutions[i].width == x)
                        {
                            Screen.SetResolution(Screen.resolutions[i].width, Screen.resolutions[i].height, true);
                            hardwareInterfaceManager.fullscreenRes.height = y;
                            hardwareInterfaceManager.fullscreenRes.width = x;
                            foundGoodResolution = true;
                            break;
                        }
                    }
                }
            }
        }
        else if (doc.SelectSingleNode("PlayerSettings/windowedResMultipler") != null)
        {
            int result;
            if (int.TryParse(doc.SelectSingleNode("PlayerSettings.windowedResMultiplier").Value, out result) == true && result > -1 && result <= (int)WindowedResolutionMultiplier.x14)
            {
                hardwareInterfaceManager.windowedRes = (WindowedResolutionMultiplier)result;
                hardwareInterfaceManager.RefreshWindow();
                foundGoodResolution = true;
            }
        }
        if (foundGoodResolution == false)
        {
            hardwareInterfaceManager.windowedRes = WindowedResolutionMultiplier.x2;
            hardwareInterfaceManager.RefreshWindow();
        }
    }
}
