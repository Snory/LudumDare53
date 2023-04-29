using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralEventListener : MonoBehaviour
{
    public GeneralEvent Event;
    public UnityEvent<EventArgs> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised()
    {
        OnEventRaised(EventArgs.Empty);
    }

    public void OnEventRaised(EventArgs data)
    { Response?.Invoke(data); }
}
