using UnityEngine;
using System.Collections;

public class TitleBG : MonoBehaviour
{
    private Vector3 originalTransform;
    public int loopDistance;
    public int scrollingSpeed;
    int i;
    private bool offFrame;
	
    void Start()
    {
        originalTransform = transform.position;
    }

	// Update is called once per frame
	void Update ()
    {
        if (offFrame == false)
        {
            offFrame = true;
            i += scrollingSpeed;
            if (i >= loopDistance)
            {
                transform.position = originalTransform + (HammerConstants.LogicalResolution_Horizontal * Vector3.left);
                i = HammerConstants.LogicalResolution_Horizontal;
            }
            else
            {
                transform.position += Vector3.left * scrollingSpeed;
            }
        }
        else
        {
            offFrame = false;
        }
    }
}
