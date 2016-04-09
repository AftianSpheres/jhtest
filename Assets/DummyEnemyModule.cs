using UnityEngine;
using System.Collections;

/// <summary>
/// Enemy-specific behavior module for "enemies" that don't actually HAVE
/// any AI. 
/// </summary>
public class DummyEnemyModule : MonoBehaviour {
    public CommonEnemyController common;
	
	// Update is called once per frame
	void Update ()
    {
	    if (common.CurrentHP < 0)
        {
            common.Kill();
        }
	}
}
