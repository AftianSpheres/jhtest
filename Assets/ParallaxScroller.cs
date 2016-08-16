using UnityEngine;
using System.Collections;

public class ParallaxScroller : MonoBehaviour
{
    public Camera cam;
    public uint depth; // number of pixels the camera has to scroll to scroll us 1px in that direction
    public Rect rect;
    public bool lockHorizontal;
    public bool lockVertical;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        float coX = 0;
        float coY = 0;
        if (!lockHorizontal) coX =(cam.transform.position.x - rect.center.x) / depth;
        if (!lockVertical) coY = (cam.transform.position.y - rect.center.y) / depth;
        transform.position = new Vector3(rect.min.x + coX, rect.min.y + coY, transform.position.z);

        // subtract 

    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(rect.min.x, rect.min.y), new Vector3(rect.max.x, rect.min.y));
        Gizmos.DrawLine(new Vector3(rect.max.x, rect.min.y), new Vector3(rect.max.x, rect.max.y));
        Gizmos.DrawLine(new Vector3(rect.min.x, rect.min.y), new Vector3(rect.min.x, rect.max.y));
        Gizmos.DrawLine(new Vector3(rect.min.x, rect.max.y), new Vector3(rect.max.x, rect.max.y));
    }
#endif
}
