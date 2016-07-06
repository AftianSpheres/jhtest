using UnityEngine;
using System.Collections;

public class EnemyJellyFire : StateMachineBehaviour
{
    private CommonEnemyController controller;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<CommonEnemyController>();
        Vector3 hiPos;
        Vector3 loPos;
        float px = controller.transform.position.x - controller.room.world.player.collider.bounds.center.x;
        float py = controller.transform.position.y - controller.room.world.player.collider.bounds.center.y;

        if (Mathf.Abs(px) > Mathf.Abs(py))
        {
            hiPos = controller.room.world.player.collider.bounds.center + (16f * Vector3.up);
            loPos = controller.room.world.player.collider.bounds.center + (16f * Vector3.down);
        }
        else
        {
            hiPos = controller.room.world.player.collider.bounds.center + (16f * Vector3.right);
            loPos = controller.room.world.player.collider.bounds.center + (16f * Vector3.left);
        }
        int speed = Random.Range(2, 4);
        controller.room.world.EnemyBullets.FireBullet(WeaponType.eGenericMid, speed, controller.ShotDmg, 1, loPos, controller.collider.bounds.center);
        controller.room.world.EnemyBullets.FireBullet(WeaponType.eGenericMid, speed, controller.ShotDmg, 1, controller.room.world.player.collider.bounds.center, controller.collider.bounds.center);
        controller.room.world.EnemyBullets.FireBullet(WeaponType.eGenericMid, speed, controller.ShotDmg, 1, hiPos, controller.collider.bounds.center);
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
