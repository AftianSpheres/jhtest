using UnityEngine;
using System.Collections;

public class MidBossWait : StateMachineBehaviour
{
    private CommonEnemyController common;
    private Vector3 PlayerLastFramePos;
    private int FrameCtr = 0;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        common = animator.gameObject.GetComponent<CommonEnemyController>();
        PlayerLastFramePos = common.room.world.player.transform.position;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Housekeeping

        FrameCtr++;
        Vector3 PosMod = common.room.world.player.transform.position - PlayerLastFramePos;
        
        // Match the player's movements

        ExpensiveAccurateCollision.CollideWithScenery(animator, common.room.Colliders, PosMod, common.collider);

        // ...if we've spent more than 3/4 of a second, decide whether or not to try and attack.
        // More common when we're low on health. Always attack if the player is below 33% energy.

        bool AttackThisFrame;

        if (FrameCtr > 45)
        {
            if (common.room.world.player.energy.CheckIfCanFireWpn(33, false) == false)
            {
                AttackThisFrame = true;
            }
            else if (Random.Range(common.MaxHP - common.CurrentHP, common.MaxHP *300f) < common.MaxHP)
            {
                AttackThisFrame = true;
                if (AttackThisFrame == true)
                {
                    animator.SetTrigger("Hide");
                }
            }
        }

        // Housekeeping

        PlayerLastFramePos = common.room.world.player.transform.position;
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
