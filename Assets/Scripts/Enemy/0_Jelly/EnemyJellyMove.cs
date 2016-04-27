using UnityEngine;
using System;
using System.Collections;

public class EnemyJellyMove : StateMachineBehaviour
{
    private PlayerReticleController reticle;
    private CommonEnemyController common;
    private Vector2 heading;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        common = animator.GetComponent<CommonEnemyController>();
        reticle = common.room.world.reticle;
	}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{


    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float cx;
        float cy;
        int r = UnityEngine.Random.Range(0, 64);
        if (r == 0 || r == 1)
        {
            if (reticle.transform.localPosition.x > -1 * reticle.HalfSizeOfReticleSprite)
            {
                cx = -1;
            }
            else if (reticle.transform.localPosition.x < -1 * reticle.HalfSizeOfReticleSprite)
            {
                cx = 1;
            }
            else
            {
                cx = UnityEngine.Random.Range(-1, 2);
            }
            if (reticle.transform.localPosition.y > 1)
            {
                cy = -1;
            }
            else if (reticle.transform.localPosition.y < 1)
            {
                cy = 1;
            }
            else
            {
                cy = UnityEngine.Random.Range(-1, 2);
            }

            heading = new Vector2(cx, cy);
        }
        else if (r == 2)
        {
            heading = Vector2.zero;
        }
        else if (r == 62 || r == 63)
        {
            cx = UnityEngine.Random.Range(-1, 2);
            cy = UnityEngine.Random.Range(-1, 2);
            heading = new Vector2(cx, cy);
        }

        float nx = animator.transform.position.x + heading.x;
        float ny = animator.transform.position.y + heading.y;
        if (nx + (common.collider.size.x) > common.register.room.bounds.max.x)
        {
            nx = animator.transform.position.x;
            heading = new Vector2(heading.x * -1, heading.y);
        }
        else if (nx < common.register.room.bounds.min.x)
        {
            nx = animator.transform.position.x;
            heading = new Vector2(heading.x * -1, heading.y);
        }
        if (ny > common.register.room.bounds.max.y)
        {
            ny = animator.transform.position.y;
            heading = new Vector2(heading.x, heading.y * -1);
        }
        else if (ny - (common.collider.size.y) < common.register.room.bounds.min.y)
        {
            ny = animator.transform.position.y;
            heading = new Vector2(heading.x, heading.y * -1);
        }
        animator.transform.position = new Vector3(nx, ny, animator.transform.position.z);
        common.Heading = heading;
    }

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
