using UnityEngine;
using System.Collections;

public class mu_Checkpoint : MonoBehaviour
{
    public RoomController room;
    public ExtantCheckpoints checkpointValue;
    public Vector2 SpawnPosition;

	// Use this for initialization
	void Start ()
    {
	}

    public void Activate ()
    {
        room.world.GameStateManager.LastCheckpoint = this;
        if (room.world.GameStateManager.availableCheckpoints[(int)checkpointValue] == false)
        {
            room.world.GameStateManager.availableCheckpoints[(int)checkpointValue] = true;
        }
    }

    public void RespawnAt ()
    {
        room.world.player.transform.position = new Vector3(room.bounds.min.x + SpawnPosition.x, room.bounds.min.y + SpawnPosition.y + HammerConstants.SizeOfOneTile, room.world.player.transform.position.z);
        StartCoroutine(room.world.cameraController.InstantChangeScreen(room, 60));
    }
}
