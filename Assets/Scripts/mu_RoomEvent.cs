using UnityEngine;
using System.Collections;

public enum RoomEventConditions
{
    None,
    TargetNumberOfEnemiesKilled,
    DamageDealt
}

public class mu_RoomEvent : MonoBehaviour
{
    public RoomController room;
    public RoomEventConditions condition;
    public int TargetNumber;
    public bool EventActive;
    private int counter;


	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
	    switch (condition)
        {
            case RoomEventConditions.TargetNumberOfEnemiesKilled:
                counter = 0;
                for (int i = 0; i < room.Enemies.Length; i++)
                {
                    if (room.Enemies[i].GetComponent<CommonEnemyController>().isDead == true)
                    {
                        counter++;
                    }
                }
                if (counter >= TargetNumber)
                {
                    EventActive = true;
                }
                break;

            case RoomEventConditions.DamageDealt:
                if (counter >= TargetNumber)
                {
                    EventActive = true;
                }
                break;
        }
	}

    public void BulletStrike (BulletController bullet)
    {
        if (condition == RoomEventConditions.DamageDealt)
        {
            counter += bullet.Damage;
        }
    }

    public void Reset ()
    {
        counter = 0;
        EventActive = false;
    }

}
