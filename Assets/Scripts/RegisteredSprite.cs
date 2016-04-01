using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RegisteredSprite : MonoBehaviour
{
    public RoomController room;
    public bool Toggled;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (room != null && room.isActiveRoom == false)
        {
            Toggled = false;
        }
	}

}
