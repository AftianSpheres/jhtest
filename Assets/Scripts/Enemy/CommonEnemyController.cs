using UnityEngine;
using System.Collections;

[RequireComponent (typeof(RegisteredSprite))]
public class CommonEnemyController : MonoBehaviour
{
    public WorldController world;
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



	// Use this for initialization
	void Awake ()
    {
        world = GameObject.Find("Universe/World").GetComponent<WorldController>();
	}
	
	// Update is called once per frame
	void Update ()
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet") == true)
        {
            Hit(other.gameObject.GetComponent<BulletController>());
        }
    }

    void Die()
    {
        animator.SetBool("Dead", true);
    }

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

    void TriggerHitstun(BulletController bullet)
    {

    }

    void TriggerInvuln()
    {
        animator.SetInteger("InvulnTime", InvulnTime);
    }
}
