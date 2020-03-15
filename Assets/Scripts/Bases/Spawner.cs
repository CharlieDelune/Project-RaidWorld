using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnTime;
    public List<GameObject> spawnWave;
    [SerializeField]
    private Globals globals;
    private float timeUntilSpawn;
    [SerializeField]
    private GameObject unitParent;

    private bool searchingForGrid;
    
    void Start()
    {
        globals = GameObject.FindGameObjectWithTag("Globals").GetComponent<Globals>();
        timeUntilSpawn = spawnTime;
        searchingForGrid = true;
    }

    
    void Update()
    {
        if(searchingForGrid)
        {
            globals.PlaceBaseOrSpawner((int)transform.position.x, (int)transform.position.z);
            searchingForGrid = false;
        }
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn < 0)
        {
            SpawnWave();
            timeUntilSpawn = spawnTime;
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
}
