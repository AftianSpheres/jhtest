using System;

[Flags]
public enum EventFlags_Global
{
    None = 0,
    ForestBoss1Dead = 1,
    ForestBoss2Dead = 1 << 1,
    ValleyBoss1Dead = 1 << 2,
    ValleyBoss2Dead = 1 << 3,
    MarinaBoss1Dead = 1 << 4,
    MarinaBoss2Dead = 1 << 5,
    CircleBossDead =  1 << 6,
    MidgameWorldChanges = 1 << 7
}
