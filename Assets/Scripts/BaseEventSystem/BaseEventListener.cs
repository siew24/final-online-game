
using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener : MonoBehaviour, IOnEventCallback
{
    public BaseEvent baseEvent;
    UnityEvent response;


    void Awake()
    {
        response = new();
    }

    void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        baseEvent.RegisterListener(this);
    }

    void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
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

    public void OnEvent(EventData photonEvent)
    {
        if (baseEvent.EventCode == null)
            return;

        if (photonEvent.Code == (byte)baseEvent.EventCode)
        {
            Debug.Log($"<color=cyan>Received event: {Enum.GetName(typeof(Constants.Events), baseEvent.EventCode)}</color> on <color=red>{gameObject.name}</color>");
            OnEventRaised();
        }
    }
}

public class BaseEventListener<T> : MonoBehaviour, IOnEventCallback
{
    public BaseEvent<T> baseEvent;
    UnityEvent<T> response;

    void Awake()
    {
        response = new();
    }

    void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        baseEvent.RegisterListener(this);
    }

    void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
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

    public void OnEvent(EventData photonEvent)
    {
        if (baseEvent.EventCode == null)
            return;

        if (photonEvent.Code == (byte)baseEvent.EventCode)
        {
            Debug.Log($"<color=cyan>Received event: {Enum.GetName(typeof(Constants.Events), baseEvent.EventCode)}</color> on <color=red>{gameObject.name}</color>");
            OnEventRaised((T)photonEvent.CustomData);
        }
    }
}