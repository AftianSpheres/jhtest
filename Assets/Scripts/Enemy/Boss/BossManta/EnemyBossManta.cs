using UnityEngine;
using System.Collections;

public class EnemyBossManta : EnemyModule
{
    public Direction facingDir;
    public EnemyBossManta_Tail tailController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
}
