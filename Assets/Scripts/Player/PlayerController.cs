using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Static class containing hashes for player animator states.
/// </summary>
public static class PlayerAnimatorHashes
{
    public static int PlayerStand_D = Animator.StringToHash("Base Layer.StdStates.Neutral.Standing.PlayerStand_D");
    public static int PlayerStand_U = Animator.StringToHash("Base Layer.StdStates.Neutral.Standing.PlayerStand_U");
    public static int PlayerStand_L = Animator.StringToHash("Base Layer.StdStates.Neutral.Standing.PlayerStand_L");
    public static int PlayerStand_R = Animator.StringToHash("Base Layer.StdStates.Neutral.Standing.PlayerStand_R");
    public static int PlayerWalk_D = Animator.StringToHash("Base Layer.StdStates.Neutral.Walking.PlayerWalk_D");
    public static int PlayerWalk_U = Animator.StringToHash("Base Layer.StdStates.Neutral.Walking.PlayerWalk_U");
    public static int PlayerWalk_L = Animator.StringToHash("Base Layer.StdStates.Neutral.Walking.PlayerWalk_L");
    public static int PlayerWalk_R = Animator.StringToHash("Base Layer.StdStates.Neutral.Walking.PlayerWalk_R");
    public static int PlayerCorpse = Animator.StringToHash("Base Layer.StdStates.PlayerCorpse");
    public static int PlayerDisintegrate = Animator.StringToHash("Base Layer.StdStates.PlayerDisintegrate");
    public static int CutsceneWalk_D = Animator.StringToHash("Base Layer.StdStates.Neutral.Walking.CutsceneWalk_D");
    public static int CutsceneWalk_U = Animator.StringToHash("Base Layer.StdStates.Neutral.Walking.CutsceneWalk_U");
    public static int CutsceneWalk_L = Animator.StringToHash("Base Layer.StdStates.Neutral.Walking.CutsceneWalk_L");
    public static int CutsceneWalk_R = Animator.StringToHash("Base Layer.StdStates.Neutral.Walking.CutsceneWalk_R");
    public static int Dead = Animator.StringToHash("Base Layer.Dead");
    public static int Pitfall = Animator.StringToHash("Base Layer.PlayerFall");
    public static int paramDead = Animator.StringToHash("Dead");
    public static int paramFacingDir = Animator.StringToHash("FacingDir");
    public static int paramHeldFire1 = Animator.StringToHash("HeldFire1");
    public static int paramHeldFire2 = Animator.StringToHash("HeldFire2");
    public static int paramHeldRight = Animator.StringToHash("HeldRight");
    public static int paramHeldLeft = Animator.StringToHash("HeldLeft");
    public static int paramHeldDown = Animator.StringToHash("HeldDown");
    public static int paramHeldUp = Animator.StringToHash("HeldUp");
    public static int triggerDie = Animator.StringToHash("Die");
    public static int triggerDodgeBurst = Animator.StringToHash("DodgeBurst");
    public static int paramExternalMoveSpeedMulti = Animator.StringToHash("ExternalMoveSpeedMulti");
    public static int paramInternalMoveSpeedMulti = Animator.StringToHash("InternalMoveSpeedMulti");
    public static int paramMoveSpeed = Animator.StringToHash("MoveSpeed");
}

/// <summary>
/// Controls player. Input handling, coordinating other MonoBehaviours, etc.
/// Kinda ugly.
/// </summary>
[RequireComponent(typeof(PauseableSprite))]
public class PlayerController : MonoBehaviour {
    private static int[] DodgeAllowedStates = { PlayerAnimatorHashes.PlayerStand_D, PlayerAnimatorHashes.PlayerStand_U, PlayerAnimatorHashes.PlayerStand_L, PlayerAnimatorHashes.PlayerStand_R,
    PlayerAnimatorHashes.PlayerWalk_D, PlayerAnimatorHashes.PlayerWalk_U, PlayerAnimatorHashes.PlayerWalk_L, PlayerAnimatorHashes.PlayerWalk_R };

    public WorldController world;
    new public BoxCollider2D collider;
    new public SpriteRenderer renderer;
    public Animator animator;
    public AudioSource source;
    public FlickerySprite fs;
    public PitfallableSprite pitfallable;
    public Hurtbox dodgeHurtbox;
    public PlayerBulletOrigin bulletOrigin;
    public PlayerEnergy energy;
    public PlayerWeaponManager wpnManager;
    public Vector3 KnockbackHeading;
    public AudioClip fallSFX;
    public AudioClip hitSFX;
    public AudioClip regainSFX;
    public bool DontWarp;
    public bool Dead;
    public bool IgnoreCollision;
    public bool Invincible;
    public bool Locked;
    public bool isFlying;
    public bool isVanished = false;
    public int InvulnTime;
    public int KnockbackFrames;
    public int DodgeHurtboxBaseDamage;
    private bool DiscardDodgeInputs;
    public uint[] CurrentWorldCoords = { 0, 0 };
    public uint[] CurrentRoomCoords = { 0, 0 };
    private RoomController lastRoom;
    [SerializeField]
    private Sprite specialPoseGFX;
    public Direction facingDir;
    public Bounds whiffBox;
    public bool hasBeenHit = false;
    public GameObject shadow;
    public bool isNeutral = true;
    public SpriteMover mover;

	void Start ()
    {
        animator.Play(world.GameStateManager.levelLoadPlayerAnimHash, 0, world.GameStateManager.levelLoadPlayerAnimTime);
    }

    // Update is called once per frame
    void Update ()
    {
#if (DEVELOPMENT_BUILD || UNITY_EDITOR)
        #region DEBUGTRASH
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            if (Invincible == true)
            {
                Invincible = false;
                Debug.Log("No longer invincible");
            }
            else
            {
                Invincible = true;
                Debug.Log("Invincible");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            world.GameStateManager.heldPassiveItems ^= HeldPassiveItems.DashThingy;
            Debug.Log("Toggled dash whatsit WHICH IS UNIMPLEMENTED");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            world.GameStateManager.heldPassiveItems ^= HeldPassiveItems.DodgeBooster;
            Debug.Log("Toggled dodge booster");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            world.GameStateManager.heldPassiveItems ^= HeldPassiveItems.DodgeAttack;
            Debug.Log("Toggled dodge attack");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            world.GameStateManager.heldPassiveItems ^= HeldPassiveItems.SecretSensor;
            Debug.Log("Toggled secret sensor WHICH IS UNIMPLEMENTED");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            world.GameStateManager.heldPassiveItems ^= HeldPassiveItems.AutoscrollRider;
            Debug.Log("Toggled autoscroll floor rider whatsit WHICH IS EXTRA UNIMPLEMENTED like I don't even know how those rooms work yet");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            world.GameStateManager.heldPassiveItems ^= HeldPassiveItems.RedBull;
            Debug.Log("Toggled flight whatsit WHICH IS UNIMPLEMENTED");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            world.GameStateManager.heldPassiveItems ^= HeldPassiveItems.TabooRegenUpThingy;
            Debug.Log("Toggled taboo regen booster");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug.Log("reserved function");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.Log("reserved function");
        }
        #endregion
#endif
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
        }
        if (Locked == false)
        {
            facingDir = (Direction)animator.GetInteger(PlayerAnimatorHashes.paramFacingDir);
            isNeutral = DodgeAllowedStates.Contains(animator.GetCurrentAnimatorStateInfo(0).fullPathHash);
            HandleInputs();
            if (DontWarp == true)
            {
                DontWarp = false;
            }
        }
        else
        {
            animator.SetBool(PlayerAnimatorHashes.paramHeldFire1, false);
            animator.SetBool(PlayerAnimatorHashes.paramHeldFire2, false);
            animator.SetBool(PlayerAnimatorHashes.paramHeldRight, false);
            animator.SetBool(PlayerAnimatorHashes.paramHeldLeft, false);
            animator.SetBool(PlayerAnimatorHashes.paramHeldDown, false);
            animator.SetBool(PlayerAnimatorHashes.paramHeldUp, false);
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
            if (other.gameObject.GetComponent<CommonEnemyController>() != null)
            {
                Hit(other.gameObject.GetComponent<CommonEnemyController>());

            }
            else
            {
                //TO DO: eventually I'mma need to abstract this into a list of "bit" object types and matching paths through their hierarchy to get to CEC
                Component c = other.gameObject.GetComponent<EnemyBossManta_TailBit>();
                if (c != null)
                {
                    Hit((c as EnemyBossManta_TailBit).tailController.master.common);
                }
            } 

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
    public void Die ()
    {
        animator.SetBool(PlayerAnimatorHashes.paramDead, true);
        Dead = true;
        animator.SetTrigger(PlayerAnimatorHashes.triggerDie);
    }

    /// <summary>
    /// Ugly piece of shit that turns Unity inputs into state machine attributes.
    /// </summary>
    void HandleInputs()
    {
        if (isNeutral == true)
        {
            DiscardDodgeInputs = false;
            if (animator.GetBool(PlayerAnimatorHashes.triggerDodgeBurst) == true)
            {
                animator.SetBool(PlayerAnimatorHashes.triggerDodgeBurst, false);
            }
        }
        else
        {
            DiscardDodgeInputs = true;
        }
        // Taboos
        if (wpnManager.Taboo != TabooType.None && wpnManager.TabooReady == true && world.HardwareInterfaceManager.Fire1.Pressed == true && world.HardwareInterfaceManager.Fire2.Pressed == true && 
            animator.GetBool("HeldFire1") == false && animator.GetBool("HeldFire2") == false)
        {
            wpnManager.TabooCooldownTimer = PlayerWeaponManager.TabooCooldownTime;
            wpnManager.TabooReady = false;
            world.TabooOverlay.DisplayTabooGlyph(wpnManager.Taboo);
            animator.SetBool("CastTaboo", true);
            wpnManager.InvokeTaboo();
        }
        else
        {
            //HeldFire1 bool
            if (world.HardwareInterfaceManager.Fire1.Pressed == true && wpnManager.SlotAWpn != WeaponType.None)
            {
                animator.SetBool("HeldFire1", true);
            }
            else
            {
                animator.SetBool("HeldFire1", false);
            }
            //HeldFire2 bool
            if (world.HardwareInterfaceManager.Fire2.Pressed == true && wpnManager.SlotBWpn != WeaponType.None)
            {
                animator.SetBool("HeldFire2", true);
                animator.SetBool("FireSlotB", true);
            }
            else
            {
                animator.SetBool("HeldFire2", false);
            }
        }
        if (DiscardDodgeInputs == false)
        {
            if (world.HardwareInterfaceManager.Dodge.BtnDown == true)
            {
                // InputRight/InputLeft triggers
                if (world.HardwareInterfaceManager.Left.Pressed == true)
                {
                    animator.SetBool("DodgeLeft", true);
                }
                else if (world.HardwareInterfaceManager.Right.Pressed == true)
                {
                    animator.SetBool("DodgeRight", true);
                }
                // InputDown/InputUp triggers
                if (world.HardwareInterfaceManager.Down.Pressed == true)
                {
                    animator.SetBool("DodgeDown", true);
                }
                else if (world.HardwareInterfaceManager.Up.Pressed == true)
                {
                    animator.SetBool("DodgeUp", true);
                }
                else
                {
                    switch (facingDir)
                    {
                        case Direction.Down:
                            animator.SetBool("DodgeDown", true);
                            break;
                        case Direction.Up:
                            animator.SetBool("DodgeUp", true);
                            break;
                        case Direction.Left:
                            animator.SetBool("DodgeLeft", true);
                            break;
                        case Direction.Right:
                            animator.SetBool("DodgeRight", true);
                            break;
                    }
                }
            }
            else
            {
                animator.SetBool("DodgeDown", false);
                animator.SetBool("DodgeUp", false);
                animator.SetBool("DodgeLeft", false);
                animator.SetBool("DodgeRight", false);
            }
        }
        // HeldRight/HeldLeft bools
        if (world.HardwareInterfaceManager.Right.Pressed == true)
        {
            animator.SetBool("HeldRight", true);
            animator.SetBool("HeldLeft", false);
        }
        else if (world.HardwareInterfaceManager.Left.Pressed == true)
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
        if (world.HardwareInterfaceManager.Up.Pressed == true)
        {
            animator.SetBool("HeldUp", true);
            animator.SetBool("HeldDown", false);
        }
        else if (world.HardwareInterfaceManager.Down.Pressed == true)
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

    public void Hit(int dmg, Vector3 center, int knockback)
    {
        if (Invincible == false && Locked == false && animator.GetBool("Dead") == false && hasBeenHit == false)
        {
            if (animator.GetBool(PlayerAnimatorHashes.triggerDodgeBurst) == false && InvulnTime < 1)
            {
                animator.SetTrigger("Hit");
                energy.Damage(dmg);
                source.PlayOneShot(hitSFX);
                hasBeenHit = true;
                if (center.y > collider.bounds.center.y)
                {
                    KnockbackHeading = Vector2.down * knockback;
                }
                else if (center.y < collider.bounds.center.y)
                {
                    KnockbackHeading = Vector2.up * knockback;
                }
                else if (center.x < collider.bounds.center.x)
                {
                    KnockbackHeading = Vector2.left * knockback;
                }
                else
                {
                    KnockbackHeading = Vector2.right * knockback;
                }
                KnockbackFrames = knockback;
            }
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
            energy.Reset();
        }
        else if (Invincible == false && Locked == false && hasBeenHit == false)
        {
            if (animator.GetBool(PlayerAnimatorHashes.triggerDodgeBurst) == false)
            {
                if (InvulnTime < 1)
                {
                    bullet.HitTarget();
                    animator.SetTrigger("Hit");
                    energy.Damage(bullet.Damage);
                    KnockbackHeading = bullet.Heading;
                    KnockbackFrames = bullet.Weight;
                    source.PlayOneShot(hitSFX);
                    hasBeenHit = true;
                    InvulnTime = 270;
                }
            }
        }
    }

    /// <summary>
    /// We've been hit by something: in this case, a dude.
    /// Handles collision, files assault charges.
    /// </summary>
    void Hit(CommonEnemyController enemy)
    {
        if (Invincible == false && Locked == false && animator.GetBool("Dead") == false && hasBeenHit == false)
        {
            if (animator.GetBool(PlayerAnimatorHashes.triggerDodgeBurst) == false && enemy.CollideDmg > 0)
            {
                if (InvulnTime < 1)
                {
                    animator.SetTrigger("Hit");
                    energy.Damage(enemy.CollideDmg);
                    KnockbackHeading = enemy.Heading;
                    KnockbackFrames = enemy.Weight;
                    source.PlayOneShot(hitSFX);
                    hasBeenHit = true;
                    InvulnTime = 270;
                }
            }
            else
            {
                if ((world.GameStateManager.heldPassiveItems & HeldPassiveItems.DodgeAttack) == HeldPassiveItems.DodgeAttack)
                {
                    dodgeHurtbox.Damage = energy.Level * DodgeHurtboxBaseDamage;
                    dodgeHurtbox.Hurt(enemy);
                }
            }
        }
    }

    /// <summary>
    /// We've been hit by something: in this case, an explosion.
    /// </summary>
    void Hit(BoomEffect boom)
    {
        if (Invincible == false && Locked == false && boom.owner != gameObject && animator.GetBool("Dead") == false && hasBeenHit == false)
        {
            if (animator.GetBool(PlayerAnimatorHashes.triggerDodgeBurst) == false && boom.Collideable == true && InvulnTime < 1)
            {
                animator.SetTrigger("Hit");
                energy.Damage(boom.Damage);
                source.PlayOneShot(hitSFX);
                hasBeenHit = true;
                if (boom.collider.bounds.center.y > collider.bounds.center.y)
                {
                    KnockbackHeading = Vector2.down * boom.PushbackStrength;
                }
                else if (boom.collider.bounds.center.y < collider.bounds.center.y)
                {
                    KnockbackHeading = Vector2.up * boom.PushbackStrength;
                }
                else if (boom.collider.bounds.center.x < collider.bounds.center.x)
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

    public void MoveOrBeCrushed (Vector3 heading, GameObject crusher)
    {
        Bounds[] collision = world.activeRoom.collision.getAllCollisionButSpecificObject(crusher);
        ExpensiveAccurateCollision.CollideWithScenery(mover, collision, heading, collider);
        for (int i = 0; i < world.activeRoom.collision.allCollision.Length; i++)
        {
            if (collider.bounds.Intersects(world.activeRoom.collision.allCollision[i]) == true)
            {
                Die();
                break;
            }
        } 
    }
    
    public void Pitfall ()
    {
        source.PlayOneShot(fallSFX);
        mover.heading = Vector3.zero;
        mover.virtualPosition = transform.position;
        animator.Play(PlayerAnimatorHashes.Pitfall, 0);
    }

    /// <summary>
    /// Respawns player.
    /// </summary>
    public void Respawn ()
    {
        Dead = false;
        world.GameStateManager.RespawnPlayer();
        animator.ResetTrigger("Hit");
        hasBeenHit = false;
    }

    /// <summary>
    /// Damages player and puts them on safe ground after pitfalling.
    /// </summary>
    public void RespawnAfterPitfall()
    {
        transform.position = pitfallable.respawnPosition;
        mover.heading = Vector3.zero;
        mover.virtualPosition = transform.position;
        animator.SetTrigger("Hit");
        energy.Damage(20 * energy.Level, true);
        source.PlayOneShot(hitSFX);
        hasBeenHit = true;
    }

    /// <summary>
    /// Toggles the DodgeBurst property on the animator state machine.
    /// </summary>
    public void SetAnimatorDodgeBurst ()
    {
        animator.SetBool(PlayerAnimatorHashes.triggerDodgeBurst, true);
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

    public void StartHitFlash (int invulnTime = 30)
    {
        StartCoroutine(GFXHelpers.FlashEffect(renderer, 15));
        InvulnTime = invulnTime;
    }
}
