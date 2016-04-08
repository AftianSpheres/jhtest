using UnityEngine;
using System.Collections;

public class MidBossHeart : MonoBehaviour
{
    public CommonEnemyController common;
    public bool Vulnerable = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet") == true && Vulnerable == true)
        {
            common.Hit(other.gameObject.GetComponent<BulletController>());
        }
    }
}
