using UnityEngine;
using System.Collections;

public class EnemyJellyFire : StateMachineBehaviour
{
    private CommonEnemyController controller;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<CommonEnemyController>();
        controller.room.world.EnemyBullets.FireBullet(PlayerWeapon.EnemyShot, 1.0f, controller.ShotDmg, 1, controller.room.world.player.GetComponent<Collider2D>().bounds.center, controller.transform.position);
        controller.room.world.EnemyBullets.FireBullet(PlayerWeapon.EnemyShot, 2.0f, controller.ShotDmg, 1, controller.room.world.player.GetComponent<Collider2D>().bounds.center, controller.transform.position);
        controller.room.world.EnemyBullets.FireBullet(PlayerWeapon.EnemyShot, 3.0f, controller.ShotDmg, 1, controller.room.world.player.GetComponent<Collider2D>().bounds.center, controller.transform.position);
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

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
