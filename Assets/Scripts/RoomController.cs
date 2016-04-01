using UnityEngine;
using System.Collections;

public class RoomController : MonoBehaviour
{
    public WorldController world;
    public RegisteredSprite[] Occupants;
    public BoxCollider[] Colliders;
    public Bounds bounds;
    public uint xPosition;
    public uint yPosition;
    public bool isActiveRoom;


	// Use this for initialization
	void Awake ()
    {
        world.rooms[yPosition, xPosition] = this;
        for (int i = 0; i < Occupants.Length; i++)
            {
            Occupants[i].room = this;
            }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (world.cameraController.activeRoom == this)
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
	}

    void WakeUpOccupants ()
    {
        for (int i = 0; i < Occupants.Length; i++)
        {
            Occupants[i].Toggled = true;
        }
    }


}
