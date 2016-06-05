using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    new public BoxCollider2D collider;
    public bool hurtEnemies;
    public bool hurtPlayer;
    public bool stunThings;
    public int Damage;

    public void Hurt(CommonEnemyController enemy)
    {
        if (hurtEnemies == true)
        {
            enemy.Hit(this);
        }
    }
}
