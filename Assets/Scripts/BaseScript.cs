﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour
{
    public int health;

    private bool searchingForGrid;

    void Start()
    {
        searchingForGrid = true;
    }

    void Update()
    {
        if (searchingForGrid)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down,out hit, 1))
            {
                if (hit.transform.gameObject.tag == "GridFloor"){
                    hit.transform.gameObject.GetComponent<GridCell>().buildable = false;
                    searchingForGrid = false;
                }
            }
            else{
                Debug.Log("Base is off the grid!");
                Destroy(gameObject);
            }
        }
    }

    public void DealDamage(int damage)
    {
        health -= damage;
    }
}
