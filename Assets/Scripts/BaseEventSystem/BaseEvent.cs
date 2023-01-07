using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class BaseEvent : ScriptableObject
{
    Constants.Events? eventCode;

    public List<BaseEventListener> listeners;

    public virtual Constants.Events? EventCode { get { return eventCode; } protected set { eventCode = value; } }

    public void RegisterListener(BaseEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(BaseEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Raise(bool networked = false)
    {
        if (networked)
        {
            RaiseEventOptions raiseEventOptions = new() { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent((byte)eventCode, null, raiseEventOptions, SendOptions.SendReliable);
            return;
        }

        Propagate();
    }

    void Propagate()
    {
        foreach (BaseEventListener listener in listeners)
        {
            listener.OnEventRaised();
        }
    }
}

public class BaseEvent<T> : ScriptableObject
{
    Constants.Events? eventCode;

    public List<BaseEventListener<T>> listeners;

    public virtual Constants.Events? EventCode { get { return eventCode; } protected set { eventCode = value; } }

    public void RegisterListener(BaseEventListener<T> listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(BaseEventListener<T> listener)
    {
        listeners.Remove(listener);
    }

    public void Raise(T item, bool networked = false)
    {
        if (networked)
        {
            RaiseEventOptions raiseEventOptions = new() { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent((byte)eventCode, item, raiseEventOptions, SendOptions.SendReliable);
            return;
        }

        Propagate(item);
    }

    void Propagate(T item)
    {
        foreach (BaseEventListener<T> listener in listeners)
        {
            listener.OnEventRaised(item);
        }
    }
}