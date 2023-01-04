
using UnityEngine;

[CreateAssetMenu(fileName = "OnPlayerDetected", menuName = "Events/OnPlayerDetected", order = 0)]
public class OnPlayerDetected : BaseEvent 
{


}

//{
    //internal int currentWaypoint;
    //internal object reachedWaypointEvent;
//}

/*using UnityEngine.Events;

public class AIPatrol : BaseEvent
{
    public Transform[] waypoints;
    public float speed = 1.0f;
    public int currentWaypoint = 0;
    public UnityEvent reachedWaypointEvent;

    void Update()
    {
        // Move the AI character towards the current waypoint
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed * Time.deltaTime);

        // If the AI character has reached the waypoint, trigger the event
        if (transform.position == waypoints[currentWaypoint].position)
        {
            reachedWaypointEvent.Invoke();
        }
    }
}*/