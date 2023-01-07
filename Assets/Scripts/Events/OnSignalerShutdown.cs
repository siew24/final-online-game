using UnityEngine;

[CreateAssetMenu(fileName = "OnSignalerShutdown", menuName = "Events/OnSignalerShutdown", order = 0)]
public class OnSignalerShutdown : BaseEvent
{
    void OnEnable()
    {
        EventCode = Constants.Events.OnSignalerShutdown;
    }


    public void Raise()
    {
        Raise(true);
    }
}