using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, Observer
{
    public float spawnTime;
    public List<GameObject> spawnWave;
    private float timeUntilSpawn;
    [SerializeField]
    private GameObject unitParent;
    private List<GridCell> path;
    [SerializeField]
    private Base baseScript;
    private Grid gridCreator;

    private bool setUpObservers;
    private bool searchingForGrid;
    
    void Start()
    {
        timeUntilSpawn = spawnTime;
        path = null;

        setUpObservers = false;
        searchingForGrid = true;

        gridCreator = GameObject.FindGameObjectsWithTag("GridHolder")[0].GetComponent<Grid>();
    }

    
    void Update()
    {
        if (searchingForGrid)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down,out hit, 1))
            {
                if (hit.transform.gameObject.tag == "GridFloor"){
                    GridCell cell = hit.transform.gameObject.GetComponent<GridCell>();
                    cell.buildable = false;
                    foreach(GridCell neighbor in cell.GetNeighbors())
                    {
                        neighbor.buildable = false;
                    }
                    searchingForGrid = false;
                }
            }
            else{
                Debug.Log("Spawner is off the grid!");
                Destroy(gameObject);
            }
        }
        if (!setUpObservers)
        {
            SetUpObservers();
        }
        if (path == null)
        {
            path = Pathfinder.FindPath((int)transform.position.x, (int)transform.position.z, (int)baseScript.transform.position.x, (int)baseScript.transform.position.z);
            Pathfinder.DrawPath(path, Color.blue);
        }

        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn < 0)
        {
            SpawnWave();
            timeUntilSpawn = spawnTime;
        }
    }

    public void OnNotify(PublisherEvent ev)
    {
        switch(ev)
        {
            case PublisherEvent.BuiltWall:
                path = Pathfinder.FindPath((int)transform.position.x, (int)transform.position.z, (int)baseScript.transform.position.x, (int)baseScript.transform.position.z);
                gridCreator.ResetGridColors();
                Pathfinder.DrawPath(path, Color.blue);
                break;
            default:
                break;
        }
    }

    private void SpawnWave()
    {
        if(spawnWave.Count > 0)
        {
            GameObject enemy = Instantiate(spawnWave[0], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            enemy.transform.SetParent(unitParent.transform);
            spawnWave.RemoveAt(0);
        }
    }

    private void SetUpObservers()
    {
        foreach(GridCell cell in gridCreator.gridCells)
        {
            cell.AddObserver(this);
        }
        setUpObservers = true;
    }
}
