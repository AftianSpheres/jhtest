﻿using UnityEngine;
using System.Collections;

enum EnemyBossManta_TailMode
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
    [SerializeField] private EnemyBossManta_TailMode mode;
    public EnemyBossManta master;
    public EnemyBossManta_TailBit[] tailBits;
    public Vector3[] anchorPoints;
    public int tailHP;
    public int tailStartingHP;

    void Start()
    {
        for (int i = 0; i < anchorPoints.Length; i++)
        {
            anchorPoints[i] = GetAnchorPoint();
        }
    }

    void Update()
    {
        if (master.common.register.Toggled == true)
        {
            anchorPoints[7] = anchorPoints[6];
            anchorPoints[6] = anchorPoints[5];
            anchorPoints[5] = anchorPoints[4];
            anchorPoints[4] = anchorPoints[3];
            anchorPoints[3] = anchorPoints[2];
            anchorPoints[2] = anchorPoints[1];
            anchorPoints[1] = anchorPoints[0];
            anchorPoints[0] = GetAnchorPoint();
            if (mode == EnemyBossManta_TailMode.Neutral)
            {
                StartCoroutine(TailSweep(6));
            }
        }
    }

    Vector3 GetAnchorPoint ()
    {
        Vector3 anchorPoint;
        switch (tailDirection)
        {
            case Direction.Down:
                anchorPoint = new Vector3(0, 52, 0) + master.transform.localPosition;
                break;
            case Direction.Up:
                anchorPoint = new Vector3(master.transform.position.x + 20, master.transform.position.y, master.transform.position.z);
                break;
            case Direction.Left:
                anchorPoint = new Vector3(-27, -28, 0) + master.transform.position;
                break;
            case Direction.Right:
                anchorPoint = new Vector3(27, -28, 0) + master.transform.position;
                break;
            case Direction.DownLeft:
                anchorPoint = new Vector3(-24, -51, 0) + master.transform.position;
                break;
            case Direction.DownRight:
                anchorPoint = new Vector3(24, -51, 0) + master.transform.position;
                break;
            case Direction.UpLeft:
                anchorPoint = new Vector3(-24, -4, 0) + master.transform.position;
                break;
            case Direction.UpRight:
                anchorPoint = new Vector3(24, -4, 0) + master.transform.position;
                break;
            default:
                throw new System.Exception("Invalid direction in GetAnchorPoint: " + tailDirection.ToString());
        }
        return anchorPoint;
    }

    public IEnumerator TailSweep(int speed)
    {
        mode = EnemyBossManta_TailMode.Sweeping;
        Bounds[] interceptZones = new Bounds[6];
        Bounds virtualTailtip = new Bounds(tailBits[tailBits.Length - 1].collider.bounds.center, tailBits[tailBits.Length - 1].collider.bounds.size);
        switch (tailDirection)
        {
            case Direction.Up:
                interceptZones[0] = new Bounds(new Vector3(anchorPoints[0].x - 96, anchorPoints[0].y, 0), new Vector3(16, 16, 500));
                interceptZones[1] = new Bounds(new Vector3(anchorPoints[0].x - 48, anchorPoints[0].y - 96, 0), new Vector3(8, 8, 500));
                interceptZones[2] = new Bounds(new Vector3(anchorPoints[0].x - 24, anchorPoints[0].y - 120, 0), new Vector3(8, 8, 500));
                interceptZones[3] = new Bounds(new Vector3(anchorPoints[0].x - 24, anchorPoints[0].y - 120, 0), new Vector3(8, 8, 500));
                interceptZones[4] = new Bounds(new Vector3(anchorPoints[0].x + 48, anchorPoints[0].y - 96, 0), new Vector3(8, 8, 500));
                interceptZones[5] = new Bounds(new Vector3(anchorPoints[0].x + 96, anchorPoints[0].y, 0), new Vector3(8, 8, 500));
                break;
            default:
                throw new System.Exception("");
        }
        int interceptStage = 0;
        int c = 0;
        float vx = interceptZones[0].center.x - virtualTailtip.center.x;
        float vy = interceptZones[0].center.y - virtualTailtip.center.y;
        Vector3 virtualTailtipHeading = new Vector3(vx / (Mathf.Abs(vx) + Mathf.Abs(vy)), vy / (Mathf.Abs(vx) + Mathf.Abs(vy)), 0);
        Vector3 virtualTailtipLastFramePos;
        
        while (interceptStage < interceptZones.Length)
        {
            if (interceptZones[interceptStage].Intersects(tailBits[tailBits.Length - 1].collider.bounds) == true)
            {
                interceptStage++;
                Debug.Break();
            }
            else
            {
                virtualTailtipLastFramePos = virtualTailtip.center;
                virtualTailtip.center += virtualTailtipHeading;
                if (c == speed)
                {
                    c = 0;
                    for (int i = 0; i < tailBits.Length; i++)
                    {
                        tailBits[i].moveQueue.Enqueue((virtualTailtip.center - virtualTailtipLastFramePos) * ((float)tailBits[i].distanceFromBody / tailBits.Length));
                    }
                }
                else
                {
                    c++;
                }

            }
            yield return null;
        }
    }

    public void TailStab()
    {

    }

}
