﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum PlayerWeapon
{
    WeenieGun,
    Shotgun,

    GenericEnemyShot = 100000,
    MidBoss_Arrow_Vert,
    MidBoss_Arrow_Horiz
}

public class BulletController : MonoBehaviour
{
    public WorldController world;
    public PlayerWeapon ShotType;
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
    private Collider[] roomColliders;
    public Transform HomingTarget;

    void Start ()
    {
        LogicalPosition = transform.position;
    }

    /// <summary>
    /// Called when we recover a bullet to use with a weapon.
    /// </summary>
    public void Fire (PlayerWeapon shot, float speed, int damage, int weight, Vector3 source, Vector3 to, BulletPool pool, bool pierce)
    {
        roomColliders = world.activeRoom.Colliders;
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
            case PlayerWeapon.MidBoss_Arrow_Vert:
                renderer.sprite = pool.frames[1];
                break;
            case PlayerWeapon.MidBoss_Arrow_Horiz:
                renderer.sprite = pool.frames[2];
                break;
        }
    }
	
    void Retire ()
    {
        q.Enqueue(this);
        gameObject.SetActive(false);
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
                for (int i = 0; i < roomColliders.Length; i++)
                {
                    if (roomColliders[i] != null)
                    {
                        if (collider.bounds.Intersects(roomColliders[i].bounds))
                        {
                            mu_RoomEvent rb = roomColliders[i].gameObject.GetComponent<mu_RoomEvent>();
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
