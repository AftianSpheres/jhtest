using UnityEngine;
using System;
using System.Collections;

public class HUDEnergyBar : MonoBehaviour
{
    public WorldController world;
    private PlayerEnergy playerEnergy;
    public int EnergyBarLen;
    public SpriteRenderer EnergyBarFill;

	// Use this for initialization
	void Start ()
    {
        playerEnergy = world.player.energy;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float v = (float)Math.Round(EnergyBarLen * ((float)playerEnergy.CurrentEnergy / playerEnergy.EnergyBound), 0, MidpointRounding.AwayFromZero);
        if (v < 0)
        {
            v = 0;
        }
        EnergyBarFill.transform.localScale = new Vector3(v, EnergyBarFill.transform.localScale.y, EnergyBarFill.transform.localScale.z);
	}
}
