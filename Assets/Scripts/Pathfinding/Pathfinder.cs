using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    private static GridCreator gridCreator = GameObject.FindGameObjectsWithTag("GridHolder")[0].GetComponent<GridCreator>();
    private static  List<GridCell> gridCells = gridCreator.gridCells;
    private static  List<GridCell> openCells;
    private static  List<GridCell> closedCells;

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        GridCell startCell = gridCreator.GetGridCell((int)Mathf.Floor(startWorldPosition.x), (int)Mathf.Floor(startWorldPosition.z));
        GridCell endCell = gridCreator.GetGridCell((int)Mathf.Floor(endWorldPosition.x), (int)Mathf.Floor(endWorldPosition.z));

        List<GridCell> path = FindPath(startCell.x, startCell.z, endCell.x, endCell.z);
        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (GridCell cell in path)
            {
                vectorPath.Add(new Vector3(cell.x, startWorldPosition.y, cell.z));
            }
            return vectorPath;
        }
    }

    public static List<GridCell> FindPath(int startX, int startZ, int endX, int endZ)
    {
        gridCreator.ResetGridColors();

        GridCell startCell = GetCell(startX, startZ);
        GridCell endCell = GetCell(endX, endZ);

        openCells = new List<GridCell>() { startCell };
        closedCells = new List<GridCell>();

        for (int x = 0; x <= gridCreator.GetGridSize().x; x++)
        {
            for (int z = 0; z <= gridCreator.GetGridSize().z; z++)
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
                if (!neighbor.passable)
                {
                    closedCells.Add(neighbor);
                    continue;
                }

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

    private static  int CalculateDistance(GridCell a, GridCell b)
    {
        float xDistance = Mathf.Abs(a.gameObject.transform.localPosition.x - b.gameObject.transform.localPosition.x);
        float zDistance = Mathf.Abs(a.gameObject.transform.localPosition.z - b.gameObject.transform.localPosition.z);
        float remaining = Mathf.Abs(xDistance - zDistance);
        return (int)(MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining);
    }

    private static  GridCell GetLowestFCostCell(List<GridCell> gridCellList)
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

    private static  List<GridCell> GetNeighborList(GridCell currentCell)
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

        return neighborList;
    }

    private static  GridCell GetCell(int x, int z)
    {
        return gridCreator.GetGridCell(x, z);
    }

    private static  List<GridCell> CalculatePath(GridCell endCell)
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

        if (path != null){
            foreach (GridCell cell in path)
            {
                cell.SetColor(Color.blue);
            }
        }
        
        return path;
    }
}
