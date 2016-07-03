using UnityEngine;

public class PlayerSlide : StateMachineBehaviour
{
    public int slideInterval;
    private int frameCtr;
    private Bounds[] roomColliders;
    private Collider2D collider;
    private PlayerController master;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController pc = animator.gameObject.GetComponent<PlayerController>();
        if (pc.world.activeRoom == null)
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
        else
        {
            roomColliders = pc.world.activeRoom.collision.allCollision;
            collider = animator.GetComponent<Collider2D>();
            master = animator.GetComponent<PlayerController>();
            if ((master.world.GameStateManager.heldPassiveItems & HeldPassiveItems.DodgeBooster) == HeldPassiveItems.DodgeBooster)
            {
                slideInterval = Mathf.FloorToInt(slideInterval / 2);
            }
            frameCtr = slideInterval - 1;
        }
    }

	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (roomColliders != null)
        {
            frameCtr++;
            if (frameCtr == slideInterval)
            {
                frameCtr = 0;
                Vector3 PosMod = Vector3.zero;
                Direction dir = (Direction)animator.GetInteger("FacingDir");
                switch (dir)
                {
                    case Direction.Down:
                        PosMod = _in_SlideInDir(Vector3.down, master.world.HardwareInterfaceManager.Down.Pressed, true);
                        break;
                    case Direction.Up:
                        PosMod = _in_SlideInDir(Vector3.up, master.world.HardwareInterfaceManager.Up.Pressed, true);
                        break;
                    case Direction.Left:
                        PosMod = _in_SlideInDir(Vector3.left, master.world.HardwareInterfaceManager.Left.Pressed, false);
                        break;
                    case Direction.Right:
                        PosMod = _in_SlideInDir(Vector3.right, master.world.HardwareInterfaceManager.Right.Pressed, false);
                        break;
                }
                ExpensiveAccurateCollision.CollideWithScenery(animator, roomColliders, PosMod, collider);
            }
        }
    }

    Vector3 _in_SlideInDir (Vector3 baseVector, bool input, bool moveVertically)
    {
        if (input == true)
        {
            baseVector = Vector3.zero;
        }
        else if (moveVertically == true)
        {
            if (master.world.HardwareInterfaceManager.Left.Pressed == true)
            {
                baseVector += Vector3.left;
            }
            else if (master.world.HardwareInterfaceManager.Right.Pressed == true)
            {
                baseVector += Vector3.right;
            }
        }
        else
        {
            if (master.world.HardwareInterfaceManager.Down.Pressed == true)
            {
                baseVector += Vector3.down;
            }
            else if (master.world.HardwareInterfaceManager.Up.Pressed == true)
            {
                baseVector += Vector3.up;
            }
        }
        return baseVector;
    }
}
