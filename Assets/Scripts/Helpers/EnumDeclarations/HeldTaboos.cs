using System;

[Flags]
public enum HeldTaboos
{
    None = 0,
    Eyes = 1,
    Fire = 1 << 1,
    Life = 1 << 2,
    Wind = 1 << 3,
    Water = 1 << 4,
    Machine = 1 << 5,
    War = 1 << 6,
    Death = 1 << 7,
    Ice = 1 << 8,
    Time = 1 << 9
}
