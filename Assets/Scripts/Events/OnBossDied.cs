using UnityEngine;

[CreateAssetMenu(fileName = "OnBossDied", menuName = "Events/OnBossDied", order = 0)]
public class OnBossDied : BaseEvent
{
    void OnEnable()
    {
        EventCode = Constants.Events.OnBossDied;
    }

    public void Raise()
    {
        Raise(true);
    }
}