using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public Grid mainGrid;
    public Grid tempGrid;
    public Grid flightGrid;
    public Grid tempFlightGrid;
    public Base mainBase;

    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private GameObject turretPrefab;
    private GameObject[,] walls;
    private GameObject[,] turrets;
    private GameObject wallHolder;
    private GameObject weaponHolder;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        mainGrid = GameObject.FindGameObjectWithTag("MainGrid").GetComponent<Grid>();
        mainBase = GameObject.FindGameObjectWithTag("Base").GetComponent<Base>();
        wallHolder = GameObject.FindGameObjectWithTag("Walls");
        weaponHolder = GameObject.FindGameObjectWithTag("Weapons");
        walls = new GameObject[mainGrid.GetGridSize().x + 1, mainGrid.GetGridSize().z + 1];
        turrets = new GameObject[mainGrid.GetGridSize().x + 1, mainGrid.GetGridSize().z + 1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {}

    public void Victory()
    {}

    public void BuildWall(GridCell cell)
    {
        cell.passable = false;
        cell.wall = true;
        GameObject newWall = Instantiate(wallPrefab, new Vector3(cell.x, 0.5f, cell.z), Quaternion.identity);
        newWall.transform.SetParent(wallHolder.transform, true);
        walls[cell.x, cell.z] = newWall;
        cell.ResetCellColor();
        Publisher.Notify(PublisherEvent.BuiltWall);
    }

    public void DestroyWall(GridCell cell)
    {
        cell.passable = true;
        cell.wall = false;
        GameObject oldWall = walls[cell.x, cell.z];
        Destroy(oldWall);
        walls[cell.x, cell.z] = null;
        cell.ResetCellColor();
        Publisher.Notify(PublisherEvent.RemovedWall);
    }

    public void BuildTurret(GridCell cell)
    {
        cell.turret = true;
        GameObject newTurret = Instantiate(turretPrefab, new Vector3(cell.x, 1.5f, cell.z), Quaternion.identity);
        newTurret.transform.SetParent(weaponHolder.transform, true);
        turrets[cell.x, cell.z] = newTurret;
        Publisher.Notify(PublisherEvent.BuiltTurret);
    }

    public void DestroyTurret(GridCell cell)
    {
        cell.turret = false;
        GameObject oldTurret = turrets[cell.x, cell.z];
        Destroy(oldTurret);
        turrets[cell.x, cell.z] = null;
        Publisher.Notify(PublisherEvent.RemovedTurret);
    }
}
