using UnityEngine;
using System;

public class PlayerShootCommon : StateMachineBehaviour
{
    private PlayerController player;
    private GameObject reticle;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.gameObject.GetComponent<PlayerController>();
        reticle = player.world.reticle.gameObject;
    }

	// OnStateMove is called before OnStateMove is called on any state inside this state machine
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player != null && reticle != null)
        {
            float dx = player.transform.position.x - reticle.transform.position.x;
            float dy = player.transform.position.y - reticle.transform.position.y;
            if (Math.Abs(dy) > Math.Abs(dx))
            {
                if (dy < 0)
                {
                    animator.SetInteger("FacingDir", 1);
                }
                else
                {
                    animator.SetInteger("FacingDir", 0);
                }
            }
            else
            {
                if (dx < 0)
                {
                    animator.SetInteger("FacingDir", 3);
                }
                else
                {
                    animator.SetInteger("FacingDir", 2);
                }
            }
        }
    }

	// OnStateIK is called before OnStateIK is called on any state inside this state machine
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

}
