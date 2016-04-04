using UnityEngine;
using System;
using System.Collections;

[Flags]
public enum ExtantCheckpoints
{
    None,
    Checkpoint00
}

public class GameStateManager : Manager<GameStateManager> {
    public mu_Checkpoint LastCheckpoint;
    public int SessionFingerprint;
    public ulong ElapsedFrames = 0;
    public uint DeathCounter = 0;
    public bool[] availableCheckpoints;


    // Use this for initialization
    void Start ()
    {
        availableCheckpoints = new bool[Enum.GetNames(typeof(ExtantCheckpoints)).GetUpperBound(0)];
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
