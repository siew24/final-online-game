using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using UnityEngine.Events;

public class UINotificationTrigger : MonoBehaviour
{
    [SerializeField] string text;
    [SerializeField] OnNotification onNotification;

    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            onNotification.Raise(text);
            Destroy(this);
        }
    }
}