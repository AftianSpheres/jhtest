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
        roomColliders = animator.gameObject.GetComponent<PlayerController>().world.activeRoom.collision.allCollision;
        collider = animator.GetComponent<Collider2D>();
        master = animator.GetComponent<PlayerController>();
        if ((master.world.GameStateManager.heldPassiveItems & HeldPassiveItems.DodgeBooster) == HeldPassiveItems.DodgeBooster)
        {
            slideInterval = Mathf.FloorToInt(slideInterval / 2);
        }
        frameCtr = slideInterval - 1;
    }

	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
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
                    PosMod = _in_SlideInDir(Vector3.down, Input.GetKey(master.world.PlayerDataManager.K_VertUp), true);
                    break;
                case Direction.Up:
                    PosMod = _in_SlideInDir(Vector3.up, Input.GetKey(master.world.PlayerDataManager.K_VertDown), true);
                    break;
                case Direction.Left:
                    PosMod = _in_SlideInDir(Vector3.left, Input.GetKey(master.world.PlayerDataManager.K_HorizRight), false);
                    break;
                case Direction.Right:
                    PosMod = _in_SlideInDir(Vector3.right, Input.GetKey(master.world.PlayerDataManager.K_HorizLeft), false);
                    break;
            }
            ExpensiveAccurateCollision.CollideWithScenery(animator, roomColliders, PosMod, collider);
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
            if (Input.GetKey(master.world.PlayerDataManager.K_HorizLeft) == true)
            {
                baseVector += Vector3.left;
            }
            else if (Input.GetKey(master.world.PlayerDataManager.K_HorizRight) == true)
            {
                baseVector += Vector3.right;
            }
        }
        else
        {
            if (Input.GetKey(master.world.PlayerDataManager.K_VertDown) == true)
            {
                baseVector += Vector3.down;
            }
            else if (Input.GetKey(master.world.PlayerDataManager.K_VertUp) == true)
            {
                baseVector += Vector3.up;
            }
        }
        return baseVector;
    }
}
