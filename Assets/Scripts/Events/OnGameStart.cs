using UnityEngine;

[CreateAssetMenu(fileName = "OnGameStart", menuName = "Events/OnGameStart", order = 0)]
public class OnGameStart : BaseEvent
{
    void OnEnable()
    {
        EventCode = Constants.Events.OnGameStart;
    }

    public void Raise()
    {
        Debug.Log($"Raising OnGameStart");
        Raise(true);
    }
}