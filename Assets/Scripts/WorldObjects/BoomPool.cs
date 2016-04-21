using UnityEngine;
using System.Collections.Generic;

public enum BoomType
{
    SmokePuff,
    EnergyThingy
}

public class BoomPool : MonoBehaviour
{
    public WorldController world;
    public Queue<BoomEffect> q;
    public int MaximumAllowedBooms;
    public GameObject prefab;

    // Use this for initialization
    void Start ()
    {
        q = new Queue<BoomEffect>(MaximumAllowedBooms);
        for (int i = 0; i < MaximumAllowedBooms; i++)
        {
            GameObject boom = Instantiate(prefab);
            BoomEffect boomEffect = boom.GetComponent<BoomEffect>();
            boomEffect.world = world;
            q.Enqueue(boomEffect);
            boom.gameObject.SetActive(false);
            boom.gameObject.name = "Boom" + i;
            boom.transform.SetParent(transform);
        }
    }
	
    public void StartBoom (Vector3 center, BoomType type, bool collideable = false, int damage = 0, int duration = 20, int pushbackStrength = 0, GameObject owner = default(GameObject))
    {
        BoomEffect boomEffect = q.Dequeue();
        boomEffect.gameObject.SetActive(true);
        boomEffect.owner = owner;
        boomEffect.q = q;
        Sprite[] sprites;
        switch (type)
        {
            case BoomType.SmokePuff:
                sprites = GlobalStaticResources.Boom0;
                break;
            case BoomType.EnergyThingy:
                sprites = GlobalStaticResources.Boom1;
                break;
            default:
                throw new System.Exception("Invalid boom type: " + type);
        }
        boomEffect.Boom(collideable, damage, duration, pushbackStrength, sprites);
        boomEffect.transform.position = new Vector3(center.x - .5f * boomEffect.collider.bounds.extents.x, center.y + .5f * boomEffect.collider.bounds.extents.y, 0);
    }
}
