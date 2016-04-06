using UnityEngine;
using System.Collections;

/// <summary>
/// A room. Stores room data. Does room things.
/// </summary>
public class RoomController : MonoBehaviour
{
    public WorldController world;
    public AudioClip bgm;
    public RegisteredSprite[] Enemies;
    public RegisteredSprite[] NonEnemyOccupants;
    public mu_Checkpoint RoomCheckpoint;
    public BoxCollider[] Colliders;
    public Bounds bounds;
    public Vector2 BigRoomCellSize;
    public uint xPosition;
    public uint yPosition;
    public bool isActiveRoom;
    public Vector2[] EntryPoints;
    private GameStateManager gameStateManager;


    // Use this for initialization
    void Awake ()
    {
        if (BigRoomCellSize.x > 1 || BigRoomCellSize.y > 1)
        {
            for (int iy = 0; iy < BigRoomCellSize.y; iy++)
            {
                for (int ix = 0; ix < BigRoomCellSize.x; ix++)
                {
                    world.rooms[yPosition + iy, xPosition + ix] = this;
                }
            }
        }
        else
        {
            world.rooms[yPosition, xPosition] = this;
        }
        for (int i = 0; i < Enemies.Length; i++)
            {
            Enemies[i].room = this;
            }
        gameStateManager = GameObject.Find("Universe/GameStateManager").GetComponent<GameStateManager>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (world.activeRoom == this)
        {
            if (isActiveRoom == false)
            {
                WakeUpOccupants();
            }
            isActiveRoom = true;
        }
	    else
        {
            isActiveRoom = false;
        }
        if (RoomCheckpoint != null)
        {
            if (bounds.Contains(world.player.collider.bounds.center) == true && gameStateManager.LastCheckpoint != RoomCheckpoint)
            {
                RoomCheckpoint.Activate();
            } 
        }
	}

    /// <summary>
    /// Respawns respawnable room elements.
    /// </summary>
    public void Refresh ()
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            CommonEnemyController e = Enemies[i].GetComponent<CommonEnemyController>();
            e.Kill();
            e.Respawn();
            isActiveRoom = false;
        }
        for (int i = 0; i < NonEnemyOccupants.Length; i++)
        {
            NonEnemyOccupants[i].roomObjectRespawnAction.Invoke();
        }
    }

    /// <summary>
    /// Wakes up any RegisteredSprites attached to the room.
    /// </summary>
    void WakeUpOccupants ()
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            Enemies[i].Toggled = true;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.min.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.min.x, bounds.max.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z - 100));

    }
#endif
}
