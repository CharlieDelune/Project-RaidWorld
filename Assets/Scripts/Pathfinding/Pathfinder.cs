using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    public static List<Vector3> FindPath(Grid grid, Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        GridCell startCell = grid.GetGridCell((int)Mathf.Floor(startWorldPosition.x), (int)Mathf.Floor(startWorldPosition.z));
        GridCell endCell = grid.GetGridCell((int)Mathf.Floor(endWorldPosition.x), (int)Mathf.Floor(endWorldPosition.z));

        List<GridCell> path = FindPath(grid, startCell.x, startCell.z, endCell.x, endCell.z);
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

    public static List<Vector3> FindVectorPath(List<GridCell> path)
    {
        GridCell startCell = path[0];
        GridCell endCell = path[path.Count -1];

        List<Vector3> vectorPath = new List<Vector3>();
        foreach (GridCell cell in path)
        {
            vectorPath.Add(new Vector3(cell.x, startCell.transform.position.y, cell.z));
        }
        return vectorPath;
    }

    public static List<GridCell> FindPath(Grid grid, int startX, int startZ, int endX, int endZ)
    {
        PathNodeHolder<GridCell> startCell = new PathNodeHolder<GridCell>();
        PathNodeHolder<GridCell> endCell = new PathNodeHolder<GridCell>();

        BinaryTree<PathNodeHolder<GridCell>> openCells = new BinaryTree<PathNodeHolder<GridCell>>();
        HashSet<PathNodeHolder<GridCell>> closedCells = new HashSet<PathNodeHolder<GridCell>>();

        int gridSizeX = grid.GetGridSize().x;
        int gridSizeZ = grid.GetGridSize().z;

        PathNodeHolder<GridCell>[,] nodeGrid = new PathNodeHolder<GridCell>[gridSizeX + 1, gridSizeZ + 1];

        for (int x = 0; x <= gridSizeX; x++)
        {
            for (int z = 0; z <= gridSizeZ; z++)
            {
                GridCell cell = grid.GetGridCell(x, z);
                PathNodeHolder<GridCell> holder = new PathNodeHolder<GridCell>();
                holder.node = cell;
                holder.gCost = int.MaxValue;
                holder.hCost = cell.hCost;
                holder.CalculateFCost();
                holder.previousNodeHolder = null;
                holder.SetNeighbors(cell.GetNeighbors());
                holder.id = holder.node.GetInstanceID();
                if (cell.x == startX && cell.z == startZ)
                {
                    startCell = holder;
                }
                if (cell.x == endX && cell.z == endZ)
                {
                    endCell = holder;
                }
                nodeGrid[x,z] = holder;
            }
        }

        foreach(PathNodeHolder<GridCell> node in nodeGrid)
        {
            List<PathNodeHolder<GridCell>> neighbors = new List<PathNodeHolder<GridCell>>();

            if (node.node.x - 1 >= 0)
            {
                neighbors.Add(nodeGrid[node.node.x - 1, node.node.z]);
            }
            if (node.node.x + 1 <= gridSizeX)
            {
                neighbors.Add(nodeGrid[node.node.x + 1, node.node.z]);
            }
            if (node.node.z - 1 >= 0)
            {
                neighbors.Add(nodeGrid[node.node.x, node.node.z - 1]);
            }
            if (node.node.z + 1 <= gridSizeZ)
            {
                neighbors.Add(nodeGrid[node.node.x, node.node.z + 1]);
            }

            node.SetNeighbors(neighbors);
        }

        startCell.gCost = 0;
        startCell.hCost = CalculateDistance(startCell.node, endCell.node);
        startCell.CalculateFCost();

        openCells.Add(startCell);

        while(openCells.Count() > 0)
        {
            PathNodeHolder<GridCell> currentCell = GetLowestFCostCell(openCells);
            if(currentCell.node == endCell.node)
            {
                return CalculatePath(endCell);
            }

            openCells.Remove(currentCell);
            closedCells.Add(currentCell);

            List<PathNodeHolder<GridCell>> currentCellNeighbors = currentCell.GetNeighbors();

            foreach (PathNodeHolder<GridCell> neighbor in currentCellNeighbors)
            {
                if (closedCells.Contains(neighbor))
                {
                    continue;
                }
                if (!neighbor.node.passable)
                {
                    openCells.Remove(currentCell);
                    closedCells.Add(neighbor);
                    continue;
                }

                int tentativeGCost = CalculateDistance(currentCell.node, neighbor.node);
                if (tentativeGCost < neighbor.gCost)
                {
                    neighbor.previousNodeHolder = currentCell;
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = CalculateDistance(neighbor.node, endCell.node);
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

    public static void DrawPath(List<GridCell> path)
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

    public static void ResetPath(Grid grid)
    {
        grid.ResetGridColors();
    }

    private static  int CalculateDistance(GridCell a, GridCell b)
    {
        int moveCost = 10;
        float xDistance = Mathf.Abs(a.gameObject.transform.localPosition.x - b.gameObject.transform.localPosition.x);
        float zDistance = Mathf.Abs(a.gameObject.transform.localPosition.z - b.gameObject.transform.localPosition.z);
        float remaining = Mathf.Abs(xDistance - zDistance);
        return (int)((moveCost * Mathf.Min(xDistance, zDistance)) + (moveCost * remaining));
    }

    private static PathNodeHolder<GridCell> GetLowestFCostCell(BinaryTree<PathNodeHolder<GridCell>> gridCellList)
    {
        return gridCellList.FindLowestNode();
    }

    private static  List<GridCell> CalculatePath(PathNodeHolder<GridCell> endCell)
    {
        List<GridCell> instancePath = new List<GridCell>();
        instancePath.Add(endCell.node);
        PathNodeHolder<GridCell> currentCell = endCell;
        while (currentCell.previousNodeHolder != null)
        {
            instancePath.Add(currentCell.previousNodeHolder.node);
            currentCell = currentCell.previousNodeHolder;
        }
        instancePath.Reverse();
        
        return instancePath;
    }
}
