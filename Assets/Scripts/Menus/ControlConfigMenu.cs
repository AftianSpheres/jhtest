using UnityEngine;
using System;
using System.Collections;

enum ControlInputType
{
    None,
    Up,
    Down,
    Left,
    Right,
    Confirm,
    Cancel,
    Dodge,
    Menu,
    Fire1,
    Fire2,
    QuickTaboo
}

public class ControlConfigMenu : MonoBehaviour
{
    private HardwareInterfaceManager hwInterfaceManager;
    private PlayerSettingsManager playerSettingsManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if we hit confirm on a button, we wait for input
        // while waiting for input: CheckAxes() checkKeys() every frame
	}

    void CheckAxes ()
    {

    }

    bool CheckKeys (ControlInputType control, bool isGamepadButton, bool isAxis, int gamepadIndex)
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
        if (isGamepadButton == true && (playerSettingsManager.setControlMode == ControlMode.Gamepad || playerSettingsManager.setControlMode == ControlMode.Gamepad_Mouse_Hybrid))
        {
            if (isAxis == true)
            {
                switch (control)
                {
                    case ControlInputType.Up:
                        playerSettingsManager.controlPrefs_GamepadDPadYAxis = axisName;
                        playerSettingsManager.controlPrefs_GamepadDPadIsMappedToAxis = true;
                        if (Input.GetAxis(axisName) < 0)
                        {
                            playerSettingsManager.controlPrefs_GamepadDPadYAxisInverted = true;
                        }
                        else
                        {
                            playerSettingsManager.controlPrefs_GamepadDPadYAxisInverted = false;
                        }
                        break;
                    case ControlInputType.Down:
                        playerSettingsManager.controlPrefs_GamepadDPadYAxis = axisName;
                        playerSettingsManager.controlPrefs_GamepadDPadIsMappedToAxis = true;
                        if (Input.GetAxis(axisName) > 0)
                        {
                            playerSettingsManager.controlPrefs_GamepadDPadYAxisInverted = true;
                        }
                        else
                        {
                            playerSettingsManager.controlPrefs_GamepadDPadYAxisInverted = false;
                        }
                        break;
                    case ControlInputType.Left:
                        playerSettingsManager.controlPrefs_GamepadDPadXAxis = axisName;
                        playerSettingsManager.controlPrefs_GamepadDPadIsMappedToAxis = true;
                        if (Input.GetAxis(axisName) > 0)
                        {
                            playerSettingsManager.controlPrefs_GamepadDPadXAxisInverted = true;
                        }
                        else
                        {
                            playerSettingsManager.controlPrefs_GamepadDPadXAxisInverted = false;
                        }
                        break;
                    case ControlInputType.Right:
                        playerSettingsManager.controlPrefs_GamepadDPadXAxis = axisName;
                        playerSettingsManager.controlPrefs_GamepadDPadIsMappedToAxis = true;
                        if (Input.GetAxis(axisName) < 0)
                        {
                            playerSettingsManager.controlPrefs_GamepadDPadXAxisInverted = true;
                        }
                        else
                        {
                            playerSettingsManager.controlPrefs_GamepadDPadXAxisInverted = false;
                        }
                        break;
                    case ControlInputType.Fire1:
                        playerSettingsManager.controlPrefs_GamepadFire1Axis = axisName;
                        playerSettingsManager.controlPrefs_GamepadFire1IsMappedToAxis = true;
                        break;
                    case ControlInputType.Fire2:
                        playerSettingsManager.controlPrefs_GamepadFire2Axis = axisName;
                        playerSettingsManager.controlPrefs_GamepadFire2IsMappedToAxis = true;
                        break;
                    case ControlInputType.Dodge:
                        playerSettingsManager.controlPrefs_GamepadDodgeAxis = axisName;
                        playerSettingsManager.controlPrefs_GamepadDodgeIsMappedToAxis = true;
                        break;
                    case ControlInputType.QuickTaboo:
                        playerSettingsManager.controlPrefs_GamepadQuickTabooAxis = axisName;
                        playerSettingsManager.controlPrefs_GamepadQuickTabooIsMappedToAxis = true;
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
                        playerSettingsManager.controlPrefs_GamepadUp = keyCode;
                        playerSettingsManager.controlPrefs_GamepadDPadIsMappedToAxis = false;
                        break;
                    case ControlInputType.Down:
                        playerSettingsManager.controlPrefs_GamepadDown = keyCode;
                        playerSettingsManager.controlPrefs_GamepadDPadIsMappedToAxis = false;
                        break;
                    case ControlInputType.Left:
                        playerSettingsManager.controlPrefs_GamepadLeft = keyCode;
                        playerSettingsManager.controlPrefs_GamepadDPadIsMappedToAxis = false;
                        break;
                    case ControlInputType.Right:
                        playerSettingsManager.controlPrefs_GamepadRight = keyCode;
                        playerSettingsManager.controlPrefs_GamepadDPadIsMappedToAxis = false;
                        break;
                    case ControlInputType.Confirm:
                        playerSettingsManager.controlPrefs_GamepadConfirm = keyCode;
                        break;
                    case ControlInputType.Cancel:
                        playerSettingsManager.controlPrefs_GamepadCancel = keyCode;
                        break;
                    case ControlInputType.Dodge:
                        playerSettingsManager.controlPrefs_GamepadDodge = keyCode;
                        playerSettingsManager.controlPrefs_GamepadDodgeIsMappedToAxis = false;
                        break;
                    case ControlInputType.Menu:
                        playerSettingsManager.controlPrefs_GamepadMenu = keyCode;
                        break;
                    case ControlInputType.Fire1:
                        playerSettingsManager.controlPrefs_GamepadFire1 = keyCode;
                        playerSettingsManager.controlPrefs_GamepadFire1IsMappedToAxis = false;
                        break;
                    case ControlInputType.Fire2:
                        playerSettingsManager.controlPrefs_GamepadFire2 = keyCode;
                        playerSettingsManager.controlPrefs_GamepadFire2IsMappedToAxis = false;
                        break;
                    case ControlInputType.QuickTaboo:
                        playerSettingsManager.controlPrefs_GamepadQuickTaboo = keyCode;
                        playerSettingsManager.controlPrefs_GamepadQuickTabooIsMappedToAxis = false;
                        break;
                    default:
                        return false;
                }
            }
            return true;
        }
        else if (isGamepadButton == false && (playerSettingsManager.setControlMode == ControlMode.Mouse_Keyboard || playerSettingsManager.setControlMode == ControlMode.Gamepad_Mouse_Hybrid))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
