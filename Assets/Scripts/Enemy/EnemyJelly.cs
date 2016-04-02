using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CommonEnemyController))]
public class EnemyJelly: MonoBehaviour
{
    public CommonEnemyController common;

	// Use this for initialization
	void Start ()
    {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (common.register.Toggled == true)
        {
            bool FireThisFrame = true;
            for (int i = 0; i < common.register.room.Colliders.Length; i++)
            {
                if (common.register.room.Colliders[i].bounds != null)
                {
                    if (common.collider.bounds.Intersects(common.register.room.Colliders[i].bounds))
                    {
                        FireThisFrame = false;
                    }
                }
            }
            if (Random.Range(0, 90) != 0)
            {
                FireThisFrame = false;
            }
            if (FireThisFrame == true)
            {
                common.animator.SetTrigger("Fire");
            }
        }
    }
}
