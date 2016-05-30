using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Static class containing always-loaded resources for easy use.
/// There should be a reasonable expectation that anything here could be called at any time during gameplay!
/// If that doesn't hold, DON'T PUT IT HERE.
/// </summary>
public static class GlobalStaticResourcePaths
{
    public static Func<String, TextAsset> loadTextResource = Resources.Load<TextAsset>;
    public static string p_boss_names = "Text/" + HammerConstants.LocalizationPrefix + "/system/boss_names";
    public static string p_subregion_names = "Text/" + HammerConstants.LocalizationPrefix + "/system/world/subregions";
    public static string p_wpn_names = "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn_names";
    public static string[] p_wpn_shortdescs =
    {
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn00_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn01_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn02_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn03_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn04_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn05_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn06_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn07_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn08_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn09_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn10_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn11_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn12_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn13_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn14_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn15_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn16_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn17_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn18_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn19_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn20_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn21_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn22_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn23_short",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn24_short",
    };


    public static string p_CursorDownSFX = "SFX/cursor_down";
    public static string p_CursorUpSFX = "SFX/cursor_up";
    public static string p_EnemyFireStrangeBurstSFX = "SFX/enemy_fire_strangeburst";
    public static string p_ExplosionSFX = "SFX/explosion";
    public static string p_FireBasicSFX = "SFX/fire_wg";
    public static string p_FireShotgunSFX = "SFX/fire_sg";
    public static string p_FireShadowSFX = "SFX/fire_shd";
    public static string p_KeySFX = "SFX/key";
    public static string p_MenuCloseSFX = "SFX/menu_close";
    public static string p_MenuOpenSFX = "SFX/menu_open";
    public static string p_PlayerHitSFX = "SFX/player_hit";
    public static string p_PlayerRollSFX = "SFX/player_roll";
    public static string p_SubregionPopupFanfare = "SFX/ff_subregion_popup";
    public static string p_TextPrintSFX = "SFX/text_print";
    public static string p_WeaponGetFanfare = "SFX/ff_wpn";
    public static string p_FlashMat = "FlashMat";

    public static string p_BoomGFX = "GFX/World/boom";
    public static int[] i_Boom0 = { 0, 1, 2, 3 };
    public static int[] i_Boom1 = { 4, 5, 6, 7 };
    public static string p_PlayerWeaponIcons = "GFX/UI/HUD/wpnicons";
    public static string p_TabooOverlay_Eyes = "GFX/UI/TabooOverlays/taboo_eyes";

    public static string p_EnergyRecoverGFX = "GFX/World/energy_recover";

}
