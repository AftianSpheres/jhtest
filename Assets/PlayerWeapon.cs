using UnityEngine;
using System;

public enum WeaponMobilityMode
{
    None,
    FullMobility,
    WalkingOnly,
    HalfSpeedWalking,
    NoMobility
}

enum WeaponState
{
    Ready,
    Charging,
    Firing,
    Recoil,
    OnCooldown
}

static class PlayerWeaponStatic
{
    public static bool initialized = false;

    public static Action<PlayerWeapon, WeaponType>[] chargeActions =
    {
        default(Action<PlayerWeapon, WeaponType>)
    };
    public static WeaponMobilityMode[] chargeMobility =
    {
        WeaponMobilityMode.None
    };
    public static AudioClip[] chargeSFX;
    public static int[] chargeThresholds =
    {
        -1
    };

    public static Action<PlayerWeapon, WeaponType>[] fireActions =
    {
        fire_STD
    };
    public static WeaponMobilityMode[] fireMobility =
    {
        WeaponMobilityMode.HalfSpeedWalking
    };
    public static AudioClip[] fireSFX;

    public static Action<PlayerWeapon, WeaponType>[] recoilActions =
    {
        default(Action<PlayerWeapon, WeaponType>)
    };
    public static WeaponMobilityMode[] recoilMobility =
    {
        WeaponMobilityMode.None
    };
    public static AudioClip[] recoilSFX;

    public static Action<PlayerWeapon, WeaponType>[] cooldownActions =
    {
        default(Action<PlayerWeapon, WeaponType>)
    };
    public static AudioClip[] cooldownSFX;
    public static int[] cooldownTimes =
    {
        15
    };

    public static void LoadResources()
    {
        chargeSFX = new AudioClip[]
        {
            default(AudioClip)
        };
        fireSFX = new AudioClip[]
        {
            Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_FireBasicSFX)
        };
        recoilSFX = new AudioClip[]
        {
            default(AudioClip)
        };
        cooldownSFX = new AudioClip[]
        {
            default(AudioClip)
        };
        initialized = true;
    }

    public static void fire_STD (PlayerWeapon wpn, WeaponType wt)
    {
        wpn.manager.master.energy.Damage(PlayerWeaponManager.ShotEnergyCosts[(int)wt]);
        wpn.manager.FireBullet(wt);
        wpn.manager.master.source.PlayOneShot(chargeSFX[(int)wt]);
    }

}

public class PlayerWeapon : MonoBehaviour
{
    public PlayerWeaponManager manager;
    public bool isWpn2;
    public WeaponType wpnTypeBuffer;
    private WeaponState state;
    public int cooldown;
    public int charge;
    private int FrameCtr;
    private Action<PlayerWeapon, WeaponType> chargeAction;
    private WeaponMobilityMode chargeMobility;
    private Action<PlayerWeapon, WeaponType> fireAction;
    private WeaponMobilityMode fireMobility;
    private Action<PlayerWeapon, WeaponType> recoilAction;
    private WeaponMobilityMode recoilMobility;
    private Action<PlayerWeapon, WeaponType> cooldownAction;

    /// <summary>
    /// PlayerWeapon is (not yet...) slaved to the main PlayerController object.
    /// </summary>
    void Update ()
    {
        if (PlayerWeaponStatic.initialized == false) PlayerWeaponStatic.LoadResources();
        if (state == WeaponState.Ready) FrameCtr = 0;
        if (isWpn2 == false)
        {
            if (manager.SlotAWpn != wpnTypeBuffer)
            {
                wpnTypeBuffer = manager.SlotAWpn;
                ReloadActions();
            }
        }
        else
        {
            if (manager.SlotBWpn != wpnTypeBuffer)
            {
                wpnTypeBuffer = manager.SlotBWpn;
                ReloadActions();
            }
        }
        switch (state)
        {
            case WeaponState.Charging:
                if (chargeAction != null)
                {
                    chargeAction(this, wpnTypeBuffer);
                }
                if (charge < 0)
                {
                    charge = 0;
                    state = WeaponState.Firing;
                }
                break;
        }
    }

    public void ReceiveInput (VirtualButton btn)
    {
        FrameCtr++;
        switch (state)
        {
            case WeaponState.Ready:
                if (btn.BtnDown)
                {
                    manager.master.animator.SetBool(PlayerAnimatorHashes.paramNowFiring, true);
                    if (chargeAction != null)
                    {
                        state = WeaponState.Charging;
                    }
                    else
                    {
                        state = WeaponState.Firing;
                    }
                }
                break;
            case WeaponState.Charging:
                if (btn.Pressed == false)
                {
                    if (charge < PlayerWeaponStatic.chargeThresholds[(int)wpnTypeBuffer])
                    {
                        charge = 0;
                        state = WeaponState.Ready;
                    }
                    else
                    {
                        state = WeaponState.Firing;
                    }
                }
                MoveDuringFireAnim(chargeMobility);
                break;
            case WeaponState.Firing:
                MoveDuringFireAnim(fireMobility);
                break;
            case WeaponState.Recoil:
                MoveDuringFireAnim(recoilMobility);
                break;

        }
    }

    void MoveDuringFireAnim (WeaponMobilityMode mobility)
    {
        switch (mobility)
        {
            case WeaponMobilityMode.HalfSpeedWalking:
                Vector3 PosMod = new Vector3(0, 0, 0);
                if (FrameCtr % 2 == 0)
                {
                    if (HardwareInterfaceManager.Instance.Down.Pressed)
                    {
                        if (HardwareInterfaceManager.Instance.Left.Pressed)
                        {
                            if (FrameCtr % 4 == 0)
                            {
                                PosMod = new Vector3(-1, -1, 0);
                            }
                        }
                        else if (HardwareInterfaceManager.Instance.Right.Pressed)
                        {
                            if (FrameCtr % 4 == 0)
                            {
                                PosMod = new Vector3(1, -1, 0);
                            }
                        }
                        else
                        {
                            PosMod = new Vector3(0, -1, 0);
                        }
                    }
                    else if (HardwareInterfaceManager.Instance.Up.Pressed)
                    {
                        if (HardwareInterfaceManager.Instance.Left.Pressed)
                        {
                            if (FrameCtr % 4 == 0)
                            {
                                PosMod = new Vector3(-1, 1, 0);
                            }
                        }
                        else if (HardwareInterfaceManager.Instance.Right.Pressed)
                        {
                            if (FrameCtr % 4 == 0)
                            {
                                PosMod = new Vector3(1, 1, 0);
                            }
                        }
                        else
                        {
                            PosMod = new Vector3(0, 1, 0);
                        }
                    }
                    else if (HardwareInterfaceManager.Instance.Left.Pressed)
                    {
                        PosMod = new Vector3(-1, 0, 0);
                    }
                    else if (HardwareInterfaceManager.Instance.Right.Pressed)
                    {
                        PosMod = new Vector3(1, 0, 0);
                    }
                }
                PosMod *= (manager.master.animator.GetFloat(PlayerAnimatorHashes.paramMoveSpeed) * manager.master.animator.GetFloat(PlayerAnimatorHashes.paramInternalMoveSpeedMulti) * manager.master.animator.GetFloat(PlayerAnimatorHashes.paramExternalMoveSpeedMulti));
                ExpensiveAccurateCollision.CollideWithScenery(manager.master.mover, manager.master.world.activeRoom.collision.allCollision, PosMod, manager.master.collider);
                break;
        }
    }

    void ReloadActions ()
    {
        cooldown = 0;
        chargeAction = PlayerWeaponStatic.chargeActions[(int)wpnTypeBuffer];
        chargeMobility = PlayerWeaponStatic.chargeMobility[(int)wpnTypeBuffer];
        fireAction = PlayerWeaponStatic.fireActions[(int)wpnTypeBuffer];
        fireMobility = PlayerWeaponStatic.fireMobility[(int)wpnTypeBuffer];
        recoilAction = PlayerWeaponStatic.recoilActions[(int)wpnTypeBuffer];
        recoilMobility = PlayerWeaponStatic.recoilMobility[(int)wpnTypeBuffer];
        cooldownAction = PlayerWeaponStatic.cooldownActions[(int)wpnTypeBuffer];
    }


}
