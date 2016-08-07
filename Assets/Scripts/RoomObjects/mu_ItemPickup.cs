using UnityEngine;
using System.Collections;

/// <summary>
/// A RoomObject representing an item (guns, etc.) which can be picked up and acquired.
/// </summary>
public class mu_ItemPickup : MonoBehaviour
{
    public RoomController room;
    public PickupType pickupType;
    public WeaponType weaponType;
    public HeldPassiveItems passiveType;
    public mufm_Generic Flag;
    public int displayLifespan;
    private bool isOverhead;
    private bool stoppedBGM = false;
    private bool markedForDeath = false;
    AudioClip clip;
	
    void Awake ()
    {
        if (pickupType == PickupType.Weapon)
        {
            clip = Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_WeaponGetFanfare);
        }
        else if (pickupType == PickupType.PassiveItem)
        {
            if (passiveType == HeldPassiveItems.ForestMonolithChunk || passiveType == HeldPassiveItems.MarinaMonolithChunk || passiveType == HeldPassiveItems.ValleyMonolithChunk ||
                passiveType == HeldPassiveItems.WorldChangeToken || passiveType == HeldPassiveItems.EndgameKey)
            {
                clip = Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_ArtifactGetFanfare);
            }
            else
            {
                clip = Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_PassiveGetFanfare);
            }
        }
        else if (pickupType == PickupType.AreaKey)
        {
            clip = Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_KeySFX);
        }
    }

	/// <summary>
    /// Monobehaviour.Update()
    /// </summary>
	void Update ()
    {
        if (isOverhead == false)
        {
            if (Flag != null && Flag.CheckFlag() == true)
            {
                markedForDeath = true;
            }
            switch (pickupType)
            {
                case PickupType.Weapon:
                    int mask = 1 << ((int)weaponType);
                    if (((int)room.world.player.wpnManager.WpnUnlocks & mask) == mask) // even if the flag isn't set, if the weapon's already been obtained the pickup doesn't appear
                    {
                        markedForDeath = true;
                    }
                    break;
                case PickupType.PassiveItem:
                    if ((GameStateManager.Instance.heldPassiveItems & passiveType) == passiveType) // even if the flag isn't set, if the weapon's already been obtained the pickup doesn't appear
                    {
                        markedForDeath = true;
                    }
                    break;
            }
            if (markedForDeath == true)
            {
                if (stoppedBGM == true)
                {
                    room.world.BGM0.Play();
                }
                Destroy(gameObject);
            }
        }
        else if (displayLifespan > 0)
        {
            displayLifespan--;
            transform.position = room.world.player.transform.position + (Vector3.up * 8);
        }
        else
        {
            isOverhead = false;
            markedForDeath = true;
        }
    }

    /// <summary>
    /// Pickup() picks up pickup.
    /// </summary>
    public void Pickup ()
    {
        TextAsset text = default(TextAsset);
        switch (pickupType)
        {
            case PickupType.Weapon:
                room.world.player.wpnManager.AddWeapon(weaponType);
                text = Resources.Load<TextAsset>(GlobalStaticResourcePaths.p_wpn_descs[(int)weaponType]);
                room.world.player.DoSpecialPose();
                room.world.FanfarePlayer.Play(clip);
                break;
            case PickupType.PassiveItem:
                GameStateManager.Instance.heldPassiveItems |= passiveType;
                int i = 1;
                int index = 0;
                while (i < (int)passiveType)
                {
                    i = i << 1;
                    index++;
                }
                text = Resources.Load<TextAsset>(GlobalStaticResourcePaths.p_passive_descs[index]);
                room.world.player.DoSpecialPose();
                room.world.FanfarePlayer.Play(clip);
                break;
            case PickupType.AreaKey:
                if (room.world.Area > AreaType.None)
                {
                    room.world.GameStateManager.areaKeys[(int)room.world.Area]++;
                }
                else
                {
                    throw new System.Exception("Tried to pick up an area key, but WorldController.Area is defined as None!");
                }
                room.world.BGS0.PlayOneShot(clip);
                break;
            default:
                throw new System.Exception("Pickup " + gameObject.name + " is of invalid type " + pickupType);
        }
        if (Flag != null)
        {
            Flag.ActivateFlag();
        }
        if (text != null)
        {
            room.world.MainTextbox.StartPrinting(text);
        }
        transform.position = room.world.player.transform.position + (Vector3.up * 8);
        isOverhead = true;
    }
}
