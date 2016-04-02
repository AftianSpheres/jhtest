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
    [SerializeField]
    private int worldSize_X;
    public int WorldSize_X
    {
        get { return worldSize_X; }
    }
    [SerializeField]
    private int worldSize_Y;
    public int WorldSize_Y
    {
        get { return worldSize_Y; }
    }
    public RoomController[,] rooms;
    [SerializeField]
    private PlayerController _player;
    public PlayerController player
    {
        get { return _player; }
    }
    [SerializeField]
    private GameObject windowLayer;
    public GameObject WindowLayer
    {
        get { return windowLayer; }
    }
    [SerializeField]
    private Camera _mainCamera;
    public Camera mainCamera
    {
        get { return _mainCamera; }
    }
    [SerializeField]
    private CameraController _cameraController;
    public CameraController cameraController
    {
        get { return _cameraController; }
    }
    [SerializeField]
    private PlayerReticleController _reticle;
    public PlayerReticleController reticle
    {
        get { return _reticle; }
    }
    [SerializeField]
    private AudioSource _BGM0; 
    public AudioSource BGM0
    {
        get { return _BGM0; }
    }
    [SerializeField]
    private AudioSource _BGS0;
    public AudioSource BGS0
    {
        get { return _BGS0; }
    }
    [SerializeField]
    private BulletPool playerBullets;
    public BulletPool PlayerBullets
    {
        get { return playerBullets; }
    }
    [SerializeField]
    private BulletPool enemyBullets;
    public BulletPool EnemyBullets
    {
        get { return enemyBullets; }
    }
    [SerializeField]
    private Vector2 firstRoom;
    public Vector2 FirstRoom
    {
        get { return firstRoom; }
    }

    // Use this for initialization
    void Awake ()
    {
        rooms = new RoomController[WorldSize_Y, WorldSize_X];
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
