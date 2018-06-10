using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void EventCallback(params object[] paramsContainer);

    private static Dictionary<string, EventCallback> _events;

    public static void AddEventListener(string eventName, EventCallback listener)
    {
        if (_events == null) _events = new Dictionary<string, EventCallback>();

        if (!_events.ContainsKey(eventName)) _events.Add(eventName, null);

        _events[eventName] += listener;
    }

    public static void RemoveEventListener(string eventName, EventCallback listener)
    {
        if (_events != null && _events.ContainsKey(eventName)) _events[eventName] -= listener;
    }

    public static void DispatchEvent(string eventName)
    {
        DispatchEvent(eventName, null);
    }

    public static void DispatchEvent(string eventName, params object[] paramsContainer)
    {
        if (_events != null && _events.ContainsKey(eventName) && _events[eventName] != null) _events[eventName](paramsContainer);
    }

    public static void ClearAllEvents()
    {
        _events = null;
    }
}
