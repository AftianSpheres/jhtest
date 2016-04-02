﻿using UnityEngine;
using System.Collections;

public class WarpZone : MonoBehaviour
{
    public RoomController room;
    public RoomController DestinationRoom;
    public uint DestinationEntryPoint;
    public Bounds bounds;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (bounds.Contains(room.world.player.collider.bounds.center))
        {
            room.world.player.transform.position = new Vector3(DestinationRoom.bounds.min.x + DestinationRoom.EntryPoints[DestinationEntryPoint].x, 
                DestinationRoom.bounds.min.y + 16 + DestinationRoom.EntryPoints[DestinationEntryPoint].y, transform.position.z);
            room.world.cameraController.InstantChangeScreen(DestinationRoom);
        }
	}

    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.min.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.min.x, bounds.max.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z - 100));

    }
}
