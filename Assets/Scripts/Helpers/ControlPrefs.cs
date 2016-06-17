using System;
using UnityEngine;

/// <summary>
/// Horrible bloated struct that contains player's control settings.
/// We use this to pass those as a unit, because there's a shitload of variables here.
/// </summary>
[Serializable]
public struct ControlPrefs
{
    public static string[] axisNames =
    {
        "joy1-1",
        "joy1-2",
        "joy1-3",
        "joy1-4",
        "joy1-5",
        "joy1-6",
        "joy1-7",
        "joy1-8",
        "joy1-9",
        "joy1-10"
    };

    public ControlModeType setControlMode;
    public float MouseSensitivity;
    public bool MouseInvertX;
    public bool MouseInvertY;
    public KeyCode KBMUp;
    public KeyCode KBMDown;
    public KeyCode KBMLeft;
    public KeyCode KBMRight;
    public KeyCode KBMConfirm;
    public KeyCode KBMCancel;
    public KeyCode KBMMenu;
    public KeyCode KBMFire1;
    public KeyCode KBMFire2;
    public KeyCode KBDodge;
    public KeyCode KBMQuickTaboo;
    public bool GamepadDPadIsMappedToAxis;
    public string GamepadDPadXAxis;
    public float GamepadDPadXAxisDeadZone;
    public bool GamepadDPadXAxisInverted;
    public string GamepadDPadYAxis; // lack of sensitivity here not an issue, dón't add it - remember, this is digital
    public float GamepadDPadYAxisDeadZone;
    public bool GamepadDPadYAxisInverted;
    public KeyCode GamepadUp;
    public KeyCode GamepadDown;
    public KeyCode GamepadLeft;
    public KeyCode GamepadRight;
    public string GamepadAimXAxis;
    public float GamepadAimXAxisSensitivity;
    public float GamepadAimXAxisDeadZone;
    public bool GamepadAimXAxisInverted;
    public string GamepadAimYAxis;
    public float GamepadAimYAxisSensitivity;
    public float GamepadAimYAxisDeadZone;
    public bool GamepadAimYAxisInverted;
    public KeyCode GamepadConfirm;
    public bool inHybridConfirmIsOnMouse;
    public KeyCode GamepadCancel;
    public bool inHybridCancelIsOnMouse;
    public KeyCode GamepadMenu;
    public bool inHybridMenuIsOnMouse;
    public bool inHybridFire1IsOnMouse;
    public bool GamepadFire1IsMappedToAxis;
    public string GamepadFire1Axis;
    public float GamepadFire1AxisMin;
    public float GamepadFire1AxisMax;
    public KeyCode GamepadFire1;
    public bool inHybridFire2IsOnMouse;
    public bool GamepadFire2IsMappedToAxis;
    public string GamepadFire2Axis;
    public float GamepadFire2AxisMin;
    public float GamepadFire2AxisMax;
    public KeyCode GamepadFire2;
    public bool inHybridDodgeIsOnMouse;
    public bool GamepadDodgeIsMappedToAxis;
    public string GamepadDodgeAxis;
    public float GamepadDodgeAxisMin;
    public float GamepadDodgeAxisMax;
    public KeyCode GamepadDodge;
    public bool inHybridQuickTabooIsOnMouse;
    public bool GamepadQuickTabooIsMappedToAxis;
    public string GamepadQuickTabooAxis;
    public float GamepadQuickTabooAxisMin;
    public float GamepadQuickTabooAxisMax;
    public KeyCode GamepadQuickTaboo;

    public ControlPrefs(bool get_defaults)
    {
        if (get_defaults == false)
        {
            throw new System.Exception("The ControlPrefs constructor is a shitty hack! Don't call it if you don't actually want it to set the defaults, man! Come on!");
        }
        setControlMode = ControlModeType.Gamepad;
        MouseSensitivity = 1.0f;
        MouseInvertX = false;
        MouseInvertY = false;
        KBMUp = KeyCode.W;
        KBMDown = KeyCode.S;
        KBMLeft = KeyCode.A;
        KBMRight = KeyCode.D;
        KBMConfirm = KeyCode.E;
        KBMCancel = KeyCode.Q;
        KBMMenu = KeyCode.LeftShift;
        KBMFire1 = KeyCode.Mouse0;
        KBMFire2 = KeyCode.Mouse1;
        KBDodge = KeyCode.Space;
        KBMQuickTaboo = KeyCode.Mouse2;
        GamepadDPadIsMappedToAxis = true;
        GamepadDPadXAxis = "joy1-6";
        GamepadDPadXAxisDeadZone = 0;
        GamepadDPadXAxisInverted = false;
        GamepadDPadYAxis = "joy1-7";
        GamepadDPadYAxisDeadZone = 0;
        GamepadDPadYAxisInverted = false;
        GamepadUp = KeyCode.None;
        GamepadDown = KeyCode.None;
        GamepadLeft = KeyCode.None;
        GamepadRight = KeyCode.None;
        GamepadAimXAxis = "joy1-4";
        GamepadAimXAxisSensitivity = 2.5f;
        GamepadAimXAxisDeadZone = 0.2f;
        GamepadAimXAxisInverted = true;
        GamepadAimYAxis = "joy1-5";
        GamepadAimYAxisSensitivity = 2.5f;
        GamepadAimYAxisDeadZone = 0.2f;
        GamepadAimYAxisInverted = true;
        GamepadConfirm = KeyCode.Joystick1Button0;
        inHybridConfirmIsOnMouse = false;
        GamepadCancel = KeyCode.Joystick1Button1;
        inHybridCancelIsOnMouse = false;
        GamepadMenu = KeyCode.Joystick1Button7;
        inHybridMenuIsOnMouse = false;
        inHybridFire1IsOnMouse = true;
        GamepadFire1IsMappedToAxis = false;
        GamepadFire1Axis = "";
        GamepadFire1AxisMin = 0.0f;
        GamepadFire1AxisMax = 1.0f;
        GamepadFire1 = KeyCode.Joystick1Button5;
        inHybridFire2IsOnMouse = true;
        GamepadFire2IsMappedToAxis = true;
        GamepadFire2Axis = "joy1-10";
        GamepadFire2AxisMin = 0.1f;
        GamepadFire2AxisMax = 1.0f;
        GamepadFire2 = KeyCode.None;
        inHybridDodgeIsOnMouse = false;
        GamepadDodgeIsMappedToAxis = false;
        GamepadDodgeAxis = "";
        GamepadDodgeAxisMin = 0.0f;
        GamepadDodgeAxisMax = 1.0f;
        GamepadDodge = KeyCode.Joystick1Button4;
        inHybridQuickTabooIsOnMouse = false;
        GamepadQuickTabooIsMappedToAxis = true;
        GamepadQuickTabooAxis = "joy1-9";
        GamepadQuickTabooAxisMin = 0.1f;
        GamepadQuickTabooAxisMax = 1.0f;
        GamepadQuickTaboo = KeyCode.None;
    }
}