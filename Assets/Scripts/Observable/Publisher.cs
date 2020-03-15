using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Publisher
{
    private static List<Observer> observers = new List<Observer>();

    public static void Notify(PublisherEvent ev)
    {
        foreach (Observer observer in observers)
        {
            observer.OnNotify(ev);
        }
    }

    public static void AddObserver(Observer observer)
    {
        observers.Add(observer);
    }

    public static void RemoveObserver(Observer observer)
    {
        observers.Remove(observer);
    }
}

public enum PublisherEvent
{
    BuiltWall,
    BuiltTurret,
    RemovedWall,
    RemovedTurret,
    MouseOverBuildableCell,
    GameModeChanged
}
