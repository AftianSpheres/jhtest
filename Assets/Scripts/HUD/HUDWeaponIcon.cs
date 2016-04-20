using UnityEngine;
using System.Collections;

public class HUDWeaponIcon : MonoBehaviour
{
    public WorldController world;
    public PlayerWeaponManager wpnManager;
    new public SpriteRenderer renderer;
    public bool isSlotB;

	// Use this for initialization
	void Start ()
    {
        wpnManager = world.player.wpnManager;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (isSlotB == false && wpnManager.SlotAWpn > WeaponType.None)
        {
            renderer.sprite = GlobalStaticResources.PlayerWeaponIcons[(int)wpnManager.SlotAWpn];
        }
        else if (wpnManager.SlotBWpn > WeaponType.None)
        {
            renderer.sprite = GlobalStaticResources.PlayerWeaponIcons[(int)wpnManager.SlotBWpn];
        }
	}
}
