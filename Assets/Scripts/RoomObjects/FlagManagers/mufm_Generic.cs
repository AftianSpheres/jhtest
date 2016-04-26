using UnityEngine;

public abstract class mufm_Generic : MonoBehaviour
{
    public RoomController room;

    public abstract void ActivateFlag();
    public abstract bool CheckFlag();
    public abstract void DeactivateFlag();
    public abstract void ToggleFlag();
}
