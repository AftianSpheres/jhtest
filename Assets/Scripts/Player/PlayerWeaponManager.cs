using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(PlayerController))]
public class PlayerWeaponManager : MonoBehaviour
{
    public PlayerController master;
    private Animator animator;
    private PlayerReticleController reticle;
    private BulletPool bulletPool;
    private PlayerEnergy energy;
    public PlayerWeapon SlotAWpn;
    public PlayerWeapon SlotBWpn;
    public bool[] WpnUnlocks;
    public static int[] ShotEnergyCosts =
        {
        0, // weenie gun
        5, // shotgun
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

    public int CalcShotDamage(PlayerWeapon shot)
    {
        if (shot == PlayerWeapon.WeenieGun)
        {
            return energy.Level;
        }
        else
        {
            return ShotEnergyCosts[(int)shot] * energy.Level;
        }
    }

    public void FireBullet(PlayerWeapon shot)
    {

        switch (shot)
        {
            case PlayerWeapon.WeenieGun:
                bulletPool.FireBullet(shot, 2f, CalcShotDamage(shot), 1, new Vector3(reticle.transform.position.x + reticle.HalfSizeOfReticleSprite, reticle.transform.position.y - reticle.HalfSizeOfReticleSprite, transform.position.z), transform.position);
                break;
            case PlayerWeapon.Shotgun:
                for (int i = 0; i < 8; i++)
                {
                    bulletPool.FireBullet(shot, Random.Range(2.25f, 3.75f), CalcShotDamage(shot), 5, new Vector3(reticle.transform.position.x + reticle.HalfSizeOfReticleSprite + Random.Range(-3, 4), reticle.transform.position.y - reticle.HalfSizeOfReticleSprite + Random.Range(-3, 4), transform.position.z), transform.position);
                }
                break;
            default:
                throw new System.Exception("Tried to fire an invalid player weapon type");

        }
    }

}
