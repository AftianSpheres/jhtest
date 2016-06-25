using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Weapon select menu MonoBehaviour.
/// Just one kinda gnarly ball of logic that juggles some "dumb" sprites.
/// </summary>
public class WeaponMenu : MonoBehaviour
{
    public MenuSystem menuSystem;
    public PlayerWeaponManager wpnManager;
    public SpriteRenderer[] slotASelection;
    public SpriteRenderer[] slotBSelection;
    public SpriteRenderer cursorSprite;
    public Sprite[] cursorFrames;
    public GameObject uiSpritePrefab;
    public bool ActiveSlotIsSlotB = false;
    public bool open = false;
    private int index_a = -1;
    private int index_b = -1;
    private int wpnCount_a = 0;
    private int wpnCount_b = 0;
    static Vector3 upperCursorPos = new Vector3(48, -4, -10);
    static Vector3 lowerCursorPos = new Vector3(48, -60, -10);
    static Vector3 upperCenterIconPos = new Vector3(72, -22, 0);
    static Vector3 lowerCenterIconPos = new Vector3(72, -52, 0);
    public RetroPrinterScriptBasic textbox;
    private Sprite[] WpnIcons;


    /// <summary>
    /// MonoBehaviour.Awake()
    /// </summary>
	void Awake ()
    {
        WpnIcons = Resources.LoadAll<Sprite>(GlobalStaticResourcePaths.p_PlayerWeaponIcons);
        wpnManager = menuSystem.world.player.wpnManager;
        slotASelection = new SpriteRenderer[HammerConstants.NumberOfWeapons + 1];
        slotBSelection = new SpriteRenderer[HammerConstants.NumberOfWeapons + 1];
        for (int i = 0; i < HammerConstants.NumberOfWeapons + 1; i++)
        {
            SpriteRenderer s = Instantiate(uiSpritePrefab).GetComponent<SpriteRenderer>();
            if (i < WpnIcons.Length)
            {
                s.sprite = WpnIcons[i];
            }
            else
            {
                s.sprite = default(Sprite);
            }
            s.enabled = false;
            slotASelection[i] = s;
            s.transform.SetParent(transform);
            s = Instantiate(s);
            slotBSelection[i] = s;
            s.transform.SetParent(transform);
            if (wpnManager.SlotAWpn == (WeaponType)i && wpnManager.SlotAWpn != WeaponType.None)
            {
                index_a = i;
            }
            else if (wpnManager.SlotBWpn == (WeaponType)i && wpnManager.SlotAWpn != WeaponType.None)
            {
                index_b = i;
            }
        }
        cursorSprite.transform.localPosition = upperCursorPos;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// MonoBehaviour.Update()
    /// </summary>
    void Update ()
    {
        wpnCount_a = 0;
        for (int i = 0; i < slotASelection.Length; i++)
        {
            if (slotASelection[i].enabled == true)
            {
                wpnCount_a++;
            }
        }
        wpnCount_b = 0;
        for (int i = 0; i < slotBSelection.Length; i++)
        {
            if (slotBSelection[i].enabled == true)
            {
                wpnCount_b++;
            }
        }
        if (open == true && menuSystem.menuActive == true)
        {
            if (menuSystem.world.HardwareInterfaceManager.Menu.BtnDown == true || menuSystem.world.HardwareInterfaceManager.Cancel.BtnDown == true)
            {
                menuSystem.ChangeMode(MenuSystemMode.None);
            }
            else if (wpnManager.SlotAWpn != WeaponType.None && wpnManager.SlotBWpn != WeaponType.None)
            {
                if (menuSystem.world.HardwareInterfaceManager.Up.BtnDown == true)
                {
                    SwitchActiveSlot();
                    menuSystem.source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_CursorUpSFX));
                }
                else if (menuSystem.world.HardwareInterfaceManager.Down.BtnDown == true)
                {
                    SwitchActiveSlot();
                    menuSystem.source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_CursorDownSFX));
                }
            }
            if ((ActiveSlotIsSlotB == false && wpnCount_a > 1) || (wpnManager.SlotBWpn != WeaponType.None && wpnCount_b > 1))
            {
                if (menuSystem.world.HardwareInterfaceManager.Left.BtnDown == true)
                {
                    ScrollWpnSelection(true, ActiveSlotIsSlotB);
                    menuSystem.source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_CursorDownSFX));
                }
                else if (menuSystem.world.HardwareInterfaceManager.Right.BtnDown == true)
                {
                    ScrollWpnSelection(false, ActiveSlotIsSlotB);
                    menuSystem.source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_CursorUpSFX));
                }
            }
        }
        DrawWpnSequence();
        menuSystem.world.player.wpnManager.ChangeActiveWeapon((WeaponType)index_a);
        menuSystem.world.player.wpnManager.ChangeActiveWeapon((WeaponType)index_b, true);
    }

    /// <summary>
    /// Menu's Close action. Does necessary cleanup.
    /// </summary>
    public void Close ()
    {
        textbox.Stop();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Menu's Open action. Handles anything that we need to do after locking our position but before we can start using the menu.
    /// </summary>
    public void Open ()
    {
        open = true;
        menuSystem.menuCloseAction = Close;
        if (ActiveSlotIsSlotB == false)
        {
            textbox.Play(GlobalStaticResourcePaths.loadTextResource(GlobalStaticResourcePaths.p_wpn_descs[index_a]));
        }
        else if (wpnManager.SlotBWpn != WeaponType.None && ActiveSlotIsSlotB == true)
        {
            textbox.Play(GlobalStaticResourcePaths.loadTextResource(GlobalStaticResourcePaths.p_wpn_descs[index_b]));
        }
    }

    /// <summary>
    /// Menu's PreOpen action. Handles anything that needs to be loaded before we start scrolling it into the frame.
    /// </summary>
    public void PreOpen ()
    {
        for (int i = 0; i < HammerConstants.NumberOfWeapons + 1; i++)
        {
            int mask = 1 << i;
            if (((int)wpnManager.WpnUnlocks & mask) == mask)
            {
                slotASelection[i].enabled = true;
                slotBSelection[i].enabled = true;
                if (wpnManager.SlotAWpn == (WeaponType)i && wpnManager.SlotAWpn != WeaponType.None)
                {
                    index_a = i;
                }
                else if (wpnManager.SlotBWpn == (WeaponType)i && wpnManager.SlotAWpn != WeaponType.None)
                {
                    index_b = i;
                }
            }
            else
            {
                slotASelection[i].enabled = false;
                slotBSelection[i].enabled = false;
            }
        }
    }

    /// <summary>
    /// Draws upper and lower rows of weapon graphics.
    /// </summary>
    void DrawWpnSequence ()
    {
        if (wpnManager.SlotAWpn != WeaponType.None)
        {
            _in_DrawWpnSequence(ref index_a, ref index_b, ref wpnCount_a, ref slotASelection, ref upperCenterIconPos);
        }
        if (wpnManager.SlotBWpn != WeaponType.None)
        {
            _in_DrawWpnSequence(ref index_b, ref index_a, ref wpnCount_b, ref slotBSelection, ref lowerCenterIconPos);
        }
    }

    /// <summary>
    /// Back-end: draws a row of weapon icons on the screen.
    /// </summary>
    void _in_DrawWpnSequence (ref int index, ref int opposingIndex, ref int wpnCount, ref SpriteRenderer[] a, ref Vector3 defaultPos)
    {
        int sprCountToRight = 1;
        int sprCountToLeft = 1;
        int mask;
        if (index > -1 && a[index] != null)
        {
            a[index].transform.localPosition = defaultPos;
        }
        if (opposingIndex > -1 && a[opposingIndex] != null)
        {
            a[opposingIndex].enabled = false;
        }
        for (int i = index - 1; i > -1; i--)
        {
            mask = 1 << i;
            if (a[i] != null && i != opposingIndex && ((int)wpnManager.WpnUnlocks & mask) == mask)
            {
                a[i].transform.localPosition = defaultPos + (Vector3.left * sprCountToLeft * 24);
                a[i].enabled = true;
                sprCountToLeft++;
            }
        }
        for (int i = index + 1; i < a.Length; i++)
        {
            mask = 1 << i;
            if (a[i] != null && i != opposingIndex && ((int)wpnManager.WpnUnlocks & mask) == mask)
            {
                a[i].transform.localPosition = defaultPos + (Vector3.right * sprCountToRight * 24);
                a[i].enabled = true;
                sprCountToRight++;
            }
        }
        if (sprCountToRight < sprCountToLeft)
        {
            for (int i = 0; i < index; i++)
            {
                mask = 1 << i;
                if (sprCountToRight >= sprCountToLeft)
                {
                    break;
                }
                else if (a[i] != null && i != opposingIndex && ((int)wpnManager.WpnUnlocks & mask) == mask)
                {
                    a[i].transform.localPosition = defaultPos + (Vector3.right * sprCountToRight * 24);
                    a[i].enabled = true;
                    sprCountToRight++;
                    sprCountToLeft--;
                }
            }
        }
        else if (sprCountToLeft < sprCountToRight)
        {
            for (int i = HammerConstants.NumberOfWeapons - 1; i > index; i--)
            {
                mask = 1 << i;
                if (sprCountToLeft >= sprCountToRight)
                {
                    break;
                }
                else if (a[i] != null && i != opposingIndex && ((int)wpnManager.WpnUnlocks & mask) == mask)
                {
                    a[i].transform.localPosition = defaultPos + (Vector3.left * sprCountToLeft * 24);
                    a[i].enabled = true;
                    sprCountToLeft++;
                    sprCountToRight--;
                }
            }
        }
    }

    /// <summary>
    /// Scrolls one of the weapon selection rows either left or right.
    /// </summary>
    void ScrollWpnSelection (bool scrollLeft, bool slotB)
    {
        int v = 1;
        if (scrollLeft == true)
        {
            v = -1;
        }
        if (slotB == false)
        {
            _in_ScrollWpnSelection(ref v, ref index_a, ref index_b, ref slotASelection);
            textbox.Play(GlobalStaticResourcePaths.loadTextResource(GlobalStaticResourcePaths.p_wpn_descs[index_a]));
        }
        else
        {
            _in_ScrollWpnSelection(ref v, ref index_b, ref index_a, ref slotBSelection);
            textbox.Play(GlobalStaticResourcePaths.loadTextResource(GlobalStaticResourcePaths.p_wpn_descs[index_b]));

        }
    }

    /// <summary>
    /// Back-end: scrolls weapon graphics.
    /// </summary>
    void _in_ScrollWpnSelection(ref int v, ref int i, ref int I, ref SpriteRenderer[] a)
    {
        while (true)
        {
            i += v;
            if (i < 0)
            {
                i = a.Length - 1;
            }
            else if (i >= a.Length)
            {
                i = 0;
            }
            int mask = 1 << i;
            if (((int)wpnManager.WpnUnlocks & mask) == mask && i != I)
            {
                break;
            }
        }
    }

    /// <summary>
    /// Switches from slot A to slot B or vice versa.
    /// </summary>
    void SwitchActiveSlot()
    {
        if (ActiveSlotIsSlotB == false)
        {
            ActiveSlotIsSlotB = true;
            cursorSprite.sprite = cursorFrames[1];
            cursorSprite.transform.localPosition = lowerCursorPos;
            textbox.Play(GlobalStaticResourcePaths.loadTextResource(GlobalStaticResourcePaths.p_wpn_descs[index_b]));
        }
        else
        {
            ActiveSlotIsSlotB = false;
            cursorSprite.sprite = cursorFrames[0];
            cursorSprite.transform.localPosition = upperCursorPos;
            textbox.Play(GlobalStaticResourcePaths.loadTextResource(GlobalStaticResourcePaths.p_wpn_descs[index_a]));
        }
    }

}
