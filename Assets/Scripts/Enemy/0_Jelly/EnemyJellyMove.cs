using UnityEngine;
using System;
using System.Collections;

public class EnemyJellyMove : StateMachineBehaviour
{
    private PlayerReticleController reticle;
    private CommonEnemyController common;
    private Vector2 heading;

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (common == null || reticle == null)
        {
            common = animator.GetComponent<CommonEnemyController>();
            reticle = common.room.world.reticle;
        }
        else
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
            else if (r == 2 || r == 3 || r == 4 || r == 5)
            {
                heading = Vector2.zero;
            }
            else if (r == 6 || r == 7)
            {
                heading *= -1;
            }
            else if (r == 62 || r == 63)
            {
                cx = UnityEngine.Random.Range(-1, 2);
                cy = UnityEngine.Random.Range(-1, 2);
                heading = new Vector2(cx, cy);
            }
            Vector2 modifiedHeading;
            if (r % 2 == 0)
            {
                modifiedHeading = heading * 2;
            }
            else
            {
                modifiedHeading = heading;
            }
            float nx = animator.transform.position.x + modifiedHeading.x;
            float ny = animator.transform.position.y + modifiedHeading.y;
            if (nx + (common.collider.size.x) > common.register.room.bounds.max.x)
            {
                nx = animator.transform.position.x;
                modifiedHeading = new Vector2(modifiedHeading.x * -1, modifiedHeading.y);
                heading = new Vector2(heading.x * -1, heading.y);
            }
            else if (nx < common.register.room.bounds.min.x)
            {
                nx = animator.transform.position.x;
                modifiedHeading = new Vector2(modifiedHeading.x * -1, modifiedHeading.y);
                heading = new Vector2(heading.x * -1, heading.y);
            }
            if (ny > common.register.room.bounds.max.y)
            {
                ny = animator.transform.position.y;
                modifiedHeading = new Vector2(modifiedHeading.x, modifiedHeading.y * -1);
                heading = new Vector2(heading.x, heading.y * -1);
            }
            else if (ny - (common.collider.size.y) < common.register.room.bounds.min.y)
            {
                ny = animator.transform.position.y;
                modifiedHeading = new Vector2(modifiedHeading.x, modifiedHeading.y * -1);
                heading = new Vector2(heading.x, heading.y * -1);
            }
            common.mover.heading += new Vector3(modifiedHeading.x, modifiedHeading.y, 0);
            common.Heading = modifiedHeading;
        }
    }

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
