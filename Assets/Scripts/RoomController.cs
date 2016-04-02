﻿using UnityEngine;
using System.Collections;

public class RoomController : MonoBehaviour
{
    public WorldController world;
    public RegisteredSprite[] Enemies;
    public RegisteredSprite[] NonEnemyOccupants;
    public GameObject[] RoomObjects;
    public BoxCollider[] Colliders;
    public Bounds bounds;
    public Vector2 BigRoomCellSize;
    public uint xPosition;
    public uint yPosition;
    public bool isActiveRoom;
    public Vector2[] EntryPoints;


	// Use this for initialization
	void Awake ()
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
        for (int i = 0; i < Enemies.Length; i++)
            {
            Enemies[i].room = this;
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
        for (int i = 0; i < Enemies.Length; i++)
        {
            Enemies[i].Toggled = true;
        }
    }

    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.min.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z - 100), new Vector3(bounds.min.x, bounds.max.y, bounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z - 100), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z - 100));

    }

}
