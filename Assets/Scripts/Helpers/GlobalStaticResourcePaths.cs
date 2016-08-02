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
    public static string p_passive_names = "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/passive_names";
    public static string[] p_passive_descs =
{
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/passive00",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/passive01",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/passive02",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/passive03",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/passive04",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/passive05",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/passive06",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/passive07",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/passive08",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/passive09",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/passive10",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/passive11",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/passive12",
    };
    public static string p_subregion_names = "Text/" + HammerConstants.LocalizationPrefix + "/system/world/subregions";
    public static string p_windowed_resolutions = "Text/" + HammerConstants.LocalizationPrefix + "/system/windowed_resolutions";
    public static string p_wpn_names = "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn_names";
    public static string[] p_wpn_descs =
    {
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn00",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn01",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn02",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn03",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn04",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn05",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn06",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn07",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn08",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn09",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn10",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn11",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn12",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn13",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn14",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn15",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn16",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn17",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn18",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn19",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn20",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn21",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn22",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn23",
        "Text/" + HammerConstants.LocalizationPrefix + "/system/inventory/wpn24",
    };

    public static string p_CursorDownSFX = "SFX/cursor_down";
    public static string p_CursorUpSFX = "SFX/cursor_up";
    public static string p_EnemyFireStrangeBurstSFX = "SFX/enemy_fire_strangeburst";
    public static string p_ExplosionSFX = "SFX/explosion";
    public static string p_FireBasicSFX = "SFX/fire_wg";
    public static string p_FireShotgunSFX = "SFX/fire_sg";
    public static string p_FireShadowSFX = "SFX/fire_shd";
    public static string p_FireFlameSFX = "SFX/door";
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
