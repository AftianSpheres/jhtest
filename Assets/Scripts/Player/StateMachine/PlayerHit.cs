using UnityEngine;
using System.Collections;

public class PlayerHit : StateMachineBehaviour {

    private AudioSource source;
    private PlayerController controller;
    private AudioClip clip = GlobalStaticResources.PlayerHitSFX;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        source = animator.gameObject.GetComponent<AudioSource>();
        source.PlayOneShot(clip);
        controller = animator.gameObject.GetComponent<PlayerController>();
        animator.SetBool("DodgeBurst", false);
        controller.InvulnTime = 30;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controller.KnockbackFrames > 0 && controller.world.activeRoom != null)
        {
            ExpensiveAccurateCollision.CollideWithScenery(animator, controller.world.activeRoom.collision.allCollision, controller.KnockbackHeading, controller.collider);
            controller.KnockbackFrames--;
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
