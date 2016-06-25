using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Valid operating modes for the menu system.
/// </summary>
public enum MenuSystemMode
{
    None,
    WeaponSelect,
    Inventory,
    Meta
}

/// <summary>
/// Virtual menu system. Interact with this to bring menus onto the screen.
/// </summary>
public class MenuSystem : MonoBehaviour {
    public WorldController world;
    public Action menuOpenAction;
    public Action menuCloseAction;
    public AudioSource source;
    public AudioClip menuOpenClip;
    public AudioClip menuCloseClip;
    public AudioClip menuForbiddenClip;
    public GameObject TargetingReticle;
    public MenuSystemMode mode;
    public InventoryMenu inventoryMenu;
    public MetaMenu metaMenu;
    public WeaponMenu weaponSelectMenu;
    public bool menuActive;
    private bool inTransition;
    private MenuSystemMode lastOpenMenu = MenuSystemMode.Inventory;

    /// <summary>
    /// MonoBehaviour.Update()
    /// </summary>
    void Update ()
    {
        if (inTransition == false)
        {
            switch (mode)
            {
                case MenuSystemMode.None: // not in menu
                    if (world.HardwareInterfaceManager.Menu.BtnDown == true && world.player.Locked == false && world.paused == false)
                    {
                        ChangeMode(lastOpenMenu);
                    }
                    break;
                case MenuSystemMode.WeaponSelect:
                    if (world.HardwareInterfaceManager.RightBumper.BtnDown == true)
                    {
                        ChangeMode(MenuSystemMode.Inventory);
                    }
                    else if (world.HardwareInterfaceManager.LeftBumper.BtnDown == true)
                    {
                        ChangeMode(MenuSystemMode.Meta);
                    }
                    break;
                case MenuSystemMode.Inventory:
                    if (world.HardwareInterfaceManager.RightBumper.BtnDown == true)
                    {
                        ChangeMode(MenuSystemMode.Meta);
                    }
                    else if (world.HardwareInterfaceManager.LeftBumper.BtnDown == true)
                    {
                        ChangeMode(MenuSystemMode.WeaponSelect);
                    }
                    break;
                case MenuSystemMode.Meta:
                    if (world.HardwareInterfaceManager.RightBumper.BtnDown == true)
                    {
                        ChangeMode(MenuSystemMode.WeaponSelect);
                    }
                    else if (world.HardwareInterfaceManager.LeftBumper.BtnDown == true)
                    {
                        ChangeMode(MenuSystemMode.Inventory);
                    }
                    break;
            }
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
                    inventoryMenu.open = false;
                    metaMenu.open = false;
                    menuOpenAction = weaponSelectMenu.Open;
                    weaponSelectMenu.gameObject.SetActive(true);
                    TargetingReticle.SetActive(false);
                    if (mode == MenuSystemMode.None)
                    {
                        mode = MenuSystemMode.WeaponSelect;
                        StartCoroutine(ScrollMenusIntoFrame());
                    }
                    else
                    {
                        mode = MenuSystemMode.WeaponSelect;
                        ScrollMenusSideways();
                    }
                }
                else
                {
                    source.PlayOneShot(menuForbiddenClip);
                }
                break;
            case MenuSystemMode.Inventory:
                weaponSelectMenu.open = false;
                metaMenu.open = false;
                menuOpenAction = inventoryMenu.Open;
                inventoryMenu.gameObject.SetActive(true);
                TargetingReticle.SetActive(false);
                if (mode == MenuSystemMode.None)
                {
                    mode = MenuSystemMode.Inventory;
                    StartCoroutine(ScrollMenusIntoFrame());
                }
                else
                {
                    mode = MenuSystemMode.Inventory;
                    ScrollMenusSideways();
                }
                break;
            case MenuSystemMode.Meta:
                weaponSelectMenu.open = false;
                inventoryMenu.open = false;
                menuOpenAction = metaMenu.Open;
                metaMenu.gameObject.SetActive(true);
                TargetingReticle.SetActive(false);
                if (mode == MenuSystemMode.None)
                {
                    mode = MenuSystemMode.Meta;
                    StartCoroutine(ScrollMenusIntoFrame());
                }
                else
                {
                    mode = MenuSystemMode.Meta;
                    ScrollMenusSideways();
                }
                break;
            default:
                throw new System.Exception("Invalid menu system mode: " + _mode);
        }
    }

    public void ScrollMenusSideways ()
    {
        inTransition = true;
        source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_MenuOpenSFX));
        switch (mode)
        {
            case MenuSystemMode.WeaponSelect:
                inventoryMenu.gameObject.SetActive(false);
                metaMenu.gameObject.SetActive(false);
                break;
            case MenuSystemMode.Inventory:
                weaponSelectMenu.gameObject.SetActive(false);
                metaMenu.gameObject.SetActive(false);
                break;
            case MenuSystemMode.Meta:
                weaponSelectMenu.gameObject.SetActive(false);
                inventoryMenu.gameObject.SetActive(false);
                break;
        }
        menuOpenAction();
        inTransition = false;
        lastOpenMenu = mode;
    }

    /// <summary>
    /// Coroutine: scrolls menus into frame from "under" HUD bar.
    /// </summary>
    public IEnumerator ScrollMenusIntoFrame ()
    {
        inTransition = true;
        world.Pause();
        weaponSelectMenu.PreOpen();
        inventoryMenu.PreOpen();
        metaMenu.PreOpen();
        source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_MenuOpenSFX));
        while (transform.localPosition.y != 0)
        {
            transform.localPosition = transform.localPosition + (Vector3.down * 4);
            yield return null;
        }

        menuActive = true;
        menuOpenAction();
        inTransition = false;
    }

    /// <summary>
    /// Coroutine: scrolls menus out of frame and "under" HUD bar.
    /// </summary>
    public IEnumerator ScrollMenusOutOfFrame (bool withHUD = true)
    {
        inTransition = true;
        menuActive = false;
        int v = 0;
        if (withHUD == true)
        {
            v = HammerConstants.HeightOfHUD;
        }
        source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_MenuCloseSFX));
        while (transform.localPosition.y < 128 + v)
        {
            transform.localPosition = transform.localPosition + (Vector3.up * 4);
            yield return null;
        }
        TargetingReticle.SetActive(true);
        mode = MenuSystemMode.None;
        if (menuCloseAction != null)
        {
            menuCloseAction();
            menuCloseAction = default(Action);
        }
        world.Unpause();
        inTransition = false;
    }

}
