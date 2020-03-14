using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public int health;

    private bool searchingForGrid;
    [SerializeField]
    private Globals globals;

    void Start()
    {
        searchingForGrid = true;
    }

    void Update()
    {
        if(searchingForGrid)
        {
            globals.PlaceBaseOrSpawner((int)transform.position.x, (int)transform.position.z);
            searchingForGrid = false;
        }
    }

    public void DealDamage(int damage)
    {
        health -= damage;
    }
}
