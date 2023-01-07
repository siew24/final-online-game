using UnityEngine;

[CreateAssetMenu(fileName = "OnGameEnd", menuName = "Events/OnGameEnd", order = 0)]
public class OnGameEnd : BaseEvent
{
    void OnEnable()
    {
        EventCode = Constants.Events.OnGameEnd;
    }

    public void Raise()
    {
        Raise(true);
    }
}