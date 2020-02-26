using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour, INodable
{
    public bool passable;
    public bool buildable;
    public int x, z, gCost, hCost, fCost;
    public Grid grid;
    public GridCell previousCell;
    public int nodeValue {get; set;}
    public int id { get; set; }
    public GameObject turretPrefab;

    private List<GridCell> neighbors;
    private Renderer rend;
    private Color color;
    private Publisher publisher;
    private bool wall;
    private bool turret;

    void Start()
    {
        publisher = new Publisher();
        rend =GetComponent<Renderer>();
        color = Color.white;
        passable = true;
        buildable = true;
        this.x = (int)this.gameObject.transform.localPosition.x;
        this.z = (int)this.gameObject.transform.localPosition.z;
        neighbors = grid.GetNeighborsForCell(x, z);
        nodeValue = 0;
        id = GetInstanceID();
        wall = false;
        turret = false;
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

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (wall && !turret)
            {
                BuildTurret();
            }
        }
    }

    void BuildTurret()
    {
        turret = true;
        GameObject newTurret = Instantiate(turretPrefab);
        newTurret.transform.SetParent(this.transform, false);
    }

    void BuildWall()
    {
        wall = true;
        passable = false;
        color = Color.grey;
        this.transform.localScale  += new Vector3(0, 0.5f , 0);
        this.transform.localPosition  += new Vector3(0, 0.25f , 0);
    }

    void RemoveWall()
    {
        passable = true;
        wall = false;
        color = Color.white;
        this.transform.localScale  -= new Vector3(0, 0.5f , 0);
        this.transform.localPosition  -= new Vector3(0, 0.25f , 0);
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
        nodeValue = fCost;
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

    public List<GridCell> GetNeighbors()
    {
        return neighbors;
    }

}
