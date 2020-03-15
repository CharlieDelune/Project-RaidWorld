using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewGridCell : MonoBehaviour, INodable
{
    public bool passable;
    public bool buildable;
    public int x, z, gCost, hCost, fCost;
    public Grid grid;
    public GridCell previousCell;
    public int nodeValue {get; set;}
    public int id { get; set; }

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
        this.x = (int)this.gameObject.transform.localPosition.x;
        this.z = (int)this.gameObject.transform.localPosition.z;
        neighbors = grid.GetNeighborsForCell(x, z);
        nodeValue = 0;
        id = GetInstanceID();
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
