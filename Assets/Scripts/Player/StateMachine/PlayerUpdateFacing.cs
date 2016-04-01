using UnityEngine;
using System.Collections;

public class PlayerUpdateFacing : StateMachineBehaviour
{

	// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsTag("FaceDown")) 
        {
            animator.SetInteger("FacingDir", 0);
        }
        else if (stateInfo.IsTag("FaceUp"))
        {
            animator.SetInteger("FacingDir", 1);
        }
        else if (stateInfo.IsTag("FaceLeft"))
        {
            animator.SetInteger("FacingDir", 2);
        }
        else if (stateInfo.IsTag("FaceRight"))
        {
            animator.SetInteger("FacingDir", 3);
        }
    }
}
