using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Is a reticle. Exists relative to viewport. Moves around. You get the idea.
/// </summary>
public class PlayerReticleController : MonoBehaviour
{
    public WorldController world;
    private Camera mainCamera;
    new public SpriteRenderer renderer;
    public Sprite OpenSprite;
    public Sprite ClosedSprite;
    public uint HalfSizeOfReticleSprite = 7;
    private float adjX;
    private float adjY;
    public Vector2 DefaultOffset;
    private Vector3 virtualPosition;

	// Use this for initialization
	void Start ()
    {
        mainCamera = world.mainCamera;
        virtualPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (world.HardwareInterfaceManager.Fire1.Pressed == true || world.HardwareInterfaceManager.Fire2.Pressed == true)
        {
            renderer.sprite = ClosedSprite;
        }
        else
        {
            renderer.sprite = OpenSprite;
        }
        if (world.HardwareInterfaceManager.usingRightStick == false)
        {
            adjX = virtualPosition.x + Input.GetAxis("MouseX");
            if (mainCamera.WorldToViewportPoint(new Vector3((adjX + HalfSizeOfReticleSprite), 0, 0)).x < 0)
            {
                adjX = mainCamera.ViewportToWorldPoint(Vector3.zero).x - HalfSizeOfReticleSprite;
            }
            else if (mainCamera.WorldToViewportPoint(new Vector3((adjX + HalfSizeOfReticleSprite), 0, 0)).x > 1)
            {
                adjX = mainCamera.ViewportToWorldPoint(Vector3.one).x - HalfSizeOfReticleSprite;
            }
            adjY = virtualPosition.y + Input.GetAxis("MouseY");
            if (mainCamera.WorldToViewportPoint(new Vector3(0, (adjY - HalfSizeOfReticleSprite), 0)).y < 0)
            {
                adjY = mainCamera.ViewportToWorldPoint(Vector3.zero).y + HalfSizeOfReticleSprite;
            }
            else if (mainCamera.WorldToViewportPoint(new Vector3(0, (adjY - HalfSizeOfReticleSprite + 16), 0)).y > 1)
            {
                adjY = mainCamera.ViewportToWorldPoint(Vector3.one).y + HalfSizeOfReticleSprite - 16;
            }
            virtualPosition = new Vector3(adjX, adjY, virtualPosition.z);
            transform.position = new Vector3((float)Math.Round(adjX, 0, MidpointRounding.AwayFromZero), (float)Math.Round(adjY, 0, MidpointRounding.AwayFromZero), transform.position.z);
        }
        else
        {
            adjX = virtualPosition.x + (2 * Input.GetAxis(HardwareInterfaceManager.RightStickXes[world.HardwareInterfaceManager.GamepadIndex]));
            if (mainCamera.WorldToViewportPoint(new Vector3((adjX + HalfSizeOfReticleSprite), 0, 0)).x < 0)
            {
                adjX = mainCamera.ViewportToWorldPoint(Vector3.zero).x - HalfSizeOfReticleSprite;
            }
            else if (mainCamera.WorldToViewportPoint(new Vector3((adjX + HalfSizeOfReticleSprite), 0, 0)).x > 1)
            {
                adjX = mainCamera.ViewportToWorldPoint(Vector3.one).x - HalfSizeOfReticleSprite;
            }
            adjY = virtualPosition.y + 2 * (Input.GetAxis(HardwareInterfaceManager.RightStickYs[world.HardwareInterfaceManager.GamepadIndex]));
            if (mainCamera.WorldToViewportPoint(new Vector3(0, (adjY - HalfSizeOfReticleSprite), 0)).y < 0)
            {
                adjY = mainCamera.ViewportToWorldPoint(Vector3.zero).y + HalfSizeOfReticleSprite;
            }
            else if (mainCamera.WorldToViewportPoint(new Vector3(0, (adjY - HalfSizeOfReticleSprite + 16), 0)).y > 1)
            {
                adjY = mainCamera.ViewportToWorldPoint(Vector3.one).y + HalfSizeOfReticleSprite - 16;
            }
            virtualPosition = new Vector3(adjX, adjY, virtualPosition.z);
            transform.position = new Vector3((float)Math.Round(adjX, 0, MidpointRounding.AwayFromZero), (float)Math.Round(adjY, 0, MidpointRounding.AwayFromZero), transform.position.z);
        }
    }

    /// <summary>
    /// Snaps reticle back to default screen co-ords.
    /// </summary>
    public void Snap()
    {
        transform.localPosition = DefaultOffset;
    }

}
