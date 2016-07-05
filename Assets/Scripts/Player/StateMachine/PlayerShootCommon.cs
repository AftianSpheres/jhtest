using UnityEngine;
using System;
using System.Collections;
public class PlayerShootCommon : StateMachineBehaviour
{
    private PlayerController player;
    private GameObject reticle;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("Shooting") == false)
        {
            animator.SetInteger("FrameCtr", 0); // housekeeping: any state that uses FrameCtr needs to clean it up in OnStateEnter
            animator.SetInteger("Cooldown", 0);
        }
        player = animator.gameObject.GetComponent<PlayerController>();
        reticle = player.world.reticle.gameObject;
    }

	// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int FrameCtr = animator.GetInteger("FrameCtr");
        int HaltFrames = animator.GetInteger("HaltFrames");
        animator.SetInteger("FrameCtr", FrameCtr + 1);
        if (HaltFrames > 0)
        {
            animator.SetInteger("HaltFrames", HaltFrames - 1);
        }
        animator.SetInteger("Cooldown", animator.GetInteger("Cooldown") - 1);
    }

	// OnStateExit is called before OnStateExit is called on any state inside this state machine
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   // {
    
   // }

	// OnStateMove is called before OnStateMove is called on any state inside this state machine
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player.transform.position != null && reticle.transform.position != null)
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
