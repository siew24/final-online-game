using UnityEngine;

[CreateAssetMenu(fileName = "OnHintCollected", menuName = "Events/OnHintCollected", order = 0)]
public class OnHintCollected : BaseEvent<string>
{
    void OnEnable()
    {
        EventCode = Constants.Events.OnHintCollected;
    }


    public void Raise(string item)
    {
        base.Raise(item, true);
    }
}