using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public bool passable;
    public bool buildable;
    public int x, z, gCost, hCost, fCost;
    public GridCell previousCell;

    private Renderer rend;
    private Color color;

    private Publisher publisher;

    void Start()
    {
        publisher = new Publisher();
        rend =GetComponent<Renderer>();
        color = Color.white;
        passable = true;
        buildable = true;
        this.x = (int)this.gameObject.transform.localPosition.x;
        this.z = (int)this.gameObject.transform.localPosition.z;
    }

    void OnMouseEnter()
    {
        if (this.buildable)
        {
            rend.material.color = Color.green;
        }
    }

    void OnMouseExit()
    {
        rend.material.color = color;
    }

    void OnMouseDown()
    {
        if (this.buildable)
        {
            this.passable = !this.passable;
            this.color = this.passable ? Color.white : Color.black;
            rend.material.color = color;
            publisher.Notify(PublisherEvent.TestNotify);
        }
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void SetColor(Color inputColor)
    {
        if (rend != null)
        {
            color = inputColor;
            rend.material.color = color;
        }
    }

    public void ResetCell()
    {
        this.SetColor(Color.white);
        this.passable = true;
    }

    public void AddObserver(Observer observer)
    {
        publisher.AddObserver(observer);
    }

}
