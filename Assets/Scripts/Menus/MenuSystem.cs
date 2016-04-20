using UnityEngine;
using System;
using System.Collections;

public enum MenuSystemMode
{
    None,
    WeaponSelect
}

public class MenuSystem : MonoBehaviour {
    public WorldController world;
    public MenuSystemMode mode;
    public GameObject WanderingSprites;
    public GameObject GeneratedSprites;
    public GameObject Rooms;
    public WeaponSelect WpnSelectMenu;
    public GameObject TargetingReticle;
    public bool menuActive;
    public AudioSource source;
    public Action menuOpenAction;
    public Action menuCloseAction;

    void Update ()
    {
        switch (mode)
        {
            case MenuSystemMode.None: // not in menu
                if (Input.GetKeyDown(world.PlayerDataManager.K_Menu) == true && world.player.Locked == false)
                {
                    ChangeMode(MenuSystemMode.WeaponSelect);
                }
                break;
        }
    }

    void EnableBGSprites ()
    {
        WanderingSprites.SetActive(true);
        GeneratedSprites.SetActive(true);
        Rooms.SetActive(true);
    }

    void DisableBGSprites ()
    {
        WanderingSprites.SetActive(false);
        GeneratedSprites.SetActive(false);
        Rooms.SetActive(false);
    }

    public void ChangeMode(MenuSystemMode _mode)
    {
        switch (_mode)
        {
            case MenuSystemMode.None:
                StartCoroutine(ScrollMenusOutOfFrame());
                break;
            case MenuSystemMode.WeaponSelect:
                menuOpenAction = WpnSelectMenu.Open;
                WpnSelectMenu.gameObject.SetActive(true);
                TargetingReticle.SetActive(false);
                StartCoroutine(ScrollMenusIntoFrame());
                mode = MenuSystemMode.WeaponSelect;
                break;
            default:
                throw new System.Exception("Invalid menu system mode: " + _mode);
        }
    }

    public IEnumerator ScrollMenusIntoFrame (bool withHUD = true)
    {
        int v = 0;
        if (withHUD == true)
        {
            v = HammerConstants.HeightOfHUD;
        }
        source.PlayOneShot(GlobalStaticResources.MenuOpenSFX);
        while (transform.localPosition.y != 0)
        {
            transform.localPosition = transform.localPosition + (Vector3.down * 4);
            yield return null;
        }
        DisableBGSprites();
        menuActive = true;
        menuOpenAction();
    }

    public IEnumerator ScrollMenusOutOfFrame (bool withHUD = true)
    {
        EnableBGSprites();
        menuActive = false;
        int v = 0;
        if (withHUD == true)
        {
            v = HammerConstants.HeightOfHUD;
        }
        source.PlayOneShot(GlobalStaticResources.MenuCloseSFX);
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
    }

}
