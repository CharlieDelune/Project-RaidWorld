using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Publisher
{
    List<Observer> observers = new List<Observer>();

    public void Notify(PublisherEvent ev)
    {
        foreach (Observer observer in observers)
        {
            observer.OnNotify(ev);
        }
    }

    public void AddObserver(Observer observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(Observer observer)
    {
        observers.Remove(observer);
    }
}

public enum PublisherEvent
{
    BuiltWall,
    MouseOverBuildableCell
}
