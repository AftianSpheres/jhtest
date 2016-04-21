using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages a specialized "only bullets" object pool.
/// </summary>
public class BulletPool : MonoBehaviour
{
    public WorldController world;
    public Queue<BulletController> q;
    public int MaximumAllowedBullets;
    public GameObject prefab;
    public Sprite[] frames;
    public BoomPool boomPool;

    // Use this for initialization
    void Start ()
    {
        q = new Queue<BulletController>(MaximumAllowedBullets);
        for (int i = 0; i < MaximumAllowedBullets; i++)
        {
            GameObject bullet = Instantiate(prefab);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.world = world;
            bulletController.boomPool = boomPool;
            q.Enqueue(bulletController);
            bullet.gameObject.SetActive(false);
            bullet.gameObject.name = "[" + gameObject.name + "] Bullet " + i;
            bullet.transform.SetParent(transform);
        }
    }

    /// <summary>
    /// Fires a bullet from the pool.
    /// Takes a shitload of arguments, but luckily the names & types are intuitive enough.
    /// </summary>
    public void FireBullet(WeaponType shot, float speed, int damage, int weight, Vector3 to, Vector3 from, bool pierce = false)
    {
        if (world.activeRoom != null)
        {
            BulletController bulletController = q.Dequeue();
            bulletController.gameObject.SetActive(true);
            bulletController.Fire(shot, speed, damage, weight, from, to, this, pierce);
        }
    }
}
