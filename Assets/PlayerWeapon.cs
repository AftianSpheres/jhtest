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

public enum WeaponState
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
        default(Action<PlayerWeapon, WeaponType>), // wg
        default(Action<PlayerWeapon, WeaponType>), // wg2
        default(Action<PlayerWeapon, WeaponType>), // shotgun
        default(Action<PlayerWeapon, WeaponType>), // shadow
        default(Action<PlayerWeapon, WeaponType>), // flamethrower
    };
    public static WeaponMobilityMode[] chargeMobility =
    {
        WeaponMobilityMode.None, // wg
        WeaponMobilityMode.None, // wg2
        WeaponMobilityMode.None, // shotgun
        WeaponMobilityMode.None, // shadow
        WeaponMobilityMode.None, // flamethrower
    };
    public static AudioClip[] chargeSFX;
    public static int[] chargeThresholds =
    {
        -1, // wg
        -1, // wg2
        -1, // shotgun
        -1, // shadow
        -1, // flamethrower
    };

    public static Action<PlayerWeapon, WeaponType>[] fireActions =
    {
        fire_STD, // wg
        fire_STD, // wg2
        fire_STD, // shotgun
        fire_STD, // shadow
        fire_STD, // flamethrower
    };
    public static WeaponMobilityMode[] fireMobility =
    {
        WeaponMobilityMode.HalfSpeedWalking, // wg
        WeaponMobilityMode.HalfSpeedWalking, // wg2
        WeaponMobilityMode.NoMobility, // shotgun
        WeaponMobilityMode.NoMobility, // shadow
        WeaponMobilityMode.HalfSpeedWalking, // flamethrower
    };
    public static AudioClip[] fireSFX;

    public static Action<PlayerWeapon, WeaponType>[] recoilActions =
    {
        default(Action<PlayerWeapon, WeaponType>), // wg
        default(Action<PlayerWeapon, WeaponType>), // wg2
        recoil_Shotgun, // shotgun
        default(Action<PlayerWeapon, WeaponType>), // shadow
        default(Action<PlayerWeapon, WeaponType>), // flamethrower
    };
    public static WeaponMobilityMode[] recoilMobility =
    {
        WeaponMobilityMode.None, // wg
        WeaponMobilityMode.None, // wg2
        WeaponMobilityMode.None, // shotgun
        WeaponMobilityMode.None, // shadow
        WeaponMobilityMode.None, // flamethrower
    };
    public static AudioClip[] recoilSFX;

    public static Action<PlayerWeapon, WeaponType>[] cooldownActions =
    {
        default(Action<PlayerWeapon, WeaponType>), // wg
        default(Action<PlayerWeapon, WeaponType>), // wg2
        default(Action<PlayerWeapon, WeaponType>), // shotgun
        default(Action<PlayerWeapon, WeaponType>), // shadow
        default(Action<PlayerWeapon, WeaponType>), // flamethrower
    };
    public static WeaponMobilityMode[] cooldownMobility =
    {
        WeaponMobilityMode.HalfSpeedWalking, // wg
        WeaponMobilityMode.HalfSpeedWalking, // wg2
        WeaponMobilityMode.HalfSpeedWalking, // shotgun
        WeaponMobilityMode.HalfSpeedWalking, // shadow
        WeaponMobilityMode.HalfSpeedWalking, // flamethrower
    };
    public static AudioClip[] cooldownSFX;
    public static int[] cooldownTimes =
    {
        15, // wg
        15, // wg2
        60, // shotgun
        60, // shadow
        15, // flamethrower
    };

    public static void LoadResources()
    {
        chargeSFX = new AudioClip[]
        {
            default(AudioClip), // wg
            default(AudioClip), // wg2
            default(AudioClip), // shotgun
            default(AudioClip), // shadow
            default(AudioClip), // flamethrower
        };
        fireSFX = new AudioClip[]
        {
            Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_FireBasicSFX), // wg
            Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_FireBasicSFX), // wg2
            Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_FireShotgunSFX), // shotgun
            Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_FireShadowSFX), // shadow
            Resources.Load<AudioClip>(GlobalStaticResourcePaths.p_FireFlameSFX), // flamethrower
        };
        recoilSFX = new AudioClip[]
        {
            default(AudioClip), // wg
            default(AudioClip), // wg2
            default(AudioClip), // shotgun
            default(AudioClip), // shadow
            default(AudioClip), // flamethrower
        };
        cooldownSFX = new AudioClip[]
        {
            default(AudioClip), // wg
            default(AudioClip), // wg2
            default(AudioClip), // shotgun
            default(AudioClip), // shadow
            default(AudioClip), // flamethrower
        };
        initialized = true;
    }

    public static void fire_STD (PlayerWeapon wpn, WeaponType wt)
    {
        wpn.manager.master.energy.Damage(PlayerWeaponManager.ShotEnergyCosts[(int)wt], true);
        wpn.cooldown += cooldownTimes[(int)wt];
        wpn.manager.FireBullet(wt);
        wpn.manager.master.source.PlayOneShot(fireSFX[(int)wt]);
        wpn.firingDone = true;
    }

    public static void recoil_Shotgun (PlayerWeapon wpn, WeaponType wt)
    {
        Vector3 PosMod;
        switch (wpn.manager.master. animator.GetInteger("FacingDir"))
        {
            case 0:
                PosMod = new Vector3(0, 2, 0);
                break;
            case 1:
                PosMod = new Vector3(0, -2, 0);
                break;
            case 2:
                PosMod = new Vector3(2, 0, 0);
                break;
            case 3:
                PosMod = new Vector3(-2, 0, 0);
                break;
            default:
                throw new System.Exception("FacingDir out of bounds!");
        }
        ExpensiveAccurateCollision.CollideWithScenery(wpn.manager.master.mover, wpn.manager.master.world.activeRoom.collision.allCollision, PosMod, wpn.manager.master.collider);
        wpn.recoilDone = true;
    }

}

public class PlayerWeapon : MonoBehaviour
{
    public PlayerWeaponManager manager;
    public bool isWpn2;
    public WeaponType wpnTypeBuffer;
    public WeaponState state;
    public int cooldown;
    public int charge;
    public bool firingDone = false;
    public bool recoilDone = false;
    private VirtualButton btn;
    private int FrameCtr;
    private Action<PlayerWeapon, WeaponType> chargeAction;
    private WeaponMobilityMode chargeMobility;
    private Action<PlayerWeapon, WeaponType> fireAction;
    private WeaponMobilityMode fireMobility;
    private Action<PlayerWeapon, WeaponType> recoilAction;
    private WeaponMobilityMode recoilMobility;
    private Action<PlayerWeapon, WeaponType> cooldownAction;
    private WeaponMobilityMode cooldownMobility;

    /// <summary>
    /// PlayerWeapon is (not yet...) slaved to the main PlayerController object.
    /// </summary>
    void Update ()
    {
        if (PlayerWeaponStatic.initialized == false) PlayerWeaponStatic.LoadResources();
        if (state == WeaponState.Ready) FrameCtr = 0;
        else if (btn!= null && (state != WeaponState.OnCooldown || btn.Pressed == true)) manager.lastFiredWeapon = this;
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
                else
                {
                    state = WeaponState.Firing;
                    goto case WeaponState.Firing; // charging state doesn't "exist," go to next state
                }
                if (charge < 0)
                {
                    charge = 0;
                    state = WeaponState.Firing;
                }
                break;
            case WeaponState.Firing:
                fireAction(this, wpnTypeBuffer);
                if (firingDone)
                {
                    state = WeaponState.Recoil;
                    firingDone = false;
                }
                break;
            case WeaponState.Recoil:
                if (recoilAction != null)
                {
                    recoilAction(this, wpnTypeBuffer);
                }
                else
                {
                    state = WeaponState.OnCooldown;
                    goto case WeaponState.OnCooldown;
                }
                if (recoilDone)
                {
                    state = WeaponState.OnCooldown;
                    recoilDone = false;
                }
                break;
            case WeaponState.OnCooldown:
                if (cooldownAction != null)
                {
                    cooldownAction(this, wpnTypeBuffer);
                }
                cooldown--;
                if (cooldown < 1)
                {
                    state = WeaponState.Ready;
                    charge = 0;
                    firingDone = false;
                    recoilDone = false;
                    cooldown = 0;
                    FrameCtr = 0;
                }
                break;
        }
    }

    public void ReceiveInput (VirtualButton _btn)
    {
        FrameCtr++;
        if (btn == null)
        {
            btn = _btn;
        }
        switch (state)
        {
            case WeaponState.Ready:
                if (btn.Pressed && wpnTypeBuffer != WeaponType.None)
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
            case WeaponState.OnCooldown:

                if (manager.master.animator.GetBool(PlayerAnimatorHashes.paramNowFiring) == true)
                {
                    if (btn.Pressed == false)
                    {
                        manager.master.animator.SetBool(PlayerAnimatorHashes.paramNowFiring, false);
                    }
                    else
                    {
                        MoveDuringFireAnim(cooldownMobility);
                    }
                }
                break;

        }
    }

    void MoveDuringFireAnim (WeaponMobilityMode mobility)
    {
        switch (mobility)
        {
            case WeaponMobilityMode.FullMobility:
                throw new NotImplementedException();
                //break;
            case WeaponMobilityMode.WalkingOnly:
                throw new NotImplementedException();
                //break;
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

    public void Flush ()
    {
        state = WeaponState.Ready;
        charge = 0;
        firingDone = false;
        recoilDone = false;
        cooldown = 0;
        FrameCtr = 0;
    }

    void ReloadActions ()
    {
        Flush();
        chargeAction = PlayerWeaponStatic.chargeActions[(int)wpnTypeBuffer];
        chargeMobility = PlayerWeaponStatic.chargeMobility[(int)wpnTypeBuffer];
        fireAction = PlayerWeaponStatic.fireActions[(int)wpnTypeBuffer];
        fireMobility = PlayerWeaponStatic.fireMobility[(int)wpnTypeBuffer];
        recoilAction = PlayerWeaponStatic.recoilActions[(int)wpnTypeBuffer];
        recoilMobility = PlayerWeaponStatic.recoilMobility[(int)wpnTypeBuffer];
        cooldownAction = PlayerWeaponStatic.cooldownActions[(int)wpnTypeBuffer];
        cooldownMobility = PlayerWeaponStatic.cooldownMobility[(int)wpnTypeBuffer];
    }


}
