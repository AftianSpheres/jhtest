﻿using UnityEngine;
using System.Collections;

/// <summary>
/// HUD weapon icon element.
/// </summary>
public class HUDWeaponIcon : MonoBehaviour
{
    public WorldController world;
    public PlayerWeaponManager wpnManager;
    new public SpriteRenderer renderer;
    public WeaponType wpnValueCache;
    public bool isSlotB;
    private Sprite[] frames;

	/// <summary>
    /// Monobehaviour.Start()
    /// </summary>
	void Start ()
    {
        wpnManager = world.player.wpnManager;
        frames = Resources.LoadAll<Sprite>(GlobalStaticResourcePaths.p_PlayerWeaponIcons);
    }
	
	/// <summary>
    /// Monobehaviour.Update()
    /// </summary>
	void Update ()
    {
	    if (isSlotB == false)
        {
            if (wpnManager.SlotAWpn != wpnValueCache)
            {
                wpnValueCache = wpnManager.SlotAWpn;
                if (wpnManager.SlotAWpn == WeaponType.None)
                {
                    renderer.sprite = default(Sprite);
                }
                else
                {
                    renderer.sprite = frames[(int)wpnManager.SlotAWpn];
                }
            }
        }
        else
        {
            if (wpnManager.SlotBWpn != wpnValueCache)
            {
                wpnValueCache = wpnManager.SlotBWpn;
                if (wpnManager.SlotBWpn == WeaponType.None)
                {
                    renderer.sprite = default(Sprite);
                }
                else
                {
                    renderer.sprite = frames[(int)wpnManager.SlotBWpn];
                }
            }
        }
	}
}
