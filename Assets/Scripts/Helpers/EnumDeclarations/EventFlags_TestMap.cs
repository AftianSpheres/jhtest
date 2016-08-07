using System;

[Flags]
public enum EventFlags_TestMap
{
    None = 0,
    Rm0x0KeyObtained = 1,
    Rm0x1WGObtained = 1 << 1,
    BossDead = 1 << 2,
    BossDropShadowObtained = 1 << 3,
    Rm0x1NorthDoorOpen = 1 << 4,
    ShittyDemoLadder = 1 << 5,
    FlamethrowerGotten = 1 << 6,
    CentiRingGet = 1 << 7
}