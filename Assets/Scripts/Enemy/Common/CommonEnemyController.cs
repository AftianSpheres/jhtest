using UnityEngine;
using System.Collections;

/// <summary>
/// MonoBehavior providing shared state and functionality for all enemy types.
/// </summary>
[RequireComponent (typeof(RegisteredSprite))]
[RequireComponent (typeof(PauseableSprite))]
public class CommonEnemyController : MonoBehaviour
{
    public RoomController room;
    public EnemyModule module;
    public RegisteredSprite register;
    public Animator animator;
    public AudioClip HitSFX;
    public AudioSource source;
    new public SpriteRenderer renderer;
    public int Weight;
    public Vector3 Heading;
    new public BoxCollider2D collider;
    public int CurrentHP;
    public int MaxHP;
    public int CollideDmg;
    public int ShotDmg;
    public int InvulnTime;
    public int DamageQueue;
    private Vector3 StartingPos;
    public Vector3 StartingCenter;
    private Sprite StartingFrame;
    private int DefaultStateHash;
    public bool isDead = false;
    public bool Vulnerable;
    public bool Collideable;
    public FlickerySprite flicker;



	// Use this for initialization
	void Awake ()
    {
        StartingPos = transform.position;
        StartingCenter = collider.bounds.center;
        StartingFrame = renderer.sprite;
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
            }
            else
            {
                animator.enabled = false;
            }
        }     
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet") == true && Vulnerable == true)
        {
            Hit(other.gameObject.GetComponent<BulletController>());
        }
        else if (other.CompareTag("Boom") == true && Vulnerable == true)
        {
            Hit(other.gameObject.GetComponent<BoomEffect>());
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
    public void Hit(BulletController bullet)
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
                StartCoroutine(GFXHelpers.FlashEffect(renderer, 10));
            }
            bullet.HitTarget();
            DamageQueue += bullet.Damage;
            source.PlayOneShot(HitSFX);   
        }

    }

    /// <summary>
    /// We've been hit by something: in this case, an explosion.
    /// </summary>
    void Hit(BoomEffect boom)
    {
        if (animator.GetInteger("InvulnTime") >= 0 && boom.owner != gameObject)
        {
            if (boom.PushbackStrength >= Weight)
            {
                animator.SetTrigger("HitHeavy");
                TriggerInvuln();
            }
            else
            {
                StartCoroutine(GFXHelpers.FlashEffect(renderer, 10));
            }
            animator.SetTrigger("Hit");
            DamageQueue += boom.Damage;
            source.PlayOneShot(HitSFX);
        }
    }

    /// <summary>
    /// ...also kills an enemy?
    /// This should, uh, probably be refactored to something sane.
    /// This is the final "finish the animation, we dead dead" part, though.
    /// So, uh: don't use this outside of the attached animator.
    /// (exception to that rule: Generic Fleshy Things that just sit and die when they're killed)
    /// </summary>
    public void Kill ()
    {
        animator.Play(DefaultStateHash);
        room.world.EnemyBullets.FireBullet(WeaponType.spEnergyRecover, 5, 0, 0, room.world.player.collider.bounds.center, collider.bounds.center, true, room.world.player.collider, 0);
        CurrentHP = MaxHP;
        transform.position = StartingPos;
        DamageQueue = 0;
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
        animator.Rebind();
        renderer.sprite = StartingFrame;
        isDead = false;
        flicker.skip = false;
        if (room.world.activeRoom != room)
        {
            register.Toggled = false;
        }
        module.Respawn();
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
