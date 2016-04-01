using UnityEngine;
using System.Collections;

public class PlayerShootShotgun : StateMachineBehaviour
{
    private bool active;
    private PlayerWeaponManager wpnManager;
    private int FrameCtr;
    private Collider[] roomColliders;
    private Collider2D collider;
    private AudioSource source;
    private AudioClip sfx;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sfx = Resources.Load("SFX/fire_sg") as AudioClip;
        source = animator.gameObject.GetComponent<AudioSource>();
        animator.SetInteger("HaltFrames", 10);
        if ((animator.GetInteger("SlotAWpn") == (int)PlayerWeapon.Shotgun && animator.GetBool("FireSlotB") == false) || (animator.GetInteger("SlotBWpn") == (int)PlayerWeapon.Shotgun && animator.GetBool("FireSlotB") == true))
        {
            active = true;
            wpnManager = animator.gameObject.GetComponent<PlayerController>().wpnManager;
            roomColliders = animator.gameObject.GetComponent<PlayerController>().world.cameraController.activeRoom.Colliders;
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
            if (FrameCtr % 90 == 0 || (animator.GetBool("Shooting") == false && animator.GetInteger("Cooldown") <= -1))
            {
                if (wpnManager.master.energy.CheckIfCanFireWpn(PlayerWeaponManager.ShotEnergyCosts[(int)PlayerWeapon.Shotgun]) == true)
                {
                    wpnManager.FireBullet(PlayerWeapon.Shotgun);
                    animator.SetInteger("HaltFrames", 30);
                    source.PlayOneShot(sfx, 0.5f);
                    animator.SetBool("Shooting", true);
                    animator.SetInteger("Cooldown", 90);
                    Vector3 PosMod;
                    switch (animator.GetInteger("FacingDir"))
                    {
                        case 0:
                            PosMod = new Vector3(0, 2, 0);
                            break;
                        case 1:
                            PosMod = new Vector3(0, -2, 0);
                            break;
                        case 2:
                            PosMod = new Vector3(2, 0, 0);
                            break;
                        case 3:
                            PosMod = new Vector3(-2, 0, 0);
                            break;
                        default:
                            throw new System.Exception("FacingDir out of bounds!");
                    }
                    ExpensiveAccurateCollision.CollideWithScenery(animator, roomColliders, PosMod, collider);
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

}
