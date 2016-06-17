using UnityEngine;
using System;
using System.IO;
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
	void Update ()
    {
	    if (hardwareInterfaceManager == null)
        {
            GameObject hardwareInterfaceManagerObj = GameObject.Find("Universe/HardwareInterfaceManager");
            if (hardwareInterfaceManagerObj != null)
            {
                hardwareInterfaceManager = hardwareInterfaceManagerObj.GetComponent<HardwareInterfaceManager>();
                FileToManager();
            }
        }
	}

    /// <summary>
    /// Loads settings from hammer.cfg.
    /// TO DO: have this get controls, too.
    /// </summary>
    void FileToManager ()
    {
        XmlDocument doc = new XmlDocument();
        try
        {
            doc.Load(configFilePath);
        }
        catch (FileNotFoundException) // config file doesn't exist - this is normal on first run
        {
            FileStream f = File.Open(configFilePath, FileMode.CreateNew);
            f.Close();
            //ManagerToFile()
        }
        catch (XmlException) // config file exists but is malformed
        {
            Debug.Log("hammer.cfg is malformed. The game can start, but default configuration values will be used. Debug this!");
            //ManagerToFile()
        }
        _in_LoadMusicVolume(doc);
        _in_LoadSFXVolume(doc);
        _in_LoadVideoSettings(doc);
        _in_LoadControlSettings(doc);
    }

    void _in_LoadControlSettings(XmlDocument doc)
    {
        if (doc.SelectSingleNode("ControlSettings/ControlMode") != null)
        {
            int res;
            if (int.TryParse(doc.SelectSingleNode("PlayerSettings/ControlMode").Value,  out res) == false || res < 0 || res > (int)ControlModeType.Gamepad_Mouse_Hybrid)
            {
                controlPrefs.setControlMode = ControlModeType.Mouse_Keyboard;
            }
            else
            {
                controlPrefs.setControlMode = (ControlModeType) res;
            }
        }
    }

    void _in_LoadMusicVolume(XmlDocument doc)
    {
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
    }

    void _in_LoadSFXVolume(XmlDocument doc)
    {
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
    }

    void _in_LoadVideoSettings(XmlDocument doc)
    {
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
