using UnityEngine;
using System.Collections;

public class BossContainer : MonoBehaviour
{
    public WorldController world;
    public BossType bossType;
    public CommonEnemyController bossController;
    public AudioClip bossBGM;
    public EventFlags_Global bossFlag;

	void Update ()
    {
        if ((world.GameStateManager.eventFlags_Global & bossFlag) == bossFlag)
        {
            Destroy(gameObject);
        }
        else if (bossController.isDead == true)
        {
            world.GameStateManager.eventFlags_Global |= bossFlag;
            world.ChangeBGM(world.activeRoom.bgm);
            Destroy(gameObject);
        }
    }
}
