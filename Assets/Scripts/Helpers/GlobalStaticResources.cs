using UnityEngine;
using System;
using System.Collections;

public static class GlobalStaticResources
{
    public static Func<String, TextAsset> loadTextResource = Resources.Load<TextAsset>;

    public static string p_wpn_names = "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn_names";
    public static string[] p_wpn_descs =
    {
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn00",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn01",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn02",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn03",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn04",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn05",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn06",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn07",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn08",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn09",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn10",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn11",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn12",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn13",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn14",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn15",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn16",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn17",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn18",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn19",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn20",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn21",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn22",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn23",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/wpn_menu/wpn24",
    };


    public static AudioClip CursorDownSFX = Resources.Load<AudioClip>("SFX/cursor_down");
    public static AudioClip CursorUpSFX = Resources.Load<AudioClip>("SFX/cursor_up");
    public static AudioClip FireBasicSFX = Resources.Load<AudioClip>("SFX/fire_wg");
    public static AudioClip FireShotgunSFX = Resources.Load<AudioClip>("SFX/fire_sg");
    public static AudioClip FireShadowSFX = Resources.Load<AudioClip>("SFX/fire_shd");
    public static AudioClip MenuCloseSFX = Resources.Load<AudioClip>("SFX/menu_close");
    public static AudioClip MenuOpenSFX = Resources.Load<AudioClip>("SFX/menu_open");
    public static AudioClip PlayerHitSFX = Resources.Load<AudioClip>("SFX/player_hit");
    public static AudioClip PlayerRollSFX = Resources.Load<AudioClip>("SFX/player_roll");
    public static Material FlashMat = Resources.Load<Material>("FlashMat");

    public static Sprite[] BoomGFX = Resources.LoadAll<Sprite>("GFX/boom");
    public static Sprite[] Boom0 = { BoomGFX[0], BoomGFX[1], BoomGFX[2], BoomGFX[3] };
    public static Sprite[] Boom1 = { BoomGFX[4], BoomGFX[5], BoomGFX[6], BoomGFX[7] };
    public static Sprite[] PlayerWeaponIcons = Resources.LoadAll<Sprite>("GFX/wpnicons");
}
