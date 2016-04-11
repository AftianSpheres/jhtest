using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlickerySprite : MonoBehaviour
{
    private Queue<FlickerySprite> sprites;
    public SpriteRenderer sprite;
    private bool mated = false;
    public bool skip = false;

	// Use this for initialization
	void Start ()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        mate();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (mated == false) 
        {
            mate();
        }
	    else if (sprites.Contains(this) == false)
        {
            sprites.Enqueue(this);
            sprite.enabled = false;
        }
	}

    void mate ()
    {
        GameObject mgr = GameObject.Find("Universe/StylisticHacksManager");
        if (mgr != null)
        {
            sprites = mgr.GetComponent<StylisticHacksManager>().sprites;
            if (sprites != null)
            {
                mated = true;
            }
        }
        
    }

}
