using UnityEngine;
using System.Collections.Generic;

public class EnemyBossManta_TailBit : MonoBehaviour
{
    public EnemyBossManta_Tail tailController;
    public int distanceFromBody;
    public Queue<Vector3> moveQueue;
    new public BoxCollider2D collider;
    new public SpriteRenderer renderer;
    public Vector3 originalPosition;
    Vector3 anchorPoint;
    Vector3 offset;
    Vector3 virtualPosition;


    void Start()
    {
        originalPosition = transform.position;
        Reset();
    }

    public void Reset ()
    {
        offset = Vector3.zero;
        moveQueue = new Queue<Vector3>(9);
        transform.position = originalPosition;
    }
	
	void Update ()
    {
        if (tailController.master.common.room.isActiveRoom == true)
        {
            Vector3 spacing;
            if (moveQueue.Count > 0)
            {
                offset += moveQueue.Dequeue();
            }
            switch (tailController.tailDirection)
            {
                case Direction.Down:
                    spacing = new Vector3(0, -8 * distanceFromBody, 0.01f * distanceFromBody);
                    break;
                case Direction.Up:
                    spacing = new Vector3(0, 8 * distanceFromBody, 0.01f * distanceFromBody);
                    break;
                case Direction.Left:
                    spacing = new Vector3(-8 * distanceFromBody, 0, 0.01f * distanceFromBody);
                    break;
                case Direction.Right:
                    spacing = new Vector3(8 * distanceFromBody, 0, 0.01f * distanceFromBody);
                    break;
                case Direction.DownLeft:
                    spacing = new Vector3(-4 * distanceFromBody, -4 * distanceFromBody, 0.01f * distanceFromBody);
                    break;
                case Direction.DownRight:
                    spacing = new Vector3(4 * distanceFromBody, -4 * distanceFromBody, 0.01f * distanceFromBody);
                    break;
                case Direction.UpLeft:
                    spacing = new Vector3(-4 * distanceFromBody, 4 * distanceFromBody, 0.01f * distanceFromBody);
                    break;
                case Direction.UpRight:
                    spacing = new Vector3(4 * distanceFromBody, 4 * distanceFromBody, 0.01f * distanceFromBody);
                    break;
                default:
                    throw new System.Exception("Invalid direction: " + tailController.tailDirection.ToString());
            }
            int i;
            for (i = 0; i < distanceFromBody; i++)
            {
                anchorPoint += tailController.anchorPoints[i];
            }
            anchorPoint /= i + 1;
            virtualPosition = anchorPoint + spacing + offset;
            if (tailController.mode == EnemyBossManta_TailMode.Neutral)
            {
                offset = new Vector3(offset.x * 0.75f, offset.y * 0.75f, offset.z);
            }
            transform.position = new Vector3(Mathf.Round(virtualPosition.x), Mathf.Round(virtualPosition.y), transform.position.z);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet") == true && tailController.mode != EnemyBossManta_TailMode.Dead)
        {
            tailController.Hit(other.GetComponent<BulletController>());
        }
        else if (other.CompareTag("Boom") == true && tailController.mode != EnemyBossManta_TailMode.Dead)
        {
            tailController.Hit(other.GetComponent<BoomEffect>());
        }
    }
}
