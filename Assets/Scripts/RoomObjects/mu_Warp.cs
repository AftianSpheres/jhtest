using UnityEngine;
using System.Collections;

public class mu_Warp : MonoBehaviour
{
    public RoomController room;
    public RoomController DestinationRoom;
    public AudioClip clip;
    public AudioSource source;
    public Bounds bounds;
    public uint DestinationEntryPoint;
    private bool Used;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (bounds.Contains(room.world.player.collider.bounds.center) == true && room.world.player.animator.GetBool("DodgeBurst") == false)
        {
            if (Used == false)
            {
                Used = true;
                if (room.world.player.DontWarp == false)
                {
                    room.world.player.DontWarp = true;
                    room.world.player.transform.position = new Vector3(DestinationRoom.bounds.min.x + DestinationRoom.EntryPoints[DestinationEntryPoint].x,
                        DestinationRoom.bounds.min.y + HammerConstants.SizeOfOneTile + DestinationRoom.EntryPoints[DestinationEntryPoint].y, room.world.player.transform.position.z);
                    StartCoroutine(room.world.cameraController.InstantChangeScreen(DestinationRoom, 30));
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

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.min.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.min.x, bounds.max.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z - 100));

    }

# endif
}
