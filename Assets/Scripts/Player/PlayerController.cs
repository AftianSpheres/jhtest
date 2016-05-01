using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Static class containing hashes for player animator states.
/// </summary>
public static class PlayerStateHashes
{
    public static int PlayerStand_D = Animator.StringToHash("Base Layer.Neutral.Standing.PlayerStand_D");
    public static int PlayerStand_U = Animator.StringToHash("Base Layer.Neutral.Standing.PlayerStand_U");
    public static int PlayerStand_L = Animator.StringToHash("Base Layer.Neutral.Standing.PlayerStand_L");
    public static int PlayerStand_R = Animator.StringToHash("Base Layer.Neutral.Standing.PlayerStand_R");
    public static int PlayerWalk_D = Animator.StringToHash("Base Layer.Neutral.Walking.PlayerWalk_D");
    public static int PlayerWalk_U = Animator.StringToHash("Base Layer.Neutral.Walking.PlayerWalk_U");
    public static int PlayerWalk_L = Animator.StringToHash("Base Layer.Neutral.Walking.PlayerWalk_L");
    public static int PlayerWalk_R = Animator.StringToHash("Base Layer.Neutral.Walking.PlayerWalk_R");
    public static int CutsceneWalk_D = Animator.StringToHash("Base Layer.Neutral.Walking.CutsceneWalk_D");
    public static int CutsceneWalk_U = Animator.StringToHash("Base Layer.Neutral.Walking.CutsceneWalk_U");
    public static int CutsceneWalk_L = Animator.StringToHash("Base Layer.Neutral.Walking.CutsceneWalk_L");
    public static int CutsceneWalk_R = Animator.StringToHash("Base Layer.Neutral.Walking.CutsceneWalk_R");
    public static int Dead = Animator.StringToHash("Base Layer.Dead");

}

/// <summary>
/// Controls player. Input handling, coordinating other MonoBehaviours, etc.
/// Kinda ugly.
/// </summary>
[RequireComponent(typeof(PauseableSprite))]
public class PlayerController : MonoBehaviour {
    static int DoubleTapFrameWindow = 20; // no. of frames to check for double-tap sequence over...
    private static int[] DodgeAllowedStates = { PlayerStateHashes.PlayerStand_D, PlayerStateHashes.PlayerStand_U, PlayerStateHashes.PlayerStand_L, PlayerStateHashes.PlayerStand_R,
    PlayerStateHashes.PlayerWalk_D, PlayerStateHashes.PlayerWalk_U, PlayerStateHashes.PlayerWalk_L, PlayerStateHashes.PlayerWalk_R };

    public WorldController world;
    new public BoxCollider2D collider;
    new public SpriteRenderer renderer;
    public Animator animator;
    public AudioSource source;
    public FlickerySprite fs;
    public PlayerBulletOrigin bulletOrigin;
    public PlayerEnergy energy;
    public PlayerWeaponManager wpnManager;
    public Vector3 KnockbackHeading;
    public bool DontWarp;
    public bool IgnoreCollision;
    public bool Invincible;
    public bool Locked;
    public int InvulnTime;
    public int KnockbackFrames;
    private bool DiscardDodgeInputs;
    private int[] DoubleTapWindows = { 0, 0, 0, 0 };
    public uint[] CurrentWorldCoords = { 0, 0 };
    public uint[] CurrentRoomCoords = { 0, 0 };
    private RoomController lastRoom;
    [SerializeField]
    private Sprite specialPoseGFX;
    public Direction facingDir;

	void Start ()
    {
        animator.Play(world.GameStateManager.levelLoadPlayerAnimHash, 0, world.GameStateManager.levelLoadPlayerAnimTime);
    }

    // Update is called once per frame
    void Update ()
    {
        uint chkVal;
        if (world.activeRoom != null && (lastRoom != world.activeRoom || world.activeRoom.BigRoomCellSize.x > 0 || world.activeRoom.BigRoomCellSize.y > 0))
        {
            lastRoom = world.activeRoom;
            chkVal = 0;
            if (world.activeRoom.BigRoomCellSize.y > 0)
            {
                chkVal = (uint)Mathf.FloorToInt((collider.bounds.center.y - world.activeRoom.bounds.min.y) / (HammerConstants.LogicalResolution_Vertical - HammerConstants.HeightOfHUD));
            }
            CurrentWorldCoords[0] = world.activeRoom.yPosition + chkVal;
            CurrentRoomCoords[0] = chkVal;
            chkVal = 0;
            if (world.activeRoom.BigRoomCellSize.y > 0)
            {
                chkVal = (uint)Mathf.FloorToInt((collider.bounds.center.x - world.activeRoom.bounds.min.x) / HammerConstants.LogicalResolution_Horizontal);
            }
            CurrentWorldCoords[1] = world.activeRoom.xPosition + chkVal;
            CurrentRoomCoords[1] = chkVal;
            world.Minimap.Refresh();
        }
        for (int i = 0; i < DoubleTapWindows.Length; i++)
        {
            DoubleTapWindows[i]--;
        }
        if (energy.CurrentEnergy < 1 && animator.GetBool("Dead") == false)
        {
            Die();
        }
        else
        {
            if (Locked == false)
            {
                facingDir = (Direction)animator.GetInteger("FacingDir");
                HandleInputs();
                if (DontWarp == true)
                {
                    DontWarp = false;
                }
            }
            else
            {
                animator.SetBool("HeldFire1", false);
                animator.SetBool("HeldFire2", false);
                animator.SetBool("HeldRight", false);
                animator.SetBool("HeldLeft", false);
                animator.SetBool("HeldDown", false);
                animator.SetBool("HeldUp", false);
            }
        }
        if (InvulnTime > 0)
        {
            InvulnTime--;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet") == true)
        {
            Hit(other.gameObject.GetComponent<BulletController>());
        }
        else if (other.CompareTag("Enemy") == true)
        {
            Hit(other.gameObject.GetComponent<CommonEnemyController>());
        }
        else if (other.CompareTag("Boom") == true)
        {
            Hit(other.gameObject.GetComponent<BoomEffect>());
        }
        else if (other.CompareTag("Pickup") == true)
        {
            other.gameObject.GetComponent<mu_ItemPickup>().Pickup();
        }
    }

    /// <summary>
    /// Die motherfucker die motherfucker die.
    /// </summary>
    void Die ()
    {
        animator.SetBool("Dead", true);
        animator.SetTrigger("Die");
    }

    /// <summary>
    /// Ugly piece of shit that turns Unity inputs into state machine attributes.
    /// </summary>
    void HandleInputs()
    {
        if (DodgeAllowedStates.Contains(animator.GetCurrentAnimatorStateInfo(0).fullPathHash))
        {
            DiscardDodgeInputs = false;
        }
        else
        {
            DiscardDodgeInputs = true;
        }

        //HeldFire1 bool

        if (Input.GetKey(world.PlayerDataManager.K_Fire1) == true && wpnManager.SlotAWpn != WeaponType.None)
        {
            animator.SetBool("HeldFire1", true);
        }
        else
        {
            animator.SetBool("HeldFire1", false);
        }

        //HeldFire2 bool

        if (Input.GetKey(world.PlayerDataManager.K_Fire2) == true && wpnManager.SlotBWpn != WeaponType.None)
        {
            animator.SetBool("HeldFire2", true);
            animator.SetBool("FireSlotB", true);
        }
        else
        {
            animator.SetBool("HeldFire2", false);
        }

        if (DiscardDodgeInputs == false)
        {
            // InputRight/InputLeft triggers

            if (Input.GetKeyDown(world.PlayerDataManager.K_HorizLeft) == true)
            {
                if (DoubleTapWindows[(int)Direction.Left] > 0)
                {
                    animator.SetTrigger("InputLeft");
                }
                else
                {
                    DoubleTapWindows[(int)Direction.Left] = DoubleTapFrameWindow;
                }
            }
            else if (Input.GetKeyDown(world.PlayerDataManager.K_HorizRight) == true)
            {
                if (DoubleTapWindows[(int)Direction.Right] > 0)
                {
                    animator.SetTrigger("InputRight");
                }
                else
                {
                    DoubleTapWindows[(int)Direction.Right] = DoubleTapFrameWindow;
                }
            }

            // InputDown/InputUp triggers

            if (Input.GetKeyDown(world.PlayerDataManager.K_VertDown) == true)
            {
                if (DoubleTapWindows[(int)Direction.Down] > 0)
                {
                    animator.SetTrigger("InputDown");
                }
                else
                {
                    DoubleTapWindows[(int)Direction.Down] = DoubleTapFrameWindow;
                }
            }
            else if (Input.GetKeyDown(world.PlayerDataManager.K_VertUp) == true)
            {
                if (DoubleTapWindows[(int)Direction.Up] > 0)
                {
                    animator.SetTrigger("InputUp");
                }
                else
                {
                    DoubleTapWindows[(int)Direction.Up] = DoubleTapFrameWindow;
                }
            }
        }

        // HeldRight/HeldLeft bools

        if (Input.GetKey(world.PlayerDataManager.K_HorizRight) == true)
        {
            animator.SetBool("HeldRight", true);
            animator.SetBool("HeldLeft", false);
        }
        else if (Input.GetKey(world.PlayerDataManager.K_HorizLeft) == true)
        {
            animator.SetBool("HeldRight", false);
            animator.SetBool("HeldLeft", true);
        }
        else
        {
            animator.SetBool("HeldRight", false);
            animator.SetBool("HeldLeft", false);
        }

        // HeldDown/HeldUp bools

        if (Input.GetKey(world.PlayerDataManager.K_VertUp) == true)
        {
            animator.SetBool("HeldUp", true);
            animator.SetBool("HeldDown", false);
        }
        else if (Input.GetKey(world.PlayerDataManager.K_VertDown) == true)
        {
            animator.SetBool("HeldUp", false);
            animator.SetBool("HeldDown", true);
        }
        else
        {
            animator.SetBool("HeldUp", false);
            animator.SetBool("HeldDown", false);
        }
    }

    /// <summary>
    /// We've been hit by something: in this case, a bullet.
    /// </summary>
    void Hit(BulletController bullet)
    {
        if (animator.GetBool("Dead") == true)
        {
            bullet.HitTarget();
        }
        else if (bullet.ShotType == WeaponType.spEnergyRecover) // not a "real" bullet, but a thing that acts like an enemy bullet, sorta
        {
            bullet.HitTarget();
            energy.Recover(100);
        }
        else if (Invincible == false && Locked == false)
        {
            if (animator.GetBool("DodgeBurst") == false && InvulnTime < 1)
            {
                bullet.HitTarget();
                animator.SetTrigger("Hit");
                energy.CurrentEnergy = energy.CurrentEnergy - bullet.Damage;
                KnockbackHeading = bullet.Heading;
                KnockbackFrames = bullet.Weight;
            }
        }
    }

    /// <summary>
    /// We've been hit by something: in this case, a dude.
    /// Handles collision, files assault charges.
    /// </summary>
    void Hit(CommonEnemyController enemy)
    {
        if (Invincible == false && Locked == false && animator.GetBool("Dead") == false)
        {
            if (animator.GetBool("DodgeBurst") == false && enemy.CollideDmg > 0 && InvulnTime < 1)
            {
                animator.SetTrigger("Hit");
                energy.CurrentEnergy = energy.CurrentEnergy - enemy.CollideDmg;
                KnockbackHeading = enemy.Heading;
                KnockbackFrames = enemy.Weight;
            }
        }
    }

    /// <summary>
    /// We've been hit by something: in this case, an explosion.
    /// </summary>
    void Hit(BoomEffect boom)
    {
        if (Invincible == false && Locked == false && boom.owner != gameObject && animator.GetBool("Dead") == false)
        {
            if (animator.GetBool("DodgeBurst") == false && boom.Collideable == true && InvulnTime < 1)
            {
                animator.SetTrigger("Hit");
                energy.CurrentEnergy = energy.CurrentEnergy - boom.Damage;
                if (boom.collider.bounds.center.y > collider.bounds.center.y)
                {
                    KnockbackHeading = Vector2.down * boom.PushbackStrength;
                }
                else if (boom.collider.bounds.center.y < collider.bounds.center.y)
                {
                    KnockbackHeading = Vector2.up * boom.PushbackStrength;
                }
                else if (boom.collider.bounds.center.y < collider.bounds.center.y)
                {
                    KnockbackHeading = Vector2.left * boom.PushbackStrength;
                }
                else
                {
                    KnockbackHeading = Vector2.right * boom.PushbackStrength;
                }
                KnockbackFrames = boom.PushbackStrength;
            }
        }
    }

    public void DoSpecialPose ()
    {
        renderer.sprite = specialPoseGFX;
    }

    /// <summary>
    /// Respawns player.
    /// </summary>
    public void Respawn ()
    {
        world.GameStateManager.RespawnPlayer();
        animator.ResetTrigger("Hit");
    }

    /// <summary>
    /// Toggles the DodgeBurst property on the animator state machine.
    /// </summary>
    public void SetAnimatorDodgeBurst ()
    {
        if (animator.GetBool("DodgeBurst") == true)
        {
            animator.SetBool("DodgeBurst", false);
        }
        else
        {
            animator.SetBool("DodgeBurst", true);
        }       
    }

    /// <summary>
    /// Toggles the Braking property on the animator state machine.
    /// </summary>
    public void SetAnimatorBraking ()
    {
        if (animator.GetBool("Braking") == true)
        {
            animator.SetBool("Braking", false);
        }
        else
        {
            animator.SetBool("Braking", true);
        }

    }
}
