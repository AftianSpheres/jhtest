using UnityEngine;
using System.Collections;

/// <summary>
/// A room. Stores room data. Does room things.
/// </summary>
[RequireComponent(typeof(RoomCollider))]
[RequireComponent(typeof(RoomPriorityMap))]
public class RoomController : MonoBehaviour
{
    public WorldController world;
    public AudioClip bgm;
    public BossContainer Boss;
    public RegisteredSprite[] Enemies;
    public RegisteredSprite[] NonEnemyOccupants;
    public mu_Checkpoint RoomCheckpoint;
    public RoomCollider collision;
    public RoomPriorityMap priorityMap;
    public SubregionType Subregion;
    public Bounds bounds;
    public FXLayer fx;
    public Vector2 BigRoomCellSize;
    public uint replacementValue = 0;
    public uint xPosition;
    public uint yPosition;
    public bool isActiveRoom;
    public Vector2[] EntryPoints;
    private GameStateManager gameStateManager;
    public Bounds[] pitfallZones;
    public UnityTileMap.TileMapBehaviour tilemap;


    /// <summary>
    /// MonoBehaviour.Awake()
    /// </summary>
    void Awake ()
    {
        if (replacementValue == 0)
        {
            PutRoomInWorldCoords();
        }
        gameStateManager = GameObject.Find("Universe/GameStateManager").GetComponent<GameStateManager>();
    }

    public void PutRoomInWorldCoords ()
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
            if (tilemap != null)
            {
                tilemap.animateTiles = true;
            }
            isActiveRoom = true;
        }
	    else
        {
            isActiveRoom = false;
            if (tilemap != null)
            {
                tilemap.animateTiles = false;
            }
        }
        if (RoomCheckpoint != null)
        {
            if (bounds.Contains(world.player.collider.bounds.center) == true && gameStateManager.LastCheckpoint != RoomCheckpoint.checkpointValue)
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
            if (NonEnemyOccupants[i].roomObjectRespawnAction != null)
            {
                NonEnemyOccupants[i].roomObjectRespawnAction();

            }
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
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, 0), new Vector3(bounds.max.x, bounds.min.y, 0));
        Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.min.y, 0), new Vector3(bounds.max.x, bounds.max.y, 0));
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, 0), new Vector3(bounds.min.x, bounds.max.y, 0));
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y, 0), new Vector3(bounds.max.x, bounds.max.y, 0));
        Gizmos.color = Color.yellow;
        for (int i = 0; i < pitfallZones.Length; i++)
        {
            Gizmos.DrawLine(new Vector3(pitfallZones[i].min.x, pitfallZones[i].min.y, 0), new Vector3(pitfallZones[i].max.x, pitfallZones[i].min.y, 0));
            Gizmos.DrawLine(new Vector3(pitfallZones[i].max.x, pitfallZones[i].min.y, 0), new Vector3(pitfallZones[i].max.x, pitfallZones[i].max.y, 0));
            Gizmos.DrawLine(new Vector3(pitfallZones[i].min.x, pitfallZones[i].min.y, 0), new Vector3(pitfallZones[i].min.x, pitfallZones[i].max.y, 0));
            Gizmos.DrawLine(new Vector3(pitfallZones[i].min.x, pitfallZones[i].max.y, 0), new Vector3(pitfallZones[i].max.x, pitfallZones[i].max.y, 0));
        }
    }
#endif
}
