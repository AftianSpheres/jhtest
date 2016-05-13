using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// A horrible, messy thing that controls the logic for every single "bullet" in the game.
/// Because I want to keep bullets in a single more or less static object pool that never has to fuck around with adding or removing components...
/// this has to handle the logic for every single shot type as necessary. Eww!
/// </summary>
public class BulletController : MonoBehaviour
{
    public WorldController world;
    private Queue<BulletController> q;
    new public BoxCollider2D collider;
    new public SpriteRenderer renderer;
    public BoomPool boomPool;
    public BulletPool bulletPool;
    public BoxCollider2D HomingTarget;
    public FlickerySprite fs;
    private Bounds[] roomColliders;
    private Vector3 LogicalPosition = Vector3.zero;
    private Vector3 snapToThing;
    public Vector3 TargetPosition;
    public Vector2 Heading;
    public Vector2 Trail;
    public WeaponType ShotType;
    public string OriginalTag;
    public bool PierceScenery;
    public bool PierceTargets;
    public bool isExplosive;
    public float Range;
    public float Speed;
    public int Damage;
    public int HomingPrecision;
    public int HomingWindow;
    public int Priority;
    public int Weight;
    public GameObject owner;

    /// <summary>
    /// UnityEngine.Update()
    /// </summary>
    public void Update()
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
            if (world.activeRoom.bounds.Contains(collider.bounds.center) == false)
            {
                Retire();
            }
            else if (Range > 0 && Mathf.Abs(Trail.x) + Mathf.Abs(Trail.y) > Range)
            {
                Retire();
            }
            else if (PierceScenery == false)
            {
                CollisionCheck();
            }
            if (HomingTarget != null && (HomingTarget.enabled == false ||
                Mathf.Abs(HomingTarget.bounds.center.x - collider.bounds.center.x) + Mathf.Abs(HomingTarget.bounds.center.y - collider.bounds.center.y) < HomingPrecision ||
                Mathf.Abs(HomingTarget.bounds.center.x - collider.bounds.center.x) + Mathf.Abs(HomingTarget.bounds.center.y - collider.bounds.center.y) > HomingWindow))
            {
                HomingTarget = default(BoxCollider2D); // target is no longer viable, we're too close, or we're too far - in any case: release it
            }
            FiringAdjust();
            Trail += Heading;
            LogicalPosition = new Vector3(LogicalPosition.x + Heading.x, LogicalPosition.y + Heading.y, LogicalPosition.z);
            if (float.IsNaN((float)Math.Round(LogicalPosition.x, 0, MidpointRounding.AwayFromZero)) == true || float.IsNaN((float)Math.Round(LogicalPosition.y, 0, MidpointRounding.AwayFromZero)) == true)
            {
                Retire();
            }
            else
            {
                transform.position = new Vector3((float)Math.Round(LogicalPosition.x, 0, MidpointRounding.AwayFromZero), (float)Math.Round(LogicalPosition.y, 0, MidpointRounding.AwayFromZero), LogicalPosition.z);
            }
        }
    }

    /// <summary>
    /// Called when we recover a bullet to use with a weapon.
    /// </summary>
    public void Fire (WeaponType shot, float speed, int damage, int weight, Vector3 source, Vector3 to, BulletPool pool, bool pierceScenery, BoxCollider2D homingTarget = default(BoxCollider2D), int homingPrecision = 0, int homingWindow = int.MaxValue)
    {
        tag = OriginalTag;
        Range = -1;
        Trail = Vector2.zero;
        roomColliders = world.activeRoom.collision.fullCollide;
        Damage = damage;
        PierceScenery = pierceScenery;
        Weight = weight;
        q = pool.q;
        bulletPool = pool;
        Speed = speed;
        ShotType = shot;
        TargetPosition = to;
        transform.position = source;
        LogicalPosition = transform.position;
        HomingTarget = homingTarget;
        HomingPrecision = homingPrecision;
        HomingWindow = homingWindow;
        float rise = (TargetPosition.y - collider.bounds.center.y);
        float run = TargetPosition.x - collider.bounds.center.x;
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
            case WeaponType.eGeneric:
                renderer.sprite = pool.frames[0];
                break;
            case WeaponType.eMidBoss_Arrow_Vert:
                renderer.sprite = pool.frames[1];
                break;
            case WeaponType.eMidBoss_Arrow_Horiz:
                renderer.sprite = pool.frames[2];
                break;
            case WeaponType.eGenericBomb:
                renderer.sprite = pool.frames[0];
                isExplosive = true;
                break;
            case WeaponType.spEnergyRecover:
                renderer.sprite = Resources.Load<Sprite>(GlobalStaticResources.p_EnergyRecoverGFX);
                break;
        }
    }

    /// <summary>
    /// Called by hittable whatsits what when the bullet hits them.
    /// </summary>
    public void HitTarget ()
    {
        if (isExplosive == true || PierceScenery == false || (HomingTarget != null && HomingPrecision < 1))
        {
            Retire();
        }
    }

    /// <summary>
    /// Cleans up after us and returns the bullet to the queue.
    /// </summary>
    void Retire ()
    {
        if (q != null)
        {
            q.Enqueue(this);
        }
        gameObject.SetActive(false);
        
        switch (ShotType)
        {
            case WeaponType.pWG:
            case WeaponType.pWGII:
            case WeaponType.pShotgun:
                boomPool.StartBoom(collider.bounds.center, BoomType.EnergyThingy);
                break;
            case WeaponType.pShadow:
                boomPool.StartBoom(collider.bounds.center, BoomType.EnergyThingy);
                _Retire_pShadow();
                break;
            case WeaponType.eGeneric:
                boomPool.StartBoom(collider.bounds.center, BoomType.EnergyThingy);
                break;
            case WeaponType.eGenericBomb:
                boomPool.StartBoom(collider.bounds.center, BoomType.SmokePuff, true, 50, 20, 16, owner);
                break;
        }
    }

    /// <summary>
    /// Handles special-case logic for what happens when a bullet of WeaponType.pShadow retires, and the player needs to un-vanish.
    /// </summary>
    void _Retire_pShadow ()
    {
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
        world.player.transform.position = new Vector3(np.x, np.y, 0) + new Vector3(world.player.transform.position.x - world.player.collider.bounds.center.x, world.player.transform.position.y - world.player.collider.bounds.center.y, world.player.transform.position.z);
    }

    /// <summary>
    /// Bullet collision handling.
    /// </summary>
    void CollisionCheck()
    {
        bool chkCollision = true;
        for (int i = 0; i < world.activeRoom.priorityMap.priorities.Length; i++)
        {
            if (collider.bounds.Intersects(world.activeRoom.priorityMap.zones[i]) == true)
            {
                if (world.activeRoom.priorityMap.priorities[i] > Priority)
                {
                    Retire();
                    break;
                }
                else if (world.activeRoom.priorityMap.priorities[i] < Priority)
                {
                    chkCollision = false;
                    break;
                }
            }
        }
        if (chkCollision == true)
        {
            for (int i = 0; i < roomColliders.Length; i++)
            {
                if (roomColliders[i] != default(Bounds))
                {
                    if (collider.bounds.Intersects(roomColliders[i]))
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

    /// <summary>
    /// Adjusts the firing path of a bullet as necessary.
    /// Right now, this just does homing - but shot-type-specific things could also happen here.
    /// </summary>
    void FiringAdjust()
    {
        if (HomingTarget != null)
        {
            float rise = (HomingTarget.bounds.center.y - collider.bounds.center.y);
            float run = HomingTarget.bounds.center.x - collider.bounds.center.x;
            float normalizationFactor = 1 / (Math.Abs(rise) + Math.Abs(run));
            Heading = new Vector2(normalizationFactor * run * Speed, normalizationFactor * rise * Speed);
        }
    }
}
