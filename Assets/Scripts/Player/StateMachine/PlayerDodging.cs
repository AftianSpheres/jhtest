using UnityEngine;
using System.Collections;
using System;

public class PlayerDodging : StateMachineBehaviour
{
    private Bounds[] roomColliders;
    private Collider2D collider;
    bool DirReleased;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        roomColliders = animator.gameObject.GetComponent<PlayerController>().world.activeRoom.collision.allCollision;
        collider = animator.GetComponent<Collider2D>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1.0f;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 PosMod = new Vector3(0, 0, 0);
        if (animator.GetBool("DodgeBurst") == true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FaceDown"))
            {
                if (animator.GetBool("HeldLeft") == true)
                {
                    PosMod = new Vector3(-1, -1, 0);
                }

                else if (animator.GetBool("HeldRight") == true)
                {
                    PosMod = new Vector3(1, -1, 0);
                }

                else
                {
                    PosMod = new Vector3(0, -1, 0);
                }

                if (animator.GetBool("HeldDown") == true)
                {
                    PosMod = PosMod + new Vector3(0, -1, 0);
                    animator.speed = 1.0f;
                }
                else
                {
                    animator.speed = 1.1f;
                }
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FaceUp"))
            {
                if (animator.GetBool("HeldLeft") == true)
                {
                    PosMod = new Vector3(-1, 1, 0);
                }

                else if (animator.GetBool("HeldRight") == true)
                {
                    PosMod = new Vector3(1, 1, 0);
                }

                else
                {
                    PosMod = new Vector3(0, 1, 0);
                }

                if (animator.GetBool("HeldUp") == true)
                {
                    PosMod = PosMod + new Vector3(0, 1, 0);
                    animator.speed = 1.0f;
                }
                else
                {
                    animator.speed = 1.1f;
                }
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FaceLeft"))
            {
                if (animator.GetBool("HeldDown") == true)
                {
                    PosMod = new Vector3(-1, -1, 0);
                }

                else if (animator.GetBool("HeldUp") == true)
                {
                    PosMod = new Vector3(-1, 1, 0);
                }

                else
                {
                    PosMod = new Vector3(-1, 0, 0);
                }

                if (animator.GetBool("HeldLeft") == true)
                {
                    PosMod = PosMod + new Vector3(-1, 0, 0);
                    animator.speed = 1.0f;
                }
                else
                {
                    animator.speed = 1.1f;
                }
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FaceRight"))
            {
                if (animator.GetBool("HeldDown") == true)
                {
                    PosMod = new Vector3(1, -1, 0);
                }

                else if (animator.GetBool("HeldUp") == true)
                {
                    PosMod = new Vector3(1, 1, 0);
                }

                else
                {
                    PosMod = new Vector3(1, 0, 0);
                }

                if (animator.GetBool("HeldRight") == true)
                {
                    PosMod = PosMod + new Vector3(1, 0, 0);
                    animator.speed = 1.0f;
                }
                else
                {
                    animator.speed = 1.1f;
                }
            }
        }
        ExpensiveAccurateCollision.CollideWithScenery(animator, roomColliders, PosMod, collider);
    }

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
