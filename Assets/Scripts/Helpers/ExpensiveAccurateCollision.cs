using UnityEngine;
using System;
using System.Collections;

public static class ExpensiveAccurateCollision
{
    /// <summary>
    /// Collides a sprite with scenery colliders.
    /// Less horrible after some refactoring; still kinda gnarly.
    /// TO DO: eliminate use of "real" collider entirely - use mover virtual position
    /// </summary>
    public static bool CollideWithScenery(SpriteMover mover, Bounds[] roomColliders, Vector3 PosMod, Collider2D baseCollider, bool ignoreCollision = false)
    {
        bool ret = false;
        if (ignoreCollision == true)
        {
            mover.heading += PosMod;
        }
        else
        {
            Bounds projectionOfRealCurrentCollider = new Bounds(new Vector3(mover.virtualPosition.x + (baseCollider.offset.x) + mover.heading.x, mover.virtualPosition.y + (baseCollider.offset.y) + mover.heading.y, mover.virtualPosition.z), baseCollider.bounds.size);
            Vector3 KnownGood = mover.virtualPosition;
            Vector3 prospectivePos = mover.virtualPosition;
            Bounds newCollider = new Bounds();
            bool Collided = false;
            float ax = PosMod.x / Mathf.Floor(Math.Abs(PosMod.x));
            float ay = PosMod.y / Mathf.Floor(Math.Abs(PosMod.y));
            _in_CollideWithScenery_phase1(ref Collided, ref newCollider, ref roomColliders, ref KnownGood, ref prospectivePos, PosMod.y, ay, 0, 1, projectionOfRealCurrentCollider, mover);
            ret = Collided;
            Collided = false;
            prospectivePos = KnownGood;
            _in_CollideWithScenery_phase1(ref Collided, ref newCollider, ref roomColliders, ref KnownGood, ref prospectivePos, PosMod.x, ax, 1, 0, projectionOfRealCurrentCollider, mover);
            if (ret == false)
            {
                ret = Collided;
            }
            Collided = false;
            mover.heading += (KnownGood - mover.virtualPosition);
            Vector3 scrapHeading = new Vector3(PosMod.x - (int)PosMod.x, PosMod.y - (int)PosMod.y, 0);
            ax = 0;
            if (scrapHeading.x != 0)
            {
                ax = scrapHeading.x / Mathf.Abs(scrapHeading.x);
            }
            ay = 0;
            if (scrapHeading.y != 0)
            {
                ay = scrapHeading.y / Mathf.Abs(scrapHeading.y);
            }
            Vector3 testHeading = new Vector3(ax, ay, 0);
            _in_CollideWithScenery_phase2(ref Collided, ref newCollider, ref roomColliders, ref KnownGood, ref scrapHeading, ref testHeading, 0, scrapHeading.y, testHeading.x, 0, projectionOfRealCurrentCollider, mover);
            _in_CollideWithScenery_phase2(ref Collided, ref newCollider, ref roomColliders, ref KnownGood, ref scrapHeading, ref testHeading, scrapHeading.x, 0, 0, scrapHeading.y, projectionOfRealCurrentCollider, mover);
            mover.heading += scrapHeading;
            
        }
        return ret;
    }

    private static void _in_CollideWithScenery_phase1 (ref bool Collided, ref Bounds newCollider, ref Bounds[] roomColliders, ref Vector3 KnownGood, ref Vector3 prospectivePos, float dist, float a, float xmulti, float ymulti, Bounds Collider, SpriteMover mover)
    {
        Vector3 v = KnownGood;
        for (int i = 1; i < Mathf.Floor(Math.Abs(dist)) + 1; i++)
        {
            prospectivePos = new Vector3(KnownGood.x + (i * a * xmulti), KnownGood.y + (i * a * ymulti), mover.transform.position.z);
            newCollider = new Bounds(new Vector3(Collider.center.x + (i * a * xmulti), Collider.center.y + (i * a * ymulti), Collider.center.z), Collider.size);
            for (int i2 = 0; i2 < roomColliders.Length; i2++)
            {
                if (roomColliders[i2] != default(Bounds) && newCollider.Intersects(roomColliders[i2]))
                {
                    Collided = true;
                }
            }
            if (Collided == false)
            {
                v = prospectivePos;
            }
        }
        KnownGood = v;
    }

    private static void _in_CollideWithScenery_phase2 (ref bool Collided, ref Bounds newCollider, ref Bounds[] roomColliders, ref Vector3 KnownGood, ref Vector3 scrapHeading, ref Vector3 testHeading, float ax, float ay, float vx, float vy, Bounds Collider, SpriteMover mover)
    {
        newCollider = new Bounds(new Vector3(Collider.center.x + (KnownGood.x - mover.virtualPosition.x) + vx, Collider.center.y + (KnownGood.y - mover.virtualPosition.y) + vy, Collider.center.z), Collider.size);
        for (int i = 0; i < roomColliders.Length; i++)
        {
            if (roomColliders[i] != default(Bounds) && newCollider.Intersects(roomColliders[i]))
            {
                Collided = true;
            }
        }
        if (Collided == true)
        {
            scrapHeading = new Vector3(ax, ay, 0);
        }
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