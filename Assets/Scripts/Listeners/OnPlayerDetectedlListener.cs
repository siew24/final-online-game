using UnityEngine;

public class OnPlayerDetectedListener : BaseEventListener 
{

    public BaseEvent baseEvent;
}


    /* The event to trigger when the AI reaches a waypoint
    public AIPatrol aiPatrol;
    void Start()
    {
        AIPatrol aiPatrol = GetComponent<AIPatrol>();
        aiPatrol.reachedWaypointEvent.AddListener(MoveToNextWaypoint);
    }

    public override void OnEventRaised()
    {
        MoveToNextWaypoint();
        Debug.Log("Example event was raised!");
    }
    public void MoveToNextWaypoint()
    {
        AIPatrol aiPatrol = GetComponent<AIPatrol>();
        aiPatrol.currentWaypoint = (aiPatrol.currentWaypoint + 1) % aiPatrol.waypoints.Length;
    }*/
//}
