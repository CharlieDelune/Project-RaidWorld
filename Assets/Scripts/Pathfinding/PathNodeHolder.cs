using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNodeHolder<T> : INodable
{
    public int gCost, hCost, fCost;
    public T node { get; set; }
    public PathNodeHolder<T> previousNodeHolder { get; set; }

    public int id { get; set; }
    public int nodeValue {get; set;}
    public List<PathNodeHolder<T>> neighbors { get; set; }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
        nodeValue = fCost;
    }

    public void SetNeighbors(List<T> toAdd)
    {
        neighbors = new List<PathNodeHolder<T>>();
        foreach(T neighbor in toAdd)
        {
            PathNodeHolder<T> newNode = new PathNodeHolder<T>(){
                node = neighbor,
                gCost = int.MaxValue,
                hCost = 0,
                previousNodeHolder = null
            };

            newNode.CalculateFCost();

            neighbors.Add(newNode);
        }
    }

    public void SetNeighbors(List<PathNodeHolder<T>> toAdd)
    {
        neighbors = toAdd;
    }

    public List<PathNodeHolder<T>> GetNeighbors()
    {
        return neighbors;
    }
}
