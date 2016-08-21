using UnityEngine;

public class PlayerDodging : StateMachineBehaviour
{
    public AudioClip clip;
    private Bounds[] roomColliders;
    private Collider2D collider;
    private PlayerController controller;
    private static int basicDodgeFrameLength = 45;
    private static int dodgeBoostLength = 11;
    private int dodgeBonus;
    int FrameCtr;
    bool hitWall = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<PlayerController>();
        if (controller.world.activeRoom != null)
        {
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
        else
        {
            switch ((Direction)animator.GetInteger(PlayerAnimatorHashes.paramFacingDir))
            {
                case Direction.Down:
                    animator.Play(PlayerAnimatorHashes.PlayerStand_D);
                    break;
                case Direction.Up:
                    animator.Play(PlayerAnimatorHashes.PlayerStand_U);
                    break;
                case Direction.Left:
                    animator.Play(PlayerAnimatorHashes.PlayerStand_L);
                    break;
                case Direction.Right:
                    animator.Play(PlayerAnimatorHashes.PlayerStand_R);
                    break;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(PlayerAnimatorHashes.paramDodgeCooldown, 45);
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
        else if (hitWall == true)
        {
            if (FrameCtr % 4 == 0)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FaceDown"))
                {
                    PosMod = Vector3.up * 1;
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FaceUp"))
                {
                    PosMod = Vector3.down * 1;
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FaceLeft"))
                {
                    PosMod = Vector3.right * 1;
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FaceRight"))
                {
                    PosMod = Vector3.left * 1;
                }
            }
        }
        PosMod *= (animator.GetFloat(PlayerAnimatorHashes.paramMoveSpeed) * animator.GetFloat(PlayerAnimatorHashes.paramInternalMoveSpeedMulti) * animator.GetFloat(PlayerAnimatorHashes.paramExternalMoveSpeedMulti));
        bool hit = false;
        if (ExpensiveAccurateCollision.CollideWithScenery(controller.mover, roomColliders, PosMod, collider) == true)
        {
            hit = true;
        }
        if (hit == true)
        {
            Vector3 v0;
            Vector3 v1;
            Vector3 vBase;
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FaceDown"))
            {
                v0 = Vector3.left;
                v1 = Vector3.right;
                vBase = Vector3.down;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FaceUp"))
            {
                v0 = Vector3.left;
                v1 = Vector3.right;
                vBase = Vector3.up;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FaceLeft"))
            {
                v0 = Vector3.up;
                v1 = Vector3.down;
                vBase = Vector3.left;
            }
            else
            {
                v0 = Vector3.up;
                v1 = Vector3.down;
                vBase = Vector3.right;
            }
            Vector3 prospectivePosMod;
            controller.mover.heading = Vector3.zero;
            for (int i = 1; i < 7; i++)
            {
                prospectivePosMod = (v0 * i);
                if (ExpensiveAccurateCollision.CollideWithScenery(controller.mover, roomColliders, prospectivePosMod, collider) == false && 
                    ExpensiveAccurateCollision.CollideWithScenery(controller.mover, roomColliders, vBase, collider) == false) hit = false;
                else
                {
                    controller.mover.heading = Vector3.zero;
                    prospectivePosMod = (v1 * i);
                    if (ExpensiveAccurateCollision.CollideWithScenery(controller.mover, roomColliders, prospectivePosMod, collider) == false &&
                        ExpensiveAccurateCollision.CollideWithScenery(controller.mover, roomColliders, vBase, collider) == false) hit = false;
                    else controller.mover.heading = Vector3.zero;
                }
                if (hit == false) break;
            }
            if (hit == true)
            {
                animator.SetBool(PlayerAnimatorHashes.triggerDodgeBurst, false);
                animator.SetBool("DodgeHitWall", true);
                hitWall = true;
                controller.source.PlayOneShot(clip);
            }
        }
    }

    private Vector3 _in_RollInDirection (Vector3 _baseVector, Animator animator, bool input, bool moveVertically)
    {
        Vector3 baseVector = _baseVector;
        if (input == true)
        {
            baseVector = baseVector * 2;
        }
        else if (FrameCtr >= basicDodgeFrameLength)
        {
            animator.SetBool(PlayerAnimatorHashes.triggerDodgeBurst, false); // you can cancel dodges early if you have the dodge booster, so you're never forced into a longer animation because you're holding it
        }
        if (FrameCtr > (basicDodgeFrameLength + dodgeBonus) * (2/3f))
        {
            baseVector -= _baseVector;
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
