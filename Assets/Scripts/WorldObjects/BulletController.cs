using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum WeaponType
{
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
    public WeaponType ShotType;
    public Vector2 Heading;
    public Vector3 LogicalPosition;
    public float Speed;
    public int Damage;
    public int Weight;
    public bool Pierce;
    public bool Homing;
    public Vector3 TargetPosition;
    new public BoxCollider2D collider;
    new public SpriteRenderer renderer;
    private Queue<BulletController> q;
    private Bounds[] roomColliders;
    public Transform HomingTarget;

    void Start ()
    {
        LogicalPosition = transform.position;
    }

    /// <summary>
    /// Called when we recover a bullet to use with a weapon.
    /// </summary>
    public void Fire (WeaponType shot, float speed, int damage, int weight, Vector3 source, Vector3 to, BulletPool pool, bool pierce)
    {
        roomColliders = world.activeRoom.collision.allFull;
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
        switch (ShotType)
        {
            case WeaponType.pWG:
            case WeaponType.pWGII:
            case WeaponType.pShotgun:
                renderer.sprite = pool.frames[0];
                break;

            case WeaponType.pShadow:
                renderer.sprite = pool.frames[1];
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
            if (Pierce == false)
            {
                for (int i = 0; i < world.activeRoom.collision.allCollision.Length; i++)
                {
                    if (world.activeRoom.collision.allCollision[i] != null)
                    {
                        if (collider.bounds.Intersects(world.activeRoom.collision.allCollision[i]))
                        {
                            mu_RoomEvent rb = world.activeRoom.collision.GetAssocGameObject(i, rcGameObjectSearchMode.all).GetComponent<mu_RoomEvent>();
                            if (rb != null)
                            {
                                rb.BulletStrike(this);
                            }
                            Retire();
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
            transform.position = new Vector3((float)Math.Round(LogicalPosition.x, 0, MidpointRounding.AwayFromZero), (float)Math.Round(LogicalPosition.y, 0, MidpointRounding.AwayFromZero), LogicalPosition.z);
        }
    }
}
