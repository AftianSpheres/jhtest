﻿using UnityEngine;
using System;
using System.Collections;

public class ControlConfigMenu : MonoBehaviour
{
    public bool open;
    private bool waitingForInput;
    private ControlInputType waitingInputType;
    private HardwareInterfaceManager hwInterfaceManager;
    private PlayerSettingsManager playerSettingsManager;
    private ControlPrefs oldControlPrefs;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (waitingForInput == true)
        {
            CheckKeys(waitingInputType, (playerSettingsManager.controlPrefs.setControlMode == ControlModeType.Gamepad || playerSettingsManager.controlPrefs.setControlMode == ControlModeType.Gamepad_Mouse_Hybrid), 0);
        }
	}

    void Close (bool revert = false)
    {
        if (revert == true)
        {
            playerSettingsManager.controlPrefs = oldControlPrefs;
        }
        open = false;
    }

    public void Open ()
    {
        open = true;
        oldControlPrefs = playerSettingsManager.controlPrefs;
    }

    void CheckAxes ()
    {

    }

    bool CheckKeys (ControlInputType control, bool isGamepadButton, int gamepadIndex)
    {
        Array keyCodes = Enum.GetValues(typeof(KeyCode));
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown((KeyCode)i) == true)
            {
                return RegisterInput(ControlInputType.None, (KeyCode)i, true, false, 0);
            }
        }
        return false;
    }

    bool RegisterInput (ControlInputType control, KeyCode keyCode, bool isGamepadButton, bool isAxis, int gamepadIndex, string axisName = "")
    {
        if (isGamepadButton == true && (playerSettingsManager.controlPrefs.setControlMode == ControlModeType.Gamepad || playerSettingsManager.controlPrefs.setControlMode == ControlModeType.Gamepad_Mouse_Hybrid))
        {
            if (isAxis == true)
            {
                switch (control)
                {
                    case ControlInputType.Up:
                        playerSettingsManager.controlPrefs.GamepadDPadYAxis = axisName;
                        playerSettingsManager.controlPrefs.GamepadDPadIsMappedToAxis = true;
                        if (Input.GetAxis(axisName) < 0)
                        {
                            playerSettingsManager.controlPrefs.GamepadDPadYAxisInverted = true;
                        }
                        else
                        {
                            playerSettingsManager.controlPrefs.GamepadDPadYAxisInverted = false;
                        }
                        break;
                    case ControlInputType.Down:
                        playerSettingsManager.controlPrefs.GamepadDPadYAxis = axisName;
                        playerSettingsManager.controlPrefs.GamepadDPadIsMappedToAxis = true;
                        if (Input.GetAxis(axisName) > 0)
                        {
                            playerSettingsManager.controlPrefs.GamepadDPadYAxisInverted = true;
                        }
                        else
                        {
                            playerSettingsManager.controlPrefs.GamepadDPadYAxisInverted = false;
                        }
                        break;
                    case ControlInputType.Left:
                        playerSettingsManager.controlPrefs.GamepadDPadXAxis = axisName;
                        playerSettingsManager.controlPrefs.GamepadDPadIsMappedToAxis = true;
                        if (Input.GetAxis(axisName) > 0)
                        {
                            playerSettingsManager.controlPrefs.GamepadDPadXAxisInverted = true;
                        }
                        else
                        {
                            playerSettingsManager.controlPrefs.GamepadDPadXAxisInverted = false;
                        }
                        break;
                    case ControlInputType.Right:
                        playerSettingsManager.controlPrefs.GamepadDPadXAxis = axisName;
                        playerSettingsManager.controlPrefs.GamepadDPadIsMappedToAxis = true;
                        if (Input.GetAxis(axisName) < 0)
                        {
                            playerSettingsManager.controlPrefs.GamepadDPadXAxisInverted = true;
                        }
                        else
                        {
                            playerSettingsManager.controlPrefs.GamepadDPadXAxisInverted = false;
                        }
                        break;
                    case ControlInputType.Fire1:
                        playerSettingsManager.controlPrefs.GamepadFire1Axis = axisName;
                        playerSettingsManager.controlPrefs.GamepadFire1IsMappedToAxis = true;
                        break;
                    case ControlInputType.Fire2:
                        playerSettingsManager.controlPrefs.GamepadFire2Axis = axisName;
                        playerSettingsManager.controlPrefs.GamepadFire2IsMappedToAxis = true;
                        break;
                    case ControlInputType.Dodge:
                        playerSettingsManager.controlPrefs.GamepadDodgeAxis = axisName;
                        playerSettingsManager.controlPrefs.GamepadDodgeIsMappedToAxis = true;
                        break;
                    case ControlInputType.QuickTaboo:
                        playerSettingsManager.controlPrefs.GamepadQuickTabooAxis = axisName;
                        playerSettingsManager.controlPrefs.GamepadQuickTabooIsMappedToAxis = true;
                        break;
                    default:
                        return false;
                }
            }
            else
            {
                switch (control)
                {
                    case ControlInputType.Up:
                        playerSettingsManager.controlPrefs.GamepadUp = keyCode;
                        playerSettingsManager.controlPrefs.GamepadDPadIsMappedToAxis = false;
                        break;
                    case ControlInputType.Down:
                        playerSettingsManager.controlPrefs.GamepadDown = keyCode;
                        playerSettingsManager.controlPrefs.GamepadDPadIsMappedToAxis = false;
                        break;
                    case ControlInputType.Left:
                        playerSettingsManager.controlPrefs.GamepadLeft = keyCode;
                        playerSettingsManager.controlPrefs.GamepadDPadIsMappedToAxis = false;
                        break;
                    case ControlInputType.Right:
                        playerSettingsManager.controlPrefs.GamepadRight = keyCode;
                        playerSettingsManager.controlPrefs.GamepadDPadIsMappedToAxis = false;
                        break;
                    case ControlInputType.Confirm:
                        playerSettingsManager.controlPrefs.GamepadConfirm = keyCode;
                        break;
                    case ControlInputType.Cancel:
                        playerSettingsManager.controlPrefs.GamepadCancel = keyCode;
                        break;
                    case ControlInputType.Dodge:
                        playerSettingsManager.controlPrefs.GamepadDodge = keyCode;
                        playerSettingsManager.controlPrefs.GamepadDodgeIsMappedToAxis = false;
                        break;
                    case ControlInputType.Menu:
                        playerSettingsManager.controlPrefs.GamepadMenu = keyCode;
                        break;
                    case ControlInputType.Fire1:
                        playerSettingsManager.controlPrefs.GamepadFire1 = keyCode;
                        playerSettingsManager.controlPrefs.GamepadFire1IsMappedToAxis = false;
                        break;
                    case ControlInputType.Fire2:
                        playerSettingsManager.controlPrefs.GamepadFire2 = keyCode;
                        playerSettingsManager.controlPrefs.GamepadFire2IsMappedToAxis = false;
                        break;
                    case ControlInputType.QuickTaboo:
                        playerSettingsManager.controlPrefs.GamepadQuickTaboo = keyCode;
                        playerSettingsManager.controlPrefs.GamepadQuickTabooIsMappedToAxis = false;
                        break;
                    default:
                        return false;
                }
            }
            return true;
        }
        else if (isGamepadButton == false && (playerSettingsManager.controlPrefs.setControlMode == ControlModeType.Mouse_Keyboard || playerSettingsManager.controlPrefs.setControlMode == ControlModeType.Gamepad_Mouse_Hybrid))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
