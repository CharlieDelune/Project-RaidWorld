using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour, Observer
{
    public float spawnTime;
    public List<GameObject> spawnWave;
    private float timeUntilSpawn;
    [SerializeField]
    private GameObject unitParent;
    private List<GridCell> path;
    [SerializeField]
    private BaseScript baseScript;
    private Grid gridCreator;

    private bool setUpObservers;
    
    void Start()
    {
        timeUntilSpawn = spawnTime;
        path = null;

        setUpObservers = false;

        gridCreator = GameObject.FindGameObjectsWithTag("GridHolder")[0].GetComponent<Grid>();
    }

    
    void Update()
    {
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
