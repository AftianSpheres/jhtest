using UnityEngine;

public class PlayerDodging : StateMachineBehaviour
{
    private Bounds[] roomColliders;
    private Collider2D collider;
    private PlayerController controller;
    private static int basicDodgeFrameLength = 30;
    private static int dodgeBoostLength = 10;
    private int dodgeBonus;
    int FrameCtr;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<PlayerController>();
        roomColliders = controller.world.activeRoom.collision.allCollision;
        collider = animator.GetComponent<Collider2D>();
        controller.source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_PlayerRollSFX));
        if ((controller.world.GameStateManager.heldPassiveItems & HeldPassiveItems.DodgeBooster) == HeldPassiveItems.DodgeBooster)
        {
            dodgeBonus = dodgeBoostLength;
            animator.SetFloat("DodgeAnimsSpeedMulti", 2f);          
        }
        else
        {
            dodgeBonus = 0;
            animator.SetFloat("DodgeAnimsSpeedMulti", 1f);
        }
        if ((controller.world.GameStateManager.heldPassiveItems & HeldPassiveItems.DodgeAttack) == HeldPassiveItems.DodgeAttack)
        {
            animator.SetBool("DodgeIsFireball", true);
        }
        else
        {
            animator.SetBool("DodgeIsFireball", false);
        }
        FrameCtr = 0;
    }

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FrameCtr++;
        Vector3 PosMod = new Vector3(0, 0, 0);
        if (FrameCtr >= basicDodgeFrameLength + dodgeBonus)
        {
            animator.SetBool(PlayerAnimatorHashes.triggerDodgeBurst, false);
        }
        else if (animator.GetBool(PlayerAnimatorHashes.triggerDodgeBurst) == true && (FrameCtr % 2 == 0 || FrameCtr * 2 < basicDodgeFrameLength + dodgeBonus))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FaceDown"))
            {
                PosMod = _in_RollInDirection(Vector3.down, animator, animator.GetBool("HeldDown"), true);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FaceUp"))
            {
                PosMod = _in_RollInDirection(Vector3.up, animator, animator.GetBool("HeldUp"), true);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FaceLeft"))
            {
                PosMod = _in_RollInDirection(Vector3.left, animator, animator.GetBool("HeldLeft"), false);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FaceRight"))
            {
                PosMod = _in_RollInDirection(Vector3.right, animator, animator.GetBool("HeldRight"), false);
            }
        }
        PosMod *= (animator.GetFloat(PlayerAnimatorHashes.paramMoveSpeed) * animator.GetFloat(PlayerAnimatorHashes.paramInternalMoveSpeedMulti) * animator.GetFloat(PlayerAnimatorHashes.paramExternalMoveSpeedMulti));
        ExpensiveAccurateCollision.CollideWithScenery(controller.mover, roomColliders, PosMod, collider);
    }

    private Vector3 _in_RollInDirection (Vector3 baseVector, Animator animator, bool input, bool moveVertically)
    {
        if (input == true)
        {
            baseVector = baseVector * 2;
        }
        else if (FrameCtr >= basicDodgeFrameLength)
        {
            animator.SetBool(PlayerAnimatorHashes.triggerDodgeBurst, false); // you can cancel dodges early if you have the dodge booster, so you're never forced into a longer animation because you're holding it
        }
        if (moveVertically == true)
        {
            if (animator.GetBool("HeldLeft") == true)
            {
                baseVector += Vector3.left;
            }
            else if (animator.GetBool("HeldRight") == true)
            {
                baseVector += Vector3.right;
            }
        }
        else
        {
            if (animator.GetBool("HeldDown") == true)
            {
                baseVector += Vector3.down;
            }
            else if (animator.GetBool("HeldUp") == true)
            {
                baseVector += Vector3.up;
            }
        }
        return baseVector;
    }
}
