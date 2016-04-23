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
    public bool isSlotB;

	/// <summary>
    /// Monobehaviour.Start()
    /// </summary>
	void Start ()
    {
        wpnManager = world.player.wpnManager;
	}
	
	/// <summary>
    /// Monobehaviour.Update()
    /// </summary>
	void Update ()
    {
	    if (isSlotB == false)
        {
            if (wpnManager.SlotAWpn == WeaponType.None)
            {
                renderer.sprite = default(Sprite);
            }
            else
            {
                renderer.sprite = GlobalStaticResources.PlayerWeaponIcons[(int)wpnManager.SlotAWpn];
            }
        }
        else
        {
            if (wpnManager.SlotBWpn == WeaponType.None)
            {
                renderer.sprite = default(Sprite);
            }
            else
            {
                renderer.sprite = GlobalStaticResources.PlayerWeaponIcons[(int)wpnManager.SlotBWpn];
            }
        }
	}
}
