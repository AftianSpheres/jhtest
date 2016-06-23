using UnityEngine;
using System.Collections;

public class FXLayer : MonoBehaviour
{
    public WorldController world;
    public RoomController[] applicableRooms;
    public SpriteRenderer[] gfxElements;
    public ScrollingLayer scrollingLayer;
    public CameraController cameraController;
    private bool active = false;
	
	// Update is called once per frame
	void Update ()
    {
        active = false;
	    for (int i = 0; i < applicableRooms.Length; i++)
        {
            if (applicableRooms[i] == world.activeRoom || (world.activeRoom == null && applicableRooms[i] == cameraController.nextRoom))
            {
                active = true;
                break;
            }
        }
        if (active == false && gfxElements[0].enabled == true)
        {
            for (int i = 0; i < gfxElements.Length; i++)
            {
                gfxElements[i].enabled = false;
            }
            scrollingLayer.enabled = false;
        }
        else if (active == true && gfxElements[0].enabled == false)
        {
            for (int i = 0; i < gfxElements.Length; i++)
            {
                gfxElements[i].enabled = true;
            }
            scrollingLayer.enabled = true;
        }
	}
}
