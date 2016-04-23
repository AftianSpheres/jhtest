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
    private Animator animator;
    private PlayerReticleController reticle;
    private BulletPool bulletPool;
    private PlayerEnergy energy;
    public WeaponType SlotAWpn;
    public WeaponType SlotBWpn;
    public bool[] WpnUnlocks;
    public static int[] ShotEnergyCosts =
        {
        0, // weenie gun
        0, // wgII
        25, // shotgun
        33, // shadow
        10, // flamethrower
        3 // icicle
        };
    public static int[] DamageMultipliers =
        {
        1, // weenie gun
        5, // wgII
        1, // shotgun
        2, // shadow
        1, // flamethrower
        1, // icicle
        };
    private static int SlotAWpnHash = Animator.StringToHash("SlotAWpn");
    private static int SlotBWpnHash = Animator.StringToHash("SlotBWpn");

    /// <summary>
    /// MonoBehaviour.Start()
    /// </summary>
    void Start ()
    {
        animator = master.animator;
        reticle = master.world.reticle;
        bulletPool = master.world.PlayerBullets;
        energy = master.energy;
	}
	
	/// <summary>
    /// MonoBehaviour.Update()
    /// </summary>
	void Update ()
    {
        animator.SetInteger(SlotAWpnHash, (int)SlotAWpn);
        animator.SetInteger(SlotBWpnHash, (int)SlotBWpn);
    }

    /// <summary>
    /// Unlocks a weapon, and sets it to a slot if none are full.
    /// </summary>
    /// <param name="wpn"></param>
    public void AddWeapon(WeaponType wpn)
    {
        if (WpnUnlocks[(int)wpn] == false)
        {
            WpnUnlocks[(int)wpn] = true;
            if (SlotAWpn == WeaponType.None)
            {
                SlotAWpn = wpn;
            }
            else if (SlotBWpn == WeaponType.None)
            {
                SlotBWpn = wpn;
            }
        }
    }

    /// <summary>
    /// Calculates damage dealt by shot.
    /// Returns int.
    /// </summary>
    public int CalcShotDamage(WeaponType shot)
    {
        if (ShotEnergyCosts[(int)shot] == 0)
        {
            return energy.Level * DamageMultipliers[(int)shot];
        }
        else
        {
            return ShotEnergyCosts[(int)shot] * energy.Level * DamageMultipliers[(int)shot];
        }
    }

    /// <summary>
    /// Fires a player shot of type passed as argument.
    /// </summary>
    public void FireBullet(WeaponType shot)
    {
        BoxCollider2D homingTarget = default(BoxCollider2D);
        float nearestDist = float.MaxValue;
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
                bulletPool.FireBullet(shot, 2f, CalcShotDamage(shot), 1, r, master.bulletOrigin.transform.position, false);
                break;
            case WeaponType.pShotgun:
                for (int i = 0; i < 8; i++)
                {
                    bulletPool.FireBullet(shot, Random.Range(2.25f, 3.75f), CalcShotDamage(shot), 5, r + Random.Range(-3, 4) * Vector3.right + Random.Range(-3, 4) * Vector3.up, master.bulletOrigin.transform.position);
                }
                break;
            case WeaponType.pShadow:
                bulletPool.FireBullet(shot, 5f, CalcShotDamage(shot), 1, r, master.bulletOrigin.transform.position);
                master.collider.enabled = false;
                master.Locked = true;
                animator.enabled = false;
                master.renderer.enabled = false;
                master.fs.skip = true;
                master.bulletOrigin.gameObject.SetActive(false);
                // We restore all of these things when the bullet hits, except we change our position to match the bullet's current position
                break;
            default:
                throw new System.Exception("Tried to fire an invalid player weapon type");

        }
    }

}
