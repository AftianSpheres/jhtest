using System;

[Flags]
public enum HeldWeapons
{
    None = 0,
    WG = 1,
    WGII = 1 << 1,
    Shotgun = 1 << 2,
    Shadow = 1 << 3,
    Flamethrower = 1 << 4,
    Icicle = 1 << 5,
    Rift = 1 << 6,
    Missile = 1 << 7,
    Laser = 1 << 8,
    Gatling = 1 << 9,
    EGrenade = 1 << 10,
    Sword = 1 << 11,
    IceRay = 1 << 12,
    DiscountWavebuster = 1 << 13,
    RocketHook = 1 << 14,
    FragPistol = 1 << 15,
    DarkBow = 1 << 16,
    LaserDrone = 1 << 17,
    PlasmaGatling = 1 << 18,
    BouncyIcePistol = 1 << 19,
    DroneSwarm = 1 << 20,
    MagnetTrawler = 1 << 21,
    SonicCannon = 1 << 22,
    PsychicRing = 1 << 23,
    PsychicRay = 1 << 24 
}