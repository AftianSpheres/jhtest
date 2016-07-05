using UnityEngine;
using System.Collections;

public enum EnemyBossManta_TailMode
{
    Neutral,
    Sweeping, // 96 at widest point
    Stabbing,
    Locked,
    Dead
}

public class EnemyBossManta_Tail : MonoBehaviour
{
    public Direction tailDirection;
    public EnemyBossManta_TailMode mode;
    public EnemyBossManta master;
    public EnemyBossManta_TailBit[] tailBits;
    public Vector3[] anchorPoints;
    public int tailHP;
    public int tailStartingHP;
    public AudioClip hitSFX;
    int tailReviveTimer;
    Bounds virtualTailtip;
    bool OffFrame = false;
# if UNITY_EDITOR
    Ray r;
    Vector3 v;
#endif

    void Start()
    {
        for (int i = 0; i < anchorPoints.Length; i++)
        {
            anchorPoints[i] = GetAnchorPoint();
        }
    }

    void Update()
    {
        if (master.common.room.isActiveRoom == true && OffFrame == true)
        {
            anchorPoints[7] = anchorPoints[6];
            anchorPoints[6] = anchorPoints[5];
            anchorPoints[5] = anchorPoints[4];
            anchorPoints[4] = anchorPoints[3];
            anchorPoints[3] = anchorPoints[2];
            anchorPoints[2] = anchorPoints[1];
            anchorPoints[1] = anchorPoints[0];
            anchorPoints[0] = GetAnchorPoint();
            OffFrame = false;
        }
        else
        {
            OffFrame = true;
        }
        if (tailHP < 1 && mode != EnemyBossManta_TailMode.Dead && mode != EnemyBossManta_TailMode.Locked)
        {
            StartCoroutine(TailDie());
        }
        if (mode == EnemyBossManta_TailMode.Dead)
        {
            tailReviveTimer--;
            if (tailReviveTimer < 0)
            {
                StartCoroutine(TailRevive());
            }
        }
    }

    Vector3 GetAnchorPoint ()
    {
        Vector3 anchorPoint;
        switch (tailDirection)
        {
            case Direction.Down:
                anchorPoint = new Vector3(master.common.collider.bounds.center.x, master.common.collider.bounds.min.y, master.transform.position.z);
                break;
            case Direction.Up:
                anchorPoint = new Vector3(master.common.collider.bounds.center.x, master.common.collider.bounds.max.y, master.transform.position.z);
                break;
            case Direction.Left:
                anchorPoint = new Vector3(master.common.collider.bounds.min.x, master.common.collider.bounds.center.y, master.transform.position.z);
                break;
            case Direction.Right:
                anchorPoint = new Vector3(master.common.collider.bounds.max.x, master.common.collider.bounds.center.y, master.transform.position.z);
                break;
            case Direction.DownLeft:
                anchorPoint = new Vector3((master.common.collider.bounds.center.x + master.common.collider.bounds.min.x)/2, (master.common.collider.bounds.center.y + master.common.collider.bounds.min.y)/2, master.transform.position.z);
                break;
            case Direction.DownRight:
                anchorPoint = new Vector3((master.common.collider.bounds.center.x + master.common.collider.bounds.max.x) / 2, (master.common.collider.bounds.center.y + master.common.collider.bounds.min.y) / 2, master.transform.position.z);
                break;
            case Direction.UpLeft:
                anchorPoint = new Vector3((master.common.collider.bounds.center.x + master.common.collider.bounds.min.x) / 2, (master.common.collider.bounds.center.y + master.common.collider.bounds.max.y) / 2, master.transform.position.z);
                break;
            case Direction.UpRight:
                anchorPoint = new Vector3((master.common.collider.bounds.center.x + master.common.collider.bounds.max.x) / 2, (master.common.collider.bounds.center.y + master.common.collider.bounds.max.y) / 2, master.transform.position.z);
                break;
            default:
                throw new System.Exception("Invalid direction in GetAnchorPoint: " + tailDirection.ToString());
        }
        return anchorPoint;
    }

    public IEnumerator TailDie()
    {
        mode = EnemyBossManta_TailMode.Locked;
        for (int i = 0; i < tailBits.Length; i++)
        {
            yield return null;
            if (master.common.room.world.paused == true)
            {
                yield return null;
            }
            master.common.source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_ExplosionSFX));
            master.common.room.world.Booms.StartBoom(new Vector3(tailBits[i].collider.bounds.min.x, tailBits[i].collider.bounds.max.y, tailBits[i].collider.bounds.center.z), BoomType.SmokePuff);
            yield return null;
        }
        mode = EnemyBossManta_TailMode.Dead;
        for (int i = tailBits.Length - 1; i > -1; i--)
        {
            master.common.room.world.Booms.StartBoom(new Vector3(tailBits[i].collider.bounds.max.x, tailBits[i].collider.bounds.min.y, tailBits[i].collider.bounds.center.z), BoomType.SmokePuff);
            tailBits[i].gameObject.SetActive(false);
        }
        tailReviveTimer = Random.Range(600, 1200);
    }

    public IEnumerator TailRevive()
    {
        mode = EnemyBossManta_TailMode.Locked;
        for (int i = tailBits.Length - 1; i > -1; i--)
        {
            yield return null;
            tailBits[i].Reset();
            master.common.room.world.Booms.StartBoom(new Vector3(tailBits[i].collider.bounds.max.x, tailBits[i].collider.bounds.min.y, tailBits[i].collider.bounds.center.z), BoomType.EnergyThingy);
            tailBits[i].gameObject.SetActive(true);
            Vector3 dest;
            for (int i2 = 0; i2 < 4; i2++)
            {
                if (i2 == 0)
                {
                    dest = new Vector3(tailBits[i].collider.bounds.center.x, tailBits[i].collider.bounds.center.y - 1, tailBits[i].collider.bounds.center.z - 1);
                }
                else if (i2 == 1)
                {
                    dest = new Vector3(tailBits[i].collider.bounds.center.x, tailBits[i].collider.bounds.center.y + 1, tailBits[i].collider.bounds.center.z - 1);
                }
                else if (i2 == 2)
                {
                    dest = new Vector3(tailBits[i].collider.bounds.center.x - 1, tailBits[i].collider.bounds.center.y, tailBits[i].collider.bounds.center.z - 1);
                }
                else
                {
                    dest = new Vector3(tailBits[i].collider.bounds.center.x + 1, tailBits[i].collider.bounds.center.y, tailBits[i].collider.bounds.center.z - 1);
                }
                master.common.room.world.EnemyBullets.FireBullet(WeaponType.eGenericTiny, 2, master.common.ShotDmg, 5, tailBits[i].collider.bounds.center, dest, true, master.common.room.world.player.collider, 16, int.MaxValue, master.gameObject);
            }
            master.common.source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_EnemyFireStrangeBurstSFX));
            yield return null;
        }
        tailHP = tailStartingHP;
        mode = EnemyBossManta_TailMode.Neutral;
    }

    public IEnumerator TailSweep(float speed)
    {
        master.common.inMovingAttack = true;
        mode = EnemyBossManta_TailMode.Sweeping;
        Bounds[] interceptZones = new Bounds[8];
        virtualTailtip = new Bounds(tailBits[tailBits.Length - 1].collider.bounds.center, tailBits[tailBits.Length - 1].collider.bounds.size);
        switch (tailDirection)
        {
            case Direction.Down:
                interceptZones[0] = new Bounds(new Vector3(anchorPoints[0].x - 66, anchorPoints[0].y, 0), new Vector3(8, 8, 500));
                interceptZones[1] = new Bounds(new Vector3(anchorPoints[0].x - 80, anchorPoints[0].y + 42, 0), new Vector3(8, 8, 500));
                interceptZones[2] = new Bounds(new Vector3(anchorPoints[0].x - 76, anchorPoints[0].y + 78, 0), new Vector3(8, 8, 500));
                interceptZones[3] = new Bounds(new Vector3(anchorPoints[0].x, anchorPoints[0].y + 96, 0), new Vector3(8, 8, 500));
                interceptZones[4] = new Bounds(new Vector3(anchorPoints[0].x + 76, anchorPoints[0].y + 78, 0), new Vector3(8, 8, 500));
                interceptZones[5] = new Bounds(new Vector3(anchorPoints[0].x + 80, anchorPoints[0].y + 42, 0), new Vector3(8, 8, 500));
                interceptZones[6] = new Bounds(new Vector3(anchorPoints[0].x + 66, anchorPoints[0].y, 0), new Vector3(8, 8, 500));
                interceptZones[7] = new Bounds(new Vector3(anchorPoints[0].x, anchorPoints[0].y + (virtualTailtip.center.y - anchorPoints[0].y), 0), new Vector3(8, 8, 500));
                break;
            case Direction.Up:
                interceptZones[0] = new Bounds(new Vector3(anchorPoints[0].x - 66, anchorPoints[0].y, 0), new Vector3(8, 8, 500));
                interceptZones[1] = new Bounds(new Vector3(anchorPoints[0].x - 80, anchorPoints[0].y - 42, 0), new Vector3(8, 8, 500));
                interceptZones[2] = new Bounds(new Vector3(anchorPoints[0].x - 76, anchorPoints[0].y - 78, 0), new Vector3(8, 8, 500));
                interceptZones[3] = new Bounds(new Vector3(anchorPoints[0].x, anchorPoints[0].y - 96, 0), new Vector3(8, 8, 500));
                interceptZones[4] = new Bounds(new Vector3(anchorPoints[0].x + 76, anchorPoints[0].y - 78, 0), new Vector3(8, 8, 500));
                interceptZones[5] = new Bounds(new Vector3(anchorPoints[0].x + 80, anchorPoints[0].y - 42, 0), new Vector3(8, 8, 500));
                interceptZones[6] = new Bounds(new Vector3(anchorPoints[0].x + 66, anchorPoints[0].y, 0), new Vector3(8, 8, 500));
                interceptZones[7] = new Bounds(new Vector3(anchorPoints[0].x, anchorPoints[0].y + (virtualTailtip.center.y - anchorPoints[0].y), 0), new Vector3(8, 8, 500)); 
                break;
            case Direction.Left:
                interceptZones[0] = new Bounds(new Vector3(anchorPoints[0].x, anchorPoints[0].y - 66, 0), new Vector3(8, 8, 500));
                interceptZones[1] = new Bounds(new Vector3(anchorPoints[0].x + 42, anchorPoints[0].y - 80, 0), new Vector3(8, 8, 500));
                interceptZones[2] = new Bounds(new Vector3(anchorPoints[0].x + 78, anchorPoints[0].y - 76, 0), new Vector3(8, 8, 500));
                interceptZones[3] = new Bounds(new Vector3(anchorPoints[0].x + 96, anchorPoints[0].y, 0), new Vector3(8, 8, 500));
                interceptZones[4] = new Bounds(new Vector3(anchorPoints[0].x + 78, anchorPoints[0].y + 76, 0), new Vector3(8, 8, 500));
                interceptZones[5] = new Bounds(new Vector3(anchorPoints[0].x + 42, anchorPoints[0].y + 80, 0), new Vector3(8, 8, 500));
                interceptZones[6] = new Bounds(new Vector3(anchorPoints[0].x, anchorPoints[0].y + 66, 0), new Vector3(8, 8, 500));
                interceptZones[7] = new Bounds(new Vector3(anchorPoints[0].x + (virtualTailtip.center.x - anchorPoints[0].x), anchorPoints[0].y, 0), new Vector3(8, 8, 500));
                break;
            case Direction.Right:
                interceptZones[0] = new Bounds(new Vector3(anchorPoints[0].x, anchorPoints[0].y - 66, 0), new Vector3(8, 8, 500));
                interceptZones[1] = new Bounds(new Vector3(anchorPoints[0].x - 42, anchorPoints[0].y - 80, 0), new Vector3(8, 8, 500));
                interceptZones[2] = new Bounds(new Vector3(anchorPoints[0].x - 78, anchorPoints[0].y - 76, 0), new Vector3(8, 8, 500));
                interceptZones[3] = new Bounds(new Vector3(anchorPoints[0].x - 96, anchorPoints[0].y, 0), new Vector3(8, 8, 500));
                interceptZones[4] = new Bounds(new Vector3(anchorPoints[0].x - 78, anchorPoints[0].y + 76, 0), new Vector3(8, 8, 500));
                interceptZones[5] = new Bounds(new Vector3(anchorPoints[0].x - 42, anchorPoints[0].y + 80, 0), new Vector3(8, 8, 500));
                interceptZones[6] = new Bounds(new Vector3(anchorPoints[0].x, anchorPoints[0].y + 66, 0), new Vector3(8, 8, 500));
                interceptZones[7] = new Bounds(new Vector3(anchorPoints[0].x + (virtualTailtip.center.x - anchorPoints[0].x), anchorPoints[0].y, 0), new Vector3(8, 8, 500));
                break;
            default:
                throw new System.Exception("");
        }
        int interceptStage = 0;
        float vx = interceptZones[0].center.x - virtualTailtip.center.x;
        float vy = interceptZones[0].center.y - virtualTailtip.center.y;
        Vector3 virtualTailtipHeading = new Vector3(vx / (Mathf.Abs(vx) + Mathf.Abs(vy)), vy / (Mathf.Abs(vx) + Mathf.Abs(vy)), 0) * speed;
        Vector3 virtualTailtipLastFramePos;
        while (interceptStage < interceptZones.Length)
        {
            if (master.common.room.world.paused == true)
            {
                yield return null;
            }
            if (master.common.room.isActiveRoom == false)
            {
                yield return null;
            }
            else
            {
                vx = interceptZones[interceptStage].center.x - virtualTailtip.center.x;
                vy = interceptZones[interceptStage].center.y - virtualTailtip.center.y;
                virtualTailtipHeading = new Vector3(vx / (Mathf.Abs(vx) + Mathf.Abs(vy)), vy / (Mathf.Abs(vx) + Mathf.Abs(vy)), 0) * speed;
#if UNITY_EDITOR
                v = interceptZones[interceptStage].center;
                r = new Ray(tailBits[tailBits.Length - 1].collider.bounds.center, -1 * virtualTailtipHeading);
#endif
                if (Mathf.Abs(tailBits[tailBits.Length - 1].collider.bounds.center.x + virtualTailtipHeading.x - interceptZones[interceptStage].center.x) > Mathf.Abs(tailBits[tailBits.Length - 1].collider.bounds.center.x - interceptZones[interceptStage].center.x) ||
                    Mathf.Abs(tailBits[tailBits.Length - 1].collider.bounds.center.y + virtualTailtipHeading.y - interceptZones[interceptStage].center.y) > Mathf.Abs(tailBits[tailBits.Length - 1].collider.bounds.center.y - interceptZones[interceptStage].center.y))
                {
                    virtualTailtip = new Bounds(tailBits[tailBits.Length - 1].collider.bounds.center, tailBits[tailBits.Length - 1].collider.bounds.size);
                    interceptStage++;
                    if (interceptStage >= interceptZones.Length)
                    {
                        break;
                    }
                }
                else
                {
                    virtualTailtipLastFramePos = virtualTailtip.center;
                    virtualTailtip.center += virtualTailtipHeading;
                    for (int i = 0; i < tailBits.Length; i++)
                    {
                        tailBits[i].moveQueue.Enqueue(((virtualTailtip.center - virtualTailtipLastFramePos) * ((float)tailBits[i].distanceFromBody - 1 / tailBits.Length)));
                    }
                }
                yield return null;
            }
        }
        mode = EnemyBossManta_TailMode.Neutral;
        master.common.inMovingAttack = false;
    }

    public IEnumerator TailStab(float speed)
    {
        master.common.inMovingAttack = true;
        mode = EnemyBossManta_TailMode.Stabbing;
        Bounds[] interceptZones = new Bounds[2];
        virtualTailtip = new Bounds(tailBits[tailBits.Length - 1].collider.bounds.center, tailBits[tailBits.Length - 1].collider.bounds.size);
        switch (tailDirection)
        {
            case Direction.Down:
                interceptZones[0] = new Bounds(new Vector3(master.common.room.world.player.collider.bounds.center.x, master.common.room.world.player.collider.bounds.center.y + 16, 0), new Vector3(8, 8, 500));
                interceptZones[1] = new Bounds(new Vector3(anchorPoints[0].x, anchorPoints[0].y + (virtualTailtip.center.y - anchorPoints[0].y), 0), new Vector3(8, 8, 500));
                break;
            case Direction.Up:
                interceptZones[0] = new Bounds(new Vector3(master.common.room.world.player.collider.bounds.center.x, master.common.room.world.player.collider.bounds.center.y - 16, 0), new Vector3(8, 8, 500));
                interceptZones[1] = new Bounds(new Vector3(anchorPoints[0].x, anchorPoints[0].y + (virtualTailtip.center.y - anchorPoints[0].y), 0), new Vector3(8, 8, 500));
                break;
            case Direction.Left:
                interceptZones[0] = new Bounds(new Vector3(master.common.room.world.player.collider.bounds.center.x + 16, master.common.room.world.player.collider.bounds.center.y, 0), new Vector3(8, 8, 500));
                interceptZones[1] = new Bounds(new Vector3(anchorPoints[0].x + (virtualTailtip.center.x - anchorPoints[0].x), anchorPoints[0].y, 0), new Vector3(8, 8, 500));
                break;
            case Direction.Right:
                interceptZones[0] = new Bounds(new Vector3(master.common.room.world.player.collider.bounds.center.x - 16, master.common.room.world.player.collider.bounds.center.y, 0), new Vector3(8, 8, 500));
                interceptZones[1] = new Bounds(new Vector3(anchorPoints[0].x + (virtualTailtip.center.x - anchorPoints[0].x), anchorPoints[0].y, 0), new Vector3(8, 8, 500));
                break;
            default:
                throw new System.Exception("Can't do tail stab while facing this way!");
        }
        int interceptStage = 0;
        float vx = interceptZones[0].center.x - virtualTailtip.center.x;
        float vy = interceptZones[0].center.y - virtualTailtip.center.y;
        Vector3 virtualTailtipHeading = new Vector3(vx / (Mathf.Abs(vx) + Mathf.Abs(vy)), vy / (Mathf.Abs(vx) + Mathf.Abs(vy)), 0) * speed;
        Vector3 virtualTailtipLastFramePos;
        while (interceptStage < interceptZones.Length)
        {
            if (master.common.room.world.paused == true)
            {
                yield return null;
            }
            if (master.common.room.isActiveRoom == false)
            {
                yield return null;
            }
            else
            {
                vx = interceptZones[interceptStage].center.x - virtualTailtip.center.x;
                vy = interceptZones[interceptStage].center.y - virtualTailtip.center.y;
                virtualTailtipHeading = new Vector3(vx / (Mathf.Abs(vx) + Mathf.Abs(vy)), vy / (Mathf.Abs(vx) + Mathf.Abs(vy)), 0) * speed;
#if UNITY_EDITOR
                v = interceptZones[interceptStage].center;
                r = new Ray(tailBits[tailBits.Length - 1].collider.bounds.center, -1 * virtualTailtipHeading);
#endif
                if (Mathf.Abs(tailBits[tailBits.Length - 1].collider.bounds.center.x + virtualTailtipHeading.x - interceptZones[interceptStage].center.x) > Mathf.Abs(tailBits[tailBits.Length - 1].collider.bounds.center.x - interceptZones[interceptStage].center.x) ||
                    Mathf.Abs(tailBits[tailBits.Length - 1].collider.bounds.center.y + virtualTailtipHeading.y - interceptZones[interceptStage].center.y) > Mathf.Abs(tailBits[tailBits.Length - 1].collider.bounds.center.y - interceptZones[interceptStage].center.y))
                {
                    virtualTailtip = new Bounds(tailBits[tailBits.Length - 1].collider.bounds.center, tailBits[tailBits.Length - 1].collider.bounds.size);
                    interceptStage++;
                    if (interceptStage >= interceptZones.Length)
                    {
                        break;
                    }
                }
                else
                {
                    virtualTailtipLastFramePos = virtualTailtip.center;
                    virtualTailtip.center += virtualTailtipHeading;
                    for (int i = 0; i < tailBits.Length; i++)
                    {
                        tailBits[i].moveQueue.Enqueue(((virtualTailtip.center - virtualTailtipLastFramePos) * ((float)tailBits[i].distanceFromBody - 1 / tailBits.Length)));
                    }
                }
                yield return null;
            }

        }
        mode = EnemyBossManta_TailMode.Neutral;
        master.common.inMovingAttack = false;
    }

    public void Hit (BulletController bullet)
    {
        tailHP -= bullet.Damage;
        master.common.source.PlayOneShot(hitSFX);
        for (int i = 0; i < tailBits.Length; i++)
        {
            StartCoroutine(GFXHelpers.FlashEffect(tailBits[i].renderer, 30));
        }
        bullet.HitTarget();
    }

    public void Hit(BoomEffect boom)
    {
        tailHP -= boom.Damage;
        master.common.source.PlayOneShot(hitSFX);
        for (int i = 0; i < tailBits.Length; i++)
        {
            StartCoroutine(GFXHelpers.FlashEffect(tailBits[i].renderer, 30));
        }
    }

#if UNITY_EDITOR

    void OnDrawGizmos()
    {
        if (mode == EnemyBossManta_TailMode.Sweeping || mode == EnemyBossManta_TailMode.Stabbing)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(r.origin, r.origin + (120 * r.direction));
            Gizmos.DrawSphere(v, 16);
            Gizmos.DrawWireSphere(virtualTailtip.center, 16);
        }
    }

#endif
}
