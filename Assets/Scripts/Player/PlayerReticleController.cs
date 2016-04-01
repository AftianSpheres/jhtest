using UnityEngine;
using System;
using System.Collections;

public class PlayerReticleController : MonoBehaviour
{
    public WorldController world;
    new private Camera mainCamera;
    private PlayerController player;
    new public SpriteRenderer renderer;
    public Sprite OpenSprite;
    public Sprite ClosedSprite;
    public uint HalfSizeOfReticleSprite = 7;
    private float adjX;
    private float adjY;
    public Vector2 DefaultOffset;

	// Use this for initialization
	void Start ()
    {
        mainCamera = world.mainCamera;
        player = world.player;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
        {
            renderer.sprite = ClosedSprite;
        }
        else
        {
            renderer.sprite = OpenSprite;
        }

        adjX = transform.position.x + Input.GetAxis("CursorHoriz");
        if (mainCamera.WorldToViewportPoint(new Vector3((adjX + HalfSizeOfReticleSprite), 0, 0)).x < 0)
        {
            adjX = mainCamera.ViewportToWorldPoint(Vector3.zero).x - HalfSizeOfReticleSprite;
        }
        else if (mainCamera.WorldToViewportPoint(new Vector3((adjX + HalfSizeOfReticleSprite), 0, 0)).x > 1)
        {
            adjX = mainCamera.ViewportToWorldPoint(Vector3.one).x - HalfSizeOfReticleSprite;
        }
        adjY = transform.position.y + Input.GetAxis("CursorVert");
        if (mainCamera.WorldToViewportPoint(new Vector3(0, (adjY - HalfSizeOfReticleSprite), 0)).y < 0)
        {
            adjY = mainCamera.ViewportToWorldPoint(Vector3.zero).y + HalfSizeOfReticleSprite;
        }
        else if (mainCamera.WorldToViewportPoint(new Vector3(0, (adjY - HalfSizeOfReticleSprite + 16), 0)).y > 1)
        {
            adjY = mainCamera.ViewportToWorldPoint(Vector3.one).y + HalfSizeOfReticleSprite - 16;
        }
        transform.position = new Vector3((float)Math.Round(adjX, 0, MidpointRounding.AwayFromZero), (float)Math.Round(adjY, 0, MidpointRounding.AwayFromZero), transform.position.z);

    }

    public void Snap()
    {
        transform.localPosition = DefaultOffset;
    }

}
