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

}

/// <summary>
/// Controls player. Input handling, coordinating other MonoBehaviours, etc.
/// Kinda ugly.
/// </summary>
public class PlayerController : MonoBehaviour {
    public WorldController world;
    new public SpriteRenderer renderer;
    public FlickerySprite fs;
    public PlayerWeaponManager wpnManager;
    public PlayerEnergy energy;
    public Animator animator;
    public PlayerBulletOrigin bulletOrigin;
    new public BoxCollider2D collider;
    public Vector3 KnockbackHeading;
    public int KnockbackFrames;
    public bool Invincible;
    public bool Locked;
    public bool IgnoreCollision;
    static int DoubleTapFrameWindow = 20; // no. of frames to check for double-tap sequence over...
    private bool DiscardDodgeInputs;
    public int InvulnTime;
    private int[] DoubleTapWindows = { 0, 0, 0, 0 };
    private static int[] DodgeAllowedStates = { PlayerStateHashes.PlayerStand_D, PlayerStateHashes.PlayerStand_U, PlayerStateHashes.PlayerStand_L, PlayerStateHashes.PlayerStand_R,
    PlayerStateHashes.PlayerWalk_D, PlayerStateHashes.PlayerWalk_U, PlayerStateHashes.PlayerWalk_L, PlayerStateHashes.PlayerWalk_R };
    public AudioSource source;
	
	// Update is called once per frame
	void Update ()
    {
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
                HandleInputs();
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
    }

    /// <summary>
    /// Die motherfucker die motherfucker die.
    /// Then load the Game Over screen motherfucker load the Game Over screen motherfucker load the Game Over screen.
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

        if (Input.GetKey(KeyCode.Slash) == true)
        {
            Debug.Break();
        }
        else
        {
        }

        if (Input.GetKey(world.PlayerDataManager.K_Fire1) == true)
        {
            animator.SetBool("HeldFire1", true);
        }
        else
        {
            animator.SetBool("HeldFire1", false);
        }

        //HeldFire2 bool

        if (Input.GetKey(world.PlayerDataManager.K_Fire2) == true)
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
        if (Invincible == false && Locked == false)
        {
            if (animator.GetBool("DodgeBurst") == false && InvulnTime < 1)
            {
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
        if (Invincible == false && Locked == false)
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
    /// Respawns player.
    /// </summary>
    public void Respawn ()
    {
        world.GameStateManager.LastCheckpoint.RespawnAt();
        world.GameStateManager.RerollSessionFingerprint();
        energy.Recover(100);
        animator.Play("PlayerStand_D");
        animator.SetBool("Dead", false);
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
