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
    private static Vector3[] offsets = { new Vector3(4, -11, 0), new Vector3(10, -7, 0), new Vector3(-1, -11, 0), new Vector3(7, -11, 0) };
	
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
                    renderer.sprite = default(Sprite);
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
