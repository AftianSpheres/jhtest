using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class PlayerEnergy : MonoBehaviour
{
    public PlayerController master;
    private AudioSource BGS0;
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
        BGS0 = master.world.BGS0;
        MaxEnergy = EnergyPerLevel * Level;
        CurrentEnergy = MaxEnergy;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

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

    public bool CheckIfCanFireWpn(int Percentage, bool apply_immediate = true)
    {
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

    public void Recover (int Percentage)
    {
        CurrentEnergy = CurrentEnergy + (Percentage * Level);
        if (CurrentEnergy > MaxEnergy)
        {
            CurrentEnergy = MaxEnergy;
        }
    }
}
