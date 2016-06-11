using UnityEngine;
using System;
using System.Collections;

public enum ControlMode
{
    Mouse_Keyboard,
    Gamepad,
    Gamepad_Mouse_Hybrid,
    Wiimote
}

[Serializable]
public class PlayerSettingsManager : Manager<PlayerSettingsManager>
{
    public static float VolumeMin = 0.0f;
    public static float VolumeMax = 1.0f;
    public float MusicVolume = 1f;
    public float SFXVolume = 1f;

    public ControlMode setControlMode;

    // preferences for kb/m and gamepad stored separately - you don't lose your gamepad config because you switched to KB/M

    public float controlPrefs_MouseSensitivity;
    public bool controlPrefs_MouseInvertX;
    public bool controlPrefs_MouseInvertY;
    public KeyCode controlPrefs_KBMUp;
    public KeyCode controlPrefs_KBMDown;
    public KeyCode controlPrefs_KBMLeft;
    public KeyCode controlPrefs_KBMRight;
    public KeyCode controlPrefs_KBMConfirm;
    public KeyCode controlPrefs_KBMCancel;
    public KeyCode controlPrefs_KBMMenu;
    public KeyCode controlPrefs_KBMFire1;
    public KeyCode controlPrefs_KBMFire2;
    public KeyCode controlPrefs_KBDodge;
    public KeyCode controlPrefs_KBMQuickTaboo;

    public bool controlPrefs_GamepadDPadIsMappedToAxis = false;
    public string controlPrefs_GamepadDPadXAxis;
    public float controlPrefs_GamepadDPadXAxisDeadZone;
    public bool controlPrefs_GamepadDPadXAxisInverted;
    public string controlPrefs_GamepadDPadYAxis; // lack of sensitivity here not an issue, dón't add it - remember, this is digital
    public float controlPrefs_GamepadDPadYAxisDeadZone;
    public bool controlPrefs_GamepadDPadYAxisInverted;
    public KeyCode controlPrefs_GamepadUp;
    public KeyCode controlPrefs_GamepadDown;
    public KeyCode controlPrefs_GamepadLeft;
    public KeyCode controlPrefs_GamepadRight;
    public string controlPrefs_GamepadAimXAxis;
    public float controlPrefs_GamepadAimXAxisSensitivity;
    public float controlPrefs_GamepadAimXAxisDeadZone;
    public bool controlPrefs_GamepadAimXAxisInverted;
    public string controlPrefs_GamepadAimYAxis;
    public float controlPrefs_GamepadAimYAxisSensitivity;
    public float controlPrefs_GamepadAimYAxisDeadZone;
    public bool controlPrefs_GamepadAimYAxisInverted;
    public KeyCode controlPrefs_GamepadConfirm;
    public KeyCode controlPrefs_GamepadCancel;
    public KeyCode controlPrefs_GamepadMenu;
    public bool controlPrefs_GamepadFire1IsMappedToAxis = false;
    public string controlPrefs_GamepadFire1Axis;
    public float controlPrefs_GamepadFire1AxisMin;
    public float controlPrefs_GamepadFire1AxisMax;
    public KeyCode controlPrefs_GamepadFire1;
    public bool controlPrefs_GamepadFire2IsMappedToAxis = false;
    public string controlPrefs_GamepadFire2Axis;
    public float controlPrefs_GamepadFire2AxisMin;
    public float controlPrefs_GamepadFire2AxisMax;
    public KeyCode controlPrefs_GamepadFire2;
    public bool controlPrefs_GamepadDodgeIsMappedToAxis = false;
    public string controlPrefs_GamepadDodgeAxis;
    public float controlPrefs_GamepadDodgeAxisMin;
    public float controlPrefs_GamepadDodgeAxisMax;
    public KeyCode controlPrefs_GamepadDodge;
    public bool controlPrefs_GamepadQuickTabooIsMappedToAxis = false;
    public string controlPrefs_GamepadQuickTabooAxis;
    public float controlPrefs_GamepadQuickTabooAxisMin;
    public float controlPrefs_GamepadQuickTabooAxisMax;
    public KeyCode controlPrefs_GamepadQuickTaboo;

    private HardwareInterfaceManager hardwareInterfaceManager;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
