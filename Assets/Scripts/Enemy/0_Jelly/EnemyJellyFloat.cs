using UnityEngine;
using System.Collections;

public class EnemyJellyFloat : StateMachineBehaviour
{

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int ff = animator.GetInteger("FloatingFrame");
        if (ff >= 64)
        {
            animator.SetInteger("FloatingFrame", -1);
        }
        else if (ff < 32 && ff % 8 == 0)
        {
            animator.transform.position = animator.transform.position + Vector3.up;
        }
        else if (ff % 8 == 0)
        {
            animator.transform.position = animator.transform.position + Vector3.down;
        }
        animator.SetInteger("FloatingFrame", animator.GetInteger("FloatingFrame") + 1);
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
