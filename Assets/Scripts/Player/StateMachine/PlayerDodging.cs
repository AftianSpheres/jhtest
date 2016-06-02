using UnityEngine;
using System.Collections;
using System;

public class PlayerDodging : StateMachineBehaviour
{
    private Bounds[] roomColliders;
    private Collider2D collider;
    private PlayerController controller;
    private static int basicDodgeFrameLength = 24;
    private static int dodgeBoostLength = 12;
    private int dodgeBonus;
    bool DirReleased;
    int FrameCtr;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<PlayerController>();
        roomColliders = controller.world.activeRoom.collision.allCollision;
        collider = animator.GetComponent<Collider2D>();
        controller.source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_PlayerRollSFX));
        if ((controller.world.GameStateManager.heldPassiveItems & HeldPassiveItems.DodgeBooster) == HeldPassiveItems.DodgeBooster)
        {
            dodgeBonus = dodgeBoostLength;
        }
        else
        {
            dodgeBonus = 0;
        }
        FrameCtr = 0;


    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FrameCtr++;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1.0f;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 PosMod = new Vector3(0, 0, 0);
        if (animator.GetBool("DodgeBurst") == true && ((FrameCtr > dodgeBoostLength / 3f && FrameCtr <= 2 * ((dodgeBoostLength + dodgeBonus) / 3f)) || FrameCtr % 2 == 0))
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
                    if (FrameCtr >= basicDodgeFrameLength + dodgeBonus)
                    {
                        animator.SetBool("DodgeBurst", false);
                    }
                }
                else if (FrameCtr >= basicDodgeFrameLength * (2 / 3))
                {
                    animator.SetBool("DodgeBurst", false);
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
                    if (FrameCtr >= basicDodgeFrameLength + dodgeBonus)
                    {
                        animator.SetBool("DodgeBurst", false);
                    }
                }
                else if (FrameCtr >= basicDodgeFrameLength * (2 / 3))
                {
                    animator.SetBool("DodgeBurst", false);
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
                    if (FrameCtr >= basicDodgeFrameLength + dodgeBonus)
                    {
                        animator.SetBool("DodgeBurst", false);
                    }
                }
                else if (FrameCtr >= basicDodgeFrameLength * (2/3))
                {
                    animator.SetBool("DodgeBurst", false);
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
                    if (FrameCtr >= basicDodgeFrameLength + dodgeBonus)
                    {
                        animator.SetBool("DodgeBurst", false);
                    }
                }
                else if (FrameCtr >= basicDodgeFrameLength * (2f / 3))
                {
                    animator.SetBool("DodgeBurst", false);
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
