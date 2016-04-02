using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Much less interesting than it sounds.
/// WorldController is just a big ugly thing that stores references to a bunch of scene-specific GameObjects, and allows other objects in the scene to get references to things like the background layers,
/// BGM/BGS layers, etc. without having to run a lot of expensive GameObject find function calls.
/// 
/// It also manages the 2D array of RoomController that makes screen scrolling & RegisteredSprites work.
/// </summary>
public class WorldController : MonoBehaviour
{
    public int WorldSize_X;
    public int WorldSize_Y;
    public RoomController[,] rooms;
    public PlayerController player;
    public GameObject WindowLayer;
    public Camera mainCamera;
    public CameraController cameraController;
    public PlayerReticleController reticle;
    public AudioSource BGM0;
    public AudioSource BGS0;
    public BulletPool PlayerBullets;
    public BulletPool EnemyBullets;
    public Vector2 FirstRoom;

    // Use this for initialization
    void Awake ()
    {
        rooms = new RoomController[WorldSize_Y, WorldSize_X];
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
