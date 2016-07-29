using UnityEngine;
using System;
using System.Collections;

public class HUDEnergyBar : MonoBehaviour
{
    public WorldController world;
    private PlayerEnergy playerEnergy;
    public int EnergyBarLen;
    public SpriteRenderer EnergyBarCursor;
    public Sprite f_CursorLeft;
    public Sprite f_CursorRight;
    public Sprite f_CursorLeft_Berserk;
    public Sprite f_CursorRight_Berserk;
    private Vector3 defaultCursorPos;

	// Use this for initialization
	void Awake ()
    {
        defaultCursorPos = EnergyBarCursor.transform.localPosition;
        playerEnergy = world.player.energy;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (playerEnergy.CurrentEnergy != 0)
        {
            EnergyBarCursor.transform.localPosition = defaultCursorPos + (Vector3.right * ((playerEnergy.CurrentEnergy / (float)playerEnergy.EnergyBound) * (EnergyBarLen / 2)));
        }
        else
        {
            EnergyBarCursor.transform.localPosition = defaultCursorPos;
        }
        if (playerEnergy.energyMeterMovesLeft == false)
        {
            if (playerEnergy.isBerserk == true)
            {
                EnergyBarCursor.sprite = f_CursorRight_Berserk;
            }
            else
            {
                EnergyBarCursor.sprite = f_CursorRight;
            }
        }
        else
        {
            if (playerEnergy.isBerserk == true)
            {
                EnergyBarCursor.sprite = f_CursorLeft_Berserk;
            }
            else
            {
                EnergyBarCursor.sprite = f_CursorLeft;
            }
        }
	}
}
