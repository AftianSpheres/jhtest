using UnityEngine;
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
                if (isGamepadButton == true && i  > (int)KeyCode.JoystickButton19 && i <= (int)KeyCode.Joystick8Button19)
                {
                    if ((gamepadIndex == 0 && i > (int)KeyCode.Joystick1Button19) ||
                        (gamepadIndex == 1 && (i < (int)KeyCode.Joystick2Button1 || i > (int)KeyCode.Joystick2Button19)) ||
                        (gamepadIndex == 2 && (i < (int)KeyCode.Joystick3Button1 || i > (int)KeyCode.Joystick3Button19)) ||
                        (gamepadIndex == 3 && (i < (int)KeyCode.Joystick4Button1 || i > (int)KeyCode.Joystick4Button19)) ||
                        (gamepadIndex == 4 && (i < (int)KeyCode.Joystick5Button1 || i > (int)KeyCode.Joystick5Button19)) ||
                        (gamepadIndex == 5 && (i < (int)KeyCode.Joystick6Button1 || i > (int)KeyCode.Joystick6Button19)) ||
                        (gamepadIndex == 6 && (i < (int)KeyCode.Joystick7Button1 || i > (int)KeyCode.Joystick7Button19)) ||
                        (gamepadIndex == 7 && i < (int)KeyCode.Joystick8Button1) ||
                        gamepadIndex < 0 || gamepadIndex > 7)
                    {
                        return false;
                    }
                    else
                    {
                        return RegisterInput(control, (KeyCode)i, true, false, gamepadIndex);
                    }
                }
                else if (isGamepadButton == false)
                {
                    if (i >= (int)KeyCode.JoystickButton1 && i <= (int)KeyCode.Joystick8Button19)
                    {
                        return false;
                    }
                    else
                    {
                        return RegisterInput(control, (KeyCode)i, false, false, gamepadIndex);
                    }
                }
            }
        }
        return false;
    }

    bool RegisterInput (ControlInputType control, KeyCode keyCode, bool isGamepadButton, bool isAxis, int gamepadIndex, string axisName = "")
    {
        throw new NotImplementedException();
    }
}
