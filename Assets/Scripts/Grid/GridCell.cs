using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour, INodable
{
    public bool passable;
    public bool buildable;
    public int x, z, gCost, hCost, fCost;
    public Grid grid;
    public int nodeValue {get; set;}
    public int id { get; set; }
    public bool wall;
    public bool turret;
    [SerializeField]
    private List<GridCell> neighbors;
    private Renderer rend;
    private Color color;
    private Globals globals;

    void Start()
    {
        globals = GameObject.FindGameObjectWithTag("Globals").GetComponent<Globals>();
        rend = GetComponent<Renderer>();
        color = Color.white;
        passable = true;
        buildable = true;
        x = (int)gameObject.transform.localPosition.x;
        z = (int)gameObject.transform.localPosition.z;
        neighbors = grid.GetNeighborsForCell(x, z);
        nodeValue = 0;
        id = GetInstanceID();
        wall = false;
        turret = false;
    }

    void OnMouseEnter()
    {
        if (globals.GetGameMode() == GameMode.BuildWall && buildable)
        {
            rend.material.color = Color.green;
            globals.PreviewPath(this);
        }
    }

    void OnMouseExit()
    {
        rend.material.color = color;
        globals.UnpreviewPath(this);
    }

    void OnMouseDown()
    {
        if (globals.GetGameMode() == GameMode.BuildWall && buildable)
        {
            if (passable)
            {
                globals.BuildWall(this);
            }
        }
        if (globals.GetGameMode() == GameMode.DestroyWall && wall)
        {
            if (!passable)
            {
                globals.DestroyWall(this);
            }
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (wall && !turret)
            {
                globals.BuildTurret(this);
            }
            else if (wall && turret)
            {
                globals.DestroyTurret(this);
            }
        }
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

    public void ResetCellColor()
    {
        this.SetColor(Color.white);
    }

    public void ResetCell()
    {
        this.SetColor(Color.white);
        this.passable = true;
    }

    public List<GridCell> GetNeighbors()
    {
        return neighbors;
    }

}
