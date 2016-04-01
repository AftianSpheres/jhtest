using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// All-encompassing "make Unity do things it doesn't wanna do, at an engine level" manager.
/// If it's stylistically necessary but terrible Unity usage, this piece of shit does it.
/// It's the manager we deserve, but not the one we need right now.
/// </summary>
public class StylisticHacksManager : Manager<StylisticHacksManager>
{
    public WorldController world;
    public static uint SpritesAllowedOnScreen = 40;
    public Queue<SpriteRenderer> sprites;
    private AudioSource BGM0;
	// Use this for initialization
	void Start () {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        sprites = new Queue<SpriteRenderer>();
        world = GameObject.Find("Universe/World").GetComponent<WorldController>();
        BGM0 = world.BGM0;
	}

    void LateUpdate ()
    {
        bool SpritesOK = false;
        for (int i = 0; i < SpritesAllowedOnScreen; i++)
        {
            if (sprites.Count == 0)
            {
                SpritesOK = true; // no need to slow down this frame - we can render everything
                break;
            }
            SpriteRenderer sprite = sprites.Dequeue();
            sprite.enabled = true;
        }
        if (SpritesOK == false)
        {
            Application.targetFrameRate = 30;
        }
        else
        {
            Application.targetFrameRate = 60;
        }
        BGM0.pitch = (float)Application.targetFrameRate / 60f;
    }
	
}
