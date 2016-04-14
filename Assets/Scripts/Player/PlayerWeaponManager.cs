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

	// Use this for initialization
	void Start ()
    {
        animator = master.animator;
        reticle = master.world.reticle;
        bulletPool = master.world.PlayerBullets;
        energy = master.energy;
	}
	
	// Update is called once per frame
	void Update ()
    {
        animator.SetInteger("SlotAWpn", (int)SlotAWpn);
        animator.SetInteger("SlotBWpn", (int)SlotBWpn);
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
        switch (shot)
        {
            case WeaponType.pWG:
            case WeaponType.pWGII:
                bulletPool.FireBullet(shot, 2f, CalcShotDamage(shot), 1, new Vector3(reticle.transform.position.x + reticle.HalfSizeOfReticleSprite, reticle.transform.position.y - reticle.HalfSizeOfReticleSprite, transform.position.z), master.bulletOrigin.transform.position);
                break;
            case WeaponType.pShotgun:
                for (int i = 0; i < 8; i++)
                {
                    bulletPool.FireBullet(shot, Random.Range(2.25f, 3.75f), CalcShotDamage(shot), 5, new Vector3(reticle.transform.position.x + reticle.HalfSizeOfReticleSprite + Random.Range(-3, 4), reticle.transform.position.y - reticle.HalfSizeOfReticleSprite + Random.Range(-3, 4), transform.position.z), master.bulletOrigin.transform.position);
                }
                break;
            case WeaponType.pShadow:
                bulletPool.FireBullet(shot, 5f, CalcShotDamage(shot), 1, new Vector3(reticle.transform.position.x + reticle.HalfSizeOfReticleSprite, reticle.transform.position.y - reticle.HalfSizeOfReticleSprite, transform.position.z), master.bulletOrigin.transform.position);
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
