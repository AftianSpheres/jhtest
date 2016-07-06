using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CommonEnemyController))]
public class EnemyJelly: EnemyModule
{
    int firingTimer = 0;

	// Use this for initialization
	void Start ()
    {
        firingTimer = Random.Range(120, 200);
	}

    // Update is called once per frame
    void Update()
    {
        if (common.isDead == false && common.register.Toggled == true)
        {
            firingTimer--;
            bool FireThisFrame = true;
            for (int i = 0; i < common.register.room.collision.allCollision.Length; i++)
            {
                if (common.register.room.collision.allCollision[i] != default(Bounds))
                {
                    if (common.collider.bounds.Intersects(common.register.room.collision.allCollision[i]))
                    {
                        FireThisFrame = false;
                    }
                }
            }
            if (Random.Range(0, firingTimer) > 0)
            {
                FireThisFrame = false;
            }
            if (FireThisFrame == true)
            {
                common.animator.SetTrigger("Fire");
                firingTimer = Random.Range(120, 200);
            }
        }
    }

    new public void Respawn()
    {
        common.animator.ResetTrigger("Fire");
        common.animator.SetInteger("FloatingFrame", 0);
    }
}
