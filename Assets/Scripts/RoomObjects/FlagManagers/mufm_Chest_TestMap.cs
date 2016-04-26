public class mufm_Chest_TestMap : mufm_Generic
{
    public ChestFlags_TestMap flag;

    public override void ActivateFlag()
    {
        room.world.GameStateManager.chestFlags_TestMap |= flag;
    }

    public override bool CheckFlag()
    {
        return (room.world.GameStateManager.chestFlags_TestMap & flag) == flag;
    }

    public override void DeactivateFlag()
    {
        room.world.GameStateManager.chestFlags_TestMap &= ~flag;
    }

    public override void ToggleFlag()
    {
        room.world.GameStateManager.chestFlags_TestMap ^= flag;
    }
}
