using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerController))]
public class PlayerCameraFollow : MonoBehaviour
{
    public PlayerController master;
    new public BoxCollider2D collider;
	
	// Update is called once per frame
	void Update ()
    {
        if (master.Locked == false)
        {
            if (collider.bounds.center.y < master.world.cameraController.rect.yMin)
            {
                master.world.cameraController.ScrollScreen(0);
            }
            else if (collider.bounds.center.y > master.world.cameraController.rect.yMax)
            {
                master.world.cameraController.ScrollScreen(1);
            }
            else if (collider.bounds.center.x < master.world.cameraController.rect.xMin)
            {
                master.world.cameraController.ScrollScreen(2);
            }
            else if (collider.bounds.center.x > master.world.cameraController.rect.xMax)
            {
                master.world.cameraController.ScrollScreen(3);
            }
        }

	}
}
