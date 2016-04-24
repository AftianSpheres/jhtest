﻿using UnityEngine;
using System.Collections;

public class CommonEnemyInvuln : StateMachineBehaviour
{
    private SpriteRenderer renderer;
    private Material mat1;
    private Material mat2;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        renderer = animator.gameObject.GetComponent<SpriteRenderer>();
        mat1 = renderer.material;
        mat2 = Resources.Load<Material>(GlobalStaticResources.p_FlashMat);
	}

	// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int invuln = animator.GetInteger("InvulnTime");
        if (invuln > 0)
        {
            animator.SetInteger("InvulnTime", invuln - 1);
            if (invuln % 16 == 0)
            {
                renderer.material = mat2;
            }
            else if (invuln % 8 == 0)
            {
                renderer.material = mat1;
            }
        }
	}

	// OnStateExit is called before OnStateExit is called on any state inside this state machine
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

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
