using UnityEngine;
using System.Collections;

public class ReplacementRoom : MonoBehaviour
{
    WorldController world;
    uint xPosition;
    uint yPosition;
    uint replacementValue;
    public RoomController newRoom;
    public RoomController oldRoom;
    public mu_Warp[] associatedInMapWarps;
    public mu_Door[] newRoomDoors;
    public mufm_Generic flagManager;
	
    void Awake ()
    {
        world = newRoom.world;
        xPosition = newRoom.xPosition;
        yPosition = newRoom.yPosition;
        replacementValue = newRoom.replacementValue;
        newRoom.gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update ()
    {
        if (world.rooms[yPosition, xPosition] != oldRoom)
        {
            oldRoom = world.rooms[yPosition, xPosition];
        }
        if (oldRoom.replacementValue > replacementValue) // can't replace a higher-priority replacement room
        {
            Destroy(newRoom.gameObject);
            Destroy(this);
        }
        else if (flagManager.CheckFlag() == true)
        {
            newRoom.gameObject.SetActive(true);
            if (world.activeRoom == oldRoom)
            {
                world.activeRoom = newRoom;
            }
            newRoom.transform.position = oldRoom.transform.position;
            newRoom.PutRoomInWorldCoords();
            for (int i = 0; i < associatedInMapWarps.Length; i++)
            {
                associatedInMapWarps[i].DestinationRoom = newRoom;
            }
            for (int i = 0; i < newRoomDoors.Length; i++)
            {
                newRoomDoors[i].mirror.mirror = newRoomDoors[i];
            }
            Destroy(oldRoom.gameObject);
            Destroy(this);
        }

	}
}
