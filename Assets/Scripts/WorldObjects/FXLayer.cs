using UnityEngine;
using System.Collections;

public class FXLayer : MonoBehaviour
{
    public WorldController world;
    public RoomController[] applicableRooms;
    public SpriteRenderer[] gfxElements;
    public ScrollingLayer scrollingLayer;
    public CameraController cameraController;
    public AudioClip bgs;
    public float clipVolume;
    private bool active = false;
	
	// Update is called once per frame
	void Update ()
    {
        active = false;
        if ((world.activeRoom.fx == this && cameraController.PlayerLockedToScroll == false) || (cameraController.nextRoom.fx == this && cameraController.lastRoom.fx == this))
        {
            active = true;
        }
        if (active == false && gfxElements[0].enabled == true)
        {
            for (int i = 0; i < gfxElements.Length; i++)
            {
                gfxElements[i].enabled = false;
            }
            scrollingLayer.enabled = false;
            world.BGS0.clip = default(AudioClip);
            world.BGS0.Stop();
            world.BGS0.volume /= clipVolume;
        }
        else if (active == true && gfxElements[0].enabled == false)
        {
            for (int i = 0; i < gfxElements.Length; i++)
            {
                gfxElements[i].enabled = true;
            }
            scrollingLayer.enabled = true;
            world.BGS0.clip = bgs;
            world.BGS0.volume *= clipVolume;
            world.BGS0.Play();
        }
	}
}
