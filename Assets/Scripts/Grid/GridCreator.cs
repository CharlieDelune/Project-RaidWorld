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
    private int xSize, zSize;

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
}
