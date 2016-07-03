using UnityEngine;
using System.Collections;

public class mu_IceFloor : MonoBehaviour
{
    public RoomController room;
    public Bounds[] boundses;
    public float momentumCap;
    public float momentumIncrement;

    bool playerIsOccupant;
    bool playerSpeedAdjusted;
    Vector3 playerMomentumValue;
    float ax;
    float ay;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (room.isActiveRoom == true)
        {
            playerIsOccupant = false;
            for (int i = 0; i < boundses.Length; i++)
            {
                if (boundses[i].Contains(room.world.player.collider.bounds.center) == true)
                {
                    playerIsOccupant = true;
                }
            }
            if (playerMomentumValue != Vector3.zero)
            {
                ExpensiveAccurateCollision.CollideWithScenery(room.world.player.mover, room.collision.allCollision, playerMomentumValue, room.world.player.collider, room.world.player.IgnoreCollision);
            }
            ax = 0;
            ay = 0;
            if (HardwareInterfaceManager.Instance.Down.Pressed == true)
            {
                ay = -momentumIncrement;
                if (playerMomentumValue.y > 0 && playerIsOccupant == false)
                {
                    playerMomentumValue = new Vector3(playerMomentumValue.x, 0, 0);
                }
            }
            else if (HardwareInterfaceManager.Instance.Up.Pressed == true)
            {
                ay = momentumIncrement;
                if (playerMomentumValue.y < 0 && playerIsOccupant == false)
                {
                    playerMomentumValue = new Vector3(playerMomentumValue.x, 0, 0);
                }
            }
            if (HardwareInterfaceManager.Instance.Left.Pressed == true)
            {
                ax = -momentumIncrement;
                if (playerMomentumValue.x > 0 && playerIsOccupant == false)
                {
                    playerMomentumValue = new Vector3(0, playerMomentumValue.y, 0);
                }
            }
            else if (HardwareInterfaceManager.Instance.Right.Pressed == true)
            {
                ax = momentumIncrement;
                if (playerMomentumValue.x < 0 && playerIsOccupant == false)
                {
                    playerMomentumValue = new Vector3(0, playerMomentumValue.y, 0);
                }
            }
            if (ax == 0 && ay == 0)
            {
                if (playerMomentumValue.x > 0)
                {
                    playerMomentumValue += Vector3.left * momentumIncrement;
                    if (playerMomentumValue.x < 0)
                    {
                        playerMomentumValue = new Vector3(0, playerMomentumValue.y, 0);
                    }
                }
                else if (playerMomentumValue.x < 0)
                {
                    playerMomentumValue += Vector3.right * momentumIncrement;
                    if (playerMomentumValue.x > 0)
                    {
                        playerMomentumValue = new Vector3(0, playerMomentumValue.y, 0);
                    }
                }
                if (playerMomentumValue.y > 0)
                {
                    playerMomentumValue += Vector3.down * momentumIncrement;
                    if (playerMomentumValue.y < 0)
                    {
                        playerMomentumValue = new Vector3(playerMomentumValue.x, 0, 0);
                    }
                }
                else if (playerMomentumValue.y < 0)
                {
                    playerMomentumValue += Vector3.up * momentumIncrement;
                    if (playerMomentumValue.y > 0)
                    {
                        playerMomentumValue = new Vector3(playerMomentumValue.x, 0, 0);
                    }
                }
            }
            if (playerIsOccupant == true)
            {
                if (playerSpeedAdjusted == false)
                {
                    room.world.player.animator.SetFloat(PlayerAnimatorHashes.paramExternalMoveSpeedMulti, 0.33f);
                    playerSpeedAdjusted = true;
                }
                if (ax != 0 || ay != 0)
                {
                    if (ax != 0 && ay != 0)
                    {
                        ax /= 2f;
                        ay /= 2f;
                    }
                    if (Mathf.Abs(playerMomentumValue.x) >= momentumCap)
                    {
                        ax = 0;
                    }
                    if (Mathf.Abs(playerMomentumValue.y) >= momentumCap)
                    {
                        ay = 0;
                    }
                }
                ax += playerMomentumValue.x;
                ay += playerMomentumValue.y;
                playerMomentumValue = new Vector3(ax, ay, 0);
            }
            else if (playerSpeedAdjusted == true)
            {
                room.world.player.animator.SetFloat(PlayerAnimatorHashes.paramExternalMoveSpeedMulti, 1.0f);
                playerSpeedAdjusted = false;
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < boundses.Length; i++)
        {
            Gizmos.DrawLine(new Vector3(boundses[i].min.x, boundses[i].min.y, 0), new Vector3(boundses[i].max.x, boundses[i].min.y, 0));
            Gizmos.DrawLine(new Vector3(boundses[i].max.x, boundses[i].min.y, 0), new Vector3(boundses[i].max.x, boundses[i].max.y, 0));
            Gizmos.DrawLine(new Vector3(boundses[i].min.x, boundses[i].min.y, 0), new Vector3(boundses[i].min.x, boundses[i].max.y, 0));
            Gizmos.DrawLine(new Vector3(boundses[i].min.x, boundses[i].max.y, 0), new Vector3(boundses[i].max.x, boundses[i].max.y, 0));
        }
    }
#endif
}
