using UnityEngine;
using System.Collections;

public enum RoomEventConditions
{
    None,
    TargetNumberOfEnemiesKilled
}

public class EventRemovedBlock : MonoBehaviour
{
    public RoomController room;
    public RoomEventConditions condition;
    public int TargetNumber;
    private bool DestroyThisFrame = false;
    private int counter;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        counter = 0;
        Debug.Log(room.Enemies.Length);
	    switch (condition)
        {
            case RoomEventConditions.TargetNumberOfEnemiesKilled:
                for (int i = 0; i < room.Enemies.Length; i++)
                {
                    Debug.Log(room.Enemies[i]);
                    if (room.Enemies[i] == null)
                    {

                        Debug.Log("");
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
            DestroySelf();
        }
	}

    void DestroySelf ()
    {
        Destroy(gameObject);
    }
}
