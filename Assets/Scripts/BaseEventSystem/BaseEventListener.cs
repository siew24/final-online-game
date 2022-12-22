
using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener : MonoBehaviour
{
    [SerializeField] BaseEvent baseEvent;
    UnityEvent response;

    void Awake()
    {
        response = new();
    }

    void OnEnable()
    {
        baseEvent.RegisterListener(this);
    }

    void OnDisable()
    {
        baseEvent.UnregisterListener(this);
    }

    public void Register(UnityAction action)
    {
        response.AddListener(action);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }
}

public class BaseEventListener<T> : MonoBehaviour
{
    [SerializeField] BaseEvent<T> baseEvent;
    UnityEvent<T> response;

    void Awake()
    {
        response = new();
    }

    void OnEnable()
    {
        baseEvent.RegisterListener(this);
    }

    void OnDisable()
    {
        baseEvent.UnregisterListener(this);
    }

    public void Register(UnityAction<T> action)
    {
        response.AddListener(action);
    }

    public void OnEventRaised(T item)
    {
        response.Invoke(item);
    }
}