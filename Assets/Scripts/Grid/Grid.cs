using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    GameObject gridPrefab;
    [SerializeField]
    GameObject gridHolder;
    [SerializeField]
    public List<GridCell> gridCells;
    [SerializeField]
    private int xSize, zSize;

    void Start()
    {

    }

    public (int x, int z) GetGridSize()
    {
        return (xSize - 1, zSize - 1);
    }

    public GridCell GetGridCell(int x, int z)
    {
        int start = x * xSize;
        return gridCells[start + z];
    }

    public void ResetGridColors()
    {
        foreach (GridCell cell in gridCells)
        {
            if(cell.passable)
            {
                cell.SetColor(Color.white);
            }
        }
    }

    public List<GridCell> GetNeighborsForCell(int x, int z)
    {
        List<GridCell> neighbors = new List<GridCell>();

        if (x - 1 >= 0)
        {
            neighbors.Add(GetGridCell(x - 1, z));
        }
        if (x + 1 <= xSize - 1)
        {
            neighbors.Add(GetGridCell(x + 1, z));
        }
        if (z - 1 >= 0)
        {
            neighbors.Add(GetGridCell(x, z - 1));
        }
        if (z + 1 <= zSize - 1)
        {
            neighbors.Add(GetGridCell(x, z + 1));
        }

        return neighbors;
    }
}
