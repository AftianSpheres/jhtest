using UnityEngine;
using System.Collections;

/// <summary>
/// Enum containing all valid regions that rooms can be part of. Minimap and map menu will use this to figure out which rooms to draw.
/// </summary>
public enum RoomRegion
{
    None,
    TestMap,
    TestMapB1F
}

/// <summary>
/// A room. Stores room data. Does room things.
/// </summary>
[RequireComponent(typeof(RoomCollider))]
[RequireComponent(typeof(RoomPriorityMap))]
public class RoomController : MonoBehaviour
{
    public WorldController world;
    public AudioClip bgm;
    public RegisteredSprite[] Enemies;
    public RegisteredSprite[] NonEnemyOccupants;
    public mu_Checkpoint RoomCheckpoint;
    public RoomCollider collision;
    public RoomPriorityMap priorityMap;
    public RoomRegion region;
    public Bounds bounds;
    public Vector2 BigRoomCellSize;
    public uint xPosition;
    public uint yPosition;
    public bool isActiveRoom;
    public Vector2[] EntryPoints;
    private GameStateManager gameStateManager;


    /// <summary>
    /// MonoBehaviour.Awake()
    /// </summary>
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

    /// <summary>
    /// MonoBehaviour.Update()
    /// </summary>
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
