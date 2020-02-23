using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private GridCreator gridCreator;
    private List<GridCell> gridCells;
    private List<GridCell> openCells;
    private List<GridCell> closedCells;

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    void Start()
    {
        gridCreator = GameObject.FindGameObjectsWithTag("GridHolder")[0].GetComponent<GridCreator>();
        gridCells = gridCreator.gridCells;
    }

    public List<GridCell> FindPath(int startX, int startZ, int endX, int endZ)
    {
        gridCreator.ResetGridColors();

        GridCell startCell = GetCell(startX, startZ);
        GridCell endCell = GetCell(endX, endZ);

        openCells = new List<GridCell>() { startCell };
        closedCells = new List<GridCell>();

        for (int x = 0; x < gridCreator.GetGridSize().x; x++)
        {
            for (int z = 0; z < gridCreator.GetGridSize().z; z++)
            {
                GridCell cell = GetCell(x, z);
                cell.gCost = int.MaxValue;
                cell.CalculateFCost();
                cell.previousCell = null;
            }
        }

        startCell.gCost = 0;
        startCell.hCost = CalculateDistance(startCell, endCell);
        startCell.CalculateFCost();

        while(openCells.Count > 0)
        {
            GridCell currentCell = GetLowestFCostCell(openCells);
            if(currentCell == endCell)
            {
                return CalculatePath(endCell);
            }

            openCells.Remove(currentCell);
            closedCells.Add(currentCell);

            foreach (GridCell neighbor in GetNeighborList(currentCell))
            {
                if (closedCells.Contains(neighbor))
                {
                    continue;
                }
                if (!neighbor.IsPassable())
                {
                    closedCells.Add(neighbor);
                    continue;
                }

                Debug.Log(closedCells);

                int tentativeGCost = currentCell.gCost + CalculateDistance(currentCell, neighbor);
                if (tentativeGCost < neighbor.gCost)
                {
                    neighbor.previousCell = currentCell;
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = CalculateDistance(neighbor, endCell);
                    neighbor.CalculateFCost();

                    if (!openCells.Contains(neighbor))
                    {
                        openCells.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private int CalculateDistance(GridCell a, GridCell b)
    {
        float xDistance = Mathf.Abs(a.gameObject.transform.localPosition.x - b.gameObject.transform.localPosition.x);
        float zDistance = Mathf.Abs(a.gameObject.transform.localPosition.z - b.gameObject.transform.localPosition.z);
        float remaining = Mathf.Abs(xDistance - zDistance);
        return (int)(MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining);
    }

    private GridCell GetLowestFCostCell(List<GridCell> gridCellList)
    {
        GridCell lowestFCostCell = gridCellList[0];
        for (int i = 1; i < gridCellList.Count; i++)
        {
            if (gridCellList[i].fCost < lowestFCostCell.fCost)
            {
                lowestFCostCell = gridCellList[i];
            }
        }
        return lowestFCostCell;
    }

    private List<GridCell> GetNeighborList(GridCell currentCell)
    {
        List<GridCell> neighborList = new List<GridCell>();
        if (currentCell.x - 1 >= 0)
        {
            neighborList.Add(GetCell(currentCell.x - 1, currentCell.z));
            if (currentCell.z - 1 >= 0)
            {
                neighborList.Add(GetCell(currentCell.x - 1, currentCell.z - 1));
            }
            if (currentCell.z + 1 <= gridCreator.GetGridSize().z)
            {
                neighborList.Add(GetCell(currentCell.x - 1, currentCell.z + 1));
            }
        }
        if (currentCell.x + 1 <= gridCreator.GetGridSize().x)
        {
            neighborList.Add(GetCell(currentCell.x + 1, currentCell.z));
            if (currentCell.z - 1 >= 0)
            {
                neighborList.Add(GetCell(currentCell.x + 1, currentCell.z - 1));
            }
            if (currentCell.z + 1 <= gridCreator.GetGridSize().z)
            {
                neighborList.Add(GetCell(currentCell.x + 1, currentCell.z + 1));
            }
        }
        if (currentCell.z - 1 >= 0)
        {
            neighborList.Add(GetCell(currentCell.x, currentCell.z - 1));
        }
        if (currentCell.z + 1 <= gridCreator.GetGridSize().z)
        {
            neighborList.Add(GetCell(currentCell.x, currentCell.z + 1));
        }

        foreach(GridCell cell in neighborList)
        {
            if(cell.IsPassable())
            {
                cell.SetColor(Color.yellow);
            }
        }

        return neighborList;
    }

    private GridCell GetCell(int x, int z)
    {
        return gridCreator.GetGridCell(x, z);
    }

    private List<GridCell> CalculatePath(GridCell endCell)
    {
        List<GridCell> path = new List<GridCell>();
        path.Add(endCell);
        GridCell currentCell = endCell;
        while (currentCell.previousCell != null)
        {
            path.Add(currentCell.previousCell);
            currentCell = currentCell.previousCell;
        }
        path.Reverse();
        return path;
    }
}
