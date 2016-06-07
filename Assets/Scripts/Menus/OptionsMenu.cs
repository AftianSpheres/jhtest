﻿using UnityEngine;
using System.Collections;

enum ConfirmCancelDefaultSelection
{
    Confirm,
    Cancel,
    Defaults
}

enum OptionsMenuSelections
{
    FullscreenWindowed,
    Resolution,
    ControlConfig,
    MusicVolume,
    SFXVolume,
    ConfirmCancelDefaults
}

public class OptionsMenu : MonoBehaviour
{
    public SpriteRenderer cursor;
    public Vector3[] mainCursorCoords;
    public AudioClip cursorDown;
    public AudioClip cursorUp;
    public AudioClip selectionOK;
    public AudioClip selectionForbidden;
    public AudioSource source;
    public bool suspended;
    public GameObject settingsConfirmationPrompt;
    private int ResolutionIndex;
    private static int screenSettingsRevertTime = 600;
    private ConfirmCancelDefaultSelection confirmCancelDefaults;
    private OptionsMenuSelections selection;
    private int screenSettingsTimer;
    private int ctr;
    private float musicVolumeBuffer;
    private float sfxVolumeBuffer;
    private bool ready;
    private bool fullscreenBoolBuffer;
    private WindowedResolutionMultiplier windowedResBuffer;
    private Resolution fullscreenResBuffer;
    private bool lastResFullscreen;
    private Resolution lastFullscreenRes;
    private HardwareInterfaceManager hardwareInterfaceManager;
    private PlayerSettingsManager playerSettingsManager;

	// Use this for initialization
	void Start ()
    {
        GameObject hwInterfaceManagerObj = GameObject.Find("Universe/HardwareInterfaceManager");
        if (hwInterfaceManagerObj != null)
        {
            _in_MateWithManagers();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (hardwareInterfaceManager == null)
        {
            _in_MateWithManagers();
        }
        else if (ready == false)
        {
            selection = OptionsMenuSelections.FullscreenWindowed;
            ctr = 0;
            musicVolumeBuffer = playerSettingsManager.MusicVolume;
            sfxVolumeBuffer = playerSettingsManager.SFXVolume;
            windowedResBuffer = hardwareInterfaceManager.WindowedRes;
            fullscreenResBuffer = hardwareInterfaceManager.fullscreenRes;
            fullscreenBoolBuffer = Screen.fullScreen;
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                if (Screen.resolutions[i].width == hardwareInterfaceManager.fullscreenRes.width && Screen.resolutions[i].height == hardwareInterfaceManager.fullscreenRes.height)
                {
                    ResolutionIndex = i;
                    break;
                }
            }
            ready = true;
        }
        else if (suspended == false)
        {
            if (ctr > 0)
            {
                ctr--;
            }
            switch (selection)
            {
                case OptionsMenuSelections.FullscreenWindowed:
                    if (Screen.fullScreen == true)
                    {
                        cursor.transform.localPosition = mainCursorCoords[0];
                    }
                    else
                    {
                        cursor.transform.localPosition = mainCursorCoords[1];
                    }

                    if (hardwareInterfaceManager.Down.Pressed == true && ctr <= 0)
                    {
                        selection = OptionsMenuSelections.Resolution;
                        ctr = 8;
                        source.PlayOneShot(cursorDown);
                    }
                    else if (hardwareInterfaceManager.Left.BtnDown == true || hardwareInterfaceManager.Right.BtnDown == true)
                    {
                        if (Screen.fullScreen == false)
                        {
                            settingsConfirmationPrompt.SetActive(true);
                            suspended = true;
                            lastResFullscreen = false;
                            screenSettingsTimer = screenSettingsRevertTime;
                        }
                        hardwareInterfaceManager.ToggleFullScreen();
                        source.PlayOneShot(selectionOK);
                    }
                    break;
                case OptionsMenuSelections.Resolution:
                    cursor.transform.localPosition = mainCursorCoords[2];
                    if (hardwareInterfaceManager.Down.Pressed == true && ctr <= 0)
                    {
                        selection = OptionsMenuSelections.ControlConfig;
                        ctr = 8;
                        source.PlayOneShot(cursorDown);
                    }
                    if (hardwareInterfaceManager.Up.Pressed == true && ctr <= 0)
                    {
                        selection = OptionsMenuSelections.FullscreenWindowed;
                        ctr = 8;
                        source.PlayOneShot(cursorUp);
                    }
                    else if (ctr <= 0)
                    {
                        _in_Resolution();
                    }
                    break;
                case OptionsMenuSelections.ControlConfig:
                    cursor.transform.localPosition = mainCursorCoords[3];
                    if (hardwareInterfaceManager.Down.Pressed == true && ctr <= 0)
                    {
                        selection = OptionsMenuSelections.MusicVolume;
                        ctr = 8;
                        source.PlayOneShot(cursorDown);
                    }
                    if (hardwareInterfaceManager.Up.Pressed == true && ctr <= 0)
                    {
                        selection = OptionsMenuSelections.Resolution;
                        ctr = 8;
                        source.PlayOneShot(cursorUp);
                    }
                    else if (hardwareInterfaceManager.Confirm.BtnDown == true)
                    {
                        source.PlayOneShot(selectionForbidden);
                    }
                    break;
                case OptionsMenuSelections.MusicVolume:
                    cursor.transform.localPosition = mainCursorCoords[4];
                    if (hardwareInterfaceManager.Down.Pressed == true && ctr <= 0)
                    {
                        selection = OptionsMenuSelections.SFXVolume;
                        ctr = 8;
                        source.PlayOneShot(cursorDown);
                    }
                    if (hardwareInterfaceManager.Up.Pressed == true && ctr <= 0)
                    {
                        selection = OptionsMenuSelections.ControlConfig;
                        ctr = 8;
                        source.PlayOneShot(cursorUp);
                    }
                    else if (ctr <= 0)
                    {
                        if (hardwareInterfaceManager.Left.BtnDown == true)
                        {
                            if (playerSettingsManager.MusicVolume > PlayerSettingsManager.VolumeMin)
                            {
                                playerSettingsManager.MusicVolume -= 0.1f;
                                source.PlayOneShot(cursorDown);
                                ctr = 8;
                            }
                            else
                            {
                                source.PlayOneShot(selectionForbidden);
                            }
                        }
                        else if (hardwareInterfaceManager.Right.BtnDown == true)
                        {
                            if (playerSettingsManager.MusicVolume < PlayerSettingsManager.VolumeMax)
                            {
                                playerSettingsManager.MusicVolume += 0.1f;
                                source.PlayOneShot(cursorUp);
                                ctr = 8;
                            }
                            else
                            {
                                source.PlayOneShot(selectionForbidden);
                            }
                        }
                    }
                    break;
                case OptionsMenuSelections.SFXVolume:
                    cursor.transform.localPosition = mainCursorCoords[5];
                    if (hardwareInterfaceManager.Down.Pressed == true && ctr <= 0)
                    {
                        selection = OptionsMenuSelections.ConfirmCancelDefaults;
                        ctr = 8;
                        source.PlayOneShot(cursorDown);
                    }
                    if (hardwareInterfaceManager.Up.Pressed == true && ctr <= 0)
                    {
                        selection = OptionsMenuSelections.MusicVolume;
                        ctr = 8;
                        source.PlayOneShot(cursorUp);
                    }
                    else if (ctr <= 0)
                    {
                        if (hardwareInterfaceManager.Left.BtnDown == true)
                        {
                            if (playerSettingsManager.SFXVolume > PlayerSettingsManager.VolumeMin)
                            {
                                playerSettingsManager.SFXVolume -= 0.1f;
                                source.PlayOneShot(cursorDown);
                                ctr = 8;
                            }
                            else
                            {
                                source.PlayOneShot(selectionForbidden);
                            }
                        }
                        else if (hardwareInterfaceManager.Right.BtnDown == true)
                        {
                            if (playerSettingsManager.SFXVolume < PlayerSettingsManager.VolumeMax)
                            {
                                playerSettingsManager.SFXVolume += 0.1f;
                                source.PlayOneShot(cursorUp);
                                ctr = 8;
                            }
                            else
                            {
                                source.PlayOneShot(selectionForbidden);
                            }
                        }
                    }
                    break;
                case OptionsMenuSelections.ConfirmCancelDefaults:
                    _in_ConfirmCancelDefaults();
                    break;
                default:
                    throw new System.Exception("Invalid options menu selection: " + selection.ToString());
            }
        }
        else if (suspended == true)
        {
            screenSettingsTimer--;
            if (hardwareInterfaceManager.Confirm.Pressed == true)
            {
                suspended = false;
                settingsConfirmationPrompt.SetActive(false);
            } 
            else if (hardwareInterfaceManager.Cancel.Pressed == true)
            {
                screenSettingsTimer = 0;
            }
            if (screenSettingsTimer <= 0)
            {
                if (lastResFullscreen == true)
                {
                    hardwareInterfaceManager.RefreshFullscreenRes(lastFullscreenRes);
                }
                else
                {
                    hardwareInterfaceManager.ToggleFullScreen();
                    hardwareInterfaceManager.fullscreenRes = lastFullscreenRes;
                }
                suspended = false;
                settingsConfirmationPrompt.SetActive(false);
            }
        }
	}

    void _in_ConfirmCancelDefaults ()
    {
        switch (confirmCancelDefaults)
        {
            case ConfirmCancelDefaultSelection.Confirm:
                cursor.transform.localPosition = mainCursorCoords[6];
                if (hardwareInterfaceManager.Right.BtnDown == true)
                {
                    confirmCancelDefaults = ConfirmCancelDefaultSelection.Cancel;
                    source.PlayOneShot(cursorUp);
                }
                else if (hardwareInterfaceManager.Confirm.BtnDown == true)
                {
                    source.PlayOneShot(selectionOK);
                    ready = false;
                    gameObject.SetActive(false);
                }
                break;
            case ConfirmCancelDefaultSelection.Cancel:
                cursor.transform.localPosition = mainCursorCoords[7];
                if (hardwareInterfaceManager.Right.BtnDown == true)
                {
                    confirmCancelDefaults = ConfirmCancelDefaultSelection.Defaults;
                    source.PlayOneShot(cursorUp);
                }
                else if (hardwareInterfaceManager.Left.BtnDown == true)
                {
                    confirmCancelDefaults = ConfirmCancelDefaultSelection.Confirm;
                    source.PlayOneShot(cursorDown);
                }
                else if (hardwareInterfaceManager.Confirm.BtnDown == true)
                {
                    source.PlayOneShot(selectionOK);
                    hardwareInterfaceManager.WindowedRes = windowedResBuffer;
                    if (fullscreenBoolBuffer == true)
                    {
                        hardwareInterfaceManager.RefreshFullscreenRes(fullscreenResBuffer);
                    }
                    else
                    {
                        hardwareInterfaceManager.fullscreenRes = fullscreenResBuffer;
                        hardwareInterfaceManager.RefreshWindow();
                    }
                    playerSettingsManager.MusicVolume = musicVolumeBuffer;
                    playerSettingsManager.SFXVolume = sfxVolumeBuffer;
                    ready = false;
                    gameObject.SetActive(false);
                }
                break;
            case ConfirmCancelDefaultSelection.Defaults:
                cursor.transform.localPosition = mainCursorCoords[8];
                if (hardwareInterfaceManager.Left.BtnDown == true)
                {
                    confirmCancelDefaults = ConfirmCancelDefaultSelection.Cancel;
                    source.PlayOneShot(cursorDown);
                }
                else if (hardwareInterfaceManager.Confirm.BtnDown == true)
                {
                    source.PlayOneShot(selectionForbidden);
                }
                break;
        }
    }

    void _in_MateWithManagers ()
    {
        Debug.Log("");
        GameObject hwInterfaceManagerObj = GameObject.Find("Universe/HardwareInterfaceManager");
        if (hwInterfaceManagerObj != null)
        {
            hardwareInterfaceManager = hwInterfaceManagerObj.GetComponent<HardwareInterfaceManager>();
            playerSettingsManager = GameObject.Find("Universe/PlayerSettingsManager").GetComponent<PlayerSettingsManager>();
        }
    }

    void _in_Resolution ()
    {
        if (hardwareInterfaceManager.Left.BtnDown == true)
        {
            if (Screen.fullScreen == false)
            {
                if (hardwareInterfaceManager.WindowedRes > WindowedResolutionMultiplier.x1)
                {
                    source.PlayOneShot(selectionOK);
                    hardwareInterfaceManager.WindowedRes--;
                    hardwareInterfaceManager.RefreshWindow();
                    ctr = 8;
                }
                else
                {
                    source.PlayOneShot(selectionForbidden);
                }
            }
            else
            {
                if (ResolutionIndex > 0)
                {
                    ResolutionIndex--;
                    lastFullscreenRes = hardwareInterfaceManager.fullscreenRes;
                    lastResFullscreen = true;
                    screenSettingsTimer = screenSettingsRevertTime;
                    hardwareInterfaceManager.RefreshFullscreenRes(Screen.resolutions[ResolutionIndex]);
                    source.PlayOneShot(selectionOK);
                    settingsConfirmationPrompt.SetActive(true);
                    suspended = true;
                }
                else
                {
                    source.PlayOneShot(selectionForbidden);
                }
            }
        }
        else if (hardwareInterfaceManager.Right.BtnDown == true)
        {
            if (Screen.fullScreen == false)
            {
                if (hardwareInterfaceManager.WindowedRes < WindowedResolutionMultiplier.x14 && 
                    (HammerConstants.LogicalResolution_Horizontal * (int)hardwareInterfaceManager.WindowedRes + 1) < Screen.currentResolution.width &&
                    (HammerConstants.LogicalResolution_Vertical * (int)hardwareInterfaceManager.WindowedRes + 1) < Screen.currentResolution.height)
                {
                    source.PlayOneShot(selectionOK);
                    hardwareInterfaceManager.WindowedRes++;
                    hardwareInterfaceManager.RefreshWindow();
                    ctr = 8;
                }
                else
                {
                    source.PlayOneShot(selectionForbidden);
                }
            }
            else
            {
                if (ResolutionIndex < Screen.resolutions.Length)
                {
                    ResolutionIndex++;
                    lastFullscreenRes = hardwareInterfaceManager.fullscreenRes;
                    lastResFullscreen = true;
                    screenSettingsTimer = screenSettingsRevertTime;
                    hardwareInterfaceManager.RefreshFullscreenRes(Screen.resolutions[ResolutionIndex]);
                    source.PlayOneShot(selectionOK);
                    settingsConfirmationPrompt.SetActive(true);
                    suspended = true;
                }
                else
                {
                    source.PlayOneShot(selectionForbidden);
                }
            }
        }
    }
}
