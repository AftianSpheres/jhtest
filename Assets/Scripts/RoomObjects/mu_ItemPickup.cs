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
    private bool markedForDeath = false;
	
	/// <summary>
    /// Monobehaviour.Update()
    /// </summary>
	void Update ()
    {
        if (markedForDeath == true)
        {
            Destroy(gameObject);
            room.world.BGM0.Play();
        }
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

    public void Pickup ()
    {
        TextAsset text = default(TextAsset);
        switch (pickupType)
        {
            case PickupType.Weapon:
                room.world.player.wpnManager.AddWeapon(weaponType);
                text = Resources.Load<TextAsset>(GlobalStaticResources.p_wpn_descs[(int)weaponType]);
                break;
        }
        room.world.player.DoSpecialPose();
        transform.position = room.world.player.transform.position + (Vector3.up * 16);
        room.world.MainTextbox.StartPrinting(text);
        room.world.BGM0.Stop();
        room.world.BGS0.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResources.p_WeaponGetFanfare));
        markedForDeath = true;
    }
}
