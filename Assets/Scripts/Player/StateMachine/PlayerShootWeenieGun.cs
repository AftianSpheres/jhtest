using UnityEngine;
using System.Collections;

public class PlayerShootWeenieGun : StateMachineBehaviour
{
    private bool active;
    private PlayerWeaponManager wpnManager;
    private int FrameCtr;
    private Bounds[] roomColliders;
    private Collider2D collider;
    private AudioSource source;
    private AudioClip sfx;
    private int wt;
    private bool b;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sfx = GlobalStaticResources.FireBasicSFX;
        source = animator.gameObject.GetComponent<AudioSource>();
        animator.SetInteger("HaltFrames", 10);
        if (animator.GetBool("FireSlotB") == true)
        {
            wt = animator.GetInteger("SlotBWpn");
            b = true;
        }
        else
        {
            wt = animator.GetInteger("SlotAWpn");
            b = false;
        }
        RoomController room = animator.gameObject.GetComponent<PlayerController>().world.activeRoom;
        if (((wt == (int)WeaponType.pWG || wt == (int)WeaponType.pWGII) && animator.GetBool("FireSlotB") == b) && room != null)
        {
            active = true;
            wpnManager = animator.gameObject.GetComponent<PlayerController>().wpnManager;
            roomColliders = room.collision.allCollision;
            collider = animator.GetComponent<Collider2D>();
        }
        else
        {
            active = false;
        }
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (active == true)
        {
            FrameCtr = animator.GetInteger("FrameCtr");
            if (FrameCtr % 20 == 0 || (animator.GetBool("Shooting") == false && animator.GetInteger("Cooldown") <= -1))
            {
                if ((animator.GetBool("HeldFire1") == true && animator.GetBool("FireSlotB") == false) || (animator.GetBool("HeldFire2") == true && animator.GetBool("FireSlotB") == true))
                {
                    wpnManager.FireBullet((WeaponType)wt);
                    source.PlayOneShot(sfx, 0.5f);
                    animator.SetBool("FireRoundDone", true);
                    animator.SetBool("Shooting", true);
                    animator.SetInteger("Cooldown", 20);
                }
            }
        }
	}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (active == true && animator.GetInteger("HaltFrames") <= 0)
        {
            Vector3 PosMod = new Vector3(0, 0, 0);
            if (FrameCtr % 2 == 0)
            {
                if (animator.GetBool("HeldDown") == true)
                {
                    if (animator.GetBool("HeldLeft") == true)
                    {
                        if (FrameCtr % 4 == 0)
                        {
                            PosMod = new Vector3(-1 * animator.GetInteger("MoveSpeed"), -1 * animator.GetInteger("MoveSpeed"), 0);
                        }
                    }
                    else if (animator.GetBool("HeldRight") == true)
                    {
                        if (FrameCtr % 4 == 0)
                        {
                            PosMod = new Vector3(animator.GetInteger("MoveSpeed"), -1 * animator.GetInteger("MoveSpeed"), 0);
                        }
                    }
                    else
                    {
                        PosMod = new Vector3(0, -1 * animator.GetInteger("MoveSpeed"), 0);
                    }
                }
                else if (animator.GetBool("HeldUp") == true)
                {
                    if (animator.GetBool("HeldLeft") == true)
                    {
                        if (FrameCtr % 4 == 0)
                        {
                            PosMod = new Vector3(-1 * animator.GetInteger("MoveSpeed"), animator.GetInteger("MoveSpeed"), 0);
                        }
                    }
                    else if (animator.GetBool("HeldRight") == true)
                    {
                        if (FrameCtr % 4 == 0)
                        {
                            PosMod = new Vector3(animator.GetInteger("MoveSpeed"), animator.GetInteger("MoveSpeed"), 0);
                        }           
                    }
                    else
                    {
                        PosMod = new Vector3(0, animator.GetInteger("MoveSpeed"), 0);
                    }
                }
                else if (animator.GetBool("HeldLeft") == true)
                {
                    PosMod = new Vector3(-1 * animator.GetInteger("MoveSpeed"), 0, 0);
                }
                else if (animator.GetBool("HeldRight") == true)
                {
                    PosMod = new Vector3(animator.GetInteger("MoveSpeed"), 0, 0);
                }
            }
            ExpensiveAccurateCollision.CollideWithScenery(animator, roomColliders, PosMod, collider);
        }
           
   }

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

}
