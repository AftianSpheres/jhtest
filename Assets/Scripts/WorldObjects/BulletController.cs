using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// A horrible, messy thing that controls the logic for every single "bullet" in the game.
/// Because I want to keep bullets in a single more or less static object pool that never has to fuck around with adding or removing components...
/// this has to handle the logic for every single shot type as necessary. Eww!
/// 
/// Also, it has a giant pile of static arrays that it uses to poke states into a Mecanim state machine in order to save a whopping two frames of transition lag on firing.
/// </summary>
public class BulletController : MonoBehaviour
{
    public WorldController world;
    public Animator animator;
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
    private Vector2 lastHeading;
    static int directionParam = Animator.StringToHash("Direction");
    static int shotTypeParam = Animator.StringToHash("ShotType");
    private static int enemyShotsBase = (int)WeaponType.eGenericTiny;
    #region MULTIANGLE_SHOT_FRAME_DEFS
    private static int[] shot_playerSTD =
    {
        Animator.StringToHash("shot_playerSTD_down"),
        Animator.StringToHash("shot_playerSTD_up"),
        Animator.StringToHash("shot_playerSTD_left"),
        Animator.StringToHash("shot_playerSTD_right"),
        Animator.StringToHash("shot_playerSTD_downLeft"),
        Animator.StringToHash("shot_playerSTD_downRight"),
        Animator.StringToHash("shot_playerSTD_upLeft"),
        Animator.StringToHash("shot_playerSTD_upRight")
    };
    private static int[] shot_playerBullet =
    {
        Animator.StringToHash("shot_playerBullet_down"),
        Animator.StringToHash("shot_playerBullet_up"),
        Animator.StringToHash("shot_playerBullet_left"),
        Animator.StringToHash("shot_playerBullet_right"),
        Animator.StringToHash("shot_playerBullet_downLeft"),
        Animator.StringToHash("shot_playerBullet_downRight"),
        Animator.StringToHash("shot_playerBullet_upLeft"),
        Animator.StringToHash("shot_playerBullet_upRight")
    };
    private static int[] shot_playerShadow =
    {
        Animator.StringToHash("shot_playerShadow_down"),
        Animator.StringToHash("shot_playerShadow_up"),
        Animator.StringToHash("shot_playerShadow_left"),
        Animator.StringToHash("shot_playerShadow_right"),
        Animator.StringToHash("shot_playerShadow_downLeft"),
        Animator.StringToHash("shot_playerShadow_downRight"),
        Animator.StringToHash("shot_playerShadow_upLeft"),
        Animator.StringToHash("shot_playerShadow_upRight")
    };
    private static int[] shot_playerFire =
    {
        Animator.StringToHash("shot_playerFire_down"),
        Animator.StringToHash("shot_playerFire_up"),
        Animator.StringToHash("shot_playerFire_left"),
        Animator.StringToHash("shot_playerFire_right"),
        Animator.StringToHash("shot_playerFire_downLeft"),
        Animator.StringToHash("shot_playerFire_downRight"),
        Animator.StringToHash("shot_playerFire_upLeft"),
        Animator.StringToHash("shot_playerFire_upRight")
    };
    private static int[] shot_playerDarkArrow =
    {
        Animator.StringToHash("shot_playerDarkArrow_down"),
        Animator.StringToHash("shot_playerDarkArrow_up"),
        Animator.StringToHash("shot_playerDarkArrow_left"),
        Animator.StringToHash("shot_playerDarkArrow_right"),
        Animator.StringToHash("shot_playerDarkArrow_downLeft"),
        Animator.StringToHash("shot_playerDarkArrow_downRight"),
        Animator.StringToHash("shot_playerDarkArrow_upLeft"),
        Animator.StringToHash("shot_playerDarkArrow_upRight")
    };
    #endregion
    #region SHOT_DEFS
    private static int[][] playerShots =
    {
        shot_playerSTD,
        shot_playerSTD,
        shot_playerBullet,
        shot_playerShadow,
        shot_playerFire,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerDarkArrow,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerSTD,
        shot_playerSTD
    };
    private static int[] standardEnemyShots =
    {
        Animator.StringToHash("shot_enemySTD_tiny"),
        Animator.StringToHash("shot_enemySTD_mid"),
        Animator.StringToHash("shot_enemySTD_big"),
        Animator.StringToHash("shot_enemySTD_ring")
    };
    private static int systemShot_EnergyRecover = Animator.StringToHash("shot_sysEnergyRecover");
    #endregion

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
            if (Heading != lastHeading)
            {
                animator.SetInteger(directionParam, (int)GetDirectionFromCurrentHeading());
            }
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
        Trail = Vector2.zero;
        transform.position = source;
        roomColliders = world.activeRoom.collision.fullCollide;
        Damage = damage;
        PierceScenery = pierceScenery;
        Weight = weight;
        q = pool.q;
        bulletPool = pool;
        Speed = speed;
        ShotType = shot;
        TargetPosition = to;
        HomingTarget = homingTarget;
        HomingPrecision = homingPrecision;
        HomingWindow = homingWindow;
        float rise = (TargetPosition.y - collider.bounds.center.y);
        float run = TargetPosition.x - collider.bounds.center.x;
        float normalizationFactor = 1 / (Math.Abs(rise) + Math.Abs(run));
        Heading = new Vector2(normalizationFactor * run * Speed, normalizationFactor * rise * Speed);
        Direction dir = GetDirectionFromCurrentHeading();
        animator.SetInteger(directionParam, (int)dir);
        animator.SetInteger(shotTypeParam, (int)shot);
        if (shot < WeaponType.pPsychicRay + 1)
        {
            animator.Play(playerShots[(int)shot][(int)dir], 0);
        }
        else if (shot > WeaponType.eGenericTiny - 1 && shot < WeaponType.eRing + 1)
        {
            animator.Play(standardEnemyShots[(int)shot - enemyShotsBase], 0);
        }
        else if (shot == WeaponType.spEnergyRecover)
        {
            animator.Play(systemShot_EnergyRecover, 0);
        }
        if (owner == world.player.gameObject)
        {
            switch (dir)
            {
                case Direction.Down:
                    transform.position += ((collider.bounds.size.y + 9) * Vector3.down);
                    break;
                case Direction.Up:
                    transform.position += (collider.bounds.size.y * Vector3.up);
                    break;
                case Direction.Left:
                    transform.position += (collider.bounds.size.x * Vector3.left);
                    break;
                case Direction.Right:
                    transform.position += (collider.bounds.size.x * Vector3.right);
                    break;
                case Direction.DownLeft:
                    transform.position += (((collider.bounds.size.y / 2f) + 9) * Vector3.down) + (((collider.bounds.size.x / 2f) + 2) * Vector3.left);
                    break;
                case Direction.DownRight:
                    transform.position += (((collider.bounds.size.y / 2f) + 9) * Vector3.down) + (((collider.bounds.size.x / 2f) + 2) * Vector3.right);
                    break;
                case Direction.UpLeft:
                    transform.position += ((collider.bounds.size.y / 2f) * Vector3.up) + (((collider.bounds.size.x / 2f) + 2) * Vector3.left);
                    break;
                case Direction.UpRight:
                    transform.position += ((collider.bounds.size.y / 2f) * Vector3.up) + (((collider.bounds.size.x / 2f) + 2) * Vector3.right);
                    break;
            }
        }
        LogicalPosition = transform.position;
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
            case WeaponType.pShadow:
                snapToThing = world.player.transform.position;
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
        switch (ShotType)
        {
            case WeaponType.pWG:
            case WeaponType.pWGII:
                _Retire_pWG();
                break;
            case WeaponType.pShotgun:
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    boomPool.StartBoom(collider.bounds.center, BoomType.EnergyThingy);
                }
                else
                {
                    boomPool.StartBoom(collider.bounds.center, BoomType.SmokePuff, true, Damage, 20, Weight, world.player.gameObject);
                }
                break;
            case WeaponType.pShadow:
                boomPool.StartBoom(collider.bounds.center, BoomType.EnergyThingy);
                _Retire_pShadow();
                break;
            case WeaponType.eGenericTiny:
            case WeaponType.eGenericMid:
            case WeaponType.eGenericBig:
            case WeaponType.eRing:
                boomPool.StartBoom(collider.bounds.center, BoomType.EnergyThingy);
                break;
        }
        collider.size = Vector2.zero;
        renderer.sprite = default(Sprite);
        gameObject.SetActive(false);
    }

    void _Retire_pWG ()
    {
        switch ((Direction)animator.GetInteger(directionParam))
        {
            case Direction.Down:
                boomPool.StartBoom((new Vector3(collider.bounds.center.x, collider.bounds.min.y, 0)), BoomType.EnergyThingy);
                break;
            case Direction.Up:
                boomPool.StartBoom(new Vector3(collider.bounds.center.x, collider.bounds.max.y, 0), BoomType.EnergyThingy);
                break;
            case Direction.Left:
                boomPool.StartBoom(new Vector3(collider.bounds.min.x, collider.bounds.center.y, 0), BoomType.EnergyThingy);
                break;
            case Direction.Right:
                boomPool.StartBoom(new Vector3(collider.bounds.max.x, collider.bounds.center.y, 0), BoomType.EnergyThingy);
                break;
            case Direction.DownLeft:
                boomPool.StartBoom(new Vector3(collider.bounds.min.x, collider.bounds.min.y, 0), BoomType.EnergyThingy);
                break;
            case Direction.DownRight:
                boomPool.StartBoom(new Vector3(collider.bounds.max.x, collider.bounds.min.y, 0), BoomType.EnergyThingy);
                break;
            case Direction.UpLeft:
                boomPool.StartBoom(new Vector3(collider.bounds.min.x, collider.bounds.max.y, 0), BoomType.EnergyThingy);
                break;
            case Direction.UpRight:
                boomPool.StartBoom(new Vector3(collider.bounds.max.x, collider.bounds.max.y, 0), BoomType.EnergyThingy);
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
        world.player.mover.heading = Vector3.zero;
        world.player.mover.virtualPosition = world.player.transform.position;
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

    Direction GetDirectionFromCurrentHeading ()
    {
        Direction dir;
        if (Mathf.Abs(Heading.y / 2f) > Mathf.Abs(Heading.x))
        {
            if (Heading.y < 0)
            {
                dir = Direction.Down;
            }
            else
            {
                dir = Direction.Up;
            }
        }
        else if (Mathf.Abs(Heading.x / 2f) > Mathf.Abs(Heading.y))
        {
            if (Heading.x < 0)
            {
                dir = Direction.Left;
            }
            else
            {
                dir = Direction.Right;
            }
        }
        else
        {
            if (Heading.y < 0)
            {
                if (Heading.x < 0)
                {
                    dir = Direction.DownLeft;
                }
                else
                {
                    dir = Direction.DownRight;
                }
            }
            else
            {
                if (Heading.x < 0)
                {
                    dir = Direction.UpLeft;
                }
                else
                {
                    dir = Direction.UpRight;
                }
            }
        }
        lastHeading = Heading;
        return dir;
    }
}
