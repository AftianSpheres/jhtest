using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Moves the camera, viewport rect, and everything tied to those in a nice, chunky pixel-locked fashion.
/// Basically a giant ball of nightmares.
/// </summary>
public class CameraController : MonoBehaviour
{
    public WorldController world;
    private GameObject WindowLayer;
    private PlayerController player;
    public Rect rect;
    public bool PlayerLockedToScroll;
    private RoomController nextRoom;
    private bool ScrollVertical = false;
    private int ForceScroll;
    private int SecondaryForceScroll = 0;
    private Vector3 CurrentRoomPlayerEntryPosition;
    private static int ScrollSpeed = 2;
    private static int CameraCatchUpDelay = 4;

    /// <summary>
    /// MonoBehaviour.Start()
    /// </summary>
	void Start ()
    {
        rect.center = transform.position;
        WindowLayer = world.WindowLayer;
        player = world.player;
        RoomController firstRoom = world.rooms[(int)world.FirstRoom.y, (int)world.FirstRoom.x];
        StartCoroutine(InstantChangeScreen(firstRoom, 0));
    }

    /// <summary>
    /// MonoBehaviour.Update()
    /// </summary>
    void Update ()
    {
        if (ForceScroll != 0)
        {
            if (ScrollVertical == true)
            {
                if (SecondaryForceScroll < 0)
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x - (ScrollSpeed / 2), WindowLayer.transform.position.y, WindowLayer.transform.position.z);
                    SecondaryForceScroll = SecondaryForceScroll + (ScrollSpeed / 2);
                    if (SecondaryForceScroll > 0)
                    {
                        SecondaryForceScroll = 0;
                    }
                }
                else if (SecondaryForceScroll > 0)
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x + (ScrollSpeed / 2), WindowLayer.transform.position.y, WindowLayer.transform.position.z);
                    SecondaryForceScroll = SecondaryForceScroll - (ScrollSpeed / 2);
                    if (SecondaryForceScroll < 0)
                    {
                        SecondaryForceScroll = 0;
                    }
                }
                else if (ForceScroll < 0)
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x, WindowLayer.transform.position.y - ScrollSpeed, WindowLayer.transform.position.z);
                    ForceScroll = ForceScroll + ScrollSpeed;
                    if (ForceScroll > 0)
                    {
                        ForceScroll = 0;
                    }
                }
                else
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x, WindowLayer.transform.position.y + ScrollSpeed, WindowLayer.transform.position.z);
                    ForceScroll = ForceScroll - ScrollSpeed;
                    if (ForceScroll < 0)
                    {
                        ForceScroll = 0;
                    }
                }
            }
            else
            {
                if (SecondaryForceScroll < 0)
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x, WindowLayer.transform.position.y - (ScrollSpeed / 2), WindowLayer.transform.position.z);
                    SecondaryForceScroll = SecondaryForceScroll + (ScrollSpeed / 2);
                    if (SecondaryForceScroll > 0)
                    {
                        SecondaryForceScroll = 0;
                    }
                }
                else if (SecondaryForceScroll > 0)
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x, WindowLayer.transform.position.y + (ScrollSpeed / 2), WindowLayer.transform.position.z);
                    SecondaryForceScroll = SecondaryForceScroll - (ScrollSpeed / 2);
                    if (SecondaryForceScroll < 0)
                    {
                        SecondaryForceScroll = 0;
                    }
                }
                else if (ForceScroll < 0)
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x - ScrollSpeed, WindowLayer.transform.position.y, WindowLayer.transform.position.z);
                    ForceScroll = ForceScroll + ScrollSpeed;
                    if (ForceScroll > 0)
                    {
                        ForceScroll = 0;
                    }
                }
                else
                {
                    WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x + ScrollSpeed, WindowLayer.transform.position.y, WindowLayer.transform.position.z);
                    ForceScroll = ForceScroll - ScrollSpeed;
                    if (ForceScroll < 0)
                    {
                        ForceScroll = 0;
                    }
                }
            }
            if (ForceScroll == 0)
            {
                world.ChangeRoom(nextRoom);
                world.ChangeBGM(nextRoom.bgm);
                player.Locked = false;
                PlayerLockedToScroll = false;
                CurrentRoomPlayerEntryPosition = player.transform.position;
            }
        }
        else if (player.transform.position != CurrentRoomPlayerEntryPosition && world.activeRoom != null)
        {
            int adj_x = 0;
            int adj_y = 0;
            if (world.activeRoom.BigRoomCellSize.x > 1)
            {
                if ((world.activeRoom.bounds.min.x < rect.xMin) && rect.center.x - player.collider.bounds.center.x > 1)
                {
                    adj_x = -1 * (int)Math.Round(rect.center.x - player.collider.bounds.center.x, 0, MidpointRounding.AwayFromZero) / CameraCatchUpDelay;
                    if (adj_x > 0)
                    {
                        adj_x = 0;
                    }
                    if (rect.xMin + adj_x < world.activeRoom.bounds.min.x)
                    {
                        adj_x = -1;
                    }
                }
                else if ((world.activeRoom.bounds.max.x > rect.xMax) && player.collider.bounds.center.x - rect.center.x > 1)
                {
                    adj_x = (int)Math.Round(player.collider.bounds.center.x - rect.center.x, 0, MidpointRounding.AwayFromZero) / CameraCatchUpDelay;
                    if (adj_x < 0)
                    {
                        adj_x = 0;
                    }
                    if (rect.xMax + adj_x > world.activeRoom.bounds.max.x)
                    {
                        adj_x = 1;
                    }
                }
            }
            if (world.activeRoom.BigRoomCellSize.y > 1)
            {
                if ((world.activeRoom.bounds.min.y < rect.yMin) && rect.center.y - player.collider.bounds.center.y > 1)
                {
                    adj_y = -1 * (int)Math.Round(rect.center.y - player.collider.bounds.center.y, 0, MidpointRounding.AwayFromZero) / CameraCatchUpDelay;
                    if (adj_y > 0)
                    {
                        adj_y = 0;
                    }
                    if (rect.yMin + adj_y - 8 < world.activeRoom.bounds.min.y)
                    {
                        adj_y = -1;
                    }
                }
                else if ((world.activeRoom.bounds.max.y > rect.yMax) && player.collider.bounds.center.y - rect.center.y > 1)
                {
                    adj_y = (int)Math.Round(player.collider.bounds.center.y - rect.center.y , 0, MidpointRounding.AwayFromZero) / CameraCatchUpDelay;
                    if (adj_y < 0)
                    {
                        adj_y = 0;
                    }
                    if (rect.yMax + adj_y + 8 > world.activeRoom.bounds.max.y)
                    {
                        adj_y = 1;
                    }
                }
            }
            WindowLayer.transform.position = new Vector3(WindowLayer.transform.position.x + adj_x, WindowLayer.transform.position.y + adj_y, WindowLayer.transform.position.z);
            rect.center = new Vector2(WindowLayer.transform.position.x, WindowLayer.transform.position.y - (HammerConstants.HeightOfHUD / 2));

        }
    }

    /// <summary>
    /// Immediately repositions the viewport to a given room, then does a cool fade-out effect to hide the transition.
    /// 
    /// Doesn't support big rooms right now, but probably should.
    /// </summary>
    public IEnumerator InstantChangeScreen(RoomController room, int FadeDuration)
    {
        int i = 0;
        nextRoom = room;
        if (world.activeRoom == null || nextRoom.bgm != world.activeRoom.bgm)
        {
            world.ChangeBGM(default(AudioClip));
        }
        world.activeRoom = default(RoomController);
        player.Locked = true;
        rect.center = new Vector2(room.bounds.center.x, room.bounds.center.y);
        WindowLayer.transform.position = new Vector3(room.bounds.center.x, room.bounds.center.y + 8, WindowLayer.transform.position.z);
        while (i < FadeDuration)
        {
            if (i == 0)
            {
                world.Curtain.SetActive(true);
                world.Curtain.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else if (i == (FadeDuration / 4))
            {
                world.Curtain.GetComponent<SpriteRenderer>().color = Color.black;
            }
            i++;
            yield return null;
        }
        world.Curtain.SetActive(false);
        player.Locked = false;
        world.ChangeRoom(nextRoom);
        world.ChangeBGM(nextRoom.bgm);
    }

    /// <summary>
    /// Scrolls the camera into the room in a given direction and sets that as the active room.
    /// </summary>
    /// <param name="direction">0-3; down/up/left/right; throws an exception if out of range</param>
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
                if (world.activeRoom.BigRoomCellSize.x > 1)
                {
                    adj = (player.collider.bounds.center.x - world.activeRoom.bounds.min.x) / HammerConstants.LogicalResolution_Horizontal;
                    nextRoom = world.rooms[world.activeRoom.yPosition - 1, world.activeRoom.xPosition + (int)adj];
                }
                else
                {
                    nextRoom = world.rooms[world.activeRoom.yPosition - 1, world.activeRoom.xPosition];
                }
                break;
            case 1:
                ScrollVertical = true;
                if (world.activeRoom.BigRoomCellSize.x > 1 || world.activeRoom.BigRoomCellSize.y > 1)
                {
                    adj = (player.collider.bounds.center.x - world.activeRoom.bounds.min.x) / HammerConstants.LogicalResolution_Horizontal;
                    nextRoom = world.rooms[world.activeRoom.yPosition + (int)world.activeRoom.BigRoomCellSize.x, world.activeRoom.xPosition + (int)adj];
                }
                else
                {
                    nextRoom = world.rooms[world.activeRoom.yPosition + 1, world.activeRoom.xPosition];
                }
                break;
            case 2:
                ScrollVertical = false;
                if (world.activeRoom.BigRoomCellSize.y > 1)
                {
                    adj = (player.collider.bounds.center.y - world.activeRoom.bounds.min.y) / (HammerConstants.LogicalResolution_Vertical - HammerConstants.HeightOfHUD);
                    nextRoom = world.rooms[world.activeRoom.yPosition + (int)adj, world.activeRoom.xPosition -1];
                }
                else
                {
                    nextRoom = world.rooms[world.activeRoom.yPosition, world.activeRoom.xPosition - 1];
                }
                break;
            case 3:
                ScrollVertical = false;
                if (world.activeRoom.BigRoomCellSize.x > 1 || world.activeRoom.BigRoomCellSize.y > 1)
                {
                    adj = (player.collider.bounds.center.y - world.activeRoom.bounds.min.y) / (HammerConstants.LogicalResolution_Vertical - HammerConstants.HeightOfHUD);
                    nextRoom = world.rooms[world.activeRoom.yPosition + (int)adj, world.activeRoom.xPosition + (int)world.activeRoom.BigRoomCellSize.y];
                }
                else
                {
                    nextRoom = world.rooms[world.activeRoom.yPosition, world.activeRoom.xPosition + 1];
                }
                break;
            default:
                throw new Exception("Tried to scroll the screen in an invalid direction!");
        }
        player.Locked = true;
        PlayerLockedToScroll = true;
        float x;
        float y;
        if (ScrollVertical == true)
        {
            if (nextRoom.BigRoomCellSize.x > 1)
            {
                if (world.activeRoom.BigRoomCellSize.x > 1)
                {
                    x = HammerConstants.LogicalResolution_Horizontal * (world.activeRoom.xPosition + (int)((player.collider.bounds.center.x - world.activeRoom.bounds.min.x) / HammerConstants.LogicalResolution_Horizontal));

                }
                else
                {
                    x = world.activeRoom.bounds.center.x;
                }
            }
            else
            {
                x = nextRoom.bounds.center.x;
            }
            if (nextRoom.BigRoomCellSize.y > 1)
            {
                if (world.activeRoom.yPosition < nextRoom.yPosition)
                {
                    y = nextRoom.bounds.min.y + ((HammerConstants.LogicalResolution_Vertical - HammerConstants.HeightOfHUD) / 2);
                }
                else
                {
                    y = nextRoom.bounds.max.y - ((HammerConstants.LogicalResolution_Vertical - HammerConstants.HeightOfHUD) / 2);
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
                if (world.activeRoom.BigRoomCellSize.y > 1)
                {
                    y = ((HammerConstants.LogicalResolution_Vertical - HammerConstants.HeightOfHUD) * (world.activeRoom.yPosition + (int)((player.collider.bounds.center.y - world.activeRoom.bounds.min.y) / (HammerConstants.LogicalResolution_Vertical - HammerConstants.HeightOfHUD)))) - (HammerConstants.HeightOfHUD / 2);
                }
                else
                {
                    y = world.activeRoom.bounds.center.y;
                }
            }
            else
            {
                y = nextRoom.bounds.center.y;
            }
            if (nextRoom.BigRoomCellSize.x > 1)
            {
                if (world.activeRoom.xPosition < nextRoom.xPosition)
                {
                    x = nextRoom.bounds.min.x + (HammerConstants.LogicalResolution_Horizontal / 2);
                }
                else
                {
                    x = nextRoom.bounds.max.x - (HammerConstants.LogicalResolution_Horizontal / 2);
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
        if (nextRoom.bgm != world.activeRoom.bgm)
        {
            world.ChangeBGM(default(AudioClip));
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// MonoBehaviour.OnDrawDizmosSelected()
    /// </summary>
    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(rect.min.x, rect.min.y, -250), new Vector3(rect.max.x, rect.min.y, -250));
        Gizmos.DrawLine(new Vector3(rect.max.x, rect.min.y, -250), new Vector3(rect.max.x, rect.max.y, -250));
        Gizmos.DrawLine(new Vector3(rect.min.x, rect.min.y, -250), new Vector3(rect.min.x, rect.max.y, -250));
        Gizmos.DrawLine(new Vector3(rect.min.x, rect.max.y, -250), new Vector3(rect.max.x, rect.max.y, -250));

    }
#endif
}
