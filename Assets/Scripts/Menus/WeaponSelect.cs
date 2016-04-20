using UnityEngine;
using System;
using System.Collections.Generic;

public class WeaponSelect : MonoBehaviour
{
    public MenuSystem menuSystem;
    public PlayerWeaponManager wpnManager;
    public GameObject[] slotASelection;
    public GameObject[] slotBSelection;
    public SpriteRenderer cursorSprite;
    public Sprite[] cursorFrames;
    public GameObject uiSpritePrefab;
    public bool ActiveSlotIsSlotB = false;
    public bool open = false;
    private int index_a;
    private int index_b;
    static Vector3 upperCursorPos = new Vector3(48, -4, -10);
    static Vector3 lowerCursorPos = new Vector3(48, -60, -10);
    static Vector3 upperCenterIconPos = new Vector3(72, -16, 0);
    static Vector3 lowerCenterIconPos = new Vector3(72, -48, 0);
    public RetroPrinterScriptBasic textbox;

	// Use this for initialization
	void Start ()
    {
        wpnManager = menuSystem.world.player.wpnManager;
        slotASelection = new GameObject[HammerConstants.NumberOfWeapons];
        slotBSelection = new GameObject[HammerConstants.NumberOfWeapons];
        for (int i = 0; i < wpnManager.WpnUnlocks.Length; i++)
        {
            if (wpnManager.WpnUnlocks[i] == true)
            {
                GameObject obj = Instantiate(uiSpritePrefab);
                SpriteRenderer s = obj.GetComponent<SpriteRenderer>();
                s.sprite = GlobalStaticResources.PlayerWeaponIcons[i];
                slotASelection[i] = obj;
                obj.transform.SetParent(transform);
                obj = Instantiate(obj);
                slotBSelection[i] = obj;
                obj.transform.SetParent(transform);
                if (wpnManager.SlotAWpn == (WeaponType)i)
                {
                    index_a = i;
                }
                else if (wpnManager.SlotBWpn == (WeaponType)i)
                {
                    index_b = i;
                }
            }
        }
        cursorSprite.transform.localPosition = upperCursorPos;
    }

    // Update is called once per frame
    void Update ()
    {
        if (open == true && menuSystem.menuActive == true)
        {
            if (Input.GetKeyDown(menuSystem.world.PlayerDataManager.K_Menu) == true || Input.GetKeyDown(menuSystem.world.PlayerDataManager.K_Cancel) == true)
            {
                menuSystem.ChangeMode(MenuSystemMode.None);
            }
            else if (Input.GetKeyDown(menuSystem.world.PlayerDataManager.K_VertUp) == true)
            {
                SwitchActiveSlot();
                menuSystem.source.PlayOneShot(GlobalStaticResources.CursorUpSFX);
            }
            else if (Input.GetKeyDown(menuSystem.world.PlayerDataManager.K_VertDown) == true)
            {
                SwitchActiveSlot();
                menuSystem.source.PlayOneShot(GlobalStaticResources.CursorDownSFX);
            }
            else if (Input.GetKeyDown(menuSystem.world.PlayerDataManager.K_HorizLeft) == true)
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
        DrawWpnSequence();
        menuSystem.world.player.wpnManager.SlotAWpn = (WeaponType)index_a;
        menuSystem.world.player.wpnManager.SlotBWpn = (WeaponType)index_b;
    }

    public void Close ()
    {
        textbox.Stop();
        gameObject.SetActive(false);
    }

    public void Open ()
    {
        open = true;
        menuSystem.menuCloseAction = Close;
        if (ActiveSlotIsSlotB == false)
        {
            textbox.Play(GlobalStaticResources.loadTextResource(GlobalStaticResources.p_wpn_descs[index_a]));
        }
        else
        {
            textbox.Play(GlobalStaticResources.loadTextResource(GlobalStaticResources.p_wpn_descs[index_b]));
        }
    }

    void DrawWpnSequence ()
    {
        _in_DrawWpnSequence(ref index_a, ref index_b, ref slotASelection, ref upperCenterIconPos);
        _in_DrawWpnSequence(ref index_b, ref index_a, ref slotBSelection, ref lowerCenterIconPos);
    }

    void _in_DrawWpnSequence (ref int index, ref int opposingIndex, ref GameObject[] a, ref Vector3 defaultPos)
    {
        int sprCountToRight = 1;
        int sprCountToLeft = 1;
        if (a[index] != null)
        {
            a[index].transform.localPosition = defaultPos;
        }
        if (a[opposingIndex] != null)
        {
            a[opposingIndex].transform.localPosition = new Vector3(8192, 8192, 0); // gtfo
        }
        for (int i = index - 1; i > -1; i--)
        {
            if (a[i] != null && i != opposingIndex)
            {
                a[i].transform.localPosition = defaultPos + (Vector3.left * sprCountToLeft * 24);
                sprCountToLeft++;
            }
        }
        for (int i = index + 1; i < a.Length; i++)
        {
            if (a[i] != null && i != opposingIndex)
            {
                a[i].transform.localPosition = defaultPos + (Vector3.right * sprCountToRight * 24);
                sprCountToRight++;
            }
        }
        if (sprCountToLeft < sprCountToRight - 1)
        {
            for (int i = a.Length - 1; i > index; i--)
            {
                if (a[i] != null && i != opposingIndex)
                {
                    a[i].transform.localPosition = defaultPos + (Vector3.left * sprCountToLeft * 24);
                    sprCountToLeft++;
                    if (sprCountToLeft >= sprCountToRight - 1)
                    {
                        break;
                    }
                }
            }
        }
        if (sprCountToRight < sprCountToLeft - 1)
        {
            for (int i = index - 1; i > -1; i--)
            {
                if (a[i] != null && i != opposingIndex)
                {
                    a[i].transform.localPosition = defaultPos + (Vector3.right * sprCountToRight * 24);
                    sprCountToRight++;
                    if (sprCountToRight >= sprCountToLeft - 1)
                    {
                        break;
                    }
                }
            }
        }
    }

    void DrawWpnText (string text)
    {

    }

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

    void _in_ScrollWpnSelection(ref int v, ref int i, ref int I, ref GameObject[] a)
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
