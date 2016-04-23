using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Weapon select menu MonoBehaviour.
/// Just one kinda gnarly ball of logic that juggles some "dumb" sprites.
/// </summary>
public class WeaponSelect : MonoBehaviour
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
    static Vector3 upperCenterIconPos = new Vector3(72, -16, 0);
    static Vector3 lowerCenterIconPos = new Vector3(72, -48, 0);
    public RetroPrinterScriptBasic textbox;

    /// <summary>
    /// MonoBehaviour.Awake()
    /// </summary>
	void Awake ()
    {
        wpnManager = menuSystem.world.player.wpnManager;
        slotASelection = new SpriteRenderer[HammerConstants.NumberOfWeapons];
        slotBSelection = new SpriteRenderer[HammerConstants.NumberOfWeapons];
        for (int i = 0; i < wpnManager.WpnUnlocks.Length; i++)
        {
            SpriteRenderer s = Instantiate(uiSpritePrefab).GetComponent<SpriteRenderer>();
            if (i < GlobalStaticResources.PlayerWeaponIcons.Length)
            {
                s.sprite = GlobalStaticResources.PlayerWeaponIcons[i];
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
            if (Input.GetKeyDown(menuSystem.world.PlayerDataManager.K_Menu) == true || Input.GetKeyDown(menuSystem.world.PlayerDataManager.K_Cancel) == true)
            {
                menuSystem.ChangeMode(MenuSystemMode.None);
            }
            else if (wpnManager.SlotAWpn != WeaponType.None && wpnManager.SlotBWpn != WeaponType.None)
            {
                if (Input.GetKeyDown(menuSystem.world.PlayerDataManager.K_VertUp) == true)
                {
                    SwitchActiveSlot();
                    menuSystem.source.PlayOneShot(GlobalStaticResources.CursorUpSFX);
                }
                else if (Input.GetKeyDown(menuSystem.world.PlayerDataManager.K_VertDown) == true)
                {
                    SwitchActiveSlot();
                    menuSystem.source.PlayOneShot(GlobalStaticResources.CursorDownSFX);
                }
            }
            if ((ActiveSlotIsSlotB == false && wpnCount_a > 1) || (wpnManager.SlotBWpn != WeaponType.None && wpnCount_b > 1))
            {
                if (Input.GetKeyDown(menuSystem.world.PlayerDataManager.K_HorizLeft) == true)
                {
                    ScrollWpnSelection(true, ActiveSlotIsSlotB);
                    menuSystem.source.PlayOneShot(GlobalStaticResources.CursorDownSFX);
                }
                else if (Input.GetKeyDown(menuSystem.world.PlayerDataManager.K_HorizRight) == true)
                {
                    ScrollWpnSelection(false, ActiveSlotIsSlotB);
                    menuSystem.source.PlayOneShot(GlobalStaticResources.CursorUpSFX);
                }
            }
        }
        DrawWpnSequence();
        menuSystem.world.player.wpnManager.SlotAWpn = (WeaponType)index_a;
        menuSystem.world.player.wpnManager.SlotBWpn = (WeaponType)index_b;
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
            textbox.Play(GlobalStaticResources.loadTextResource(GlobalStaticResources.p_wpn_descs[index_a]));
        }
        else if (wpnManager.SlotBWpn != WeaponType.None && ActiveSlotIsSlotB == true)
        {
            textbox.Play(GlobalStaticResources.loadTextResource(GlobalStaticResources.p_wpn_descs[index_b]));
        }
    }

    /// <summary>
    /// Menu's PreOpen action. Handles anything that needs to be loaded before we start scrolling it into the frame.
    /// </summary>
    public void PreOpen ()
    {
        for (int i = 0; i < wpnManager.WpnUnlocks.Length; i++)
        {
            if (wpnManager.WpnUnlocks[i] == true)
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
            if (a[i] != null && i != opposingIndex && wpnManager.WpnUnlocks[i] == true)
            {
                a[i].transform.localPosition = defaultPos + (Vector3.left * sprCountToLeft * 24);
                a[i].enabled = true;
                sprCountToLeft++;
            }
        }
        for (int i = index + 1; i < a.Length; i++)
        {
            if (a[i] != null && i != opposingIndex && wpnManager.WpnUnlocks[i] == true)
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
                if (sprCountToRight >= sprCountToLeft)
                {
                    break;
                }
                else if (a[i] != null && i != opposingIndex && wpnManager.WpnUnlocks[i] == true)
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
                if (sprCountToLeft >= sprCountToRight)
                {
                    break;
                }
                else if (a[i] != null && i != opposingIndex && wpnManager.WpnUnlocks[i] == true)
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
            textbox.Play(GlobalStaticResources.loadTextResource(GlobalStaticResources.p_wpn_descs[index_a]));
        }
        else
        {
            _in_ScrollWpnSelection(ref v, ref index_b, ref index_a, ref slotBSelection);
            textbox.Play(GlobalStaticResources.loadTextResource(GlobalStaticResources.p_wpn_descs[index_b]));

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
            if (wpnManager.WpnUnlocks[i] == true && i != I)
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
            textbox.Play(GlobalStaticResources.loadTextResource(GlobalStaticResources.p_wpn_descs[index_b]));
        }
        else
        {
            ActiveSlotIsSlotB = false;
            cursorSprite.sprite = cursorFrames[0];
            cursorSprite.transform.localPosition = upperCursorPos;
            textbox.Play(GlobalStaticResources.loadTextResource(GlobalStaticResources.p_wpn_descs[index_a]));
        }
    }

}
