using UnityEngine;
using System.Collections;

public class PlayerHit : StateMachineBehaviour {

    private PlayerController controller;
	 // this is a stub that just plays a clip 
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<PlayerController>();
        animator.SetBool(PlayerAnimatorHashes.triggerDodgeBurst, false);
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controller.KnockbackFrames > 0)
        {
            ExpensiveAccurateCollision.CollideWithScenery(controller.mover, controller.world.activeRoom.collision.allCollision, controller.KnockbackHeading, controller.collider);
            controller.KnockbackFrames--;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.hasBeenHit = false;
    }
}
