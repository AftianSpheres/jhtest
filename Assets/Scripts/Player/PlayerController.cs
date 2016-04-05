﻿using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

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

public class PlayerController : MonoBehaviour {
    public WorldController world;
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
    static uint DoubleTapFrameWindow = 20; // no. of frames to check for double-tap sequence over...
    private float HorizAxisDTapBuffer = 0;
    private float VertAxisDTapBuffer = 0;
    private bool HorizAxisReleasedWithinWindow = false;
    private bool VertAxisReleasedWithinWindow = false;
    private uint FramesSinceLastDTapEvent = 0;
    private bool DiscardDodgeInputs;
    public int InvulnTime;
    private static int[] DodgeAllowedStates = { PlayerStateHashes.PlayerStand_D, PlayerStateHashes.PlayerStand_U, PlayerStateHashes.PlayerStand_L, PlayerStateHashes.PlayerStand_R,
    PlayerStateHashes.PlayerWalk_D, PlayerStateHashes.PlayerWalk_U, PlayerStateHashes.PlayerWalk_L, PlayerStateHashes.PlayerWalk_R };

    // Use this for initialization
    //void Start ()
    //{
    //}
	
	// Update is called once per frame
	void Update ()
    {
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

        if (Input.GetButton("Fire1"))
        {
            animator.SetBool("HeldFire1", true);
        }
        else
        {
            animator.SetBool("HeldFire1", false);
        }

        //HeldFire2 bool

        if (Input.GetButton("Fire2"))
        {
            animator.SetBool("HeldFire2", true);
            animator.SetBool("FireSlotB", true);
        }
        else
        {
            animator.SetBool("HeldFire2", false);
        }

        // InputRight/InputLeft triggers

        if (Input.GetAxis("Horizontal") != 0 && DiscardDodgeInputs == false)
        {
            if (HorizAxisReleasedWithinWindow == true)
            {
                if (HorizAxisDTapBuffer > 0 && Input.GetAxis("Horizontal") > 0)
                {
                    animator.SetTrigger("InputRight");
                }
                else if (HorizAxisDTapBuffer < 0 && Input.GetAxis("Horizontal") < 0)
                {
                    animator.SetTrigger("InputLeft");
                }
                HorizAxisReleasedWithinWindow = false;
                FramesSinceLastDTapEvent = 0;
                HorizAxisDTapBuffer = 0;
            }
            else
            {
                HorizAxisDTapBuffer = Input.GetAxis("Horizontal");
            }
        }
        else if (Input.GetAxis("Horizontal") == 0 && HorizAxisDTapBuffer != 0)
        {
            HorizAxisReleasedWithinWindow = true;
        }

        // InputDown/InputUp triggers

        if (Input.GetAxis("Vertical") != 0 && DiscardDodgeInputs == false)
        {
            if (VertAxisReleasedWithinWindow == true)
            {
                if (VertAxisDTapBuffer > 0 && Input.GetAxis("Vertical") > 0)
                {
                    animator.SetTrigger("InputUp");
                }
                else if (VertAxisDTapBuffer < 0 && Input.GetAxis("Vertical") < 0)
                {
                    animator.SetTrigger("InputDown");
                }
                VertAxisReleasedWithinWindow = false;
                FramesSinceLastDTapEvent = 0;
                VertAxisDTapBuffer = 0;
            }
            else
            {
                VertAxisDTapBuffer = Input.GetAxis("Vertical");
            }
        }
        else if (Input.GetAxis("Vertical") == 0 && VertAxisDTapBuffer != 0)
        {
            VertAxisReleasedWithinWindow = true;
        }

        // Double-tap detection housekeeping

        if (HorizAxisDTapBuffer != 0 || VertAxisDTapBuffer != 0)
        {
            FramesSinceLastDTapEvent++;
        }
        if (FramesSinceLastDTapEvent > DoubleTapFrameWindow || DiscardDodgeInputs == true)
        {
            HorizAxisReleasedWithinWindow = false;
            VertAxisReleasedWithinWindow = false;
            FramesSinceLastDTapEvent = 0;
            HorizAxisDTapBuffer = 0;
            VertAxisDTapBuffer = 0;
        }

        // HeldRight/HeldLeft bools

        if (Input.GetAxis("Horizontal") > 0)
        {
            animator.SetBool("HeldRight", true);
            animator.SetBool("HeldLeft", false);
        }
        else if (Input.GetAxis("Horizontal") < 0)
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

        if (Input.GetAxis("Vertical") > 0)
        {
            animator.SetBool("HeldUp", true);
            animator.SetBool("HeldDown", false);
        }
        else if (Input.GetAxis("Vertical") < 0)
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
