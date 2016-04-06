using UnityEngine;
using System.Collections;

/// <summary>
/// MonoBehaviour that makes the camera follow the player outside of room bounds.
/// </summary>
[RequireComponent (typeof(PlayerController))]
public class PlayerCameraFollow : MonoBehaviour
{
    public PlayerController master;
    new public BoxCollider2D collider;
	
	// Update is called once per frame
	void Update ()
    {
        if (master.Locked == false && master.world.activeRoom != null)
        {
            if (collider.bounds.center.y < master.world.activeRoom.bounds.min.y)
            {
                master.world.cameraController.ScrollAndChangeScreen(0);
            }
            else if (collider.bounds.center.y > master.world.activeRoom.bounds.max.y)
            {
                master.world.cameraController.ScrollAndChangeScreen(1);
            }
            else if (collider.bounds.center.x < master.world.activeRoom.bounds.min.x)
            {
                master.world.cameraController.ScrollAndChangeScreen(2);
            }
            else if (collider.bounds.center.x > master.world.activeRoom.bounds.max.x)
            {
                master.world.cameraController.ScrollAndChangeScreen(3);
            }
        }

	}
}
