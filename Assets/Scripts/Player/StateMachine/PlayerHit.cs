using UnityEngine;
using System.Collections;

public class PlayerHit : StateMachineBehaviour {

    private PlayerController controller;
	 // this is a stub that just plays a clip 
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<PlayerController>();
        animator.SetBool("DodgeBurst", false);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.hasBeenHit = false;
    }
}
