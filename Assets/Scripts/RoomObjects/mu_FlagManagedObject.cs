using UnityEngine;
using System.Collections;

public class mu_FlagManagedObject : MonoBehaviour
{
    public mufm_Generic Flag;
    public GameObject managedObject;
    public bool RunIfFlagTrue = false;
    	
	// Update is called once per frame
	void Update ()
    {
        if (managedObject != null)
        {
            if (Flag.CheckFlag() == RunIfFlagTrue)
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
