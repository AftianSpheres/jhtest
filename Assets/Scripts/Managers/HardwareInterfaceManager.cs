﻿using UnityEngine;
using System.Collections;

public enum WindowedResolutionMultiplier
{
    x1, // 160x144
    x2, // 320x288
    x3, // 480x432
    x4, // 640x572
    x5, // 800x716
    x6, // 960x860
    x8, // 1280x1148
}

public class HardwareInterfaceManager : Manager <HardwareInterfaceManager>
{
    public static string[] LeftStickXes =
    {
        "joyL1x"
    };
    public static string[] LeftStickYs =
    {
        "joyL1y"
    };
    public static string[] RightStickXes =
    {
        "joyR1x"
    };
    public static string[] RightStickYs =
    {
        "joyR1y"
    };
    public static string[] DPadXes =
    {
        "joyD1x"
    };
    public static string[] DPadYs =
    {
        "joyD1y"
    };
    public static string[] LeftTriggers =
    {
        "joy1tL"
    };
    public static string[] RightTriggers =
    {
        "joy1tR"
    };
    private KeyCode K_HorizLeft = KeyCode.A;
    private KeyCode K_HorizRight = KeyCode.D;
    private KeyCode K_VertUp = KeyCode.W;
    private KeyCode K_VertDown = KeyCode.S;
    private KeyCode K_Menu = KeyCode.Joystick1Button7;
    private KeyCode K_Dodge = KeyCode.Space;
    private KeyCode K_Confirm = KeyCode.Joystick1Button0;
    private KeyCode K_Cancel = KeyCode.Joystick1Button1;
    private KeyCode K_Fire1 = KeyCode.Joystick1Button5;
    private KeyCode K_Fire2 = KeyCode.Mouse1;
    public bool DisplayFullscreen = false;
    public bool usingDPadAxes = true;
    public bool usingLeftStick = false;
    public bool usingRightStick = true;
    public bool leftTriggerDodge = true;
    public bool rightTriggerFire2 = true;
    public int GamepadIndex = 0;
    public WindowedResolutionMultiplier WindowedRes = WindowedResolutionMultiplier.x2;
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

    void Awake ()
    {
        RefreshVirtualButtons();      
    }

    void RefreshVirtualButtons ()
    {
        if (usingDPadAxes == true)
        {
            Left = new VirtualButton(DPadXes[GamepadIndex], true);
            Right = new VirtualButton(DPadXes[GamepadIndex], false);
            Up = new VirtualButton(DPadYs[GamepadIndex], false);
            Down = new VirtualButton(DPadYs[GamepadIndex], true);
        }
        else if (usingLeftStick == true)
        {
            Left = new VirtualButton(LeftStickXes[GamepadIndex], true);
            Right = new VirtualButton(LeftStickXes[GamepadIndex], false);
            Up = new VirtualButton(LeftStickYs[GamepadIndex], false);
            Down = new VirtualButton(LeftStickYs[GamepadIndex], true);
        }
        else
        {
            Left = new VirtualButton(K_HorizLeft);
            Right = new VirtualButton(K_HorizRight);
            Up = new VirtualButton(K_VertUp);
            Down = new VirtualButton(K_VertDown);
        }
        Menu = new VirtualButton(K_Menu);
        if (leftTriggerDodge == true)
        {
            Dodge = new VirtualButton(LeftTriggers[GamepadIndex], false);
        }
        else
        {
            Dodge = new VirtualButton(K_Dodge);
        }
        Confirm = new VirtualButton(K_Confirm);
        Cancel = new VirtualButton(K_Cancel);
        Fire1 = new VirtualButton(K_Fire1);
        if (rightTriggerFire2 == true)
        {
            Fire2 = new VirtualButton(RightTriggers[GamepadIndex], false);
        }
        else
        {
            Fire2 = new VirtualButton(K_Fire2);
        }
    }

}