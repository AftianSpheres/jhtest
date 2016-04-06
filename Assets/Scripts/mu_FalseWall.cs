using UnityEngine;
using System.Collections;

/// <summary>
/// False walls, destructible blocks, etc.
/// </summary>
public class mu_FalseWall : MonoBehaviour {
    public RoomController room;
    new public BoxCollider collider;
    new public SpriteRenderer renderer;
    public mu_RoomEvent roomEvent;
    public RegisteredSprite register;


    // Use this for initialization
    void Start()
    {
        register.roomObjectRespawnAction = Respawn;
    }

    // Update is called once per frame
    void Update()
    {
        if (roomEvent.EventActive == true)
        {
            Disappear();
        }
    }

    /// <summary>
    /// Goes poof.
    /// </summary>
    void Disappear()
    {
        collider.enabled = false;
        renderer.enabled = false;
    }


    /// <summary>
    /// Goes un-poof.
    /// </summary>
    public void Respawn()
    {
        roomEvent.Reset();
        collider.enabled = true;
        renderer.enabled = true;
    }
}
