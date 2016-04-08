using UnityEngine;
using System;
using System.Collections;

public class MidBossCharge : StateMachineBehaviour {
    private Vector3 ChargeHeading;
    private CommonEnemyController common;
    private Direction ChargeDir;
    private Vector3 LogicalPosition;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ChargeDir = (Direction)animator.GetInteger("ChargeDir");
        ChargeHeading = new Vector3(animator.GetFloat("ChargeHeading_X"), animator.GetFloat("ChargeHeading_Y"), 0);
        common = animator.gameObject.GetComponent<CommonEnemyController>();
        LogicalPosition = animator.transform.position;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 llp = LogicalPosition;
        LogicalPosition += ChargeHeading;
        Vector3 PosMod = new Vector3((float)Math.Round(LogicalPosition.x - llp.x, 0, MidpointRounding.AwayFromZero), (float)Math.Round(LogicalPosition.y - llp.y, 0, MidpointRounding.AwayFromZero), 0);
        if (ExpensiveAccurateCollision.CollideWithScenery(animator, common.room.Colliders, PosMod, common.collider) == true)
        {
            animator.SetTrigger("HitWall");
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
