using UnityEngine;
using System.Collections;

public class mu_Sign : MonoBehaviour
{
    public RoomController room;
    public Bounds interactionBounds;
    public string textPath;
    public Direction readingDirection;
    private TextAsset text;

	// Use this for initialization
	void Start ()
    {
        text = Resources.Load<TextAsset>("Text/" + HammerConstants.LocalizationPrefix + "/" + textPath);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (room.world.player.Locked == false && room.world.player.facingDir == readingDirection && interactionBounds.Intersects(room.world.player.collider.bounds) && HardwareInterfaceManager.Instance.Confirm.BtnDown &&
                room.world.player.interactTimer < 1)
        {
            room.world.player.interactTimer = 10;
            room.world.MainTextbox.StartPrinting(text);
        }
	}

#if UNITY_EDITOR
    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(interactionBounds.min.x, interactionBounds.min.y, interactionBounds.min.z - 100), new Vector3(interactionBounds.max.x, interactionBounds.min.y, interactionBounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(interactionBounds.max.x, interactionBounds.min.y, interactionBounds.min.z - 100), new Vector3(interactionBounds.max.x, interactionBounds.max.y, interactionBounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(interactionBounds.min.x, interactionBounds.min.y, interactionBounds.min.z - 100), new Vector3(interactionBounds.min.x, interactionBounds.max.y, interactionBounds.min.z - 100));
        Gizmos.DrawLine(new Vector3(interactionBounds.min.x, interactionBounds.max.y, interactionBounds.min.z - 100), new Vector3(interactionBounds.max.x, interactionBounds.max.y, interactionBounds.min.z - 100));
    }
#endif
}
