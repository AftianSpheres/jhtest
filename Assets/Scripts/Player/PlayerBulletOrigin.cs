using UnityEngine;
using System.Collections;

public class PlayerBulletOrigin : MonoBehaviour
{
    public PlayerController master;
    private static Vector3[] offsets = { new Vector3(1, -5, 0), new Vector3(7, -1, 0), new Vector3(-1, -5, 0), new Vector3(7, -5, 0) };

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.localPosition = offsets[master.animator.GetInteger("FacingDir")];
	}
}
