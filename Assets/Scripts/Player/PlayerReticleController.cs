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

	// Use this for initialization
	void Start ()
    {
        virtualPosition = transform.localPosition;
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
            adjY = virtualPosition.y + Input.GetAxis("MouseY");
        }
        else
        {
            adjX = virtualPosition.x + (world.HardwareInterfaceManager.RightStick.x);
            adjY = virtualPosition.y + (world.HardwareInterfaceManager.RightStick.y);
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
