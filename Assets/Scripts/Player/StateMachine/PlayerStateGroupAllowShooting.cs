﻿using UnityEngine;
using System.Collections;

public class PlayerStateGroupAllowShooting : StateMachineBehaviour {

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(PlayerAnimatorHashes.paramFiringAllowed, true);
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("CastTaboo") == true)
        {
            animator.Play("Taboo", 0);
            animator.SetBool("CastTaboo", false);
        }
	    else if (animator.GetBool("NowFiring"))
        {
            switch (animator.GetInteger("FacingDir"))
            {
                case 0:
                    animator.Play("OpenFire_D", 0);
                    break;
                case 1:
                    animator.Play("OpenFire_U", 0);
                    break;
                case 2:
                    animator.Play("OpenFire_L", 0);
                    break;
                case 3:
                    animator.Play("OpenFire_R", 0);
                    break;
                default:
                    throw new System.Exception("Player tried to open fire, but FacingDir is out of range!");
            }
        }
	}

	// OnStateMove is called before OnStateMove is called on any state inside this state machine
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called before OnStateIK is called on any state inside this state machine
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMachineEnter is called when entering a statemachine via its Entry Node
	//override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash){
	//
	//}

	// OnStateMachineExit is called when exiting a statemachine via its Exit Node
	//override public void OnStateMachineExit(Animator animator, int stateMachinePathHash) {
	//
	//}
}
