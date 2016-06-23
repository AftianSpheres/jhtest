using UnityEngine;
using System.Collections;
using InControl;

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
    private ControlPrefs controlPrefs;

    public bool usingRightStick
    {
        get
        {
            return !(controlPrefs.setControlMode == ControlModeType.Mouse_Keyboard);
        }
    }

    void Awake ()
    {
        controlPrefs = new ControlPrefs(ControlModeType.Gamepad);
        fullscreenRes = GetRecommendedFullscreenResolution();
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
        if (controlPrefs.setControlMode == ControlModeType.Gamepad)
        {
            RightStick.Update();
        }     
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

        if (controlPrefs.setControlMode == ControlModeType.Gamepad)
        {
            InputDevice device = InputManager.ActiveDevice;
            Left = new VirtualButton(controlPrefs.GamepadLeft);
            Right = new VirtualButton(controlPrefs.GamepadRight);
            Up = new VirtualButton(controlPrefs.GamepadUp);
            Down = new VirtualButton(controlPrefs.GamepadDown);
            Confirm = new VirtualButton(controlPrefs.GamepadConfirm);
            Cancel = new VirtualButton(controlPrefs.GamepadCancel);
            Menu = new VirtualButton(controlPrefs.GamepadMenu);
            Fire1 = new VirtualButton(controlPrefs.GamepadFire1);
            Fire2 = new VirtualButton(controlPrefs.GamepadFire2);
            Dodge = new VirtualButton(controlPrefs.GamepadDodge);
            QuickTaboo = new VirtualButton(controlPrefs.GamepadQuickTaboo);
            RightStick = new VirtualStick(controlPrefs.GamepadAimXAxis, controlPrefs.GamepadAimYAxis, 
            controlPrefs.GamepadAimDeadZone, controlPrefs.GamepadAimSensitivity, controlPrefs.GamepadAimXAxisInverted, controlPrefs.GamepadAimYAxisInverted);
        }
        else // hybrid isn't implemented atm
        {
            Left = new VirtualButton(controlPrefs.KBMLeft);
            Right = new VirtualButton(controlPrefs.KBMRight);
            Up = new VirtualButton(controlPrefs.KBMUp);
            Down = new VirtualButton(controlPrefs.KBMDown);
            Confirm = new VirtualButton(controlPrefs.KBMConfirm);
            Cancel = new VirtualButton(controlPrefs.KBMCancel);
            Menu = new VirtualButton(controlPrefs.KBMMenu);
            Fire1 = new VirtualButton(controlPrefs.KBMFire1);
            Fire2 = new VirtualButton(controlPrefs.KBMFire2);
            Dodge = new VirtualButton(controlPrefs.KBDodge);
            QuickTaboo = new VirtualButton(controlPrefs.KBMQuickTaboo);

        }
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

}
