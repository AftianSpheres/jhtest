using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RegisteredSprite : MonoBehaviour
{
    public RoomController room;
    public bool Toggled;
    public Action roomObjectRespawnAction; 
	
	// Update is called once per frame
	void Update ()
    {
        if (room != null && room.isActiveRoom == false)
        {
            Toggled = false;
        }
	}

}
