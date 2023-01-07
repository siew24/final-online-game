using System.Collections.Generic;
using Constants;
using UnityEngine;
using UnityEngine.Events;

public class UIGoalTrigger : MonoBehaviour
{
    [SerializeField] Area trackingArea;
    [SerializeField] UnityEvent<(int, int)> onUIGoalTriggerEnter = new();
    [SerializeField] UnityEvent onCustomUIGoalTriggerEnter = new();

    Dictionary<int, string> allHints;

    void Start()
    {
        switch (trackingArea)
        {
            case Area.Area0:
                allHints = Constants.Area0.HINT;
                break;
            case Area.Area1:
                allHints = Constants.Area1.HINT;
                break;
            case Area.Area2:
                allHints = Constants.Area2.HINT;
                break;
            case Area.Area3:
                allHints = Constants.Area3.HINT;
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            onUIGoalTriggerEnter.Invoke((0, allHints.Count));
            onCustomUIGoalTriggerEnter.Invoke();
            Destroy(this);
        }
    }
}