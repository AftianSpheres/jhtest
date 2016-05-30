﻿using UnityEngine;
using System.Collections;

public class EnemyBossManta : EnemyModule
{
    public Direction facingDir;
    public EnemyBossManta_Tail tailController;
    public bool armorOff = false;
    public static int maximumAllowedDistance = 120;
    public float moveSpeed;
    public BoxCollider sceneryCollider;
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
        Animator.StringToHash("PurgeArmor"),
        Animator.StringToHash("FireMissile"),
        Animator.StringToHash("InCharge")
    };
    private int RemainingVolleys = -1;
    private int TurnTimer = 0;
    private Vector3 virtualPosition;
    private bool playerOutOfLineOfSight;
    private bool turnAntiClockwise;
    private Vector3 move;
    private float playerDist;
    private Vector3 playerPosBuffer;
    private bool MissilePrimed;

    // Use this for initialization
    void Start ()
    {
        virtualPosition = transform.position;
        playerPosBuffer = common.room.world.player.collider.bounds.center;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (common.room == common.room.world.activeRoom)
        {
            if (tailController.mode == EnemyBossManta_TailMode.Locked)
            {
                common.animator.speed = 0;
            }
            else
            {
                common.animator.speed = 1;
            }
            playerDist = Mathf.Abs(common.collider.bounds.center.x - common.room.world.player.collider.bounds.center.x) + Mathf.Abs(common.collider.bounds.center.y - common.room.world.player.collider.bounds.center.y);
            playerOutOfLineOfSight = false;
            turnAntiClockwise = false;

            // Welcome to the hackiest shit

            if (sceneryCollider.bounds.Intersects(common.room.world.player.collider.bounds) == true)
            {
                float x = 0;
                float y = 0;
                if (common.room.world.player.collider.bounds.min.y < sceneryCollider.bounds.max.y && common.room.world.player.collider.bounds.max.y > sceneryCollider.bounds.min.y)
                {
                    x = -(common.room.world.player.collider.bounds.max.x - sceneryCollider.bounds.min.x) - 1;
                    if (-(common.room.world.player.collider.bounds.min.x - sceneryCollider.bounds.max.x) + 1 < Mathf.Abs(x))
                    {
                        x = -(common.room.world.player.collider.bounds.min.x - sceneryCollider.bounds.max.x) + 1;
                    }
                }
                if (common.room.world.player.collider.bounds.min.x < sceneryCollider.bounds.max.x && common.room.world.player.collider.bounds.max.x > sceneryCollider.bounds.min.x)
                {
                    y = -(common.room.world.player.collider.bounds.max.y - sceneryCollider.bounds.min.y) - 1;
                    if (-(common.room.world.player.collider.bounds.min.y - sceneryCollider.bounds.max.y) + 1 < Mathf.Abs(y))
                    {
                        y = -(common.room.world.player.collider.bounds.min.y - sceneryCollider.bounds.max.y) + 1;
                    }
                }
                if (Mathf.Abs(x) > 0 && Mathf.Abs(y) > Mathf.Abs(x))
                {
                    y = 0;
                }
                else if (Mathf.Abs(y) > 0 && (Mathf.Abs(x) > Mathf.Abs(y)))
                {
                    x = 0;
                }
                common.room.world.player.MoveOrBeCrushed(new Vector3(x, y, 0), sceneryCollider.gameObject);
            }

            // free and clear of the hackiest shit

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
        if ((common.animator.GetCurrentAnimatorStateInfo(0).tagHash == EnemyBossMantaAnimatorHashes[0] || common.animator.GetCurrentAnimatorStateInfo(0).tagHash == EnemyBossMantaAnimatorHashes[4]) 
            && (tailController.mode == EnemyBossManta_TailMode.Neutral || tailController.mode == EnemyBossManta_TailMode.Dead))
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
        else if (common.animator.GetBool(EnemyBossMantaAnimatorHashes[11]) == true)
        {
            // get trajectory closest to player...
            float x = (common.room.world.player.collider.bounds.center.x - common.collider.bounds.center.x);
            float y = (common.room.world.player.collider.bounds.center.y - common.collider.bounds.center.y);
            float adj = Mathf.Abs(x) + Mathf.Abs(y);
            x /= adj;
            y /= adj;
            x *= moveSpeed * 2;
            y *= moveSpeed * 2;
            switch (facingDir)
            {
                case Direction.Down:
                    if (x > Mathf.Abs(y) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = true;
                    }
                    else if (x < -Mathf.Abs(y) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = false;
                    }
                    break;
                case Direction.Up:
                    if (x > Mathf.Abs(y) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = false;
                    }
                    else if (x < -Mathf.Abs(y) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = true;
                    }
                    break;
                case Direction.Left:
                    if (y > Mathf.Abs(x) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = false;
                    }
                    else if (y < -Mathf.Abs(x) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = true;
                    }
                    break;
                case Direction.Right:
                    if (y > Mathf.Abs(x) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = true;
                    }
                    else if (y < -Mathf.Abs(x) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = false;
                    }
                    break;
                case Direction.DownLeft:
                    if (y > Mathf.Abs(x) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = false;
                    }
                    else if (x < -Mathf.Abs(y) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = true;
                    }
                    break;
                case Direction.DownRight:
                    if (y > Mathf.Abs(x) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = true;
                    }
                    else if (x > Mathf.Abs(y) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = false;
                    }
                    break;
                case Direction.UpLeft:
                    if (y < -Mathf.Abs(x) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = true;
                    }
                    else if (x > Mathf.Abs(y) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = false;
                    }
                    break;
                case Direction.UpRight:
                    if (y < -Mathf.Abs(x) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = false;
                    }
                    else if (x < -Mathf.Abs(y) * 2)
                    {
                        playerOutOfLineOfSight = true;
                        turnAntiClockwise = true;
                    }
                    break;
            }
            move = new Vector3(x, y, 0);
        }
    }

    void _in_manageArmorState ()
    {
        if (common.CurrentHP < (common.MaxHP / 2f))
        {
            if (tailController.mode == EnemyBossManta_TailMode.Dead)
            {
                StartCoroutine(tailController.TailRevive());
            }
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
        if ((common.animator.GetCurrentAnimatorStateInfo(0).tagHash == EnemyBossMantaAnimatorHashes[0] && (tailController.mode == EnemyBossManta_TailMode.Neutral || tailController.mode == EnemyBossManta_TailMode.Dead)) ||
            (common.animator.GetBool(EnemyBossMantaAnimatorHashes[11]) == true))
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
                StartCoroutine(tailController.TailStab(1 + (2 * ((playerDist - maximumAllowedDistance) / maximumAllowedDistance))));
                move = Vector3.zero;
            }
        }
        else
        {
            if (common.animator.GetCurrentAnimatorStateInfo(0).tagHash == EnemyBossMantaAnimatorHashes[0])
            {
                if (MissilePrimed == true)
                {
                    fireMissile();
                }
                int chanceToOpenFire = 0;
                if (tailController.mode == EnemyBossManta_TailMode.Dead)
                {
                    chanceToOpenFire = 80;
                }
                else if (RemainingVolleys < 0 && Random.Range(0, 360) <= chanceToOpenFire)
                {
                    openHatch();
                    move = Vector3.zero;
                }
            }
            if (tailController.mode == EnemyBossManta_TailMode.Neutral && Random.Range(0, 90) == 0)
            {
                StartCoroutine(tailController.TailSweep(1f));
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
        if (playerOutOfLineOfSight == false)
        {
            Vector3[] origins = new Vector3[2];
            float horizVal = 0;
            float vertVal = 0;
            switch (facingDir)
            {
                case Direction.Down:
                    origins[0] = new Vector3(common.collider.bounds.center.x - 12, common.collider.bounds.center.y - 8, common.collider.bounds.center.z - 1);
                    origins[1] = new Vector3(common.collider.bounds.center.x + 12, common.collider.bounds.center.y - 8, common.collider.bounds.center.z - 1);
                    horizVal = 48;
                    break;
                case Direction.Up:
                    origins[0] = new Vector3(common.collider.bounds.center.x - 12, common.collider.bounds.center.y + 8, common.collider.bounds.center.z - 1);
                    origins[1] = new Vector3(common.collider.bounds.center.x + 12, common.collider.bounds.center.y + 8, common.collider.bounds.center.z - 1);
                    horizVal = 48;
                    break;
                case Direction.Left:
                    origins[0] = new Vector3(common.collider.bounds.center.x - 8, common.collider.bounds.center.y - 12, common.collider.bounds.center.z - 1);
                    origins[1] = new Vector3(common.collider.bounds.center.x - 8, common.collider.bounds.center.y + 12, common.collider.bounds.center.z - 1);
                    vertVal = 48;
                    break;
                case Direction.Right:
                    origins[0] = new Vector3(common.collider.bounds.center.x + 8, common.collider.bounds.center.y - 12, common.collider.bounds.center.z - 1);
                    origins[1] = new Vector3(common.collider.bounds.center.x + 8, common.collider.bounds.center.y + 12, common.collider.bounds.center.z - 1);
                    vertVal = 48;
                    break;
                default:
                    throw new System.Exception("Invalid direction for firing: " + facingDir.ToString());
            }

            Vector3 dest;
            for (int i = 0; i < 10; i++)
            {
                if (i < 2)
                {
                    dest = playerPosBuffer;
                }
                else if (i < 4)
                {
                    dest = new Vector3(playerPosBuffer.x - horizVal, playerPosBuffer.y - vertVal, playerPosBuffer.z);
                }
                else if (i < 6)
                {
                    dest = new Vector3(playerPosBuffer.x + horizVal, playerPosBuffer.y + vertVal, playerPosBuffer.z);
                }
                else if (i < 8)
                {
                    dest = new Vector3(playerPosBuffer.x - (2 * horizVal), playerPosBuffer.y - (2 * vertVal), playerPosBuffer.z);
                }
                else
                {
                    dest = new Vector3(playerPosBuffer.x + (2 * horizVal), playerPosBuffer.y + (2 * vertVal), playerPosBuffer.z);
                }
                common.room.world.EnemyBullets.FireBullet(WeaponType.eGeneric, 1.75f, common.ShotDmg, 3, dest, origins[i % 2], true);
            }
            RemainingVolleys--;
        }
        else
        {
            MissilePrimed = true;
            closeHatch();
        }
        common.source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_EnemyFireStrangeBurstSFX));
    }

    public void fireMissile()
    {
        Vector3 origin;
        switch (facingDir)
        {
            case Direction.Down:
                origin = new Vector3(common.collider.bounds.center.x, common.collider.bounds.center.y - 8, common.collider.bounds.center.z - 1);
                move += Vector3.up * 8;
                break;
            case Direction.Up:
                origin = new Vector3(common.collider.bounds.center.x, common.collider.bounds.center.y + 8, common.collider.bounds.center.z - 1);
                move += Vector3.down * 8;
                break;
            case Direction.Left:
                origin = new Vector3(common.collider.bounds.center.x - 8, common.collider.bounds.center.y, common.collider.bounds.center.z - 1);
                move += Vector3.right * 8;
                break;
            case Direction.Right:
                origin = new Vector3(common.collider.bounds.center.x + 8, common.collider.bounds.center.y, common.collider.bounds.center.z - 1);
                move += Vector3.left * 8;
                break;
            default:
                throw new System.Exception("Invalid direction for firing: " + facingDir.ToString());
        }
        Vector3 dest = common.room.world.player.collider.bounds.center;
        common.room.world.EnemyBullets.FireBullet(WeaponType.eGenericBomb, 5f, Mathf.RoundToInt(common.ShotDmg * 2.5f), 6, dest, origin, true, common.room.world.player.collider, 4, 128, gameObject);
        MissilePrimed = false;
        common.source.PlayOneShot(Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_FireShadowSFX));
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
        playerPosBuffer = common.room.world.player.collider.bounds.center;
    }

    public void purgeArmor ()
    {
        armorOff = true;
        moveSpeed *= 2;
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
