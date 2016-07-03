using UnityEngine;
using System;
using System.Collections;

public static class ExpensiveAccurateCollision
{
    public static bool CollideWithScenery(Animator animator, Bounds[] roomColliders, Vector3 PosMod, Collider2D Collider)
    {
        if (animator.gameObject.GetComponent<PlayerController>() != null && animator.gameObject.GetComponent<PlayerController>().IgnoreCollision == true)
        {
            animator.transform.position += PosMod;
            return false;
        }
        Vector3 KnownGood = animator.transform.position;
        bool Collided = false;
        bool ret = false;
        float ax = PosMod.x / Math.Abs(PosMod.x);
        float ay = PosMod.y / Math.Abs(PosMod.y);
        for (int i = 1; i < Math.Abs(PosMod.y) + 1; i++)
        {
            animator.transform.position = new Vector3(animator.transform.position.x, animator.transform.position.y + (i * ay), animator.transform.position.z);
            for (int i2 = 0; i2 < roomColliders.Length; i2++)
            {
                if (roomColliders[i2] != default(Bounds) && Collider.bounds.Intersects(roomColliders[i2]))
                {
                    Collided = true;
                }
            }
            if (Collided == false)
            {
                KnownGood = animator.transform.position;
            }
        }
        ret = Collided;
        Collided = false;
        animator.transform.position = KnownGood;
        for (int i = 1; i < Math.Abs(PosMod.x) + 1; i++)
        {
            animator.transform.position = new Vector3(animator.transform.position.x + (i * ax), animator.transform.position.y, animator.transform.position.z);
            for (int i2 = 0; i2 < roomColliders.Length; i2++)
            {
                if (roomColliders[i2] != default(Bounds) && Collider.bounds.Intersects(roomColliders[i2]))
                {
                    Collided = true;
                }
            }
            if (Collided == false)
            {
                KnownGood = animator.transform.position;
            }
        }
        if (ret == false)
        {
            ret = Collided;
        }
        animator.transform.position = KnownGood;
        return ret;
    }



    public static Vector3 ShoveOutOfScenery (Collider2D collider, Bounds[] roomColliders, Vector3 newPos)
    {
        Bounds b;
        int timeOutCounter = 0;
        bool seeking;
        while (true)
        {
            timeOutCounter++;
            if (timeOutCounter > 1000) // only way this happens is if we're in a seriously anomalous state - this function should always find a valid position before the 1000th run
            {
                newPos = collider.transform.position; // we're inside collision or something - nothing to do but leave us where we were when this was called
                break;
            }
            seeking = false;
            b = new Bounds(newPos, collider.bounds.size);
            for (int i = 0; i < roomColliders.Length; i++)
            {
                if (roomColliders[i] != default(Bounds))
                {
                    if (b.Intersects(roomColliders[i]))
                    {
                        seeking = true;
                    }
                }
            }
            if (newPos.x > collider.bounds.center.x)
            {
                newPos = new Vector3(newPos.x - 1, newPos.y, newPos.z);
            }
            else if (newPos.x < collider.bounds.center.x)
            {
                newPos = new Vector3(newPos.x + 1, newPos.y, newPos.z);
            }
            if (newPos.y > collider.bounds.center.y)
            {
                newPos = new Vector3(newPos.x, newPos.y - 1, newPos.z);
            }
            else if (newPos.y < collider.bounds.center.y)
            {
                newPos = new Vector3(newPos.x, newPos.y + 1, newPos.z);
            }
            if (seeking == false)
            {
                break;
            }
        }
        return newPos;
    }
}