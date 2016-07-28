using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

[Serializable]
public class GameStateManager : Manager<GameStateManager>
{
    public WorldController world;
    public ChestFlags_Circle chestFlags_Circle = 0;
    public ChestFlags_Forest chestFlags_Forest = 0;
    public ChestFlags_Marina chestFlags_Marina = 0;
    public ChestFlags_TestMap chestFlags_TestMap = 0;
    public ChestFlags_Valley chestFlags_Valley = 0;
    public ChestFlags_Village chestFlags_Village = 0;
    public ChestFlags_Oubliette chestFlags_Oubliette = 0;
    public EventFlags_Circle eventFlags_Circle = 0;
    public EventFlags_Forest eventFlags_Forest = 0;
    public EventFlags_Global eventFlags_Global = 0;
    public EventFlags_Marina eventFlags_Marina = 0;
    public EventFlags_TestMap eventFlags_TestMap = 0;
    public EventFlags_Valley eventFlags_Valley = 0;
    public EventFlags_Village eventFlags_Village = 0;
    public EventFlags_Oubliette eventFlags_Oubliette = 0;
    public Checkpoints availableCheckpoints = 0;
    public Checkpoints LastCheckpoint;
    public HeldPassiveItems heldPassiveItems = 0;
    public HeldTaboos heldTaboos = 0;
    public HeldWeapons heldWeapons = 0;
    public TabooType activeTaboo = TabooType.None;
    public WeaponType[] activePlayerWeapons = { WeaponType.None, WeaponType.None };
    public Vector3 respawnPosition;
    public uint DeathCounter = 0;
    public uint PlayerLevel = 0;
    public int[] areaKeys = new int[HammerConstants.NumberOfAreas];
    public int levelLoadPlayerAnimHash = PlayerAnimatorHashes.PlayerStand_D;
    public float levelLoadPlayerAnimTime = 0f;
    public uint[] respawnRoomCoords = new uint[2];
    public int respawnLevelIndex = 0;
    public int SessionFingerprint;
    public ulong ElapsedFrames = 0;

    /// <summary>
    /// MonoBehavious.Start()
    /// </summary>
    void Start ()
    {
        SessionFingerprint = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
	}
	
    /// <summary>
    /// MonoBehaviour.Update()
    /// </summary>
	void Update ()
    {
        ElapsedFrames++;
    }

    /// <summary>
    /// Generates a new session fingerprint value.
    /// </summary>
    public void RerollSessionFingerprint ()
    {
        int r = SessionFingerprint;
        while (r == SessionFingerprint)
        {
            r = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }
        SessionFingerprint = r;
    }

    /// <summary>
    /// Respawns player at last visited checkpoint.
    /// </summary>
    public void RespawnPlayer()
    {
        if (SceneManager.GetActiveScene().buildIndex != respawnLevelIndex)
        {
            world = default(WorldController);
            levelLoadPlayerAnimHash = PlayerAnimatorHashes.Dead;
            levelLoadPlayerAnimTime = 1.0f;
            SceneManager.LoadScene(respawnLevelIndex, LoadSceneMode.Single);
        }
        StartCoroutine(in_RespawnPlayer());
    }

    /// <summary>
    /// INTERNAL: handles timing w/ level loading and screen fade animation
    /// </summary>
    IEnumerator in_RespawnPlayer()
    {
        while (world == null) // if we changed scenes, we need to make sure that the new scene has loaded and its WorldController has mated with the GameStateManager
        {
            yield return null;
        }
        StartCoroutine(world.cameraController.InstantChangeScreen(world.rooms[respawnRoomCoords[0], respawnRoomCoords[1]], 60));
        RerollSessionFingerprint();
        while (world.activeRoom == null)
        {
            yield return null;
        }
        world.player.mover.virtualPosition = respawnPosition;
        world.player.mover.heading = Vector3.zero;
        world.player.transform.position = respawnPosition;
        world.player.energy.Reset();
        world.player.animator.Play(PlayerAnimatorHashes.PlayerStand_D);
        world.player.animator.SetBool("Dead", false);
    }
}
