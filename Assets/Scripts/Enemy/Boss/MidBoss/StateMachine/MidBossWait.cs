using UnityEngine;
using System.Collections;

public class MidBossWait : StateMachineBehaviour
{
    private CommonEnemyController common;
    private EnemyBossMidBoss module;
    private int FrameCtr;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        common = animator.gameObject.GetComponent<CommonEnemyController>();
        module = common.module as EnemyBossMidBoss;
        FrameCtr = 0;
        animator.SetBool("ChargeIntoNeutral", false);
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Housekeeping

        FrameCtr++;

        // ...if we've spent more than 3/4 of a second, decide whether or not to try and attack.
        // Always attack if the player is below 33% energy.

        bool ChargeThisFrame = false;

        if (FrameCtr % 60 == 0)
        {
            module.Attack(BossMidBoss_Attacks.ThingyToss);
        }
        else if (FrameCtr > 240)
        {
            if (common.room.world.player.energy.CheckIfCanFireWpn(33, false) == false)
            {
                ChargeThisFrame = true;
            }
            else if (Random.Range(0, 90) == 0)
            {
                ChargeThisFrame = true;
            }
            if (ChargeThisFrame == true)
            {
                animator.SetTrigger("Hide");
            }
        }
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
