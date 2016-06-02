using UnityEngine;
using System.Collections;

public class PlayerSlide : StateMachineBehaviour
{
    private int frameCtr;
    private Bounds[] roomColliders;
    private Collider2D collider;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frameCtr = 0;
        roomColliders = animator.gameObject.GetComponent<PlayerController>().world.activeRoom.collision.allCollision;
        collider = animator.GetComponent<Collider2D>();
    }

	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frameCtr++;
        if (frameCtr == 1)
        {
            frameCtr = 0;
            Vector3 PosMod = Vector3.zero;
            Direction dir = (Direction)animator.GetInteger("FacingDir");
            switch (dir)
            {
                case Direction.Down:
                    PosMod = Vector3.down;
                    break;
                case Direction.Up:
                    PosMod = Vector3.up;
                    break;
                case Direction.Left:
                    PosMod = Vector3.left;
                    break;
                case Direction.Right:
                    PosMod = Vector3.right;
                    break;
            }
            ExpensiveAccurateCollision.CollideWithScenery(animator, roomColliders, PosMod, collider);
        }
    }
}
