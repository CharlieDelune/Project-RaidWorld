using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public float spawnTime;
    public List<GameObject> spawnWave;
    private float timeUntilSpawn;
    
    void Start()
    {
        timeUntilSpawn = spawnTime;
    }

    
    void Update()
    {
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
            Instantiate(spawnWave[0]);
            spawnWave.RemoveAt(0);
        }
    }
}
