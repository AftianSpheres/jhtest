using System;
using UnityEngine;
using InControl;

/// <summary>
/// Horrible bloated struct that contains player's control settings.
/// We use this to pass those as a unit, because there's a shitload of variables here.
/// </summary>
[Serializable]
public struct ControlPrefs
{
    public ControlModeType setControlMode;
    public float MouseSensitivity;
    public bool MouseInvertX;
    public bool MouseInvertY;
    public uint GamepadIndex;
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
    public InputControlType GamepadDPadXAxis;
    public float GamepadDPadDeadZone;
    public bool GamepadDPadXAxisInverted;
    public InputControlType GamepadDPadYAxis; // lack of sensitivity here not an issue, dón't add it - remember, this is digital
    public bool GamepadDPadYAxisInverted;
    public InputControlType GamepadUp;
    public InputControlType GamepadDown;
    public InputControlType GamepadLeft;
    public InputControlType GamepadRight;
    public InputControlType GamepadAimXAxis;
    public float GamepadAimSensitivity;
    public float GamepadAimDeadZone;
    public bool GamepadAimXAxisInverted;
    public InputControlType GamepadAimYAxis;
    public bool GamepadAimYAxisInverted;
    public InputControlType GamepadConfirm;
    public bool inHybridConfirmIsOnMouse;
    public InputControlType GamepadCancel;
    public bool inHybridCancelIsOnMouse;
    public InputControlType GamepadMenu;
    public bool inHybridMenuIsOnMouse;
    public bool inHybridFire1IsOnMouse;
    public float GamepadFire1AxisMin;
    public float GamepadFire1AxisMax;
    public InputControlType GamepadFire1;
    public bool inHybridFire2IsOnMouse;
    public float GamepadFire2AxisMin;
    public float GamepadFire2AxisMax;
    public InputControlType GamepadFire2;
    public bool inHybridDodgeIsOnMouse;
    public float GamepadDodgeAxisMin;
    public float GamepadDodgeAxisMax;
    public InputControlType GamepadDodge;
    public bool inHybridQuickTabooIsOnMouse;
    public float GamepadQuickTabooAxisMin;
    public float GamepadQuickTabooAxisMax;
    public InputControlType GamepadQuickTaboo;

    public ControlPrefs(ControlModeType mode)
    {
        setControlMode = mode;
        MouseSensitivity = 1.0f;
        MouseInvertX = false;
        MouseInvertY = false;
        GamepadIndex = 0;
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
        GamepadDPadIsMappedToAxis = false;
        GamepadDPadXAxis = InputControlType.LeftStickX;
        GamepadDPadDeadZone = 0;
        GamepadDPadXAxisInverted = false;
        GamepadDPadYAxis = InputControlType.LeftStickY;
        GamepadDPadYAxisInverted = false;
        GamepadUp = InputControlType.DPadUp;
        GamepadDown = InputControlType.DPadDown;
        GamepadLeft = InputControlType.DPadLeft;
        GamepadRight = InputControlType.DPadRight;
        GamepadAimXAxis = InputControlType.RightStickX;
        GamepadAimSensitivity = 2.5f;
        GamepadAimDeadZone = 0.2f;
        GamepadAimXAxisInverted = false;
        GamepadAimYAxis = InputControlType.RightStickY;
        GamepadAimYAxisInverted = false;
        GamepadConfirm = InputControlType.Action1;
        inHybridConfirmIsOnMouse = false;
        GamepadCancel = InputControlType.Action2;
        inHybridCancelIsOnMouse = false;
        GamepadMenu = InputControlType.Start;
        inHybridMenuIsOnMouse = false;
        inHybridFire1IsOnMouse = true;
        GamepadFire1AxisMin = 0.0f;
        GamepadFire1AxisMax = 1.0f;
        GamepadFire1 = InputControlType.RightBumper;
        inHybridFire2IsOnMouse = true;
        GamepadFire2AxisMin = 0.1f;
        GamepadFire2AxisMax = 1.0f;
        GamepadFire2 = InputControlType.RightTrigger;
        inHybridDodgeIsOnMouse = false;
        GamepadDodgeAxisMin = 0.0f;
        GamepadDodgeAxisMax = 1.0f;
        GamepadDodge = InputControlType.LeftBumper;
        inHybridQuickTabooIsOnMouse = false;
        GamepadQuickTabooAxisMin = 0.1f;
        GamepadQuickTabooAxisMax = 1.0f;
        GamepadQuickTaboo = InputControlType.LeftTrigger;
    }
}