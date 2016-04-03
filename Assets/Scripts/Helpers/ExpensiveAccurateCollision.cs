using UnityEngine;
using System;
using System.Collections;

public static class ExpensiveAccurateCollision
{
    public static void CollideWithScenery(Animator animator, Collider[] roomColliders, Vector3 PosMod, Collider2D Collider)
    {
        if (animator.gameObject.GetComponent<PlayerController>() != null && animator.gameObject.GetComponent<PlayerController>().IgnoreCollision == true)
        {
            animator.transform.position += PosMod;
            return;
        }
        Vector3 KnownGood = animator.transform.position;
        bool Collided = false;
        float ax = PosMod.x / Math.Abs(PosMod.x);
        float ay = PosMod.y / Math.Abs(PosMod.y);
        for (int i = 1; i < Math.Abs(PosMod.y) + 1; i++)
        {
            animator.transform.position = new Vector3(animator.transform.position.x, animator.transform.position.y + (i * ay), animator.transform.position.z);
            for (int i2 = 0; i2 < roomColliders.Length; i2++)
            {
                if (roomColliders[i2] != null && Collider.bounds.Intersects(roomColliders[i2].bounds))
                {
                    Collided = true;
                }
            }
            if (Collided == false)
            {
                KnownGood = animator.transform.position;
            }
        }
        Collided = false;
        animator.transform.position = KnownGood;
        for (int i = 1; i < Math.Abs(PosMod.x) + 1; i++)
        {
            animator.transform.position = new Vector3(animator.transform.position.x + (i * ax), animator.transform.position.y, animator.transform.position.z);
            for (int i2 = 0; i2 < roomColliders.Length; i2++)
            {
                if (roomColliders[i2] != null && Collider.bounds.Intersects(roomColliders[i2].bounds))
                {
                    Collided = true;
                }
            }
            if (Collided == false)
            {
                KnownGood = animator.transform.position;
            }
        }
        animator.transform.position = KnownGood;
    }
}