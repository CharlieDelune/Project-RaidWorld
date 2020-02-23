using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public bool active;
    [SerializeField]
    GameObject gridPrefab;
    [SerializeField]
    GameObject gridHolder;
    [SerializeField]
    public List<GridCell> gridCells;
    [SerializeField]
    private int xSize, zSize;
    // Start is called before the first frame update
    void Start()
    {
        if(active){
            for (int i = 0; i < xSize; i++)
            {
                for(int j = 0; j < zSize; j++)
                {
                    GameObject newCell = Instantiate(gridPrefab, new Vector3(i + 0.5f, 0, j + 0.5f), Quaternion.identity);
                    newCell.transform.SetParent(gridHolder.transform);
                    newCell.GetComponent<GridCell>().x = i;
                    newCell.GetComponent<GridCell>().z = j;
                }
            }
        }
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
            if(cell.IsPassable())
            {
                cell.SetColor(Color.white);
            }
        }
    }
}
