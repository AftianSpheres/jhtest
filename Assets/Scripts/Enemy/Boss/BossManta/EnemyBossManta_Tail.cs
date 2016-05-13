﻿using UnityEngine;
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

    public IEnumerator TailSweep(float speed)
    {
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
    }

    public IEnumerator TailStab(float speed)
    {
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
