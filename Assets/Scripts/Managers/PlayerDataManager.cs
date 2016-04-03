using UnityEngine;
using System.Collections;

public enum WindowedResolutionMultiplier
{
    x1, // 160x144
    x2, // 320x288
    x3, // 480x432
    x4, // 640x572
    x5, // 800x716
    x6, // 960x860
    x8, // 1280x1148
}

public class PlayerDataManager : Manager <PlayerDataManager>
{
    public mu_Checkpoint LastCheckpoint;
    public ulong ElapsedFrames = 0;
    public uint DeathCounter = 0;

    public bool DisplayFullscreen = false;
    public WindowedResolutionMultiplier WindowedRes = WindowedResolutionMultiplier.x2;
    public KeyCode K_HorizLeft = KeyCode.A;
    public KeyCode K_HorizRight = KeyCode.D;
    public KeyCode K_VertUp = KeyCode.W;
    public KeyCode K_VertDown = KeyCode.S;
    public KeyCode K_Menu = KeyCode.LeftShift;
    public KeyCode K_Confirm = KeyCode.E;
    public KeyCode K_Cancel = KeyCode.Q;
    public KeyCode K_Fire1 = KeyCode.Mouse0;
    public KeyCode K_Fire2 = KeyCode.Mouse1;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        ElapsedFrames++;
	}
}
