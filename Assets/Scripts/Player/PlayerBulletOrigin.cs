using UnityEngine;
using System.Collections;

/// <summary>
/// Manages weapon graphics & offsets for bullet origin.
/// </summary>
public class PlayerBulletOrigin : MonoBehaviour
{
    public PlayerController master;
    new public SpriteRenderer renderer;
    public Sprite[] spriteSet_00;
    public Sprite[] spriteSet_01;
    public Sprite[] spriteSet_02;
    public Sprite[] spriteSet_03;
    private static Vector3[] offsets = { new Vector3(0, -11, 0), new Vector3(0, -7, 0), new Vector3(-16, -6, 0), new Vector3(8, -6, 0) };
	
	// Update is called once per frame
	void Update ()
    {
        float z = 0;
        if (master.animator.GetBool("Shooting") == false)
        {
            renderer.enabled = false;
        }
        else
        {
            renderer.enabled = true;
            WeaponType wpn;
            if (master.animator.GetBool("FireSlotB") == true)
            {
                wpn = master.wpnManager.SlotBWpn;
            }
            else
            {
                wpn = master.wpnManager.SlotAWpn;
            }
            switch (wpn)
            {
                case WeaponType.pWG:
                case WeaponType.pWGII:
                    renderer.sprite = spriteSet_00[master.animator.GetInteger("FacingDir")];
                    break;
                case WeaponType.pShotgun:
                    renderer.sprite = spriteSet_01[master.animator.GetInteger("FacingDir")];
                    break;
                case WeaponType.pShadow:
                    renderer.sprite = spriteSet_02[master.animator.GetInteger("FacingDir")];
                    break;
                case WeaponType.pFlamethrower:
                    renderer.sprite = spriteSet_03[master.animator.GetInteger("FacingDir")];
                    break;
            }
            if (master.animator.GetInteger("FacingDir") == 1 || master.animator.GetInteger("FacingDir") == 2)
            {
                z = 0.01f;
            }
            else
            {
                z = -0.01f;
            }
        }
        transform.localPosition = new Vector3(offsets[master.animator.GetInteger("FacingDir")].x, offsets[master.animator.GetInteger("FacingDir")].y, z);
	}
}
