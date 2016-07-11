using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Is a reticle. Exists relative to viewport. Moves around. You get the idea.
/// </summary>
public class PlayerReticleController : MonoBehaviour
{
    public WorldController world;
    new public SpriteRenderer renderer;
    public Sprite OpenSprite;
    public Sprite ClosedSprite;
    public uint HalfSizeOfReticleSprite = 7;
    private float adjX;
    private float adjY;
    public Vector2 DefaultOffset;
    private Vector3 virtualPosition;
    private Vector3 defaultLocalPosition;
    public Transform looker;
    bool twinStickLockedCursorMode;
    static float stickDist = 40f;

	// Use this for initialization
	void Start ()
    {
        virtualPosition = defaultLocalPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update ()
    {
        twinStickLockedCursorMode = world.HardwareInterfaceManager.usingRightStick;
        if (world.HardwareInterfaceManager.Fire1.Pressed == true || world.HardwareInterfaceManager.Fire2.Pressed == true)
        {
            renderer.sprite = ClosedSprite;
        }
        else
        {
            renderer.sprite = OpenSprite;
        }
        if (twinStickLockedCursorMode == false)
        {
            adjX = virtualPosition.x + Input.GetAxis("mouse x");
            adjY = virtualPosition.y + Input.GetAxis("mouse y");
        }
        else
        {
            float stickX = world.HardwareInterfaceManager.RightStick.x;
            float stickY = world.HardwareInterfaceManager.RightStick.y;
            Vector3 c = new Vector3(world.player.renderer.bounds.center.x, world.player.renderer.bounds.center.y, 0) - transform.parent.position + defaultLocalPosition;
            // new approach: use a separate Transform on a child object that just looks at things, then put the cursor 16px along its forward direction
            looker.LookAt(new Vector3(looker.position.x + (stickX * 360), looker.position.y + (stickY * 360), 0));
            renderer.enabled = (world.HardwareInterfaceManager.RightStick.x != 0 || world.HardwareInterfaceManager.RightStick.y != 0);
            if (renderer.enabled == true)
            {
                adjX = c.x + (stickDist * looker.forward.x);
                adjY = c.y + (stickDist * looker.forward.y) + 4;
            }
        }
        if (adjX < -(HammerConstants.LogicalResolution_Horizontal / 2) - HalfSizeOfReticleSprite)
        {
            adjX = -(HammerConstants.LogicalResolution_Horizontal / 2) - HalfSizeOfReticleSprite;
        }
        else if (adjX > (HammerConstants.LogicalResolution_Horizontal / 2) - HalfSizeOfReticleSprite)
        {
            adjX = (HammerConstants.LogicalResolution_Horizontal / 2) - HalfSizeOfReticleSprite;
        }
        if (adjY < -(HammerConstants.LogicalResolution_Vertical / 2) + HalfSizeOfReticleSprite)
        {
            adjY = -(HammerConstants.LogicalResolution_Vertical / 2) + HalfSizeOfReticleSprite;
        }
        else if (adjY > (HammerConstants.LogicalResolution_Vertical / 2) + HalfSizeOfReticleSprite - HammerConstants.HeightOfHUD)
        {
            adjY = (HammerConstants.LogicalResolution_Vertical / 2) + HalfSizeOfReticleSprite - HammerConstants.HeightOfHUD;
        }
        virtualPosition = new Vector3(adjX, adjY, virtualPosition.z);
        transform.localPosition = new Vector3((float)Math.Round(adjX, 0, MidpointRounding.AwayFromZero), (float)Math.Round(adjY, 0, MidpointRounding.AwayFromZero), transform.localPosition.z);
    }

    /// <summary>
    /// Snaps reticle back to default screen co-ords.
    /// </summary>
    public void Snap()
    {
        transform.localPosition = DefaultOffset;
    }

}
