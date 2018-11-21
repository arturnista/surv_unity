using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager {

    public delegate void ListenerCallback();
    private static Dictionary<string, List<ListenerCallback>> events = new Dictionary<string, List<ListenerCallback>>();

    private static EventManager iInstance;
    public static EventManager main {
        get {
            if(iInstance == null) {
                iInstance = new EventManager();
            }

            return iInstance;
        }
    }

    private EventManager() {
        
    }

    public static void AddEventListener(string eventName, ListenerCallback callback) {
        if(!events.ContainsKey(eventName)) {
            events.Add(eventName, new List<ListenerCallback>());
        }

        events[eventName].Add(callback);
    }

    public static void RemoveEventListener(string eventName, ListenerCallback callback) {
        if(!events.ContainsKey(eventName)) {
            return;
        }

        events[eventName].Remove(callback);
    }

    public static void DispatchEvent(string eventName) {
        if(!events.ContainsKey(eventName)) {
            return;
        }
        
        foreach(ListenerCallback callback in events[eventName]) {
            callback();
        }
    }
    
}
