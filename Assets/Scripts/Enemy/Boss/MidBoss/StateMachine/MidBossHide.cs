using UnityEngine;
using System;
using System.Collections;

public class MidBossHide : StateMachineBehaviour
{
    private CommonEnemyController common;
    private int ChargeCount;
    private int FrameCtr = 0;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        common = animator.gameObject.GetComponent<CommonEnemyController>();

        // If we're entering into this with a ChargeCount stored in the animator, we use that.

        if (animator.GetInteger("ChargeCount") > 0)
        {
            ChargeCount = animator.GetInteger("ChargeCount");
        }

        // Otherwise: Decide how many charges we're going to make while in shadow form.
        // 1-3 above 50% HP, 2-4 above 25%, 3-5 after that

        else
        {
            ChargeCount = UnityEngine.Random.Range(1, 4);
            if (common.CurrentHP <= common.MaxHP / 4)
            {
                ChargeCount += 2;
            }
            else if (common.CurrentHP <= common.MaxHP / 2)
            {
                ChargeCount += 1;
            }
        }
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // After hiding for 30 frames, calculate our next charge and launch into it.

        if (FrameCtr > 30)
        {
            Vector3 TransformOffset = common.room.world.player.transform.position - animator.transform.position;
            float nf = 1 / (Math.Abs(TransformOffset.x) + Math.Abs(TransformOffset.y));
            animator.SetFloat("ChargeHeading_X", nf * TransformOffset.x * 3);
            animator.SetFloat("ChargeHeading_Y", nf * TransformOffset.y * 3);
            if (Math.Abs(TransformOffset.y) > Math.Abs(TransformOffset.x))
            {
                animator.SetInteger("ChargeDir", (int)Direction.Down);
            }
            else if (TransformOffset.x < 0)
            {
                animator.SetInteger("ChargeDir", (int)Direction.Left);
            }
            else
            {
                animator.SetInteger("ChargeDir", (int)Direction.Right);
            }
            animator.SetTrigger("Charge");
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
