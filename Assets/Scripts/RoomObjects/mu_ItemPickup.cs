using UnityEngine;
using System.Collections;

/// <summary>
/// Valid types of item pickup.
/// </summary>
public enum PickupType
{
    Weapon,
    PassiveItem,
    AreaKey,
    KeyItem
}

/// <summary>
/// A RoomObject representing an item (guns, etc.) which can be picked up and acquired.
/// </summary>
public class mu_ItemPickup : MonoBehaviour
{
    public RoomController room;
    public PickupType pickupType;
    public WeaponType weaponType;
	
	/// <summary>
    /// Monobehaviour.Update()
    /// </summary>
	void Update ()
    {
	    switch (pickupType)
        {
            case PickupType.Weapon:
                if (room.world.player.wpnManager.WpnUnlocks[(int)weaponType] == true)
                {
                    Destroy(gameObject);
                }
                break;
        }
	}
}
