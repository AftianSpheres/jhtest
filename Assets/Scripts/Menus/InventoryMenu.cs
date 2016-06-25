using UnityEngine;
using System.Collections;

public class InventoryMenu : MonoBehaviour
{
    public MenuSystem menuSystem;
    public GameStateManager gameStateManager;
    public RetroPrinterScriptBasic textbox;
    public SpriteRenderer spr_Monolith0;
    public SpriteRenderer spr_Monolith1;
    public SpriteRenderer spr_Monolith2;
    public SpriteRenderer spr_GlassBrand;
    public SpriteRenderer spr_GlassKey;
    public SpriteRenderer spr_DashAnklet;
    public SpriteRenderer spr_RollAnklet;
    public SpriteRenderer spr_HotRock;
    public SpriteRenderer spr_SeeingBell;
    public SpriteRenderer spr_SpiritAnchor;
    public SpriteRenderer spr_SentinelsMask;
    public SpriteRenderer spr_WingedCharm;
    public SpriteRenderer cursorSprite;
    public HeldPassiveItems selection = HeldPassiveItems.ForestMonolithChunk;
    public bool open = false;
    static Vector3 cursorOffset = new Vector3(2, -24, -0.1f);
    public AudioClip cursorSFX;
    private HeldPassiveItems lastSelected;

    void Awake ()
    {
        gameObject.SetActive(false);
    }

    void Update ()
    {
        _in_UpdateCursorPosition();
        if (open == true && menuSystem.menuActive == true)
        {
            if (lastSelected != selection)
            {
                RefreshTextbox();
                lastSelected = selection;
            }
            if (menuSystem.world.HardwareInterfaceManager.Menu.BtnDown == true || menuSystem.world.HardwareInterfaceManager.Cancel.BtnDown == true)
            {
                menuSystem.ChangeMode(MenuSystemMode.None);
            }
            else
            {
                if (selection == HeldPassiveItems.ForestMonolithChunk || selection == HeldPassiveItems.MarinaMonolithChunk || selection == HeldPassiveItems.ValleyMonolithChunk ||
                    selection == HeldPassiveItems.EndgameKey || selection == HeldPassiveItems.WorldChangeToken)
                {
                    _in_CursorOnKeyItems();
                }
                else
                {
                    _in_CursorOnNonKeyItems();
                }
            }
        }
    }

    void _in_UpdateCursorPosition()
    {
        switch (selection)
        {
            case HeldPassiveItems.ForestMonolithChunk:
                cursorSprite.transform.position = spr_Monolith0.transform.position + cursorOffset;
                break;
            case HeldPassiveItems.ValleyMonolithChunk:
                cursorSprite.transform.position = spr_Monolith1.transform.position + cursorOffset;
                break;
            case HeldPassiveItems.MarinaMonolithChunk:
                cursorSprite.transform.position = spr_Monolith2.transform.position + cursorOffset;
                break;
            case HeldPassiveItems.WorldChangeToken:
                cursorSprite.transform.position = spr_GlassBrand.transform.position + cursorOffset;
                break;
            case HeldPassiveItems.EndgameKey:
                cursorSprite.transform.position = spr_GlassKey.transform.position + cursorOffset;
                break;
            case HeldPassiveItems.DashThingy:
                cursorSprite.transform.position = spr_DashAnklet.transform.position + cursorOffset;
                break;
            case HeldPassiveItems.DodgeBooster:
                cursorSprite.transform.position = spr_RollAnklet.transform.position + cursorOffset;
                break;
            case HeldPassiveItems.DodgeAttack:
                cursorSprite.transform.position = spr_HotRock.transform.position + cursorOffset;
                break;
            case HeldPassiveItems.SecretSensor:
                cursorSprite.transform.position = spr_SeeingBell.transform.position + cursorOffset;
                break;
            case HeldPassiveItems.TabooRegenUpThingy:
                cursorSprite.transform.position = spr_SpiritAnchor.transform.position + cursorOffset;
                break;
            case HeldPassiveItems.AutoscrollRider:
                cursorSprite.transform.position = spr_SentinelsMask.transform.position + cursorOffset;
                break;
            case HeldPassiveItems.RedBull:
                cursorSprite.transform.position = spr_WingedCharm.transform.position + cursorOffset;
                break;
        }
    }

    void _in_CursorOnNonKeyItems()
    {
        switch (selection)
        {
            case HeldPassiveItems.SecretSensor:
                if (menuSystem.world.HardwareInterfaceManager.Right.BtnDown == true)
                {
                    _in_MoveCursorRight(1);
                }
                break;
            case HeldPassiveItems.DashThingy:
                if (menuSystem.world.HardwareInterfaceManager.Left.BtnDown == true)
                {
                    _in_MoveCursorLeft(6);
                }
                else if (menuSystem.world.HardwareInterfaceManager.Right.BtnDown == true)
                {
                    _in_MoveCursorRight(2);
                }
                break;
            case HeldPassiveItems.AutoscrollRider:
                if (menuSystem.world.HardwareInterfaceManager.Left.BtnDown == true)
                {
                    _in_MoveCursorLeft(5);
                }
                else if (menuSystem.world.HardwareInterfaceManager.Right.BtnDown == true)
                {
                    _in_MoveCursorRight(3);
                }
                break;
            case HeldPassiveItems.DodgeAttack:
                if (menuSystem.world.HardwareInterfaceManager.Left.BtnDown == true)
                {
                    _in_MoveCursorLeft(4);
                }
                else if (menuSystem.world.HardwareInterfaceManager.Right.BtnDown == true)
                {
                    _in_MoveCursorRight(4);
                }
                break;
            case HeldPassiveItems.RedBull:
                if (menuSystem.world.HardwareInterfaceManager.Left.BtnDown == true)
                {
                    _in_MoveCursorLeft(3);
                }
                else if (menuSystem.world.HardwareInterfaceManager.Right.BtnDown == true)
                {
                    _in_MoveCursorRight(5);
                }
                break;
            case HeldPassiveItems.DodgeBooster:
                if (menuSystem.world.HardwareInterfaceManager.Left.BtnDown == true)
                {
                    _in_MoveCursorLeft(2);
                }
                else if (menuSystem.world.HardwareInterfaceManager.Right.BtnDown == true)
                {
                    _in_MoveCursorRight(6);
                }
                break;
            case HeldPassiveItems.TabooRegenUpThingy:
                if (menuSystem.world.HardwareInterfaceManager.Left.BtnDown == true)
                {
                    _in_MoveCursorLeft(1);
                }
                else if (menuSystem.world.HardwareInterfaceManager.Right.BtnDown == true)
                {
                    _in_MoveCursorRight(7);
                }
                break;
        }
    }

    void _in_CursorOnKeyItems()
    {
        if (menuSystem.world.HardwareInterfaceManager.Down.BtnDown == true && selection != HeldPassiveItems.ValleyMonolithChunk && selection != HeldPassiveItems.EndgameKey)
        {
            if (spr_GlassKey.enabled == true)
            {
                selection = HeldPassiveItems.EndgameKey;
                menuSystem.source.PlayOneShot(cursorSFX);
            }
            else if (selection == HeldPassiveItems.ForestMonolithChunk)
            {
                if (spr_Monolith2.enabled == true)
                {
                    selection = HeldPassiveItems.MarinaMonolithChunk;
                    menuSystem.source.PlayOneShot(cursorSFX);
                }
                else if (spr_Monolith1.enabled == true)
                {
                    selection = HeldPassiveItems.ValleyMonolithChunk;
                    menuSystem.source.PlayOneShot(cursorSFX);
                }
            }
            else if (selection == HeldPassiveItems.MarinaMonolithChunk)
            {
                if (spr_Monolith1.enabled == true)
                {
                    selection = HeldPassiveItems.ValleyMonolithChunk;
                    menuSystem.source.PlayOneShot(cursorSFX);
                }
            }
        }
        else if (menuSystem.world.HardwareInterfaceManager.Up.BtnDown == true && selection != HeldPassiveItems.ForestMonolithChunk && selection != HeldPassiveItems.WorldChangeToken)
        {
            if (spr_GlassBrand.enabled == true)
            {
                selection = HeldPassiveItems.WorldChangeToken;
                menuSystem.source.PlayOneShot(cursorSFX);
            }
            else if (selection == HeldPassiveItems.MarinaMonolithChunk)
            {
                if (spr_Monolith0.enabled == true)
                {
                    selection = HeldPassiveItems.ForestMonolithChunk;
                    menuSystem.source.PlayOneShot(cursorSFX);
                }
            }
            else if (selection == HeldPassiveItems.ValleyMonolithChunk)
            {
                if (spr_Monolith2.enabled == true)
                {
                    selection = HeldPassiveItems.MarinaMonolithChunk;
                    menuSystem.source.PlayOneShot(cursorSFX);
                }
                else if (spr_Monolith0.enabled == true)
                {
                    selection = HeldPassiveItems.ForestMonolithChunk;
                    menuSystem.source.PlayOneShot(cursorSFX);
                }
            }
        }
        else if (menuSystem.world.HardwareInterfaceManager.Left.BtnDown == true)
        {
            _in_MoveCursorLeft(0);
        }
    }

    void _in_MoveCursorLeft (int entryPoint)
    {
        if (spr_SpiritAnchor.enabled == true && entryPoint < 1)
        {
            selection = HeldPassiveItems.TabooRegenUpThingy;
            menuSystem.source.PlayOneShot(cursorSFX);
        }
        else if (spr_RollAnklet.enabled == true && entryPoint < 2)
        {
            selection = HeldPassiveItems.DodgeBooster;
            menuSystem.source.PlayOneShot(cursorSFX);
        }
        else if (spr_WingedCharm.enabled == true && entryPoint < 3)
        {
            selection = HeldPassiveItems.RedBull;
            menuSystem.source.PlayOneShot(cursorSFX);
        }
        else if (spr_HotRock.enabled == true && entryPoint < 4)
        {
            selection = HeldPassiveItems.DodgeAttack;
            menuSystem.source.PlayOneShot(cursorSFX);
        }
        else if (spr_SentinelsMask.enabled == true && entryPoint < 5)
        {
            selection = HeldPassiveItems.AutoscrollRider;
            menuSystem.source.PlayOneShot(cursorSFX);
        }
        else if (spr_DashAnklet.enabled == true && entryPoint < 6)
        {
            selection = HeldPassiveItems.DashThingy;
            menuSystem.source.PlayOneShot(cursorSFX);
        }
        else if (spr_SeeingBell.enabled == true && entryPoint < 7)
        {
            selection = HeldPassiveItems.SecretSensor;
            menuSystem.source.PlayOneShot(cursorSFX);
        }
    }

    void _in_MoveCursorRight (int entryPoint)
    {
        if (spr_SeeingBell.enabled == true && entryPoint < 1)
        {
            selection = HeldPassiveItems.SecretSensor;
            menuSystem.source.PlayOneShot(cursorSFX);
        }
        else if (spr_DashAnklet.enabled == true && entryPoint < 2)
        {
            selection = HeldPassiveItems.DashThingy;
            menuSystem.source.PlayOneShot(cursorSFX);
        }
        else if (spr_SentinelsMask.enabled == true && entryPoint < 3)
        {
            selection = HeldPassiveItems.AutoscrollRider;
            menuSystem.source.PlayOneShot(cursorSFX);
        }
        else if (spr_HotRock.enabled == true && entryPoint < 4)
        {
            selection = HeldPassiveItems.DodgeAttack;
            menuSystem.source.PlayOneShot(cursorSFX);
        }
        else if (spr_WingedCharm.enabled == true && entryPoint < 5)
        {
            selection = HeldPassiveItems.RedBull;
            menuSystem.source.PlayOneShot(cursorSFX);
        }
        else if (spr_RollAnklet.enabled == true && entryPoint < 6)
        {
            selection = HeldPassiveItems.DodgeBooster;
            menuSystem.source.PlayOneShot(cursorSFX);
        }
        else if (spr_SpiritAnchor.enabled == true && entryPoint < 7)
        {
            selection = HeldPassiveItems.TabooRegenUpThingy;
            menuSystem.source.PlayOneShot(cursorSFX);
        }
        else if (entryPoint < 8)
        {
            if (spr_GlassBrand.enabled == true)
            {
                selection = HeldPassiveItems.WorldChangeToken;
                menuSystem.source.PlayOneShot(cursorSFX);
            }
            else if (spr_GlassKey.enabled == true)
            {
                selection = HeldPassiveItems.EndgameKey;
                menuSystem.source.PlayOneShot(cursorSFX);
            }
            else if (spr_Monolith0.enabled == true)
            {
                selection = HeldPassiveItems.ForestMonolithChunk;
                menuSystem.source.PlayOneShot(cursorSFX);
            }
            else if (spr_Monolith1.enabled == true)
            {
                selection = HeldPassiveItems.MarinaMonolithChunk;
                menuSystem.source.PlayOneShot(cursorSFX);
            }
            else if (spr_Monolith2.enabled == true)
            {
                selection = HeldPassiveItems.ValleyMonolithChunk;
                menuSystem.source.PlayOneShot(cursorSFX);
            }
        }
    }

    void RefreshTextbox()
    {
        int adjusted = (int)selection;
        int i = 0;
        while (adjusted > 1)
        {
            adjusted = adjusted >> 1;
            i++;
        }
        textbox.Play(Resources.Load<TextAsset>(GlobalStaticResourcePaths.p_passive_descs[i]));
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
    }

    public void PreOpen ()
    {
        lastSelected = selection;
        RefreshTextbox();
        gameStateManager = GameStateManager.Instance;
        _in_ToggleSprite(spr_Monolith0, HeldPassiveItems.ForestMonolithChunk);
        _in_ToggleSprite(spr_Monolith1, HeldPassiveItems.ValleyMonolithChunk);
        _in_ToggleSprite(spr_Monolith2, HeldPassiveItems.MarinaMonolithChunk);
        _in_ToggleSprite(spr_GlassBrand, HeldPassiveItems.WorldChangeToken);
        _in_ToggleSprite(spr_GlassKey, HeldPassiveItems.EndgameKey);
        _in_ToggleSprite(spr_DashAnklet, HeldPassiveItems.DashThingy);
        _in_ToggleSprite(spr_RollAnklet, HeldPassiveItems.DodgeBooster);
        _in_ToggleSprite(spr_HotRock, HeldPassiveItems.DodgeAttack);
        _in_ToggleSprite(spr_SeeingBell, HeldPassiveItems.SecretSensor);
        _in_ToggleSprite(spr_SpiritAnchor, HeldPassiveItems.TabooRegenUpThingy);
        _in_ToggleSprite(spr_SentinelsMask, HeldPassiveItems.AutoscrollRider);
        _in_ToggleSprite(spr_WingedCharm, HeldPassiveItems.RedBull);
        if (spr_GlassBrand.enabled == true || spr_GlassKey.enabled == true)
        {
            spr_Monolith0.enabled = false; // progression should make it impossible to have items from both sets of key items at the same time, but if you do, the monoliths don't appear in this menu so as to not break shit
            spr_Monolith1.enabled = false;
            spr_Monolith2.enabled = false;
        }

    }

    void _in_ToggleSprite(SpriteRenderer sprite, HeldPassiveItems item)
    {
        if ((gameStateManager.heldPassiveItems & item) == item)
        {
            sprite.enabled = true;
        }
        else
        {
            sprite.enabled = false;
        }
    }
}
