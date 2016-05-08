using UnityEngine;
using System.Collections;

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
        Animator.StringToHash("ReadyToFire"),
        Animator.StringToHash("TurnClockwise"),
        Animator.StringToHash("TurnAntiClockwise"),
        Animator.StringToHash("Firing"),
        Animator.StringToHash("OpeningHatch"),
        Animator.StringToHash("ClosingHatch")
    };
    private int RemainingVolleys = -1;
    private Vector3 virtualPosition;

	// Use this for initialization
	void Start ()
    {
        virtualPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float playerDist = Mathf.Abs(common.collider.bounds.center.x - common.room.world.player.collider.bounds.center.x) + Mathf.Abs(common.collider.bounds.center.y - common.room.world.player.collider.bounds.center.y);
        bool playerOutOfLineOfSight = false;
        bool turnAntiClockwise = false;
        Vector3 move = Vector3.zero;
        if (common.animator.GetCurrentAnimatorStateInfo(0).tagHash == EnemyBossMantaAnimatorHashes[0] && (tailController.mode == EnemyBossManta_TailMode.Neutral || tailController.mode == EnemyBossManta_TailMode.Dead))
        {
            switch (facingDir)
            {
                case Direction.Down:
                    playerOutOfLineOfSight = (common.room.world.player.collider.bounds.center.y > common.collider.bounds.min.y || common.room.world.player.collider.bounds.center.x < common.collider.bounds.min.x - (playerDist / 4f) || common.room.world.player.collider.bounds.center.x > common.collider.bounds.max.x + (playerDist / 4f));
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
                    playerOutOfLineOfSight = (common.room.world.player.collider.bounds.center.y < common.collider.bounds.max.y || common.room.world.player.collider.bounds.center.x < common.collider.bounds.min.x - (playerDist / 4f) || common.room.world.player.collider.bounds.center.x > common.collider.bounds.max.x + (playerDist / 4f));
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
                    playerOutOfLineOfSight = (common.room.world.player.collider.bounds.center.x > common.collider.bounds.min.x || common.room.world.player.collider.bounds.center.y < common.collider.bounds.min.y - (playerDist / 4f) || common.room.world.player.collider.bounds.center.y > common.collider.bounds.max.y + (playerDist / 4f));
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
                    playerOutOfLineOfSight = (common.room.world.player.collider.bounds.center.x < common.collider.bounds.max.x || common.room.world.player.collider.bounds.center.y < common.collider.bounds.min.y - (playerDist / 4f) || common.room.world.player.collider.bounds.center.y > common.collider.bounds.max.y + (playerDist / 4f));
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
        if (armorOff == false)
        {
            if (common.animator.GetCurrentAnimatorStateInfo(0).tagHash == EnemyBossMantaAnimatorHashes[4] || common.animator.GetCurrentAnimatorStateInfo(0).tagHash == EnemyBossMantaAnimatorHashes[5])
            {
                RemainingVolleys--;
                if (playerOutOfLineOfSight == true || RemainingVolleys < 0)
                {
                    closeHatch();
                }
            }
            if (common.CurrentHP < common.CurrentHP / 2)
            {
                purgeArmor();
            }
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
            else if (facingDir == Direction.Down || facingDir == Direction.Up || facingDir == Direction.Left || facingDir == Direction.Right)
            {
                if (common.animator.GetCurrentAnimatorStateInfo(0).tagHash == EnemyBossMantaAnimatorHashes[0])
                {
                    virtualPosition += move;
                }
                if (playerDist > maximumAllowedDistance)
                {
                    if (tailController.mode == EnemyBossManta_TailMode.Neutral && Random.Range(0, 90) == 0)
                    {
                        StartCoroutine(tailController.TailStab(1 + (4 * ((playerDist - maximumAllowedDistance) / maximumAllowedDistance))));
                    }
                }
                else
                {
                    if (common.animator.GetCurrentAnimatorStateInfo(0).tagHash == EnemyBossMantaAnimatorHashes[0])
                    {
                        if (RemainingVolleys < 0 && Random.Range(0, 360) == 0)
                        {
                            openHatch();
                        }
                    }
                    if (tailController.mode == EnemyBossManta_TailMode.Neutral && Random.Range(0, 90) == 0)
                    {
                        StartCoroutine(tailController.TailSweep(1.5f));
                    }
                }
            }
        }
        else
        {
            if (common.CurrentHP < 1)
            {
                startSuicideAttack();
            }
        }
        Debug.Log(move);

        transform.position = ExpensiveAccurateCollision.ShoveOutOfScenery(common.collider, common.room.collision.allNonObjCollision, new Vector3(Mathf.Round(virtualPosition.x), Mathf.Round(virtualPosition.y), transform.position.z));
	}

    public void closeHatch ()
    {

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

    }

    public void fireSuicideBeam ()
    {

    }

    public void makeBodyVulnerable ()
    {

    }

    public void makeBodyInvulnerable ()
    {

    }

    public void openHatch ()
    {

    }

    public void purgeArmor ()
    {

    }

    public void startSuicideAttack ()
    {

    }
}
