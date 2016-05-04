using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages a specialized "only bullets" object pool.
/// </summary>
public class BulletPool : MonoBehaviour
{
    public WorldController world;
    public List<BulletController> allBullets;
    public Queue<BulletController> q;
    public int MaximumAllowedBullets;
    public GameObject prefab;
    public Sprite[] frames;
    public BoomPool boomPool;

    // Use this for initialization
    void Start ()
    {
        q = new Queue<BulletController>(MaximumAllowedBullets);
        allBullets = new List<BulletController>(MaximumAllowedBullets);
        for (int i = 0; i < MaximumAllowedBullets; i++)
        {
            GameObject bullet = Instantiate(prefab);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.OriginalTag = bullet.tag;
            bulletController.world = world;
            bulletController.fs.world = world;
            bulletController.boomPool = boomPool;
            q.Enqueue(bulletController);
            allBullets.Add(bulletController);
            bullet.gameObject.SetActive(false);
            bullet.gameObject.name = "[" + gameObject.name + "] Bullet " + i;
            bullet.transform.SetParent(transform);
        }
    }

    /// <summary>
    /// Fires a bullet from the pool.
    /// Takes a shitload of arguments, but luckily the names & types are intuitive enough.
    /// </summary>
    public void FireBullet(WeaponType shot, float speed, int damage, int weight, Vector3 to, Vector3 from, bool pierce = false, BoxCollider2D homingTarget = default(BoxCollider2D), int homingPrecision = 0, int homingWindow = int.MaxValue)
    {
        if (world.activeRoom != null)
        {
            BulletController bulletController = q.Dequeue();
            bulletController.fs.room = world.activeRoom;
            bulletController.gameObject.SetActive(true);
            bulletController.Fire(shot, speed, damage, weight, from, to, this, pierce, homingTarget, homingPrecision, homingWindow);
        }
    }
}
