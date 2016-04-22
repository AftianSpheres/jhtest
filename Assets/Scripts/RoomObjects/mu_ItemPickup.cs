using UnityEngine;
using System.Collections;

public enum PickupType
{
    Weapon,
    PassiveItem,
    AreaKey,
    KeyItem
}

public class mu_ItemPickup : MonoBehaviour
{
    public RoomController room;
    public PickupType pickupType;
    public WeaponType weaponType;
	
	// Update is called once per frame
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
