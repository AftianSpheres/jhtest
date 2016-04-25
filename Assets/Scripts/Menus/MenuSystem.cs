using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Valid operating modes for the menu system.
/// </summary>
public enum MenuSystemMode
{
    None,
    WeaponSelect
}

/// <summary>
/// Virtual menu system. Interact with this to bring menus onto the screen.
/// </summary>
public class MenuSystem : MonoBehaviour {
    public WorldController world;
    public Action menuPreOpenAction;
    public Action menuOpenAction;
    public Action menuCloseAction;
    public AudioSource source;
    public GameObject TargetingReticle;
    public MenuSystemMode mode;
    public WeaponSelect WpnSelectMenu;
    public bool menuActive;

    /// <summary>
    /// MonoBehaviour.Update()
    /// </summary>
    void Update ()
    {
        switch (mode)
        {
            case MenuSystemMode.None: // not in menu
                if (Input.GetKeyDown(world.PlayerDataManager.K_Menu) == true && world.player.Locked == false && world.paused == false)
                {
                    ChangeMode(MenuSystemMode.WeaponSelect);
                }
                break;
        }
    }

    /// <summary>
    /// Changes menu system mode to the specified mode.
    /// </summary>
    /// <param name="_mode"></param>
    public void ChangeMode(MenuSystemMode _mode)
    {
        switch (_mode)
        {
            case MenuSystemMode.None:
                StartCoroutine(ScrollMenusOutOfFrame());
                break;
            case MenuSystemMode.WeaponSelect:
                if (world.player.wpnManager.SlotAWpn != WeaponType.None) // this menu can't be opened without at least one weapon
                {
                    menuPreOpenAction = WpnSelectMenu.PreOpen;
                    menuOpenAction = WpnSelectMenu.Open;
                    WpnSelectMenu.gameObject.SetActive(true);
                    TargetingReticle.SetActive(false);
                    StartCoroutine(ScrollMenusIntoFrame());
                    mode = MenuSystemMode.WeaponSelect;
                }
                break;
            default:
                throw new System.Exception("Invalid menu system mode: " + _mode);
        }
    }

    /// <summary>
    /// Coroutine: scrolls menus into frame from "under" HUD bar.
    /// </summary>
    public IEnumerator ScrollMenusIntoFrame ()
    {
        world.Pause();
        menuPreOpenAction();
        source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResources.p_MenuOpenSFX));
        while (transform.localPosition.y != 0)
        {
            transform.localPosition = transform.localPosition + (Vector3.down * 4);
            yield return null;
        }

        menuActive = true;
        menuOpenAction();
    }

    /// <summary>
    /// Coroutine: scrolls menus out of frame and "under" HUD bar.
    /// </summary>
    public IEnumerator ScrollMenusOutOfFrame (bool withHUD = true)
    {
        menuActive = false;
        int v = 0;
        if (withHUD == true)
        {
            v = HammerConstants.HeightOfHUD;
        }
        source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResources.p_MenuCloseSFX));
        while (transform.localPosition.y < 128 + v)
        {
            transform.localPosition = transform.localPosition + (Vector3.up * 4);
            yield return null;
        }
        TargetingReticle.SetActive(true);
        mode = MenuSystemMode.None;
        menuPreOpenAction = default(Action);
        if (menuCloseAction != null)
        {
            menuCloseAction();
            menuCloseAction = default(Action);
        }
        world.Unpause();
    }

}
