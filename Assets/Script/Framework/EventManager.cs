using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EventManager>();
                if (instance == null)
                {
                    instance = new GameObject("new EventManager Object").AddComponent<EventManager>().GetComponent<EventManager>();
                }
            }
            return instance;
        }
    }

    public delegate void EventCallback(params object[] paramsContainer);

    private Dictionary<string, EventCallback> _events;

    public void AddEventListener(string eventName, EventCallback listener)
    {
        if (_events == null) _events = new Dictionary<string, EventCallback>();

        if (!_events.ContainsKey(eventName)) _events.Add(eventName, null);

        _events[eventName] += listener;
    }

    public void RemoveEventListener(string eventName, EventCallback listener)
    {
        if (_events != null && _events.ContainsKey(eventName)) _events[eventName] -= listener;
    }

    public void DispatchEvent(string eventName)
    {
        DispatchEvent(eventName, null);
    }

    public void DispatchEvent(string eventName, params object[] paramsContainer)
    {
        if (_events != null && _events.ContainsKey(eventName) && _events[eventName] != null) _events[eventName](paramsContainer);
    }

    public void ClearAllEvents()
    {
        _events = null;
    }
}
