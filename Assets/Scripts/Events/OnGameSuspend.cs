using UnityEngine;

[CreateAssetMenu(fileName = "OnGameSuspend", menuName = "Events/OnGameSuspend", order = 0)]
public class OnGameSuspend : BaseEvent<bool>
{
    void OnEnable()
    {
        EventCode = Constants.Events.OnGameSuspend;
    }

    public void Raise(bool item)
    {
        base.Raise(item, true);
    }
}