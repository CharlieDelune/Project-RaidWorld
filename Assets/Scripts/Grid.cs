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
}
