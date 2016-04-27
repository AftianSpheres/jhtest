using UnityEngine;
using System.Collections;

public class mu_EventManagedObject : MonoBehaviour
{
    public mu_RoomEvent Event;
    public GameObject managedObject;
    public bool RunIfEventActive = false;
    	
	// Update is called once per frame
	void Update ()
    {
        if (managedObject != null)
        {
            if (Event.EventActive == RunIfEventActive)
            {
                managedObject.SetActive(true);
            }
            else
            {
                managedObject.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject); // if our managedObject has been destroyed there's no reason for this to continue to exist
        }
	}
}
