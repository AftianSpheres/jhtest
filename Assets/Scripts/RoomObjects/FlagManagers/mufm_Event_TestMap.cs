public class mufm_Event_TestMap : mufm_Generic
{
    public EventFlags_TestMap flag;

    public override void ActivateFlag()
    {
        room.world.GameStateManager.eventFlags_TestMap |= flag;
    }

    public override bool CheckFlag()
    {
        return (room.world.GameStateManager.eventFlags_TestMap & flag) == flag;
    }

    public override void DeactivateFlag()
    {
        room.world.GameStateManager.eventFlags_TestMap &= ~flag;
    }

    public override void ToggleFlag()
    {
        room.world.GameStateManager.eventFlags_TestMap ^= flag;
    }
}
