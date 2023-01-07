using UnityEngine;

[CreateAssetMenu(fileName = "OnNotification", menuName = "Events/OnNotification", order = 0)]
public class OnNotification : BaseEvent<string>
{
    void OnEnable()
    {
        EventCode = Constants.Events.OnNotification;
    }

    public void Raise(string item)
    {
        base.Raise(item, true);
    }
}