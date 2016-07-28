using UnityEngine;
using System.Collections;

/// <summary>
/// Handles player energy, level, multiplier.
/// </summary>
[RequireComponent(typeof(PlayerController))]
public class PlayerEnergy : MonoBehaviour
{
    public PlayerController master;
    public bool isBerserk { get; private set; }
    public ushort Level { get; private set; }
    public int CurrentEnergy { get; private set; }
    public int EnergyBound { get; private set; }
    public static int LevelMax = 20;
    private static int EnergyPerLevel = 100;
    private bool energyMeterMovesLeft = false;
    private int FrameCtr;
    private int BerserkTime;



	// Use this for initialization
	void Start ()
    {
        EnergyBound = EnergyPerLevel * Level;
        CurrentEnergy = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        FrameCtr++;
        if (BerserkTime > 0)
        {
            BerserkTime--;
            if (BerserkTime == 0)
            {
                isBerserk = false;
            }
        }
        if (energyMeterMovesLeft == false)
        {
            if (CurrentEnergy >= EnergyBound)
            {
                if (master.animator.GetBool(PlayerAnimatorHashes.paramDead) == false)
                {
                    master.Die();
                }
            }
            else if (CurrentEnergy > 0)
            {
                if (FrameCtr == 30)
                {
                    CurrentEnergy--;
                    FrameCtr = 0;
                }
            }
            else if (CurrentEnergy < 0)
            {
                if (FrameCtr == 15)
                {
                    CurrentEnergy++;
                    FrameCtr = 0;
                }
            }
        }
        else if (energyMeterMovesLeft == true)
        {
            if (CurrentEnergy <= -EnergyBound)
            {
                if (master.animator.GetBool(PlayerAnimatorHashes.paramDead) == false)
                {
                    master.Die();
                }
            }
            else if (CurrentEnergy > 0)
            {
                if (FrameCtr == 15)
                {
                    CurrentEnergy--;
                    FrameCtr = 0;
                }
            }
            else if (CurrentEnergy < 0)
            {
                if (FrameCtr == 30)
                {
                    CurrentEnergy++;
                    FrameCtr = 0;
                }
            }
        }
	}

    public void Damage(int damage, bool damageButDontKill = false)
    {
        if (isBerserk == true)
        {
            damage *= 2;
        }
        if (energyMeterMovesLeft == false)
        {
            CurrentEnergy += damage;
            if (damageButDontKill == true && CurrentEnergy >= EnergyBound)
            {
                CurrentEnergy = EnergyBound - 1;
            }
        }
        else
        {
            CurrentEnergy -= damage;
            if (damageButDontKill == true && CurrentEnergy <= EnergyBound)
            {
                CurrentEnergy = -EnergyBound + 1;
            }
        }
    }

    public void Flip ()
    {
        isBerserk = true;
        // determine berserk time...
        energyMeterMovesLeft = !energyMeterMovesLeft;
    }

    /// <summary>
    /// Levels up.
    /// Doesn't exceed level cap.
    /// </summary>
    public void LevelUp()
    {
        if (Level <= LevelMax)
        {
            int old_ecap = EnergyBound;
            Level++;
            EnergyBound = EnergyBound + EnergyPerLevel;
            CurrentEnergy = 0;
        }
    }

    public void Reset()
    {
        CurrentEnergy = 0;
        isBerserk = false;
        BerserkTime = 0;
    }
}
