using UnityEngine;
using System.Collections;

public class mu_Chest : MonoBehaviour
{
    public RoomController room;
    public mu_ItemPickup pickup;
    new public BoxCollider collider;
    new public SpriteRenderer renderer;
    public Direction directionToOpen;
    public Sprite[] frames;
    public bool open;
    public static int sensitivityZone = 2;
    public AudioSource source;
    public AudioClip clip;
    public mufm_Generic flagManager;
    private Bounds boundsToOpen;

    void Start ()
    {
        boundsToOpen = new Bounds(collider.bounds.center, new Vector3(collider.bounds.size.x + sensitivityZone, collider.bounds.size.y + sensitivityZone, collider.bounds.size.z));
        if (flagManager.CheckFlag() == true)
        {
            renderer.sprite = frames[1];
            open = true;
            Destroy(pickup);
        }
    }
	
	void Update ()
    {
        if (open == false)
        {
            renderer.sprite = frames[0];
            if (room.world.player.Locked == false && room.world.player.facingDir == directionToOpen && boundsToOpen.Intersects(room.world.player.collider.bounds) && Input.GetKeyDown(room.world.PlayerDataManager.K_Confirm) == true)
            {
                StartCoroutine(Open());
            }
        }
        else
        {
            renderer.sprite = frames[1];
        }
	}

    IEnumerator Open ()
    {
        open = true;
        room.world.player.Locked = true;
        renderer.sprite = frames[1];
        source.PlayOneShot(clip);
        for (int i = 0; i <15; i++)
        {
            yield return null;
        }
        room.world.player.Locked = false;
        pickup.gameObject.SetActive(true);
        pickup.Pickup();
        flagManager.ActivateFlag();
    }

#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        boundsToOpen = new Bounds(collider.bounds.center, new Vector3(collider.bounds.size.x + sensitivityZone, collider.bounds.size.y + sensitivityZone, collider.bounds.size.z));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(boundsToOpen.min.x, boundsToOpen.min.y, boundsToOpen.min.z - 100), new Vector3(boundsToOpen.max.x, boundsToOpen.min.y, boundsToOpen.min.z - 100));
        Gizmos.DrawLine(new Vector3(boundsToOpen.max.x, boundsToOpen.min.y, boundsToOpen.min.z - 100), new Vector3(boundsToOpen.max.x, boundsToOpen.max.y, boundsToOpen.min.z - 100));
        Gizmos.DrawLine(new Vector3(boundsToOpen.min.x, boundsToOpen.min.y, boundsToOpen.min.z - 100), new Vector3(boundsToOpen.min.x, boundsToOpen.max.y, boundsToOpen.min.z - 100));
        Gizmos.DrawLine(new Vector3(boundsToOpen.min.x, boundsToOpen.max.y, boundsToOpen.min.z - 100), new Vector3(boundsToOpen.max.x, boundsToOpen.max.y, boundsToOpen.min.z - 100));

    }
#endif
}
