using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages player's current weapons
/// and available weaponry.
/// </summary>
[RequireComponent (typeof(PlayerController))]
public class PlayerWeaponManager : MonoBehaviour
{
    public PlayerController master;
    private PlayerReticleController reticle;
    private BulletPool bulletPool;
    private PlayerEnergy energy;
    public TabooType Taboo;
    public WeaponType SlotAWpn;
    public WeaponType SlotBWpn;
    public HeldWeapons WpnUnlocks;
    public PlayerWeapon lastFiredWeapon;
    public bool TabooReady = true;
    public int TabooCooldownTimer;
    public static int TabooCooldownTime = 3600;
    public static int[] ShotEnergyCosts =
        {
        2, // weenie gun
        2, // wgII
        15, // shotgun
        20, // shadow
        5, // flamethrower
        3 // icicle
        };
    public static int[] ShotBaseDamages =
        {
        10, // weenie gun
        50, // wgII
        10, // shotgun
        20, // shadow
        15, // flamethrower
        5, // icicle
        };

    /// <summary>
    /// MonoBehaviour.Start()
    /// </summary>
    void Start ()
    {
        reticle = master.world.reticle;
        bulletPool = master.world.PlayerBullets;
        energy = master.energy;
        WpnUnlocks = master.world.GameStateManager.heldWeapons;
        Taboo = master.world.GameStateManager.activeTaboo;
        ChangeActiveWeapon(master.world.GameStateManager.activePlayerWeapons[0]);
        ChangeActiveWeapon(master.world.GameStateManager.activePlayerWeapons[1], true);

    }

    /// <summary>
    /// MonoBehaviour.Update()
    /// </summary>
    void Update()
    {
        if (TabooReady == false)
        {
            TabooCooldownTimer--;
            if ((master.world.GameStateManager.heldPassiveItems & HeldPassiveItems.TabooRegenUpThingy) == HeldPassiveItems.TabooRegenUpThingy)
            {
                TabooCooldownTimer--;
            }
            if (TabooCooldownTimer <= 0)
            {
                TabooReady = true;
            }
        }
    }

    /// <summary>
    /// Unlocks a weapon, and sets it to a slot if none are full.
    /// </summary>
    public void AddWeapon(WeaponType wpn)
    {
        HeldWeapons wpnToMask;
        if (wpn == WeaponType.None || (int)wpn > HammerConstants.NumberOfWeapons + 1)
        {
            throw new System.Exception("Tried to add invalid weapon of type " + wpn.ToString());
        }
        else
        {
            wpnToMask = (HeldWeapons)(1 << (int)wpn);
        }
        if ((WpnUnlocks & wpnToMask) != wpnToMask)
        {
            WpnUnlocks |= wpnToMask;
            if (SlotAWpn == WeaponType.None)
            {
                ChangeActiveWeapon(wpn);
            }
            else if (SlotBWpn == WeaponType.None)
            {
                ChangeActiveWeapon(wpn, true);
            }
        }
        master.world.GameStateManager.heldWeapons = WpnUnlocks;
    }

    /// <summary>
    /// Calculates damage dealt by shot.
    /// Returns int.
    /// </summary>
    public int CalcShotDamage(WeaponType shot)
    {
        int s = 1;
        if (energy.isBerserk == true)
        {
            s = 2;
        }
        s *= energy.Level * ShotBaseDamages[(int)shot];
        return s;
    }

    public void ChangeActiveWeapon(WeaponType wpn, bool isSlotB = false)
    {
        if (isSlotB == false)
        {
            SlotAWpn = wpn;
            master.world.GameStateManager.activePlayerWeapons[0] = SlotAWpn;
        }
        else
        {
            SlotBWpn = wpn;
            master.world.GameStateManager.activePlayerWeapons[1] = SlotBWpn;
        }
    }

    /// <summary>
    /// Fires a player shot of type passed as argument.
    /// </summary>
    public void FireBullet(WeaponType shot)
    {
        BoxCollider2D homingTarget = default(BoxCollider2D);
        float nearestDist = float.MaxValue;
        Vector3 s = master.bulletOrigin.renderer.bounds.center;
        Vector3 r = (new Vector3(reticle.transform.position.x + reticle.HalfSizeOfReticleSprite, reticle.transform.position.y - reticle.HalfSizeOfReticleSprite, transform.position.z));
        for (int i = 0; i < master.world.activeRoom.Enemies.Length; i++)
        {
            CommonEnemyController cec = master.world.activeRoom.Enemies[i].GetComponent<CommonEnemyController>();
            if (cec != null)
            {
                Vector3 point = cec.collider.bounds.ClosestPoint(r);
                if (Mathf.Abs(r.x - point.x) + Mathf.Abs(r.y - point.y) < nearestDist && cec.collider.enabled == true)
                {
                    nearestDist = Mathf.Abs(r.x - point.x) + Mathf.Abs(r.y - point.y);
                    homingTarget = cec.collider;
                }
            }
        }
        switch (shot)
        {
            case WeaponType.pWG:
            case WeaponType.pWGII:
                bulletPool.FireBullet(shot, 2f, CalcShotDamage(shot), 1, r, s, false);
                break;
            case WeaponType.pShotgun:
                for (int i = 0; i < 8; i++)
                {
                    bulletPool.FireBullet(shot, Random.Range(2.25f, 3.75f), CalcShotDamage(shot), 5, r + Random.Range(-3, 4) * Vector3.right + Random.Range(-3, 4) * Vector3.up, s);
                }
                break;
            case WeaponType.pShadow:
                bulletPool.FireBullet(shot, 5f, CalcShotDamage(shot), 1, r, s, false, null, 0, int.MaxValue, gameObject, 160);
                master.collider.enabled = false;
                master.Locked = true;
                master.animator.enabled = false;
                master.renderer.enabled = false;
                master.fs.skip = true;
                master.isVanished = true;
                master.bulletOrigin.gameObject.SetActive(false);
                // We restore all of these things when the bullet hits, except we change our position to match the bullet's current position
                break;
            case WeaponType.pFlamethrower:
                bulletPool.FireBullet(shot, 1f, CalcShotDamage(shot), 2, r, s, true, null, 0, int.MaxValue, gameObject, 40);
                break;
            default:
                throw new System.Exception("Tried to fire an invalid player weapon type");

        }
    }

    /// <summary>
    /// Invokes current Taboo.
    /// </summary>
    public void InvokeTaboo()
    {
        switch (Taboo)
        {
            case TabooType.Eyes:
                StartCoroutine(in_Taboo_Eyes());
                break;
            default:
                throw new System.Exception("Invoked invalid Taboo of value " + Taboo.ToString());
        }
    }

    /// <summary>
    /// Handles Taboo: Eyes
    /// </summary>
    IEnumerator in_Taboo_Eyes ()
    {
        master.InvulnTime += 90;
        Vector3 newHeading;
        switch (master.facingDir)
        {
            case Direction.Down:
                newHeading = Vector3.down;
                break;
            case Direction.Up:
                newHeading = Vector3.up;
                break;
            case Direction.Left:
                newHeading = Vector3.left;
                break;
            case Direction.Right:
                newHeading = Vector3.right;
                break;
            default:
                throw new System.Exception("Invalid player facing direction: " + master.facingDir.ToString());
        }
        for (int i = 0; i < 30; i++)
        {
            for (int i2 = 0; i2 < master.world.EnemyBullets.allBullets.Count; i2++)
            {
                if (master.world.EnemyBullets.allBullets[i2].gameObject.activeInHierarchy == true)
                {
                    master.world.EnemyBullets.allBullets[i2].Heading = newHeading;
                    master.world.EnemyBullets.allBullets[i2].tag = "PlayerBullet";
                }
            }
            yield return null;
        }
    }
}
