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
    public WindowedResolutionMultiplier resMulti
    {
        get
        {
            if (Screen.fullScreen) return fsMulti;
            else return windowedRes;
        }
    }
    private VirtualButton _left;
    private VirtualButton _right;
    private VirtualButton _up;
    private VirtualButton _down;
    public VirtualButton Menu;
    public VirtualButton Dodge;
    public VirtualButton Confirm;
    public VirtualButton Cancel;
    public VirtualButton Fire1;
    public VirtualButton Fire2;
    public VirtualButton LeftBumper;
    public VirtualButton RightBumper;
    public VirtualButton QuickTaboo;
    public VirtualButton FlipMeter;
    public VirtualStick LeftStick;
    public VirtualStick RightStick;
    public VirtualButtonMultiplexer Left;
    public VirtualButtonMultiplexer Right;
    public VirtualButtonMultiplexer Up;
    public VirtualButtonMultiplexer Down;
    private WindowedResolutionMultiplier fsMulti = WindowedResolutionMultiplier.x1;
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
        _left.Update();
        _right.Update();
        _up.Update();
        _down.Update();
        Confirm.Update();
        Cancel.Update();
        Menu.Update();
        Fire1.Update();
        Fire2.Update();
        Dodge.Update();
        QuickTaboo.Update();
        FlipMeter.Update();
        if (controlPrefs.setControlMode == ControlModeType.Gamepad)
        {
            LeftStick.Update();
            RightStick.Update();
        }
        Left.Update();
        Right.Update();
        Up.Update();
        Down.Update();
#if UNITY_EDITOR
        if (Application.isEditor && Application.isPlaying)
        {
            windowedRes = (WindowedResolutionMultiplier) ((Screen.width / 160) - 1);
        }
#endif
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
            _left = new VirtualButton(controlPrefs.GamepadLeft);
            _right = new VirtualButton(controlPrefs.GamepadRight);
            _up = new VirtualButton(controlPrefs.GamepadUp);
            _down = new VirtualButton(controlPrefs.GamepadDown);
            Confirm = new VirtualButton(controlPrefs.GamepadConfirm);
            Cancel = new VirtualButton(controlPrefs.GamepadCancel);
            Menu = new VirtualButton(controlPrefs.GamepadMenu);
            Fire1 = RightBumper = new VirtualButton(controlPrefs.GamepadFire1);
            Fire2 = new VirtualButton(controlPrefs.GamepadFire2);
            Dodge = LeftBumper = new VirtualButton(controlPrefs.GamepadDodge);
            QuickTaboo = new VirtualButton(controlPrefs.GamepadQuickTaboo);
            FlipMeter = new VirtualButton(controlPrefs.GamepadFlipMeter);
            LeftStick = new VirtualStick(controlPrefs.GamepadDPadXAxis, controlPrefs.GamepadDPadYAxis, 
            controlPrefs.GamepadDPadDeadZone, 1.0f, controlPrefs.GamepadDPadXAxisInverted, controlPrefs.GamepadDPadYAxisInverted);
            RightStick = new VirtualStick(controlPrefs.GamepadAimXAxis, controlPrefs.GamepadAimYAxis, 
            controlPrefs.GamepadAimDeadZone, controlPrefs.GamepadAimSensitivity, controlPrefs.GamepadAimXAxisInverted, controlPrefs.GamepadAimYAxisInverted);
            Left = new VirtualButtonMultiplexer(_left, LeftStick, false, true);
            Right = new VirtualButtonMultiplexer(_right, LeftStick, false, false);
            Up = new VirtualButtonMultiplexer(_up, LeftStick, true, false);
            Down = new VirtualButtonMultiplexer(_down, LeftStick, true, true);
        }
        else // hybrid isn't implemented atm
        {
            _left = new VirtualButton(controlPrefs.KBMLeft);
            _right = new VirtualButton(controlPrefs.KBMRight);
            _up = new VirtualButton(controlPrefs.KBMUp);
            _down = new VirtualButton(controlPrefs.KBMDown);
            Confirm = new VirtualButton(controlPrefs.KBMConfirm);
            Cancel = new VirtualButton(controlPrefs.KBMCancel);
            Menu = new VirtualButton(controlPrefs.KBMMenu);
            Fire1 = new VirtualButton(controlPrefs.KBMFire1);
            Fire2 = new VirtualButton(controlPrefs.KBMFire2);
            Dodge = new VirtualButton(controlPrefs.KBMDodge);
            QuickTaboo = new VirtualButton(controlPrefs.KBMQuickTaboo);
            FlipMeter = new VirtualButton(controlPrefs.KBMFlipMeter);
            LeftBumper = new VirtualButton(controlPrefs.KBMBumpLeft);
            RightBumper = new VirtualButton(controlPrefs.KBMBumpRight);
            Left = new VirtualButtonMultiplexer(_left);
            Right = new VirtualButtonMultiplexer(_right);
            Up = new VirtualButtonMultiplexer(_up);
            Down = new VirtualButtonMultiplexer(_down);
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
