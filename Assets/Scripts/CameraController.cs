using UnityEngine;
using System;
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
    private int ForceScroll;
    private int SecondaryForceScroll = 0;
    private Vector3 CurrentRoomPlayerEntryPosition;
    public static int ScrollSpeed = 2;

	void Start ()
    {
        rect.center = transform.position;
        WindowLayer = world.WindowLayer;
        player = world.player;
        activeRoom = world.rooms[(int)world.FirstRoom.y, (int)world.FirstRoom.x];
    }

    void Update ()
    {
        if (ForceScroll != 0)
        {
            if (ScrollVertical == true)
            {
                if (ForceScroll < 0)
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x, WindowLayer.transform.position.y - ScrollSpeed, WindowLayer.transform.position.z);
                    ForceScroll = ForceScroll + 2;
                    if (ForceScroll > 0)
                    {
                        ForceScroll = 0;
                    }
                }
                else
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x, WindowLayer.transform.position.y + ScrollSpeed, WindowLayer.transform.position.z);
                    ForceScroll = ForceScroll - 2;
                    if (ForceScroll < 0)
                    {
                        ForceScroll = 0;
                    }
                }
                if (SecondaryForceScroll < 0)
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x - 1, WindowLayer.transform.position.y, WindowLayer.transform.position.z);
                    SecondaryForceScroll = SecondaryForceScroll + 1;
                    if (SecondaryForceScroll > 0)
                    {
                        SecondaryForceScroll = 0;
                    }
                }
                else if (SecondaryForceScroll > 0)
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x + 1, WindowLayer.transform.position.y, WindowLayer.transform.position.z);
                    SecondaryForceScroll = SecondaryForceScroll - 1;
                    if (SecondaryForceScroll < 0)
                    {
                        SecondaryForceScroll = 0;
                    }
                }
            }
            else
            {
                if (ForceScroll < 0)
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x - ScrollSpeed, WindowLayer.transform.position.y, WindowLayer.transform.position.z);
                    ForceScroll = ForceScroll + 2;
                    if (ForceScroll > 0)
                    {
                        ForceScroll = 0;
                    }
                }
                else
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x + ScrollSpeed, WindowLayer.transform.position.y, WindowLayer.transform.position.z);
                    ForceScroll = ForceScroll - 2;
                    if (ForceScroll < 0)
                    {
                        ForceScroll = 0;
                    }
                }
                if (SecondaryForceScroll < 0)
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x , WindowLayer.transform.position.y - 1, WindowLayer.transform.position.z);
                    SecondaryForceScroll = SecondaryForceScroll + 1;
                    if (SecondaryForceScroll > 0)
                    {
                        SecondaryForceScroll = 0;
                    }
                }
                else if (SecondaryForceScroll > 0)
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x, WindowLayer.transform.position.y + 1, WindowLayer.transform.position.z);
                    SecondaryForceScroll = SecondaryForceScroll - 1;
                    if (SecondaryForceScroll < 0)
                    {
                        SecondaryForceScroll = 0;
                    }
                }
            }
            if (ForceScroll == 0)
            {
                activeRoom = nextRoom;
                player.Locked = false;
                CurrentRoomPlayerEntryPosition = player.transform.position;
            }
        }
        else if (player.transform.position != CurrentRoomPlayerEntryPosition)
        {
            int adj_x = 0;
            int adj_y = 0;
            if (activeRoom.BigRoomCellSize.x > 1)
            {
                if ((activeRoom.bounds.min.x < rect.xMin) && rect.center.x - player.collider.bounds.center.x > 1)
                {
                    adj_x = -1 * (int)Math.Round(rect.center.x - player.collider.bounds.center.x, 0, MidpointRounding.AwayFromZero) / 4;
                    if (adj_x > 0)
                    {
                        adj_x = 0;
                    }
                    if (rect.xMin + adj_x < activeRoom.bounds.min.x)
                    {
                        adj_x = -1;
                    }
                }
                else if ((activeRoom.bounds.max.x > rect.xMax) && player.collider.bounds.center.x - rect.center.x > 1)
                {
                    adj_x = (int)Math.Round(player.collider.bounds.center.x - rect.center.x, 0, MidpointRounding.AwayFromZero) / 4;
                    if (adj_x < 0)
                    {
                        adj_x = 0;
                    }
                    if (rect.xMax + adj_x > activeRoom.bounds.max.x)
                    {
                        adj_x = 1;
                    }
                }
            }
            if (activeRoom.BigRoomCellSize.y > 1)
            {
                if ((activeRoom.bounds.min.y < rect.yMin) && rect.center.y - player.collider.bounds.center.y > 1)
                {
                    adj_y = -1 * (int)Math.Round(rect.center.y - player.collider.bounds.center.y, 0, MidpointRounding.AwayFromZero) / 4;
                    if (adj_y > 0)
                    {
                        adj_y = 0;
                    }
                    if (rect.yMin + adj_y - 8 < activeRoom.bounds.min.y)
                    {
                        adj_y = -1;
                    }
                }
                else if ((activeRoom.bounds.max.y > rect.yMax) && player.collider.bounds.center.y - rect.center.y > 1)
                {
                    adj_y = (int)Math.Round(player.collider.bounds.center.y - rect.center.y , 0, MidpointRounding.AwayFromZero) / 4;
                    if (adj_y < 0)
                    {
                        adj_y = 0;
                    }
                    if (rect.yMax + adj_y + 8 > activeRoom.bounds.max.y)
                    {
                        adj_y = 1;
                    }
                }
            }
            WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x + adj_x, WindowLayer.transform.position.y + adj_y, WindowLayer.transform.position.z);
            rect.center = new Vector2(WindowLayer.transform.position.x, WindowLayer.transform.position.y - 8);
        }
    }
	
    public void InstantChangeScreen (RoomController room)
    {
        nextRoom = room;
        rect.center = new Vector2(room.bounds.center.x, room.bounds.center.y);
        WindowLayer.transform.position = new Vector3(room.bounds.center.x, room.bounds.center.y + 8, WindowLayer.transform.position.z);
        activeRoom = nextRoom;
        
    }

    public void ScrollAndChangeScreen(int direction)
    {
        if (ForceScroll != 0)
        {
            throw new System.Exception("Can't start another screen scroll during last one!");
        }
        float adj;
        switch (direction)
        {
            case 0:
                ScrollVertical = true;
                if (activeRoom.BigRoomCellSize.x > 1)
                {
                    adj = (player.collider.bounds.center.x - activeRoom.bounds.min.x) / 160;
                    nextRoom = world.rooms[activeRoom.yPosition - 1, activeRoom.xPosition + (int)adj];
                }
                else
                {
                    nextRoom = world.rooms[activeRoom.yPosition - 1, activeRoom.xPosition];
                }
                break;
            case 1:
                ScrollVertical = true;
                if (activeRoom.BigRoomCellSize.x > 1 || activeRoom.BigRoomCellSize.y > 1)
                {
                    adj = (player.collider.bounds.center.x - activeRoom.bounds.min.x) / 160;
                    nextRoom = world.rooms[activeRoom.yPosition + (int)activeRoom.BigRoomCellSize.x, activeRoom.xPosition + (int)adj];
                }
                else
                {
                    nextRoom = world.rooms[activeRoom.yPosition + 1, activeRoom.xPosition];
                }
                break;
            case 2:
                ScrollVertical = false;
                if (activeRoom.BigRoomCellSize.y > 1)
                {
                    adj = (player.collider.bounds.center.y - activeRoom.bounds.min.y) / 128;
                    nextRoom = world.rooms[activeRoom.yPosition + (int)adj, activeRoom.xPosition -1];
                }
                else
                {
                    nextRoom = world.rooms[activeRoom.yPosition, activeRoom.xPosition - 1];
                }
                break;
            case 3:
                ScrollVertical = false;
                if (activeRoom.BigRoomCellSize.x > 1 || activeRoom.BigRoomCellSize.y > 1)
                {
                    adj = (player.collider.bounds.center.y - activeRoom.bounds.min.y) / 128;
                    nextRoom = world.rooms[activeRoom.yPosition + (int)adj, activeRoom.xPosition + (int)activeRoom.BigRoomCellSize.y];
                }
                else
                {
                    nextRoom = world.rooms[activeRoom.yPosition, activeRoom.xPosition + 1];
                }
                break;
        }
        player.Locked = true;
        float x;
        float y;
        if (ScrollVertical == true)
        {
            if (nextRoom.BigRoomCellSize.x > 1)
            {
                if (activeRoom.BigRoomCellSize.x > 1)
                {
                    x = 160 * (activeRoom.xPosition + (int)((player.collider.bounds.center.x - activeRoom.bounds.min.x) / 160));
                }
                else
                {
                    x = activeRoom.bounds.center.x;
                }
            }
            else
            {
                x = nextRoom.bounds.center.x;
            }
            if (nextRoom.BigRoomCellSize.y > 1)
            {
                if (activeRoom.yPosition < nextRoom.yPosition)
                {
                    y = nextRoom.bounds.min.y + 64;
                }
                else
                {
                    y = nextRoom.bounds.max.y - 64;
                }
            }
            else
            {
                y = nextRoom.bounds.center.y;
            }
            ForceScroll = (int)(y - rect.center.y);
            SecondaryForceScroll = (int)(x - rect.center.x);
        }
        else
        {
            if (nextRoom.BigRoomCellSize.y > 1)
            {
                if (activeRoom.BigRoomCellSize.y > 1)
                {
                    y = (128 * (activeRoom.yPosition + (int)((player.collider.bounds.center.y - activeRoom.bounds.min.y) / 128))) - 8;
                }
                else
                {
                    y = activeRoom.bounds.center.y;
                }
            }
            else
            {
                y = nextRoom.bounds.center.y;
            }
            if (nextRoom.BigRoomCellSize.x > 1)
            {
                if (activeRoom.xPosition < nextRoom.xPosition)
                {
                    x = nextRoom.bounds.min.x + 64;
                }
                else
                {
                    x = nextRoom.bounds.max.x - 64;
                }
            }
            else
            {
                x = nextRoom.bounds.center.x;
            }
            ForceScroll = (int)(x - rect.center.x);
            SecondaryForceScroll = (int)(y - rect.center.y);
        }
        rect.center = new Vector2(x, y);

    }

    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(rect.min.x, rect.min.y, -250), new Vector3(rect.max.x, rect.min.y, -250));
        Gizmos.DrawLine(new Vector3(rect.max.x, rect.min.y, -250), new Vector3(rect.max.x, rect.max.y, -250));
        Gizmos.DrawLine(new Vector3(rect.min.x, rect.min.y, -250), new Vector3(rect.min.x, rect.max.y, -250));
        Gizmos.DrawLine(new Vector3(rect.min.x, rect.max.y, -250), new Vector3(rect.max.x, rect.max.y, -250));

    }
}
