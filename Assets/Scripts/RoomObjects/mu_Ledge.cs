using UnityEngine;
using System.Collections;

public class mu_Ledge : MonoBehaviour
{
    public RoomController room;
    public Bounds bounds;
    public Direction direction;
    public AudioClip dropSFX;
    public AudioSource source;
    private Vector3 heading;
    private bool inDrop;
    private int frameCtr;
    private static int framesToHold = 10; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (inDrop == false && room.isActiveRoom == true && room.world.player.isNeutral == true)
        {
            switch (direction)
            {
                case Direction.Down:
                    if (bounds.Contains(new Vector3(room.world.player.collider.bounds.center.x, room.world.player.collider.bounds.min.y, 0)) == true)
                    {
                        if (room.world.HardwareInterfaceManager.Down.Pressed == true)
                        {
                            frameCtr++;
                        }
                        else
                        {
                            frameCtr = 0;
                        }
                    }
                    break;
                case Direction.Up:
                    if (bounds.Contains(new Vector3(room.world.player.collider.bounds.center.x, room.world.player.collider.bounds.max.y, 0)) == true)
                    {
                        if (room.world.HardwareInterfaceManager.Up.Pressed == true)
                        {
                            frameCtr++;
                        }
                        else
                        {
                            frameCtr = 0;
                        }
                    }
                    break;
                case Direction.Left:
                    if (bounds.Contains(new Vector3(room.world.player.collider.bounds.min.x, room.world.player.collider.bounds.center.y, 0)) == true)
                    {
                        if (room.world.HardwareInterfaceManager.Left.Pressed == true)
                        {
                            frameCtr++;
                        }
                        else
                        {
                            frameCtr = 0;
                        }
                    }
                    break;
                case Direction.Right:
                    if (bounds.Contains(new Vector3(room.world.player.collider.bounds.max.x, room.world.player.collider.bounds.center.y, 0)) == true)
                    {
                        if (room.world.HardwareInterfaceManager.Right.Pressed == true)
                        {
                            frameCtr++;
                        }
                        else
                        {
                            frameCtr = 0;
                        }
                    }
                    break;
            }
            if (frameCtr > framesToHold)
            {
                StartCoroutine(DropPlayer());
            }
        }
        else if (frameCtr != 0)
        {
            frameCtr = 0;
        }
	}

    IEnumerator DropPlayer ()
    {
        Vector3 origShadowOffset = room.world.player.shadow.transform.localPosition;
        inDrop = true;
        room.world.player.Locked = true;
        source.PlayOneShot(dropSFX);
        room.world.player.transform.position += Vector3.up * 8;
        if (direction == Direction.Left || direction == Direction.Right)
        {
            room.world.player.shadow.transform.localPosition += Vector3.down * 8;
        }
        switch (direction)
        {
            case Direction.Down:
                while (bounds.max.y < room.world.player.collider.bounds.max.y)
                {
                    room.world.player.transform.localPosition += Vector3.down * 2;
                    yield return null;
                }
                break;
            case Direction.Up:
                while (room.world.player.collider.bounds.min.y < bounds.min.y)
                {
                    room.world.player.transform.localPosition += Vector3.up * 2;
                    yield return null;
                }
                break;
            case Direction.Left:
                while (bounds.max.x < room.world.player.collider.bounds.max.x)
                {
                    room.world.player.transform.localPosition += Vector3.left * 2;
                    yield return null;
                }
                break;
            case Direction.Right:
                while (bounds.min.x > room.world.player.collider.bounds.min.x)
                {
                    room.world.player.transform.localPosition += Vector3.right * 2;
                    yield return null;
                }
                break;
        }
        room.world.player.shadow.SetActive(true);
        switch (direction)
        {
            case Direction.Down:
                room.world.player.shadow.transform.localPosition += Vector3.down * -(bounds.min.y - room.world.player.collider.bounds.max.y);
                heading = Vector3.down;
                break;
            case Direction.Up:
                room.world.player.shadow.transform.localPosition += Vector3.up * (bounds.max.y - room.world.player.collider.bounds.min.y);
                heading = Vector3.up;
                break;
            case Direction.Left:
                room.world.player.shadow.transform.localPosition += Vector3.left * -(bounds.min.x - room.world.player.collider.bounds.max.x);
                heading = Vector3.left;
                break;
            case Direction.Right:
                room.world.player.shadow.transform.localPosition += Vector3.right * (bounds.max.x - room.world.player.collider.bounds.min.x);
                heading = Vector3.right;
                break;
        }
        int i = 0;
        bool stopFalling = false;
        while (stopFalling == false)
        {
            if (i < 8)
            {
                room.world.player.transform.position += Vector3.down;
                room.world.player.shadow.transform.localPosition += Vector3.up;
                i++;
            }
            room.world.player.transform.position += heading;
            room.world.player.shadow.transform.localPosition -= heading;
            switch (direction)
            {
                case Direction.Down:
                    if (room.world.player.collider.bounds.max.y < bounds.min.y)
                    {
                        stopFalling = true;
                    }
                    break;
                case Direction.Up:
                    if (room.world.player.collider.bounds.min.y > bounds.max.y)
                    {
                        stopFalling = true;
                    }
                    break;
                case Direction.Left:
                    if (room.world.player.collider.bounds.max.x < bounds.min.x)
                    {
                        stopFalling = true;
                    }
                    break;
                case Direction.Right:
                    if (room.world.player.collider.bounds.min.x > bounds.max.x)
                    {
                        stopFalling = true;
                    }
                    break;
            }
            yield return null;
        }
        i = 0;
        while (i < 8)
        {
            i++;
            if (i < 4)
            {
                room.world.player.transform.position += heading;
            }
            yield return null;
        }
        room.world.player.shadow.SetActive(false);
        room.world.player.shadow.transform.localPosition = origShadowOffset;
        inDrop = false;
        room.world.player.Locked = false;
    }

#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.min.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.min.x, bounds.max.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z - 100));

    }
#endif
}
