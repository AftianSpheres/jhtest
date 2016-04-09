using UnityEngine;
using System.Collections;

public class MidBossVuln : StateMachineBehaviour
{
    private int TossCtr;
    private int FrameCtr;
    private CommonEnemyController common;
    private EnemyBossMidBoss module;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        common = animator.gameObject.GetComponent<CommonEnemyController>();
        module = common.module as EnemyBossMidBoss;
        module.Attack(BossMidBoss_Attacks.ArrowRain);
        FrameCtr = 0;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
	    if (FrameCtr > 210)
        {
            animator.SetTrigger("Recover");
            animator.SetBool("ChargeIntoNeutral", true);
        }
        FrameCtr++;
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
