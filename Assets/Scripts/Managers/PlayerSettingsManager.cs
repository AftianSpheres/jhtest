using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class PlayerSettingsManager : Manager<PlayerSettingsManager>
{
    public static float VolumeMin = 0.0f;
    public static float VolumeMax = 1.0f;
    public float MusicVolume = 1f;
    public float SFXVolume = 1f;
    public ControlPrefs controlPrefs;
    private HardwareInterfaceManager hardwareInterfaceManager;

	// Use this for initialization
	void Awake ()
    {
        controlPrefs = new ControlPrefs(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
