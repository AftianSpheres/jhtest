using UnityEngine;
using System;

public class HUDBossLifebar : MonoBehaviour
{
    public WorldController world;
    public SpriteRenderer bg;
    public SpriteRenderer fill;
    public TextMesh textMesh;
    float len;
    bool active;
    BossContainer boss;
    string[] lines;
    int HPCached = 0;

	// Use this for initialization
	void Awake ()
    {
        len = fill.transform.localScale.x;
        lines = Resources.Load<TextAsset>(GlobalStaticResources.p_boss_names).ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        active = false;
        bg.enabled = false;
        textMesh.gameObject.SetActive(false);
        fill.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
	    if (boss != world.activeRoom.Boss)
        {
            boss = world.activeRoom.Boss;
            textMesh.text = lines[(int)world.activeRoom.Boss.bossType - 1];
            bg.enabled = true;
            textMesh.gameObject.SetActive(true);
            fill.gameObject.SetActive(true);
            active = true;
            HPCached = 0;
        }
        else if (active == true)
        {
            if (world.activeRoom.Boss == null)
            {
                active = false;
                bg.enabled = false;
                textMesh.gameObject.SetActive(false);
                fill.gameObject.SetActive(false);
            }
            else if (HPCached != boss.bossController.CurrentHP)
            {
                HPCached = boss.bossController.CurrentHP;
                float v = (float)Math.Round(len * ((float)boss.bossController.CurrentHP / boss.bossController.MaxHP), 0, MidpointRounding.AwayFromZero);
                if (v < 0)
                {
                    v = 0;
                }
                fill.transform.localScale = new Vector3(v, fill.transform.localScale.y, fill.transform.localScale.z);
            }
        }
	}
}
