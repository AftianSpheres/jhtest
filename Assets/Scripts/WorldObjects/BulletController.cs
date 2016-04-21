using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum WeaponType
{
    None = -1,
    pWG,
    pWGII,
    pShotgun,
    pShadow,
    pFlamethrower,
    pIcicle,
    pw7,
    pw8,
    pw9,
    pw10,
    pw11,
    pw12,
    pw13,
    pw14,
    pw15,
    pw16,
    pw17,
    pw18,
    pw19,
    pw20,
    pw21,
    pw22,
    pw23,
    pw24,

    eGeneric = 100000,
    eMidBoss_Arrow_Vert,
    eMidBoss_Arrow_Horiz
}

public class BulletController : MonoBehaviour
{
    public WorldController world;
    new public BoxCollider2D collider;
    new public SpriteRenderer renderer;
    private Bounds[] roomColliders;
    private Queue<BulletController> q;
    public Transform HomingTarget;
    public WeaponType ShotType;
    public Vector3 TargetPosition;
    private Vector3 LogicalPosition;
    private Vector3 snapToThing;
    public Vector2 Heading;
    public Vector2 Trail;
    public float Range;
    public float Speed;
    public int Damage;
    public int Weight;
    public int Priority;
    public bool Pierce;
    public bool Homing;

    void Start ()
    {
        LogicalPosition = transform.position;
    }

    /// <summary>
    /// Called when we recover a bullet to use with a weapon.
    /// </summary>
    public void Fire (WeaponType shot, float speed, int damage, int weight, Vector3 source, Vector3 to, BulletPool pool, bool pierce)
    {
        Range = -1;
        Trail = Vector2.zero;
        roomColliders = world.activeRoom.collision.fullCollide;
        Damage = damage;
        Pierce = pierce;
        Weight = weight;
        q = pool.q;
        Speed = speed;
        ShotType = shot;
        TargetPosition = to;
        transform.position = source;
        LogicalPosition = transform.position;
        float rise = (TargetPosition.y - transform.position.y);
        float run = TargetPosition.x - transform.position.x;
        float normalizationFactor = 1 / (Math.Abs(rise) + Math.Abs(run));
        Heading = new Vector2(normalizationFactor * run * Speed, normalizationFactor * rise * Speed);
        for (int i = 0; i < world.activeRoom.priorityMap.priorities.Length; i++)
        {
            if (world.activeRoom.priorityMap.zones[i].Contains(collider.bounds.center) == true)
            {
                Priority = world.activeRoom.priorityMap.priorities[i];
                break;
            }
        }
        switch (ShotType)
        {
            case WeaponType.pWG:
            case WeaponType.pWGII:
            case WeaponType.pShotgun:
                renderer.sprite = pool.frames[0];
                break;

            case WeaponType.pShadow:
                renderer.sprite = pool.frames[1];
                Range = 160;
                snapToThing = world.player.transform.position;
                break;

            case WeaponType.eMidBoss_Arrow_Vert:
                renderer.sprite = pool.frames[1];
                break;
            case WeaponType.eMidBoss_Arrow_Horiz:
                renderer.sprite = pool.frames[2];
                break;
        }
    }
	
    void Retire ()
    {
        q.Enqueue(this);
        gameObject.SetActive(false);

        switch (ShotType)
        {
            case WeaponType.pShadow:
                world.player.transform.position = snapToThing;
                float nx = collider.bounds.center.x;
                float ny = collider.bounds.center.y;
                if (nx < world.activeRoom.bounds.min.x)
                {
                    nx = world.activeRoom.bounds.min.x;
                }
                else if (nx > world.activeRoom.bounds.max.x)
                {
                    nx = world.activeRoom.bounds.max.x;
                }
                if (ny < world.activeRoom.bounds.min.y)
                {
                    ny = world.activeRoom.bounds.min.y;
                }
                else if (ny > world.activeRoom.bounds.max.y)
                {
                    ny = world.activeRoom.bounds.max.y;
                }
                world.player.Locked = false;
                world.player.renderer.enabled = true;
                world.player.collider.enabled = true;
                world.player.animator.enabled = true;
                world.player.fs.skip = false;
                world.player.bulletOrigin.gameObject.SetActive(true);
                Vector3 np = ExpensiveAccurateCollision.ShoveOutOfScenery(world.player.collider, world.activeRoom.collision.allCollision, new Vector3(nx, ny, 0));
                world.player.transform.position = new Vector3(np.x, np.y, 0) - new Vector3(world.player.collider.offset.x, world.player.collider.offset.y, 0);
                break;
        }
    }

    public void HitEnemy () { }
    
    public void HitLevel () { }

    public void WpnFiringAdjust () { }

	public void Update ()
    {
        if (world.activeRoom == null)
        {
            Retire();
        }
        else
        {
            switch (ShotType)
            {
                case WeaponType.pShadow:
                    world.player.transform.position = transform.position;
                    break;
            }
            if (Pierce == false)
            {
                bool chkCollision = true;
                for (int i = 0; i < world.activeRoom.priorityMap.priorities.Length; i++)
                {
                    if (world.activeRoom.priorityMap.priorities[i] > Priority && collider.bounds.Intersects(world.activeRoom.priorityMap.zones[i]) == true)
                    {
                        Retire();
                        break;
                    }
                }
                for (int i = 0; i < world.activeRoom.priorityMap.priorities.Length; i++)
                {
                    if (world.activeRoom.priorityMap.priorities[i] < Priority && collider.bounds.Intersects(world.activeRoom.priorityMap.zones[i]) == true)
                    {
                        chkCollision = false;
                        break;
                    }
                }
                if (chkCollision == true)
                {
                    for (int i = 0; i < world.activeRoom.collision.fullCollide.Length; i++)
                    {
                        if (world.activeRoom.collision.fullCollide[i] != null)
                        {
                            if (collider.bounds.Intersects(world.activeRoom.collision.fullCollide[i]))
                            {
                                if (world.activeRoom.collision.GetAssocGameObject(i, rcGameObjectSearchMode.fullCollide) != null)
                                {
                                    mu_RoomEvent rb = world.activeRoom.collision.GetAssocGameObject(i, rcGameObjectSearchMode.fullCollide).GetComponent<mu_RoomEvent>();
                                    if (rb != null)
                                    {
                                        rb.BulletStrike(this);
                                    }
                                }
                                Retire();
                                break;
                            }
                        }
                    }
                }
            }
            if (world.activeRoom.bounds.Contains(collider.bounds.center) == false)
            {
                Retire();
            }
            WpnFiringAdjust();
            LogicalPosition = new Vector3(LogicalPosition.x + Heading.x, LogicalPosition.y + Heading.y, LogicalPosition.z);
            Trail += Heading;
            transform.position = new Vector3((float)Math.Round(LogicalPosition.x, 0, MidpointRounding.AwayFromZero), (float)Math.Round(LogicalPosition.y, 0, MidpointRounding.AwayFromZero), LogicalPosition.z);
            if (Range > 0 && Mathf.Abs(Trail.x) + Mathf.Abs(Trail.y) > Range)
            {
                Retire();
            }
        }
    }
}
