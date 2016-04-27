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
    private GameObject _universe;
    private float TimeBuffer;
    private float NameHashBuffer;
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
    private BoomPool booms;
    public BoomPool Booms
    {
        get { return booms; }
    }
    [SerializeField]
    private Vector2 firstRoom;
    public Vector2 FirstRoom
    {
        get { return firstRoom; }
    }
    [SerializeField]
    private GameObject curtain;
    public GameObject Curtain
    {
        get { return curtain; }
    }
    [SerializeField]
    private GameStateManager gameStateManager;
    public GameStateManager GameStateManager
    {
        get { return gameStateManager; }
    }

    [SerializeField]
    private PlayerDataManager playerDataManager;
    public PlayerDataManager PlayerDataManager
    {
        get { return playerDataManager; }
    }

    [SerializeField]
    private int lastSessionFingerprint;
    public int LastSessionFingerprint
    {
        get { return lastSessionFingerprint; }
    }
    [SerializeField]
    private HUDMainTextbox mainTextbox;
    public HUDMainTextbox MainTextbox
    {
        get { return mainTextbox; }
    }
    [SerializeField]
    private AreaType area;
    public AreaType Area
    {
        get { return area; }
    }
    public RoomController activeRoom;
    public List<PauseableSprite> pauseableSprites;
    public bool paused = false;

    // Use this for initialization
    void Awake ()
    {
        rooms = new RoomController[WorldSize_Y, WorldSize_X];
        // Wait for the Universe to come online if need be
        _universe = GameObject.Find("Universe");
        if (_universe == null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            gameStateManager = GameObject.Find("Universe/GameStateManager").GetComponent<GameStateManager>();
            gameStateManager.world = this;
            playerDataManager = GameObject.Find("Universe/PlayerDataManager").GetComponent<PlayerDataManager>();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (_universe == null)
        {
            _universe = GameObject.Find("Universe");
            if (_universe != null)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                gameStateManager = GameObject.Find("Universe/GameStateManager").GetComponent<GameStateManager>();
                playerDataManager = GameObject.Find("Universe/PlayerDataManager").GetComponent<PlayerDataManager>();
            }
        }
        else
        {
            if (gameStateManager.SessionFingerprint != lastSessionFingerprint)
            {
                lastSessionFingerprint = gameStateManager.SessionFingerprint;
                for (int iy = 0; iy < rooms.GetLength(0); iy++)
                {
                    for (int ix = 0; ix < rooms.GetLength(1); ix++)
                    {
                        if (rooms[iy, ix] != null)
                        {
                            rooms[iy, ix].Refresh();
                        }
                    }
                }
            }
        }

	}

    /// <summary>
    /// Changes the active room to the RoomController passed to it.
    /// </summary>
    public void ChangeRoom(RoomController room)
    {
        activeRoom = room;
    }

    /// <summary>
    /// Stops the current BGM track, then plays the AudioClip passed to it.
    /// If arg == null evaluates as true, just stops the BGM. (no BGM)
    /// </summary>
    public void ChangeBGM(AudioClip bgm)
    {
        if (_BGM0.clip != bgm)
        {
            _BGM0.Stop();
            if (bgm != null)
            {
                _BGM0.clip = bgm;
                _BGM0.Play();
                _BGM0.time = TimeBuffer;
            }
            else
            {
                _BGM0.clip = default(AudioClip);
            }
        }
    }

    /// <summary>
    /// Pauses every PauseableSprite.
    /// </summary>
    public void Pause ()
    {
        for (int i = 0; i < pauseableSprites.Count; i++)
        {
            pauseableSprites[i].Pause();
        }
        paused = true;

    }

    /// <summary>
    /// Unpauses every PauseableSprite.
    /// </summary>
    public void Unpause ()
    {
        for (int i = 0; i < pauseableSprites.Count; i++)
        {
            pauseableSprites[i].Unpause();
        }
        paused = false;
    }
}
