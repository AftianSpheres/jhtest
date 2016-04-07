using UnityEngine;
using System.Collections;

/// <summary>
/// MonoBehavior providing shared state and functionality for all enemy types.
/// </summary>
[RequireComponent (typeof(RegisteredSprite))]
public class CommonEnemyController : MonoBehaviour
{
    public RoomController room;
    public MonoBehaviour module;
    public RegisteredSprite register;
    public Animator animator;
    public AudioClip HitSFX;
    public AudioSource source;
    new public SpriteRenderer renderer;
    public Material defaultMat;
    public Material flashMat;
    public int Weight;
    public Vector3 Heading;
    new public BoxCollider2D collider;
    public int CurrentHP;
    public int MaxHP;
    public int CollideDmg;
    public int ShotDmg;
    public int InvulnTime;
    public int DamageQueue;
    private int HitFlashCounter;
    private Vector3 StartingPos;
    private int DefaultStateHash;
    public bool isDead = false;
    public FlickerySprite flicker;



	// Use this for initialization
	void Awake ()
    {
        StartingPos = transform.position;
        DefaultStateHash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isDead == false)
        {
            if (register.Toggled == true)
            {
                if (animator.enabled == false)
                {
                    animator.enabled = true;
                }
                CurrentHP -= DamageQueue;
                DamageQueue = 0;
                if (CurrentHP <= 0)
                {
                    Die();
                }
                if (HitFlashCounter < 1)
                {
                    renderer.material = defaultMat;
                }
                HitFlashCounter -= 1;
            }
            else
            {
                animator.enabled = false;
            }
        }     
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet") == true)
        {
            Hit(other.gameObject.GetComponent<BulletController>());
        }
    }

    /// <summary>
    /// Kills an enemy.
    /// </summary>
    void Die()
    {
        animator.SetBool("Dead", true);
    }

    /// <summary>
    /// Hits an enemy with the bullet passed as argument.
    /// </summary>
    void Hit(BulletController bullet)
    {
        if (animator.GetInteger("InvulnTime") >= 0)
        {
            if (bullet.Weight >= Weight)
            {
                animator.SetTrigger("HitHeavy");
                TriggerInvuln();
            }
            else
            {
                renderer.material = flashMat;
                HitFlashCounter = 10;
            }
            DamageQueue += bullet.Damage;
            source.PlayOneShot(HitSFX);   
        }

    }

    /// <summary>
    /// ...also kills an enemy?
    /// This should, uh, probably be refactored to something sane.
    /// This is the final "finish the animation, we dead dead" part, though.
    /// So, uh: don't use this outside of the attached animator.
    /// </summary>
    public void Kill ()
    {
        animator.Play(DefaultStateHash);
        room.world.player.GetComponent<PlayerEnergy>().Recover(100);
        CurrentHP = MaxHP;
        transform.position = StartingPos;
        DamageQueue = 0;
        renderer.material = defaultMat;
        renderer.enabled = false;
        collider.enabled = false;
        isDead = true;
        flicker.skip = true;
    }

    /// <summary>
    /// Respawns an enemy.
    /// </summary>
    public void Respawn ()
    {
        renderer.enabled = true;
        collider.enabled = true;
        animator.SetBool("Dead", false);
        isDead = false;
        flicker.skip = false;
        if (room.world.activeRoom != room)
        {
            register.Toggled = false;
        }
    }

    /// <summary>
    /// Should trigger hitstun. Does not trigger hitstun.
    /// </summary>
    void TriggerHitstun(BulletController bullet)
    {

    }

    /// <summary>
    /// Triggers temporary i-frames.
    /// </summary>
    void TriggerInvuln()
    {
        animator.SetInteger("InvulnTime", InvulnTime);
    }
}
