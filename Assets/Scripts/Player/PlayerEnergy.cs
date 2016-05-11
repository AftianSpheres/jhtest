using UnityEngine;
using System.Collections;

/// <summary>
/// Handles player energy, level, multiplier.
/// </summary>
[RequireComponent(typeof(PlayerController))]
public class PlayerEnergy : MonoBehaviour
{
    public PlayerController master;
    public int Level;
    public int CurrentEnergy;
    public int MaxEnergy;
    public int CurrentMultiplierLevel;
    public static int LevelMax = 20;
    public static int EnergyPerLevel = 100;
    public static int MultiplierMax = 10;



	// Use this for initialization
	void Start ()
    {
        MaxEnergy = EnergyPerLevel * Level;
        CurrentEnergy = MaxEnergy;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (master.world.activeRoom != null && master.world.activeRoom.RoomCheckpoint != null && master.Locked == false)
        {
            Recover(1);
        }
	}

    /// <summary>
    /// Changes bonus multiplier. 
    /// Doesn't allow multiplier to exceed cap or
    /// go below 0.
    /// </summary>
    public void ChangeMultiplier(int stages)
    {
        CurrentMultiplierLevel += stages;
        if (CurrentMultiplierLevel > MultiplierMax)
        {
            CurrentMultiplierLevel = MultiplierMax;
        }
        else if (CurrentMultiplierLevel < 0)
        {
            CurrentMultiplierLevel = 0;
        }
    }

    /// <summary>
    /// Checks if player can fire an arbitrary weapon that
    /// used Percentage energy.
    /// if apply_immediate == true, then removes that much energy
    /// automatically.
    /// </summary>
    public bool CheckIfCanFireWpn(int Percentage, bool apply_immediate = true)
    {
        if (master.Invincible == true)
        {
            return true;
        }
        if (Percentage > 100 || Percentage < 0)
        {
            throw new System.Exception("Invalid weapon energy percentage!");
        }
        if (CurrentEnergy - (Percentage * Level) > 0)
        {
            if (apply_immediate == true)
            {
                CurrentEnergy = CurrentEnergy - (Percentage * Level);
            }
            return true;
        }
        return false;  
    }

    /// <summary>
    /// Levels up.
    /// Doesn't exceed level cap.
    /// </summary>
    public void LevelUp()
    {
        if (Level <= LevelMax)
        {
            int old_ecap = MaxEnergy;
            Level++;
            MaxEnergy = MaxEnergy + EnergyPerLevel;
            CurrentEnergy = CurrentEnergy * (MaxEnergy / old_ecap);
        }
    }

    /// <summary>
    /// Recovers specified percentage of HP.
    /// Doesn't exceed max energy.
    /// </summary>
    public void Recover (int Percentage)
    {
        CurrentEnergy = CurrentEnergy + (Percentage * Level);
        if (CurrentEnergy > MaxEnergy)
        {
            CurrentEnergy = MaxEnergy;
        }
    }
}
