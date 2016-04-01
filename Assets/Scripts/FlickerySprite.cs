using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlickerySprite : MonoBehaviour
{
    private Queue<SpriteRenderer> sprites;
    private SpriteRenderer sprite;
    private bool mated = false;

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
	    else if (sprites.Contains(sprite) == false)
        {
            sprites.Enqueue(sprite);
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
