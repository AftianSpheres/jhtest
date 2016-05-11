﻿using UnityEngine;
using System.Collections.Generic;

public class EnemyBossManta : EnemyModule
{
    public Direction facingDir;
    public EnemyBossManta_Tail tailController;
    public bool armorOff = false;
    public static int maximumAllowedDistance = 120;
    public float moveSpeed = 0.25f;
    private static int[] EnemyBossMantaAnimatorHashes =
    {
        Animator.StringToHash("Neutral"),
        Animator.StringToHash("FireVolley"),
        Animator.StringToHash("TurnClockwise"),
        Animator.StringToHash("TurnAntiClockwise"),
        Animator.StringToHash("Firing"),
        Animator.StringToHash("OpeningHatch"),
        Animator.StringToHash("ClosingHatch"),
        Animator.StringToHash("OpenHatch"),
        Animator.StringToHash("CloseHatch"),
        Animator.StringToHash("PurgeArmor")
    };
    private int RemainingVolleys = -1;
    private int TurnTimer = 0;
    private Vector3 virtualPosition;
    private bool playerOutOfLineOfSight;
    private bool turnAntiClockwise;
    private Vector3 move;
    private float playerDist;

    // Use this for initialization
    void Start ()
    {
        virtualPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (common.room == common.room.world.activeRoom)
        {
            playerDist = Mathf.Abs(common.collider.bounds.center.x - common.room.world.player.collider.bounds.center.x) + Mathf.Abs(common.collider.bounds.center.y - common.room.world.player.collider.bounds.center.y);
            playerOutOfLineOfSight = false;
            turnAntiClockwise = false;
            move = Vector3.zero;
            _in_getMoveDir();
            _in_executeAnyTurns();
            if (facingDir == Direction.Down || facingDir == Direction.Up || facingDir == Direction.Left || facingDir == Direction.Right)
            {
                _in_executeAttacks();
                _in_executeAnyMove();
            }
            if (armorOff == false)
            {
                _in_manageHatch();
                _in_manageArmorState();
            }
            else if (common.CurrentHP < 1)
            {
                startSuicideAttack();
            }
            transform.position = ExpensiveAccurateCollision.ShoveOutOfScenery(common.collider, common.room.collision.allNonObjCollision, new Vector3(Mathf.Round(virtualPosition.x), Mathf.Round(virtualPosition.y), transform.position.z));
        }
    }

    void _in_getMoveDir ()
    {
        if (common.animator.GetCurrentAnimatorStateInfo(0).tagHash == EnemyBossMantaAnimatorHashes[0] && (tailController.mode == EnemyBossManta_TailMode.Neutral || tailController.mode == EnemyBossManta_TailMode.Dead))
        {
            switch (facingDir)
            {
                case Direction.Down:
                    playerOutOfLineOfSight = (common.room.world.player.collider.bounds.center.y > common.collider.bounds.min.y || common.room.world.player.collider.bounds.center.x + 8 < common.collider.bounds.min.x - (playerDist / 4f) || common.room.world.player.collider.bounds.center.x - 8 > common.collider.bounds.max.x + (playerDist / 4f));
                    if (playerOutOfLineOfSight == true)
                    {
                        turnAntiClockwise = (common.room.world.player.collider.bounds.center.x > common.collider.bounds.center.x);
                    }
                    else
                    {
                        move = new Vector3(0, -moveSpeed, 0);
                    }
                    break;
                case Direction.Up:
                    playerOutOfLineOfSight = (common.room.world.player.collider.bounds.center.y < common.collider.bounds.max.y || common.room.world.player.collider.bounds.center.x + 8 < common.collider.bounds.min.x - (playerDist / 4f) || common.room.world.player.collider.bounds.center.x - 8 > common.collider.bounds.max.x + (playerDist / 4f));
                    if (playerOutOfLineOfSight == true)
                    {
                        turnAntiClockwise = (common.room.world.player.collider.bounds.center.x < common.collider.bounds.center.x);
                    }
                    else
                    {
                        move = new Vector3(0, moveSpeed, 0);
                    }
                    break;
                case Direction.Left:
                    playerOutOfLineOfSight = (common.room.world.player.collider.bounds.center.x > common.collider.bounds.min.x || common.room.world.player.collider.bounds.center.y + 8 < common.collider.bounds.min.y - (playerDist / 4f) || common.room.world.player.collider.bounds.center.y - 8 > common.collider.bounds.max.y + (playerDist / 4f));
                    if (playerOutOfLineOfSight == true)
                    {
                        turnAntiClockwise = (common.room.world.player.collider.bounds.center.y < common.collider.bounds.center.y);
                    }
                    else
                    {
                        move = new Vector3(-moveSpeed, 0, 0);
                    }
                    break;
                case Direction.Right:
                    playerOutOfLineOfSight = (common.room.world.player.collider.bounds.center.x < common.collider.bounds.max.x || common.room.world.player.collider.bounds.center.y + 8 < common.collider.bounds.min.y - (playerDist / 4f) || common.room.world.player.collider.bounds.center.y - 8 > common.collider.bounds.max.y + (playerDist / 4f));
                    if (playerOutOfLineOfSight == true)
                    {
                        turnAntiClockwise = (common.room.world.player.collider.bounds.center.y > common.collider.bounds.center.y);
                    }
                    else
                    {
                        move = new Vector3(moveSpeed, 0, 0);
                    }
                    break;
            }
        }
    }

    void _in_manageArmorState ()
    {
        if (common.CurrentHP < common.CurrentHP / 2)
        {
            purgeArmor();
        }
    }

    void _in_manageHatch ()
    {
        if (common.animator.GetCurrentAnimatorStateInfo(0).tagHash == EnemyBossMantaAnimatorHashes[4] || common.animator.GetCurrentAnimatorStateInfo(0).tagHash == EnemyBossMantaAnimatorHashes[5])
        {
            if (playerOutOfLineOfSight == true || RemainingVolleys < 0)
            {
                closeHatch();
                common.animator.ResetTrigger(EnemyBossMantaAnimatorHashes[1]);
            }
            else
            {
                common.animator.SetTrigger(EnemyBossMantaAnimatorHashes[1]);
            }
        }
    }

    void _in_executeAnyTurns ()
    {
        if (TurnTimer == 0)
        {
            if (playerOutOfLineOfSight == true)
            {
                if (common.animator.GetCurrentAnimatorStateInfo(0).tagHash == EnemyBossMantaAnimatorHashes[0])
                {
                    if (turnAntiClockwise == true)
                    {
                        common.animator.SetTrigger(EnemyBossMantaAnimatorHashes[3]);
                    }
                    else
                    {
                        common.animator.SetTrigger(EnemyBossMantaAnimatorHashes[2]);
                    }
                }
            }
            TurnTimer = 30;
        }
        else
        {
            TurnTimer--;
        }
    }

    void _in_executeAnyMove ()
    {
        if (common.animator.GetCurrentAnimatorStateInfo(0).tagHash == EnemyBossMantaAnimatorHashes[0] && (tailController.mode == EnemyBossManta_TailMode.Neutral || tailController.mode == EnemyBossManta_TailMode.Dead))
        {
            virtualPosition += move;
        }
    }

    void _in_executeAttacks ()
    {
        if (playerDist > maximumAllowedDistance)
        {
            if (tailController.mode == EnemyBossManta_TailMode.Neutral && Random.Range(0, 90) == 0)
            {
                StartCoroutine(tailController.TailStab(1 + (4 * ((playerDist - maximumAllowedDistance) / maximumAllowedDistance))));
                move = Vector3.zero;
            }
        }
        else
        {
            if (common.animator.GetCurrentAnimatorStateInfo(0).tagHash == EnemyBossMantaAnimatorHashes[0])
            {
                if (RemainingVolleys < 0 && Random.Range(0, 360) == 0)
                {
                    openHatch();
                    move = Vector3.zero;
                }
            }
            if (tailController.mode == EnemyBossManta_TailMode.Neutral && Random.Range(0, 90) == 0)
            {
                StartCoroutine(tailController.TailSweep(1.5f));
                move = Vector3.zero;
            }
        }
    }

    public void closeHatch ()
    {
        common.animator.SetTrigger(EnemyBossMantaAnimatorHashes[8]);
        RemainingVolleys = -1;
    }

    public void faceDown ()
    {
        facingDir = Direction.Down;
        tailController.tailDirection = Direction.Up;
    }

    public void faceUp ()
    {
        facingDir = Direction.Up;
        tailController.tailDirection = Direction.Down;
    }

    public void faceLeft ()
    {
        facingDir = Direction.Left;
        tailController.tailDirection = Direction.Right;
    }

    public void faceRight()
    {
        facingDir = Direction.Right;
        tailController.tailDirection = Direction.Left;
    }

    public void faceDownLeft ()
    {
        facingDir = Direction.DownLeft;
        tailController.tailDirection = Direction.UpRight;
    }

    public void faceDownRight ()
    {
        facingDir = Direction.DownRight;
        tailController.tailDirection = Direction.UpLeft;
    }

    public void faceUpLeft ()
    {
        facingDir = Direction.UpLeft;
        tailController.tailDirection = Direction.DownRight;
    }

    public void faceUpRight ()
    {
        facingDir = Direction.UpRight;
        tailController.tailDirection = Direction.UpLeft;
    }

    public void fireScattershot ()
    {
        Vector3[] origins = new Vector3[2];
        switch (facingDir)
        {
            case Direction.Down:
                origins[0] = new Vector3(common.collider.bounds.center.x - 8, common.collider.bounds.center.y, common.collider.bounds.center.z);
                origins[1] = new Vector3(common.collider.bounds.center.x + 8, common.collider.bounds.center.y, common.collider.bounds.center.z);
                break;
            case Direction.Up:
                origins[0] = new Vector3(common.collider.bounds.center.x - 8, common.collider.bounds.center.y, common.collider.bounds.center.z);
                origins[1] = new Vector3(common.collider.bounds.center.x + 8, common.collider.bounds.center.y, common.collider.bounds.center.z);
                break;
            case Direction.Left:
                origins[0] = new Vector3(common.collider.bounds.center.x, common.collider.bounds.center.y - 8, common.collider.bounds.center.z);
                origins[1] = new Vector3(common.collider.bounds.center.x, common.collider.bounds.center.y + 8, common.collider.bounds.center.z);
                break;
            case Direction.Right:
                origins[0] = new Vector3(common.collider.bounds.center.x, common.collider.bounds.center.y - 8, common.collider.bounds.center.z);
                origins[1] = new Vector3(common.collider.bounds.center.x, common.collider.bounds.center.y + 8, common.collider.bounds.center.z);
                break;
            default:
                throw new System.Exception("Invalid direction for firing: " + facingDir.ToString());
        }

        Vector3 dest;
        for (int i = 0; i < 10; i++)
        {
            if (i < 2)
            {
                dest = common.room.world.player.collider.bounds.center;
            }
            else if (i < 4)
            {
                dest = new Vector3(common.room.world.player.collider.bounds.center.x - 48, common.room.world.player.collider.bounds.center.y, common.room.world.player.collider.bounds.center.z);
            }
            else if (i < 6)
            {
                dest = new Vector3(common.room.world.player.collider.bounds.center.x + 48, common.room.world.player.collider.bounds.center.y, common.room.world.player.collider.bounds.center.z);
            }
            else if (i < 8)
            {
                dest = new Vector3(common.room.world.player.collider.bounds.center.x, common.room.world.player.collider.bounds.center.y - 48, common.room.world.player.collider.bounds.center.z);
            }
            else
            {
                dest = new Vector3(common.room.world.player.collider.bounds.center.x, common.room.world.player.collider.bounds.center.y + 48, common.room.world.player.collider.bounds.center.z);
            }
            common.room.world.EnemyBullets.FireBullet(WeaponType.eGeneric, 1, common.ShotDmg, 3, dest, origins[i % 2], true);
        }
        RemainingVolleys--;
    }

    public void fireSuicideBeam ()
    {

    }

    public void makeBodyVulnerable ()
    {
        common.Vulnerable = true;
    }

    public void makeBodyInvulnerable ()
    {
        common.Vulnerable = false;
    }

    public void openHatch ()
    {
        RemainingVolleys = Random.Range(2, 8);
        common.animator.SetTrigger(EnemyBossMantaAnimatorHashes[7]);
    }

    public void purgeArmor ()
    {
        armorOff = true;
        common.animator.SetTrigger(EnemyBossMantaAnimatorHashes[9]);
    }

    public void startSuicideAttack ()
    {

    }

    public override void Respawn()
    {
        tailController.mode = EnemyBossManta_TailMode.Neutral;
        tailController.tailHP = tailController.tailStartingHP;
        for (int i = 0; i < tailController.tailBits.Length; i++)
        {
            tailController.tailBits[i].Reset();
        }
    }
}
