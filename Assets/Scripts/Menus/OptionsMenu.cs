using UnityEngine;
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
    public TextMesh musicVolumeValueText;
    public TextMesh sfxVolumeValueText;
    public TextMesh countdownValueText;
    private int ResolutionIndex = -1;
    private static int screenSettingsRevertTime = 600;
    private ConfirmCancelDefaultSelection confirmCancelDefaults;
    private OptionsMenuSelections selection;
    private int screenSettingsTimer;
    private int ctr;
    private float musicVolumeBuffer = float.MaxValue;
    private float sfxVolumeBuffer = float.MaxValue;
    private bool ready;
    private bool fullscreenBoolBuffer;
    private WindowedResolutionMultiplier windowedResBuffer;
    private Resolution fullscreenResBuffer;
    private bool lastResFullscreen;
    private Resolution lastFullscreenRes;
    private HardwareInterfaceManager hardwareInterfaceManager;
    private PlayerSettingsManager playerSettingsManager;
    private static int delayBetweenCursorMoves = 12;

	// Use this for initialization
	void Awake ()
    {
        GameObject hwInterfaceManagerObj = GameObject.Find("Universe/HardwareInterfaceManager");
        if (hwInterfaceManagerObj != null)
        {
            _in_MateWithManagers();
        }
        musicVolumeValueText.text = Mathf.RoundToInt(playerSettingsManager.MusicVolume * 10).ToString();
        musicVolumeBuffer = playerSettingsManager.MusicVolume;
        sfxVolumeValueText.text = Mathf.RoundToInt(playerSettingsManager.SFXVolume * 10).ToString();
        sfxVolumeBuffer = playerSettingsManager.SFXVolume;
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
            windowedResBuffer = hardwareInterfaceManager.windowedRes;
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
            if (musicVolumeBuffer != playerSettingsManager.MusicVolume)
            {
                musicVolumeValueText.text = Mathf.RoundToInt(playerSettingsManager.MusicVolume * 10).ToString();
                musicVolumeBuffer = playerSettingsManager.MusicVolume;
            }
            if (sfxVolumeBuffer != playerSettingsManager.SFXVolume)
            {
                sfxVolumeValueText.text = Mathf.RoundToInt(playerSettingsManager.SFXVolume * 10).ToString();
                sfxVolumeBuffer = playerSettingsManager.SFXVolume;
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
                        ctr = delayBetweenCursorMoves;
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
                        ctr = delayBetweenCursorMoves;
                        source.PlayOneShot(cursorDown);
                    }
                    if (hardwareInterfaceManager.Up.Pressed == true && ctr <= 0)
                    {
                        selection = OptionsMenuSelections.FullscreenWindowed;
                        ctr = delayBetweenCursorMoves;
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
                        ctr = delayBetweenCursorMoves;
                        source.PlayOneShot(cursorDown);
                    }
                    if (hardwareInterfaceManager.Up.Pressed == true && ctr <= 0)
                    {
                        selection = OptionsMenuSelections.Resolution;
                        ctr = delayBetweenCursorMoves;
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
                        ctr = delayBetweenCursorMoves;
                        source.PlayOneShot(cursorDown);
                    }
                    if (hardwareInterfaceManager.Up.Pressed == true && ctr <= 0)
                    {
                        selection = OptionsMenuSelections.ControlConfig;
                        ctr = delayBetweenCursorMoves;
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
                                ctr = delayBetweenCursorMoves;
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
                                ctr = delayBetweenCursorMoves;
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
                        ctr = delayBetweenCursorMoves;
                        source.PlayOneShot(cursorDown);
                    }
                    if (hardwareInterfaceManager.Up.Pressed == true && ctr <= 0)
                    {
                        selection = OptionsMenuSelections.MusicVolume;
                        ctr = delayBetweenCursorMoves;
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
                                ctr = delayBetweenCursorMoves;
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
                                ctr = delayBetweenCursorMoves;
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
            int countdownSeconds = Mathf.CeilToInt((float)screenSettingsTimer / 60f);
            if (countdownSeconds < int.Parse(countdownValueText.text))
            {
                countdownValueText.text = countdownSeconds.ToString();
            }
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
        if (hardwareInterfaceManager.Up.BtnDown == true)
        {
            selection = OptionsMenuSelections.SFXVolume;
            ctr = delayBetweenCursorMoves;
            source.PlayOneShot(cursorUp);
        }
        else
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
                        playerSettingsManager.SaveToPlayerPrefs();
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
                        hardwareInterfaceManager.windowedRes = windowedResBuffer;
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
    }

    void _in_MateWithManagers ()
    {
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
                if (hardwareInterfaceManager.windowedRes > WindowedResolutionMultiplier.x1)
                {
                    source.PlayOneShot(selectionOK);
                    hardwareInterfaceManager.windowedRes--;
                    hardwareInterfaceManager.RefreshWindow();
                    ctr = delayBetweenCursorMoves;
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
                if (hardwareInterfaceManager.windowedRes < WindowedResolutionMultiplier.x14 && 
                    (HammerConstants.LogicalResolution_Horizontal * (int)hardwareInterfaceManager.windowedRes + 1) < Screen.currentResolution.width &&
                    (HammerConstants.LogicalResolution_Vertical * (int)hardwareInterfaceManager.windowedRes + 1) < Screen.currentResolution.height)
                {
                    source.PlayOneShot(selectionOK);
                    hardwareInterfaceManager.windowedRes++;
                    hardwareInterfaceManager.RefreshWindow();
                    ctr = delayBetweenCursorMoves;
                }
                else
                {
                    source.PlayOneShot(selectionForbidden);
                }
            }
            else
            {
                if (ResolutionIndex < Screen.resolutions.Length - 1)
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
