﻿using UnityEngine;
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
    public Queue<FlickerySprite> sprites;
    private AudioSource BGM0;
	// Use this for initialization
	void Start () {
        Application.targetFrameRate = 60;
        QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.Disable;
        QualitySettings.antiAliasing = 0;
        QualitySettings.vSyncCount = 0;
        sprites = new Queue<FlickerySprite>();

	}

    void LateUpdate ()
    {
        if (world == null)
        {
            GameObject worldobj = GameObject.Find("World");
            if (worldobj != null)
            {
                world = worldobj.GetComponent<WorldController>();
                BGM0 = world.BGM0;
            }
        }
        else
        {
            bool SpritesOK = false;
            for (int i = 0; i < SpritesAllowedOnScreen; i++)
            {
                if (sprites.Count == 0)
                {
                    SpritesOK = true; // no need to slow down this frame - we can render everything
                    break;
                }
                FlickerySprite sprite = sprites.Dequeue();
                if (sprite.skip == true)
                {
                    i -= 1;
                }
                else
                {
                    sprite.sprite.enabled = true;
                }
            }
            if (SpritesOK == false)
            {
                Application.targetFrameRate = 60 - Mathf.RoundToInt((60f / (sprites.Count)));
            }
            else
            {
                Application.targetFrameRate = 60;
            }
            BGM0.pitch = (float)Application.targetFrameRate / 60f;
        }
    }
	
}
