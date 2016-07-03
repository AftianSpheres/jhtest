using System;

[Flags]
public enum HeldPassiveItems
{
    None = 0,
    MemFragment_DontUseThis = 1,
    WorldChangeToken = 1 << 1,
    EndgameKey = 1 << 2,
    DashThingy = 1 << 3,
    DodgeBooster = 1 << 4,
    DodgeAttack = 1 << 5,
    SecretSensor = 1 << 6,
    AutoscrollRider = 1 << 7,
    RedBull = 1 << 8,
    TabooRegenUpThingy = 1 << 9,
    ForestMonolithChunk = 1 << 10,
    ValleyMonolithChunk = 1 << 11,
    MarinaMonolithChunk = 1 << 12,
    DriftwoodTotem = 1 << 13
}