﻿using UnityEngine;
using System;
using System.Collections;

public class PlayerWalking : StateMachineBehaviour
{
    private bool OffFrame = false;
    private Bounds[] roomColliders;
    private Collider2D collider;
    private PlayerController controller;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        roomColliders = animator.gameObject.GetComponent<PlayerController>().world.activeRoom.collision.allCollision;
        collider = animator.GetComponent<Collider2D>();
        animator.SetInteger("FrameCtr", 0); // housekeeping: any state that uses FrameCtr needs to clean it up in OnStateEnter
        controller = animator.gameObject.GetComponent<PlayerController>();
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
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 PosMod = new Vector3(0, 0, 0);
        int FrameCtr = animator.GetInteger("FrameCtr");
        animator.SetInteger("FrameCtr", FrameCtr + 1);
        if (OffFrame == false)
        {
            if (animator.GetBool("HeldDown") == true)
            {
                if (animator.GetBool("HeldLeft") == true)
                {
                    PosMod = new Vector3(-1, -1, 0);
                    if (FrameCtr % 2 != 0)
                    {
                        OffFrame = true;
                    }
                }
                else if (animator.GetBool("HeldRight") == true)
                {
                    PosMod = new Vector3(1, -1, 0);
                    if (FrameCtr % 2 != 0)
                    {
                        OffFrame = true;
                    }
                }
                else
                {
                    PosMod = new Vector3(0, -1, 0);
                }
            }
            else if (animator.GetBool("HeldUp") == true)
            {
                if (animator.GetBool("HeldLeft") == true)
                {
                    PosMod = new Vector3(-1, 1, 0);
                    if (FrameCtr % 2 != 0)
                    {
                        OffFrame = true;
                    }
                }
                else if (animator.GetBool("HeldRight") == true)
                {
                    PosMod = new Vector3(1, 1, 0);
                    if (FrameCtr % 2 != 0)
                    {
                        OffFrame = true;
                    }
                }
                else
                {
                    PosMod = new Vector3(0, 1, 0);
                }
            }
            else if (animator.GetBool("HeldLeft") == true)
            {
                PosMod = new Vector3(-1, 0, 0);
            }
            else if (animator.GetBool("HeldRight") == true)
            {
                PosMod = new Vector3(1, 0, 0);
            }
        }
        else
        {
            OffFrame = false;
        }
        if (FrameCtr % 8 == 0)
        {
            PosMod *= 2;
        }
        PosMod *= (animator.GetFloat(PlayerAnimatorHashes.paramMoveSpeed) * animator.GetFloat(PlayerAnimatorHashes.paramInternalMoveSpeedMulti) * animator.GetFloat(PlayerAnimatorHashes.paramExternalMoveSpeedMulti));
        ExpensiveAccurateCollision.CollideWithScenery(controller.mover, roomColliders, PosMod, collider);

	}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
