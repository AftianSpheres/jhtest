using UnityEngine;
using System.Collections;

public class EnemyBossMidBoss : MonoBehaviour
{
    public CommonEnemyController common;
    public MidBossHeart heart;
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
	}
}
