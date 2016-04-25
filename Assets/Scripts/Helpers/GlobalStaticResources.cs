using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Static class containing always-loaded resources for easy use.
/// There should be a reasonable expectation that anything here could be called at any time during gameplay!
/// If that doesn't hold, DON'T PUT IT HERE.
/// </summary>
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


    public static string p_CursorDownSFX = "SFX/cursor_down";
    public static string p_CursorUpSFX = "SFX/cursor_up";
    public static string p_FireBasicSFX = "SFX/fire_wg";
    public static string p_FireShotgunSFX = "SFX/fire_sg";
    public static string p_FireShadowSFX = "SFX/fire_shd";
    public static string p_MenuCloseSFX = "SFX/menu_close";
    public static string p_MenuOpenSFX = "SFX/menu_open";
    public static string p_PlayerHitSFX = "SFX/player_hit";
    public static string p_PlayerRollSFX = "SFX/player_roll";
    public static string p_TextPrintSFX = "SFX/text_print";
    public static string p_WeaponGetFanfare = "SFX/ff_wpn";
    public static string p_FlashMat = "FlashMat";

    public static string p_BoomGFX = "GFX/boom";
    public static int[] i_Boom0 = { 0, 1, 2, 3 };
    public static int[] i_Boom1 = { 4, 5, 6, 7 };
    public static string p_PlayerWeaponIcons = "GFX/wpnicons";

    public static string p_EnergyRecoverGFX = "GFX/energy_recover";

}
