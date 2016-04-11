using UnityEngine;
using System.Collections;

public enum BossMidBoss_Attacks
{
    None,
    ArrowRain,
    ThingyToss
}

public class EnemyBossMidBoss : EnemyModule
{
    public MidBossHeart heart;
    public CommonEnemyController[] eyes;
    public bool CurrentlyAttacking = false;
    public bool PanicMode = false;
    private static int[] VulnerableStates = { Animator.StringToHash("Base Layer.vuln") };
	
	// Update is called once per frame
	void Update ()
    {
        if (common.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == VulnerableStates[0])
        {
            heart.Vulnerable = true;
        }
        else
        {
            heart.Vulnerable = false;
        }
        PanicMode = true; // ...but this doesn't actually count for anything, unless...
        for (int i = 0; i < eyes.Length; i++)
        {
            if (eyes[i].isDead == false)
            {
                PanicMode = false; // any of the eyes is alive, we wait to start using the bullet-hell version of our arrow rain attack
            }
        }
    }

    new public void Respawn()
    {
        common.animator.ResetTrigger("Hide");
        common.animator.ResetTrigger("Charge");
        common.animator.ResetTrigger("Unhide");
        common.animator.ResetTrigger("HitWall");
        common.animator.ResetTrigger("Recover");
        common.animator.SetInteger("ChargeCount", -1);
        common.animator.SetInteger("ChargeDir", -1);
        common.animator.SetFloat("ChargeHeading_X", 0f);
        common.animator.SetFloat("ChargeHeading_Y", 0f);
        common.animator.SetBool("ChargeIntoNeutral", false);
    }

    public void Attack(BossMidBoss_Attacks atk)
    {
        if (CurrentlyAttacking == false)
        {
            switch (atk)
            {
                case BossMidBoss_Attacks.ArrowRain:
                    StartCoroutine(FireArrowRain());
                    break;
                case BossMidBoss_Attacks.ThingyToss:
                    Vector3 destination;
                    destination = new Vector3(float.MinValue, common.collider.bounds.center.y, common.collider.bounds.center.z );
                    common.room.world.EnemyBullets.FireBullet(PlayerWeapon.GenericEnemyShot, 2.0f, common.ShotDmg, 2, destination, common.collider.bounds.center, true);
                    destination = new Vector3(float.MaxValue, common.collider.bounds.center.y, common.collider.bounds.center.z);
                    common.room.world.EnemyBullets.FireBullet(PlayerWeapon.GenericEnemyShot, 2.0f, common.ShotDmg, 2, destination, common.collider.bounds.center, true);
                    destination = new Vector3(common.collider.bounds.center.x, float.MinValue, common.collider.bounds.center.z);
                    common.room.world.EnemyBullets.FireBullet(PlayerWeapon.GenericEnemyShot, 2.0f, common.ShotDmg, 2, destination, common.collider.bounds.center, true);
                    destination = new Vector3(common.collider.bounds.center.x, float.MaxValue, common.collider.bounds.center.z);
                    common.room.world.EnemyBullets.FireBullet(PlayerWeapon.GenericEnemyShot, 2.0f, common.ShotDmg, 2, destination, common.collider.bounds.center, true);
                    break;
            }
        }

    }

    private IEnumerator FireArrowRain()
    {
        int i;
        int r = Random.Range(0, 2);
        CurrentlyAttacking = true;
        if (r == 0 || PanicMode == true)
        {
            i = 0;
            while (i < 13)
            {
                int spacing = 0 + (i * 13);
                if (i % 2 == 0)
                {
                    Vector3 origin = new Vector3(common.room.bounds.min.x + spacing, common.room.bounds.max.y, transform.position.z);
                    Vector3 destination = new Vector3(common.room.bounds.min.x + spacing, common.room.bounds.min.y, transform.position.z);
                    common.room.world.EnemyBullets.FireBullet(PlayerWeapon.MidBoss_Arrow_Vert, 2.0f, common.ShotDmg, 2, destination, origin, true);
                }
                else
                {
                    Vector3 origin = new Vector3(common.room.bounds.min.x + spacing, common.room.bounds.min.y + 12, transform.position.z);
                    Vector3 destination = new Vector3(common.room.bounds.min.x + spacing, common.room.bounds.max.y + 12, transform.position.z);
                    common.room.world.EnemyBullets.FireBullet(PlayerWeapon.MidBoss_Arrow_Vert, 2.0f, common.ShotDmg, 2, destination, origin, true);
                }
                i++;
                yield return null;
            }
        }
        if (r == 1 || PanicMode == true)
        {
            i = 0;
            while (i < 13)
            {
                int spacing = 0 + (i * 13);
                if (i % 2 == 0)
                {
                    Vector3 origin = new Vector3(common.room.bounds.min.x, common.room.bounds.min.y + spacing, transform.position.z);
                    Vector3 destination = new Vector3(common.room.bounds.max.x, common.room.bounds.min.y + spacing, transform.position.z);
                    common.room.world.EnemyBullets.FireBullet(PlayerWeapon.MidBoss_Arrow_Horiz, 2.0f, common.ShotDmg, 2, destination, origin, true);
                }
                else
                {
                    Vector3 origin = new Vector3(common.room.bounds.max.x - 12, common.room.bounds.min.y + spacing, transform.position.z);
                    Vector3 destination = new Vector3(common.room.bounds.min.x - 12, common.room.bounds.min.y + spacing, transform.position.z);
                    common.room.world.EnemyBullets.FireBullet(PlayerWeapon.MidBoss_Arrow_Horiz, 2.0f, common.ShotDmg, 2, destination, origin, true);
                }
                i++;
                yield return null;
            }
        }
        CurrentlyAttacking = false;
    }
}
