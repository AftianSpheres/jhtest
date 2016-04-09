using UnityEngine;
using System.Collections;

public enum BossMidBoss_Attacks
{
    None,
    ArrowRain,
    ThingyToss
}

public class EnemyBossMidBoss : MonoBehaviour
{
    public CommonEnemyController common;
    public MidBossHeart heart;
    public bool CurrentlyAttacking = false;
    public bool PanicMode = false;
    private static int[] VulnerableStates = { Animator.StringToHash("Base Layer.vuln"),
                                              Animator.StringToHash("Base Layer.ATK: Toss"),
                                              Animator.StringToHash("Base Layer.ATK: Arrow Rain") };
	
	// Update is called once per frame
	void Update ()
    {
        if (common.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == VulnerableStates[0]
         || common.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == VulnerableStates[1]
         || common.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == VulnerableStates[2])
        {
            heart.Vulnerable = true;
        }
        else
        {
            heart.Vulnerable = false;
        }
        if (common.CurrentHP < (common.MaxHP / 2))
        {
            PanicMode = true;
        }
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
            }
        }

    }

    private IEnumerator FireArrowRain()
    {
        int i;
        int r = Random.Range(0, 2);
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
    }
}
