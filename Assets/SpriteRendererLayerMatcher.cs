using UnityEngine;
using System.Collections;

public class SpriteRendererLayerMatcher : MonoBehaviour
{
    public SpriteRenderer master;
    new private SpriteRenderer renderer;

	// Use this for initialization
	void Start ()
    {
        renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        renderer.sortingLayerID = master.sortingLayerID;
	}
}
