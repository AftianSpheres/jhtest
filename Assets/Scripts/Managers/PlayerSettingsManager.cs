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

    void _in_LoadControlSettings_GamepadDPad(XmlDocument doc)
    {
        int intRes;
        throw new NotImplementedException();
    }

    /// <summary>
    /// Loads control at xpath.
    /// Wraps some repeated logic.
    /// Hacky and shitty; less hacky and shitty than a several-hundred-line control config loader.
    /// </summary>
    void _in_LoadControlSettings_KBM(XmlDocument doc, string xpath, ref KeyCode code)
    {
        int intRes;
        if (doc.SelectSingleNode(xpath) != null)
        {
            if (int.TryParse(doc.SelectSingleNode(xpath).Value, out intRes) == true)
            {
                if (intRes >= (int)KeyCode.JoystickButton0 && intRes <= (int)KeyCode.Joystick8Button19 && intRes != (int)KeyCode.None)
                {
                    code = (KeyCode)intRes;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Gets mouse-related control settings.
    /// </summary>
    void _in_LoadControlSettingsMouse(XmlDocument doc)
    {
        bool boolRes;
        float floatRes;
        if (doc.SelectSingleNode("/ControlSettings/Mouse/XInvert") != null)
        {
            if (bool.TryParse(doc.SelectSingleNode("/ControlSettings/Mouse/XInvert").Value, out boolRes) == true)
            {
                controlPrefs.MouseInvertX = boolRes;
            }
        }
        if (doc.SelectSingleNode("/ControlSettings/Mouse/YInvert") != null)
        {
            if (bool.TryParse(doc.SelectSingleNode("/ControlSettings/Mouse/YInvert").Value, out boolRes) == true)
            {
                controlPrefs.MouseInvertY = boolRes;
            }
        }
        if (doc.SelectSingleNode("/ControlSettings/Mouse/Sensitivity") != null)
        {
            if (float.TryParse(doc.SelectSingleNode("/ControlSettings/Mouse/Sensitivity").Value, out floatRes) == true)
            {
                controlPrefs.MouseSensitivity = floatRes;
            }
        }
    }

    /// <summary>
    /// Parses XML file, loads control config settings.
    /// </summary>
    void _in_LoadControlSettings(XmlDocument doc)
    {
        int intRes;
        if (doc.SelectSingleNode("/ControlSettings/ControlMode") != null)
        {
            if (int.TryParse(doc.SelectSingleNode("/ControlSettings/ControlMode").Value,  out intRes) == false || intRes < 0 || intRes > (int)ControlModeType.Gamepad_Mouse_Hybrid)
            {
                controlPrefs.setControlMode = ControlModeType.Mouse_Keyboard;
            }
            else
            {
                controlPrefs.setControlMode = (ControlModeType) intRes;
            }
        }
        _in_LoadControlSettings_KBM(doc, "/ControlSettings/KBM_Down", ref controlPrefs.KBMDown);
        _in_LoadControlSettings_KBM(doc, "/ControlSettings/KBM_Up", ref controlPrefs.KBMUp);
        _in_LoadControlSettings_KBM(doc, "/ControlSettings/KBM_Left", ref controlPrefs.KBMLeft);
        _in_LoadControlSettings_KBM(doc, "/ControlSettings/KBM_Right", ref controlPrefs.KBMRight);
        _in_LoadControlSettings_KBM(doc, "/ControlSettings/KBM_Confirm", ref controlPrefs.KBMConfirm);
        _in_LoadControlSettings_KBM(doc, "/ControlSettings/KBM_Cancel", ref controlPrefs.KBMCancel);
        _in_LoadControlSettings_KBM(doc, "/ControlSettings/KBM_Menu", ref controlPrefs.KBMMenu);
        _in_LoadControlSettings_KBM(doc, "/ControlSettings/KBM_Dodge", ref controlPrefs.KBDodge);
        _in_LoadControlSettings_KBM(doc, "/ControlSettings/KBM_QuickTaboo", ref controlPrefs.KBMQuickTaboo);
        _in_LoadControlSettings_KBM(doc, "/ControlSettings/KBM_Fire1", ref controlPrefs.KBMFire1);
        _in_LoadControlSettings_KBM(doc, "/ControlSettings/KBM_Fire1", ref controlPrefs.KBMFire2);
    }

    /// <summary>
    /// Parses XML file, sets music volume.
    /// </summary>
    void _in_LoadMusicVolume(XmlDocument doc)
    {
        if (doc.SelectSingleNode("/PlayerSettings/MusicVolume") != null)
        {
            if (float.TryParse(doc.SelectSingleNode("/PlayerSettings/MusicVolume").Value, out MusicVolume) == false)
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

    /// <summary>
    /// Parses XML file, sets SFX volume.
    /// </summary>
    void _in_LoadSFXVolume(XmlDocument doc)
    {
        if (doc.SelectSingleNode("/PlayerSettings/SFXVolume") != null)
        {
            if (float.TryParse(doc.SelectSingleNode("/PlayerSettings/SFXVolume").Value, out SFXVolume) == false)
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

    /// <summary>
    /// Parses XML file, sets video settings.
    /// </summary>
    void _in_LoadVideoSettings(XmlDocument doc)
    {
        bool foundGoodResolution = false;
        if (doc.SelectSingleNode("/PlayerSettings/IsFullscreen") != null)
        {
            if (doc.SelectSingleNode("/PlayerSettings/FullscreenRes/Height") != null && doc.SelectSingleNode("/PlayerSettings/FullscreenRes/Width") != null)
            {
                int x;
                int y;
                if (int.TryParse(doc.SelectSingleNode("/PlayerSettings/FullscreenRes/Height").Value, out y) == true && int.TryParse(doc.SelectSingleNode("/PlayerSettings/FullscreenRes/width").Value, out x) == true)
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
        else if (doc.SelectSingleNode("/PlayerSettings/WindowedResMultipler") != null)
        {
            int result;
            if (int.TryParse(doc.SelectSingleNode("/PlayerSettings/WindowedResMultiplier").Value, out result) == true && result > -1 && result <= (int)WindowedResolutionMultiplier.x14)
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
