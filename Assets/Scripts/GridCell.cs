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
            /*
            if( Input.GetMouseButton(0))
            {
                if(passable)
                {
                    BuildWall();
                }
                else
                {
                    RemoveWall();
                }
                rend.material.color = color;
                publisher.Notify(PublisherEvent.BuiltWall);
            }
            */
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
            if (passable)
            {
                BuildWall();
            }
            else
            {
                RemoveWall();
            }
            rend.material.color = color;
            publisher.Notify(PublisherEvent.BuiltWall);
        }
    }

    void BuildWall()
    {
        passable = false;
        color = Color.grey;
        this.transform.localScale  += new Vector3(0, 0.5f , 0);
        this.transform.localPosition  += new Vector3(0, 0.25f , 0);
    }

    void RemoveWall()
    {
        passable = true;
        color = Color.white;
        this.transform.localScale  -= new Vector3(0, 0.5f , 0);
        this.transform.localPosition  -= new Vector3(0, 0.25f , 0);
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
