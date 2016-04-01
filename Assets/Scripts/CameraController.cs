using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public WorldController world;
    private GameObject WindowLayer;
    private PlayerController player;
    public RoomController activeRoom;
    public Rect rect;
    private RoomController nextRoom;
    private bool ScrollVertical = false;
    private int ScrollingPx;
    public static int ScrollSpeed = 2;

	void Start ()
    {
        rect.center = transform.position;
        WindowLayer = world.WindowLayer;
        player = world.player;
        activeRoom = world.FirstRoom;
    }

    void Update ()
    {
        if (ScrollingPx != 0)
        {
            if (ScrollVertical == true)
            {
                if (ScrollingPx < 0)
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x, WindowLayer.transform.position.y - ScrollSpeed, WindowLayer.transform.position.z);
                    ScrollingPx = ScrollingPx + 2;
                    if (ScrollingPx > 0)
                    {
                        ScrollingPx = 0;
                    }
                }
                else
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x, WindowLayer.transform.position.y + ScrollSpeed, WindowLayer.transform.position.z);
                    ScrollingPx = ScrollingPx - 2;
                    if (ScrollingPx < 0)
                    {
                        ScrollingPx = 0;
                    }
                }
            }
            else
            {
                if (ScrollingPx < 0)
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x - ScrollSpeed, WindowLayer.transform.position.y, WindowLayer.transform.position.z);
                    ScrollingPx = ScrollingPx + 2;
                    if (ScrollingPx > 0)
                    {
                        ScrollingPx = 0;
                    }
                }
                else
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x + ScrollSpeed, WindowLayer.transform.position.y, WindowLayer.transform.position.z);
                    ScrollingPx = ScrollingPx - 2;
                    if (ScrollingPx < 0)
                    {
                        ScrollingPx = 0;
                    }
                }
            }
            if (ScrollingPx == 0)
            {
                activeRoom = nextRoom;
                player.Locked = false;
            }
        }
    }
	
    public void ScrollScreen(int direction)
    {
        if (ScrollingPx != 0)
        {
            throw new System.Exception("Can't start another screen scroll during last one!");
        }
        switch (direction)
        {
            case 0:
                ScrollVertical = true;
                ScrollingPx = (int)activeRoom.bounds.extents.y * -2;
                nextRoom = world.rooms[activeRoom.yPosition - 1, activeRoom.xPosition];
                rect.center = nextRoom.bounds.center;
                player.Locked = true;
                break;
            case 1:
                ScrollVertical = true;
                ScrollingPx = (int)activeRoom.bounds.extents.y * 2;
                nextRoom = world.rooms[activeRoom.yPosition + 1, activeRoom.xPosition];
                rect.center = nextRoom.bounds.center;
                player.Locked = true;
                break;
            case 2:
                ScrollVertical = false;
                ScrollingPx = (int)activeRoom.bounds.extents.x * -2;
                nextRoom = world.rooms[activeRoom.yPosition, activeRoom.xPosition - 1];
                rect.center = nextRoom.bounds.center;
                player.Locked = true;
                break;
            case 3:
                ScrollVertical = false;
                ScrollingPx = (int)activeRoom.bounds.extents.x * 2;
                nextRoom = world.rooms[activeRoom.yPosition, activeRoom.xPosition + 1];
                Debug.Log(nextRoom);
                rect.center = nextRoom.bounds.center;
                player.Locked = true;
                break;
        }
    }
}
