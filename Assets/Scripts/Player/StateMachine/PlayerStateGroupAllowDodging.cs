using UnityEngine;
using System.Collections;

public class PlayerStateGroupAllowDodging : StateMachineBehaviour {
    static int[] dodgeHashes = { Animator.StringToHash("DodgeDown"), Animator.StringToHash("DodgeUp"), Animator.StringToHash("DodgeLeft"), Animator.StringToHash("DodgeRight"),
                                 Animator.StringToHash("Base Layer.StdStates.Dodging.PlayerDodge_D"), Animator.StringToHash("Base Layer.StdStates.Dodging.PlayerDodge_U"),
                                 Animator.StringToHash("Base Layer.StdStates.Dodging.PlayerDodge_L"), Animator.StringToHash("Base Layer.StdStates.Dodging.PlayerDodge_R")};


	// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool(dodgeHashes[0]) == true)
        {
            animator.Play(dodgeHashes[4], 0);
            animator.SetBool(dodgeHashes[0], false);
            animator.SetBool(PlayerAnimatorHashes.paramFiringAllowed, false);
        }
        else if (animator.GetBool(dodgeHashes[1]) == true)
        {
            animator.Play(dodgeHashes[5], 0);
            animator.SetBool(dodgeHashes[0], false);
            animator.SetBool(PlayerAnimatorHashes.paramFiringAllowed, false);
        }
        else if (animator.GetBool(dodgeHashes[2]) == true)
        {
            animator.Play(dodgeHashes[6], 0);
            animator.SetBool(dodgeHashes[0], false);
            animator.SetBool(PlayerAnimatorHashes.paramFiringAllowed, false);
        }
        else if (animator.GetBool(dodgeHashes[3]) == true)
        {
            animator.Play(dodgeHashes[7], 0);
            animator.SetBool(dodgeHashes[0], false);
            animator.SetBool(PlayerAnimatorHashes.paramFiringAllowed, false);
        }
    }
}
