using UnityEngine;
using System;
using System.Collections;

public class ControlConfigMenu : MonoBehaviour
{
    public bool open;
    private ControlPrefs oldControlPrefs;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    void Close (bool revert = false)
    {
        if (revert == true)
        {
            PlayerSettingsManager.Instance.controlPrefs = oldControlPrefs;
        }
        open = false;
    }

    public void Open ()
    {
        open = true;
        oldControlPrefs = PlayerSettingsManager.Instance.controlPrefs;
    }

    void CheckAxes ()
    {
        throw new NotImplementedException();
    }

    bool CheckKeys (ControlInputType control, bool isGamepadButton, int gamepadIndex)
    {
        throw new NotImplementedException();
    }

    bool RegisterInput (ControlInputType control, KeyCode keyCode, bool isGamepadButton, bool isAxis, int gamepadIndex, string axisName = "")
    {
        throw new NotImplementedException();
    }
}
