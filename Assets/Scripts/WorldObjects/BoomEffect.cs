using UnityEngine;
using System.Collections.Generic;

public class BoomEffect : MonoBehaviour
{
    public WorldController world;
    public GameObject owner;
    public Queue<BoomEffect> q;
    new public BoxCollider2D collider;
    new public SpriteRenderer renderer;
    public Sprite[] BoomSprites;
    public bool Collideable;
    public int Damage;
    public int Duration;
    public int PushbackStrength;
    private int Lifetime;
    private int SpriteProgression;
	
	// Update is called once per frame
	void Update ()
    {
        if (Lifetime >= Duration)
        {
            Retire();
        }
        else
        {
            if (Lifetime > (Duration / BoomSprites.Length) * SpriteProgression)
            {
                SpriteProgression++;
                renderer.sprite = BoomSprites[SpriteProgression - 1];
            }
            Lifetime++;
        }
	}

    public void Boom(bool collideable, int damage, int duration, int pushbackStrength, Sprite[] boomSprites)
    {
        Lifetime = 0;
        SpriteProgression = 1;
        Collideable = collideable;
        Damage = damage;
        Duration = duration;
        PushbackStrength = pushbackStrength;
        BoomSprites = boomSprites;
        renderer.sprite = boomSprites[0];
        collider.size = renderer.bounds.size;
    }

    public void Retire ()
    {
        q.Enqueue(this);
        gameObject.SetActive(false);
    }

}
