using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public WorldController world;
    private Queue<BulletController> pool;
    public int MaximumAllowedBullets;
    public GameObject prefab;

    // Use this for initialization
    void Start ()
    {
        pool = new Queue<BulletController>(MaximumAllowedBullets);
        for (int i = 0; i < MaximumAllowedBullets; i++)
        {
            GameObject bullet = Instantiate(prefab);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.world = world;
            pool.Enqueue(bulletController);
            bullet.gameObject.SetActive(false);
            bullet.transform.SetParent(transform);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void FireBullet(PlayerWeapon shot, float speed, int damage, int weight, Vector3 to, Vector3 from, bool pierce = false)
    {
        BulletController bulletController = pool.Dequeue();
        bulletController.gameObject.SetActive(true);
        bulletController.Fire(shot, speed, damage, weight, from, to, pool, pierce);
    }
}
