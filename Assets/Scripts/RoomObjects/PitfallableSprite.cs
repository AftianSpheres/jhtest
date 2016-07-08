using UnityEngine;
using System.Collections;

public class PitfallableSprite : MonoBehaviour
{
    public WorldController world;
    public PlayerController player;
    public CommonEnemyController enemy;
    public Vector3 respawnPosition;
    Vector3 lastFramePosition;
    int continuousPitfallFrames = 0;
    static int pitfallTime = 10;

	// Use this for initialization
	void Start ()
    {
        if (player != null)
        {
            respawnPosition = player.transform.position;
        }
        else
        {
            respawnPosition = enemy.transform.position;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (world.activeRoom != null && world.activeRoom.pitfallZones.Length > 0)
        {
            bool withinPit = false;
            bool touchingPit = false;
            if (player != null && player.isFlying == false && player.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != PlayerAnimatorHashes.Pitfall)
            {
                for (int i = 0; i < world.activeRoom.pitfallZones.Length; i++)
                {
                    if (world.activeRoom.pitfallZones[i].Contains(player.collider.bounds.center))
                    {
                        withinPit = true;
                    }
                    if (world.activeRoom.pitfallZones[i].Intersects(player.collider.bounds))
                    {
                        touchingPit = true;
                    }
                }
                if (touchingPit == false && player.renderer.enabled == true)
                {
                    respawnPosition = player.transform.position;
                }
                if (withinPit == false)
                {
                    continuousPitfallFrames = 0;
                }
                else
                {
                    continuousPitfallFrames++;
                    if (continuousPitfallFrames > pitfallTime || lastFramePosition == player.transform.position)
                    {
                        PitfallSomething();
                    }
                }
                lastFramePosition = player.transform.position;
            }
        }
	}

    void PitfallSomething ()
    {
        continuousPitfallFrames = 0;
        if (player != null)
        {
            player.Pitfall();
        }
    }
}
