using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    public static List<GridCell> path;
    private static Grid grid = GameObject.FindGameObjectsWithTag("GridHolder")[0].GetComponent<Grid>();
    private static  List<GridCell> gridCells = grid.gridCells;
    private static  BinaryTree<GridCell> openCells;
    private static  List<GridCell> closedCells;

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        GridCell startCell = grid.GetGridCell((int)Mathf.Floor(startWorldPosition.x), (int)Mathf.Floor(startWorldPosition.z));
        GridCell endCell = grid.GetGridCell((int)Mathf.Floor(endWorldPosition.x), (int)Mathf.Floor(endWorldPosition.z));

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
        GridCell startCell = grid.GetGridCell(startX, startZ);
        GridCell endCell = grid.GetGridCell(endX, endZ);

        openCells = new BinaryTree<GridCell>();
        closedCells = new List<GridCell>();

        for (int x = 0; x <= grid.GetGridSize().x; x++)
        {
            for (int z = 0; z <= grid.GetGridSize().z; z++)
            {
                GridCell cell = grid.GetGridCell(x, z);
                cell.gCost = int.MaxValue;
                cell.CalculateFCost();
                cell.previousCell = null;
            }
        }

        startCell.gCost = 0;
        startCell.hCost = CalculateDistance(startCell, endCell);
        startCell.CalculateFCost();

        openCells.Add(startCell);

        while(openCells.Count() > 0)
        {
            GridCell currentCell = GetLowestFCostCell(openCells);
            if(currentCell == endCell)
            {
                return CalculatePath(endCell);
            }

            openCells.Remove(currentCell);
            closedCells.Add(currentCell);

            List<GridCell> currentCellNeighbors = currentCell.GetNeighbors();

            foreach (GridCell neighbor in currentCellNeighbors)
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

    public static void DrawPath()
    {
        if (path != null){
            foreach (GridCell cell in path)
            {
                cell.SetColor(Color.blue);
            }
        }
    }

    public static void DrawPath(List<GridCell> incomingPath, Color col)
    {
        if (incomingPath != null){
            foreach (GridCell cell in incomingPath)
            {
                cell.SetColor(col);
            }
        }
    }

    private static  int CalculateDistance(GridCell a, GridCell b)
    {
        float xDistance = Mathf.Abs(a.gameObject.transform.localPosition.x - b.gameObject.transform.localPosition.x);
        float zDistance = Mathf.Abs(a.gameObject.transform.localPosition.z - b.gameObject.transform.localPosition.z);
        float remaining = Mathf.Abs(xDistance - zDistance);
        return (int)(MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining);
    }

    private static GridCell GetLowestFCostCell(BinaryTree<GridCell> gridCellList)
    {
        return gridCellList.FindLowestNode();
    }

    private static  List<GridCell> CalculatePath(GridCell endCell)
    {
        List<GridCell> instancePath = new List<GridCell>();
        instancePath.Add(endCell);
        GridCell currentCell = endCell;
        while (currentCell.previousCell != null)
        {
            instancePath.Add(currentCell.previousCell);
            currentCell = currentCell.previousCell;
        }
        instancePath.Reverse();

        path = instancePath;
        
        return instancePath;
    }
}
