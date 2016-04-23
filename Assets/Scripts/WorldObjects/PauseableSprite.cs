using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Implements the ability to arbitrarily pause and unpause execution on all Behaviours attached to an object.
/// </summary>
public class PauseableSprite : MonoBehaviour
{
    private static Type[] UnpauseableBehaviours = { typeof(AudioSource), typeof(FlickerySprite) };
    private List<Behaviour> AttachedBehaviours;
    private List<Behaviour> PausedBehaviours;
    private bool Paused = false;

	/// <summary>
    /// MonoBehaviour.Awake()
    /// </summary>
	void Awake ()
    {
        Component[] components = gameObject.GetComponents(typeof(Behaviour));
        AttachedBehaviours = new List<Behaviour>(components.Length);
        PausedBehaviours = new List<Behaviour>(AttachedBehaviours.Count);
        for (int i = 0; i < components.Length; i++)
        {
            bool addComponent = true;
            for (int i2 = 0; i2 < UnpauseableBehaviours.Length; i2++)
            {
                if (components[i].GetType() == UnpauseableBehaviours[i2])
                {
                    addComponent = false;
                }
            }
            if (addComponent == true)
            {
                AttachedBehaviours.Add((Behaviour)components[i]);
            }
        }
        WorldController world = GameObject.Find("World").GetComponent<WorldController>();
        world.pauseableSprites.Add(this);
	}

    /// <summary>
    /// Pauses a PauseableSprite.
    /// </summary>
    public void Pause ()
    {
        if (Paused == false)
        {
            for (int i = 0; i < AttachedBehaviours.Count; i++)
            {
                if (AttachedBehaviours[i] != null && AttachedBehaviours[i].enabled == true)
                {
                    AttachedBehaviours[i].enabled = false;
                    PausedBehaviours.Add(AttachedBehaviours[i]);
                }
            }
            Paused = true;
        }
    }

    /// <summary>
    /// Unpauses a PauseableSprite.
    /// </summary>
    public void Unpause ()
    {
        if (Paused == true)
        {
            for (int i = 0; i < PausedBehaviours.Count; i++)
            {
                PausedBehaviours[i].enabled = true;
            }
            PausedBehaviours = new List<Behaviour>(AttachedBehaviours.Count);
            Paused = false;
        }
    }
}
