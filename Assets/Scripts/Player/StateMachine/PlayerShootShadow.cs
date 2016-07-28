using UnityEngine;
using System.Collections;

public class PlayerShootShadow : StateMachineBehaviour
{
    private bool active;
    private PlayerWeaponManager wpnManager;
    private int FrameCtr;
    private AudioSource source;
    private AudioClip sfx;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sfx = Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_FireShadowSFX);
        source = animator.gameObject.GetComponent<AudioSource>();
        if ((animator.GetInteger("SlotAWpn") == (int)WeaponType.pShadow && animator.GetBool("FireSlotB") == false) || (animator.GetInteger("SlotBWpn") == (int)WeaponType.pShadow && animator.GetBool("FireSlotB") == true))
        {
            active = true;
            wpnManager = animator.gameObject.GetComponent<PlayerController>().wpnManager;
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
                if ((animator.GetBool("HeldFire1") == true && animator.GetBool("FireSlotB") == false) || (animator.GetBool("HeldFire2") == true && animator.GetBool("FireSlotB") == true))
                {
                    wpnManager.master.energy.Damage(PlayerWeaponManager.ShotEnergyCosts[(int)WeaponType.pShadow], true);
                    wpnManager.FireBullet(WeaponType.pShadow);
                    source.PlayOneShot(sfx, 0.5f);
                    animator.SetBool("Shooting", true);
                    animator.SetInteger("Cooldown", 10);
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
