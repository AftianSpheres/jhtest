using UnityEngine;
using System.Collections;

public enum WindowedResolutionMultiplier
{
    x1, // 160x144
    x2, // 320x288
    x3, // 480x432
    x4, // 640x572
    x5, // 800x716
    x6, // 960x860
    x7, // 1120x1008
    x8, // 1280x1152
    x9, // 1440x1296
    x10, // 1600x1440
    x11, // 1760x1584
    x12, // 1920x1728
    x13, // 2080x1872
    x14, // 2240x2016
}

public class HardwareInterfaceManager : Manager <HardwareInterfaceManager>
{
    public bool usingDPadAxes;
    public bool usingRightStick;
    public Resolution fullscreenRes;
    public VirtualButton Left;
    public VirtualButton Right;
    public VirtualButton Up;
    public VirtualButton Down;
    public VirtualButton Menu;
    public VirtualButton Dodge;
    public VirtualButton Confirm;
    public VirtualButton Cancel;
    public VirtualButton Fire1;
    public VirtualButton Fire2;
    public VirtualButton QuickTaboo;
    public VirtualStick RightStick;
    public WindowedResolutionMultiplier windowedRes = WindowedResolutionMultiplier.x4;
    private bool dPadAxesDigital = true;
    private ControlPrefs controlPrefs;
    private KeyCode K_HorizLeft;
    private KeyCode K_HorizRight;
    private KeyCode K_VertUp;
    private KeyCode K_VertDown;
    private KeyCode K_Menu;
    private KeyCode K_Dodge;
    private KeyCode K_Confirm;
    private KeyCode K_Cancel;
    private KeyCode K_Fire1;
    private KeyCode K_Fire2;
    private KeyCode K_QuickTaboo;
    private string dPadXAxisName;
    private string dPadYAxisName;
    private string rightStickXAxisName;
    private string rightStickYAxisName;

    void Awake ()
    {
        controlPrefs = new ControlPrefs(true);
        fullscreenRes = GetRecommendedFullscreenResolution();
        UpdateControlMap(controlPrefs);
        RefreshVirtualButtons();
    }

    void Update ()
    {
        Left.Update();
        Right.Update();
        Up.Update();
        Down.Update();
        Confirm.Update();
        Cancel.Update();
        Menu.Update();
        Fire1.Update();
        Fire2.Update();
        Dodge.Update();
        QuickTaboo.Update();
        RightStick.Update();
    }

    public Resolution GetRecommendedFullscreenResolution ()
    {
        Resolution scrRes = Screen.currentResolution;
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].height % HammerConstants.LogicalResolution_Vertical == 0)
            {
                scrRes = Screen.resolutions[i];
            }
        }
        return scrRes;
    }

    void RefreshVirtualButtons ()
    {
        if (usingDPadAxes == true)
        {
            Left = new VirtualButton(dPadXAxisName, true);
            Right = new VirtualButton(dPadXAxisName, false);
            Up = new VirtualButton(dPadYAxisName, false);
            Down = new VirtualButton(dPadYAxisName, true);
        }
        else
        {
            Left = new VirtualButton(K_HorizLeft);
            Right = new VirtualButton(K_HorizRight);
            Up = new VirtualButton(K_VertUp);
            Down = new VirtualButton(K_VertDown);
        }
        Menu = new VirtualButton(K_Menu);
        if (controlPrefs.GamepadDodgeIsMappedToAxis == true && controlPrefs.setControlMode == ControlModeType.Gamepad || (controlPrefs.setControlMode == ControlModeType.Gamepad_Mouse_Hybrid && controlPrefs.inHybridDodgeIsOnMouse == false))
        {
            Dodge = new VirtualButton(controlPrefs.GamepadDodgeAxis, false);
        }
        else
        {
            Dodge = new VirtualButton(K_Dodge);
        }
        Confirm = new VirtualButton(K_Confirm);
        Cancel = new VirtualButton(K_Cancel);
        if (controlPrefs.GamepadFire1IsMappedToAxis == true && controlPrefs.setControlMode == ControlModeType.Gamepad || (controlPrefs.setControlMode == ControlModeType.Gamepad_Mouse_Hybrid && controlPrefs.inHybridFire1IsOnMouse == false))
        {
            Fire1 = new VirtualButton(controlPrefs.GamepadFire1Axis, false);
        }
        else
        {
            Fire1 = new VirtualButton(K_Fire1);
        }
        if (controlPrefs.GamepadFire2IsMappedToAxis == true && controlPrefs.setControlMode == ControlModeType.Gamepad || (controlPrefs.setControlMode == ControlModeType.Gamepad_Mouse_Hybrid && controlPrefs.inHybridFire2IsOnMouse == false))
        {
            Fire2 = new VirtualButton(controlPrefs.GamepadFire2Axis, false);
        }
        else
        {
            Fire2 = new VirtualButton(K_Fire2);
        }
        if (controlPrefs.GamepadQuickTabooIsMappedToAxis == true && controlPrefs.setControlMode == ControlModeType.Gamepad || (controlPrefs.setControlMode == ControlModeType.Gamepad_Mouse_Hybrid && controlPrefs.inHybridQuickTabooIsOnMouse == false))
        {
            QuickTaboo = new VirtualButton(controlPrefs.GamepadQuickTabooAxis, false);
        }
        else
        {
            QuickTaboo = new VirtualButton(K_QuickTaboo);
        }
        RightStick = new VirtualStick(rightStickXAxisName, rightStickYAxisName, (controlPrefs.GamepadAimXAxisDeadZone + controlPrefs.GamepadAimYAxisDeadZone) / 2f, (controlPrefs.GamepadAimXAxisSensitivity + controlPrefs.GamepadAimYAxisSensitivity) / 2f, controlPrefs.GamepadAimXAxisInverted, controlPrefs.GamepadAimYAxisInverted);
    }

    public void RefreshFullscreenRes(Resolution newRes)
    {
        fullscreenRes = newRes;
        Screen.SetResolution(fullscreenRes.width, fullscreenRes.height, true, 60);
    }

    public void RefreshWindow()
    {
        Screen.SetResolution(HammerConstants.LogicalResolution_Horizontal * (int)(windowedRes + 1), HammerConstants.LogicalResolution_Vertical * (int)(windowedRes + 1), false, 60);
    }

    public void ToggleFullScreen()
    {
        if (Screen.fullScreen == false)
        {
            Screen.SetResolution(fullscreenRes.width, fullscreenRes.height, true, 60);
        }
        else
        {
            RefreshWindow();
        }
    }

    public void UpdateControlMap(ControlPrefs _controlPrefs)
    {
        controlPrefs = _controlPrefs;
        rightStickXAxisName = controlPrefs.GamepadAimXAxis;
        rightStickYAxisName = controlPrefs.GamepadAimYAxis;
        if (controlPrefs.setControlMode == ControlModeType.Gamepad || controlPrefs.setControlMode == ControlModeType.Gamepad_Mouse_Hybrid)
        {
            if (controlPrefs.GamepadDPadIsMappedToAxis == true)
            {
                usingDPadAxes = true;
                dPadXAxisName = controlPrefs.GamepadDPadXAxis;
                dPadYAxisName = controlPrefs.GamepadDPadYAxis;
            }
            else
            {
                K_VertUp = controlPrefs.GamepadUp;
                K_VertDown = controlPrefs.GamepadDown;
                K_HorizLeft = controlPrefs.GamepadLeft;
                K_HorizRight = controlPrefs.GamepadRight;
            }
            if (controlPrefs.setControlMode != ControlModeType.Gamepad_Mouse_Hybrid)
            {
                usingRightStick = true;
            }
            if (controlPrefs.inHybridFire1IsOnMouse == false || controlPrefs.setControlMode == ControlModeType.Gamepad)
            {
                K_Fire1 = controlPrefs.GamepadFire1;
            }
            if (controlPrefs.inHybridFire2IsOnMouse == false || controlPrefs.setControlMode == ControlModeType.Gamepad)
            {
                K_Fire2 = controlPrefs.GamepadFire2;
            }
            if (controlPrefs.inHybridDodgeIsOnMouse == false || controlPrefs.setControlMode == ControlModeType.Gamepad)
            {
                K_Dodge = controlPrefs.GamepadDodge;
            }
            if (controlPrefs.inHybridQuickTabooIsOnMouse == false || controlPrefs.setControlMode == ControlModeType.Gamepad)
            {
                K_QuickTaboo = controlPrefs.GamepadQuickTaboo;
            }
            if (controlPrefs.inHybridMenuIsOnMouse == false || controlPrefs.setControlMode == ControlModeType.Gamepad)
            {
                K_Menu = controlPrefs.GamepadMenu;
            }
            if (controlPrefs.inHybridConfirmIsOnMouse == false || controlPrefs.setControlMode == ControlModeType.Gamepad)
            {
                K_Confirm = controlPrefs.GamepadConfirm;
            }
            if (controlPrefs.inHybridCancelIsOnMouse == false || controlPrefs.setControlMode == ControlModeType.Gamepad)
            {
                K_Cancel = controlPrefs.GamepadCancel;
            }
        }
        else if (controlPrefs.setControlMode == ControlModeType.Mouse_Keyboard || controlPrefs.setControlMode == ControlModeType.Gamepad_Mouse_Hybrid)
        {
            K_VertDown = controlPrefs.KBMDown;
            K_VertUp = controlPrefs.KBMUp;
            K_HorizLeft = controlPrefs.KBMLeft;
            K_HorizRight = controlPrefs.KBMRight;
            usingDPadAxes = false;
            usingRightStick = false;
            if (controlPrefs.inHybridFire1IsOnMouse == true || controlPrefs.setControlMode == ControlModeType.Mouse_Keyboard)
            {
                K_Fire1 = controlPrefs.KBMFire1;
            }
            if (controlPrefs.inHybridFire2IsOnMouse == true || controlPrefs.setControlMode == ControlModeType.Mouse_Keyboard)
            {
                K_Fire2 = controlPrefs.KBMFire2;
            }
            if (controlPrefs.inHybridDodgeIsOnMouse == true || controlPrefs.setControlMode == ControlModeType.Mouse_Keyboard)
            {
                K_Dodge = controlPrefs.KBDodge;
            }
            if (controlPrefs.inHybridQuickTabooIsOnMouse == true || controlPrefs.setControlMode == ControlModeType.Mouse_Keyboard)
            {
                K_QuickTaboo = controlPrefs.KBMQuickTaboo;
            }
            if (controlPrefs.inHybridMenuIsOnMouse == true || controlPrefs.setControlMode == ControlModeType.Mouse_Keyboard)
            {
                K_Menu = controlPrefs.KBMMenu;
            }
            if (controlPrefs.inHybridConfirmIsOnMouse == true || controlPrefs.setControlMode == ControlModeType.Mouse_Keyboard)
            {
                K_Confirm = controlPrefs.KBMConfirm;
            }
            if (controlPrefs.inHybridCancelIsOnMouse == true || controlPrefs.setControlMode == ControlModeType.Mouse_Keyboard)
            {
                K_Cancel = controlPrefs.KBMCancel;
            }
        }
        else
        {
            throw new System.Exception("Bad control mode type: " + controlPrefs.setControlMode.ToString());
        }
    }

}
