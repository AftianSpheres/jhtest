using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class mu_Checkpoint : MonoBehaviour
{
    public RoomController room;
    public Checkpoints checkpointValue;
    public Vector2 SpawnPosition;

	// Use this for initialization
	void Start ()
    {
        if (room.world.GameStateManager.LastCheckpoint == Checkpoints.None && room.replacementValue == 0)
        {
            Activate();
        }
	}

    public void Activate ()
    {
        room.world.GameStateManager.LastCheckpoint = checkpointValue;
        room.world.GameStateManager.respawnPosition = new Vector3(room.bounds.min.x + SpawnPosition.x, room.bounds.min.y + SpawnPosition.y + HammerConstants.SizeOfOneTile, room.world.player.transform.position.z);
        room.world.GameStateManager.respawnLevelIndex = SceneManager.GetActiveScene().buildIndex;
        room.world.GameStateManager.respawnRoomCoords[0] = room.yPosition;
        room.world.GameStateManager.respawnRoomCoords[1] = room.xPosition;
        if ((room.world.GameStateManager.availableCheckpoints & checkpointValue) != checkpointValue)
        {
            room.world.GameStateManager.availableCheckpoints |= checkpointValue;
        }
    }
}
