using UnityEngine;
using System.Collections;

public enum RoomEventConditions
{
    None,
    TargetNumberOfEnemiesKilled
}

public class mu_RemovableBlock : MonoBehaviour
{
    public RoomController room;
    public RoomEventConditions condition;
    public int TargetNumber;
    new public BoxCollider collider;
    new public SpriteRenderer renderer;
    public RegisteredSprite register;
    private bool DestroyThisFrame = false;
    private int counter;


	// Use this for initialization
	void Start ()
    {
        register.roomObjectRespawnAction = Respawn;
	}
	
	// Update is called once per frame
	void Update ()
    {
        counter = 0;
	    switch (condition)
        {
            case RoomEventConditions.TargetNumberOfEnemiesKilled:
                for (int i = 0; i < room.Enemies.Length; i++)
                {
                    if (room.Enemies[i].GetComponent<CommonEnemyController>().isDead == true)
                    {
                        counter++;
                    }
                }
                if (counter >= TargetNumber)
                {
                    DestroyThisFrame = true;
                }
                break;
        }
        if (DestroyThisFrame == true)
        {
            Disappear();
            DestroyThisFrame = false;
        }
	}

    void Disappear ()
    {
        collider.enabled = false;
        renderer.enabled = false;
    }

    public void Respawn ()
    {
        collider.enabled = true;
        renderer.enabled = true;
    }
}
