using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitScript : MonoBehaviour
{
    private Renderer rend;
    [SerializeField]
    private GameObject target;
    private GameObject gridHolder;
    private Pathfinder pathfinder;
    // Start is called before the first frame update
    void Start()
    {
        pathfinder = GetComponent<Pathfinder>();
        rend = GetComponent<Renderer>();
        rend.material.color = Color.red;
        target = GameObject.FindGameObjectsWithTag("Base")[0];
        gridHolder = GameObject.FindGameObjectsWithTag("GridHolder")[0];
    }

    void OnMouseDown()
    {
        List<GridCell> path = pathfinder.FindPath((int)this.gameObject.transform.parent.gameObject.transform.localPosition.x, (int)this.gameObject.transform.parent.gameObject.transform.localPosition.z,
            (int)target.gameObject.transform.localPosition.x, (int)target.gameObject.transform.localPosition.z);

        if (path != null){
            foreach (GridCell cell in path)
            {
                cell.SetColor(Color.blue);
            }
        }
    }
}
