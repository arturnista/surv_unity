using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourListenable : MonoBehaviour {

    public delegate void ListenerCallback();
    protected Dictionary<string, List<ListenerCallback>> events = new Dictionary<string, List<ListenerCallback>>();

    public void AddEventListener(string eventName, ListenerCallback callback) {
        if(!events.ContainsKey(eventName)) {
            events.Add(eventName, new List<ListenerCallback>());
        }

        events[eventName].Add(callback);
    }

    public void RemoveEventListener(string eventName, ListenerCallback callback) {
        if(!events.ContainsKey(eventName)) {
            return;
        }

        events[eventName].Remove(callback);
    }

    protected void DispatchEvent(string eventName) {
        if(!events.ContainsKey(eventName)) {
            return;
        }
        
        foreach(ListenerCallback callback in events[eventName]) {
            callback();
        }
    }
    
}
