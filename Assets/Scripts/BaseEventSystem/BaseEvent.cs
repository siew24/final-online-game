using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEvent : ScriptableObject
{
    public List<BaseEventListener> listeners;

    public void RegisterListener(BaseEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(BaseEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Raise()
    {
        foreach (BaseEventListener listener in listeners)
        {
            listener.OnEventRaised();
        }
    }
}

public class BaseEvent<T> : ScriptableObject
{
    public List<BaseEventListener<T>> listeners;

    public void RegisterListener(BaseEventListener<T> listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(BaseEventListener<T> listener)
    {
        listeners.Remove(listener);
    }

    public void Raise(T item)
    {
        foreach (BaseEventListener<T> listener in listeners)
        {
            listener.OnEventRaised(item);
        }
    }
}