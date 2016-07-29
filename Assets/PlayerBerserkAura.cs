using UnityEngine;
using System.Collections;

public class PlayerBerserkAura : MonoBehaviour
{
    public PlayerController master;
    public SpriteRenderer[] sprites;
    static Color gbLightGray = new Color(184/255f, 184/255f, 184/255f);
    static Color gbDarkGray = new Color(104/255f, 104/255f, 104/255f);
    int frameCtr;
	
	// Update is called once per frame
	void Update ()
    {
	    if (master.energy.isBerserk == false && sprites[0].enabled == true)
        {
            frameCtr = 0;
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].enabled = false;
            }
        }
        else if (master.energy.isBerserk == true)
        {
            if (sprites[0].enabled == false)
            {
                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].enabled = true;
                }
            }
            if (frameCtr > 3)
            {
                frameCtr = 0;
                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].sprite = master.renderer.sprite;
                    int cv = Random.Range(0, 8);
                    if (master.energy.energyMeterMovesLeft == true)
                    {
                        if (cv < 3)
                        {
                            sprites[i].material.SetColor("_FlashColor", Color.black);
                        }
                        else if (cv < 6)
                        {
                            sprites[i].material.SetColor("_FlashColor", gbDarkGray);
                            sprites[i].color = gbDarkGray;
                        }
                        else
                        {
                            sprites[i].material.SetColor("_FlashColor", gbLightGray);
                        }
                    }
                    else
                    {
                        if (cv < 3)
                        {
                            sprites[i].material.SetColor("_FlashColor", Color.white);
                        }
                        else if (cv < 6)
                        {
                            sprites[i].material.SetColor("_FlashColor", gbLightGray);
                            sprites[i].color = gbDarkGray;
                        }
                        else
                        {
                            sprites[i].material.SetColor("_FlashColor", gbDarkGray);
                        }
                    }
                }
            }
            else
            {
                frameCtr++;
            }
        }
	}
}
