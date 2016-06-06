using UnityEngine;
using System.Collections;

public class ScrollingLayer : MonoBehaviour
{
    public Vector3 scrollVector;
    public bool scrollVertical;
    public int scrollInterval;
    private Vector3 originalTransform;
    public int loopDistance;
    public int scrollingSpeed;
    int i;
    private int ctr;
    private int res;

    void Start()
    {
        originalTransform = transform.position;
        if (scrollVertical == true)
        {
            res = HammerConstants.LogicalResolution_Vertical;
        }
        else
        {
            res = HammerConstants.LogicalResolution_Horizontal;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (ctr >= scrollInterval)
        {
            ctr = 0;
            i += scrollingSpeed;
            if (i >= loopDistance)
            {
                transform.position = originalTransform;
                i = 0;
            }
            else
            {
                transform.position += scrollVector * scrollingSpeed;
            }
        }
        else
        {
            ctr++;
        }
    }
}
