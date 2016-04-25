using UnityEngine;
using System;
using System.Collections;

[Flags]
public enum ExtantCheckpoints
{
    None = 0,
    Checkpoint00 = 1
}

[Flags]
public enum ChestFlags
{
    None = 0,
    TestMapR01ShotgunChest = 1
}

[Flags]
public enum UnlockedWeapons
{
    None = 0,
    WG = 1,
    WGII = 2,
    Shotgun = 4,
    Shadow = 8,
    Flamethrower = 16,
    Icicle = 32,
    Rift = 64,
    Missile = 128,
    Laser = 256,
    Gatling = 512
}

public class GameStateManager : Manager<GameStateManager> {
    public ChestFlags chests = 0;
    public UnlockedWeapons playerWeapons;
    public mu_Checkpoint LastCheckpoint;
    public int SessionFingerprint;
    public ulong ElapsedFrames = 0;
    public uint DeathCounter = 0;
    public bool[] availableCheckpoints;


    // Use this for initialization
    void Start ()
    {
        availableCheckpoints = new bool[Enum.GetNames(typeof(ExtantCheckpoints)).GetLength(0)];
        SessionFingerprint = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
	}
	
	// Update is called once per frame
	void Update ()
    {
        ElapsedFrames++;
    }

    public void RerollSessionFingerprint ()
    {
        int r = SessionFingerprint;
        while (r == SessionFingerprint)
        {
            r = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }
        SessionFingerprint = r;
    }
}
