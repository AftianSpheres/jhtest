using UnityEngine;
using System.Collections;

public class mu_Warp : MonoBehaviour
{
    public RoomController room;
    public RoomController DestinationRoom;
    public AudioClip clip;
    public AudioSource source;
    public Bounds bounds;
    public Direction WarpedPlayerFacingDir;
    public bool warpToOtherMap;
    public AreaType destinationLevel;
    public Vector3 destinationRoomCoords;
    public int DestinationEntryPoint;
    private bool Used;
	
	// Update is called once per frame
	void Update ()
    {
	    if (bounds.Contains(room.world.player.collider.bounds.center) == true && room.world.player.animator.GetBool(PlayerAnimatorHashes.triggerDodgeBurst) == false)
        {
            if (Used == false)
            {
                Used = true;
                if (room.world.player.DontWarp == false)
                {
                    room.world.player.DontWarp = true;
                    if (warpToOtherMap == false)
                    {
                        room.world.player.transform.position = new Vector3(DestinationRoom.bounds.min.x + DestinationRoom.EntryPoints[DestinationEntryPoint].x,
                            DestinationRoom.bounds.min.y + HammerConstants.SizeOfOneTile + DestinationRoom.EntryPoints[DestinationEntryPoint].y, room.world.player.transform.position.z);
                        StartCoroutine(room.world.cameraController.InstantChangeScreen(DestinationRoom, 30));
                    }
                    else if (destinationLevel > AreaType.None)
                    {
                        int[] coords = { (int)destinationRoomCoords.y, (int)destinationRoomCoords.x };
                        LevelLoadManager.Instance.EnterLevel(destinationLevel, coords, DestinationEntryPoint, WarpedPlayerFacingDir);
                    }
                    if (clip != null)
                    {
                        source.PlayOneShot(clip);
                    }
                }
            }
        }
        else
        {
            Used = false;
        }

	}

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (warpToOtherMap == true)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.magenta;

        }
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.min.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.min.x, bounds.max.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z - 100));
    }
# endif
}
