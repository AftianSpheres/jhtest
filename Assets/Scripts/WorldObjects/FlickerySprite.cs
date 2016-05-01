using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlickerySprite : MonoBehaviour
{
    public SpriteRenderer sprite;
    public RoomController room;
    public WorldController world;
    public bool skip = false;
	
	// Update is called once per frame
	void Update ()
    {
        if (world != null && world.StylisticHacksManager != null)
        {
            if ((room == null || world.activeRoom == room) && world.StylisticHacksManager.sprites.Contains(this) == false)
            {
                world.StylisticHacksManager.sprites.Enqueue(this);
                sprite.enabled = false;
            }
        }
	}
}
