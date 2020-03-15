using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public Grid mainGrid;
    public Grid previewGrid;
    public Base mainBase;
    public Spawner spawner;
    public GameObject mainPathHolder;
    public GameObject previewPathHolder;

    private GameMode gameMode;
    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private GameObject turretPrefab;
    private GameObject[,] walls;
    private GameObject[,] turrets;
    [SerializeField]
    private GameObject wallHolder;
    [SerializeField]
    private GameObject weaponHolder;
    [SerializeField]
    private Material pathMaterial;

    public int bits { get; set; }

    private bool pathDrawn;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        pathDrawn = false;
        walls = new GameObject[mainGrid.GetGridSize().x + 1, mainGrid.GetGridSize().z + 1];
        turrets = new GameObject[mainGrid.GetGridSize().x + 1, mainGrid.GetGridSize().z + 1];
        gameMode = GameMode.None;
        bits = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Globals was trying to find and draw the path before all of the cells had records of
        // their neigbbors. We initiate it here instead to give the cells some time.
        if(!pathDrawn)
        {
            pathDrawn = true;
            DrawPath(FindVectorPath(spawner.transform.position, mainBase.transform.position));
        }
        
    }

    public void GameOver()
    {}

    public void Victory()
    {}

    public void SetGameMode(GameMode newMode){
        gameMode = newMode;
        LineRenderer line = previewPathHolder.GetComponent<LineRenderer>();
        line.positionCount = 0;
        Publisher.Notify(PublisherEvent.GameModeChanged);
    }

    public GameMode GetGameMode(){
        return gameMode;
    }

    public void BuildWall(GridCell cell)
    {
        cell.passable = false;
        cell.wall = true;
        GameObject newWall = Instantiate(wallPrefab, new Vector3(cell.x, 0.5f, cell.z), Quaternion.identity);
        newWall.transform.SetParent(wallHolder.transform, true);
        walls[cell.x, cell.z] = newWall;
        DrawPath(FindVectorPath(spawner.transform.position, mainBase.transform.position));
        GridCell previewCell = previewGrid.GetGridCell(cell.x, cell.z);
        previewCell.passable = false;
        previewCell.wall = true;
        Publisher.Notify(PublisherEvent.BuiltWall);
    }

    public void DestroyWall(GridCell cell)
    {
        cell.passable = true;
        cell.wall = false;
        GameObject oldWall = walls[cell.x, cell.z]; 
        Destroy(oldWall);
        walls[cell.x, cell.z] = null;
        DrawPath(FindVectorPath(spawner.transform.position, mainBase.transform.position));
        GridCell previewCell = previewGrid.GetGridCell(cell.x, cell.z);
        previewCell.passable = true;
        previewCell.wall = false;
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

    public List<Vector3> FindVectorPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        return Pathfinder.FindPath(mainGrid, startWorldPosition, endWorldPosition);
    }

    public void DrawPath(List<Vector3> incomingPath)
    {
        if (incomingPath != null){
            LineRenderer line = mainPathHolder.GetComponent<LineRenderer>();
            line.positionCount = 0;
            line.positionCount = incomingPath.Count;
            for (int i = 0; i < incomingPath.Count; i++)
            {
                line.material = pathMaterial;
                line.startWidth = 0.1f;
                line.endWidth = 0.1f;
                line.startColor = Color.red;
                line.endColor = Color.red;
                line.receiveShadows = false;
                line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                line.SetPosition(i, new Vector3(incomingPath[i].x, 0.5f, incomingPath[i].z));
            }
        }
    }

    public void PreviewPath(GridCell cell)
    {
        previewGrid.GetGridCell(cell.x, cell.z).passable = false;
        List<Vector3> incomingPath = Pathfinder.FindPath(previewGrid, spawner.transform.position, mainBase.transform.position);
        if (incomingPath != null){
            LineRenderer line = previewPathHolder.GetComponent<LineRenderer>();
            line.positionCount = 0;
            line.positionCount = incomingPath.Count;
            for (int i = 0; i < incomingPath.Count; i++)
            {
                line.material = pathMaterial;
                line.startWidth = 0.1f;
                line.endWidth = 0.1f;
                line.startColor = Color.yellow;
                line.endColor = Color.yellow;
                line.receiveShadows = false;
                line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                line.SetPosition(i, new Vector3(incomingPath[i].x, 0.5f, incomingPath[i].z));
            }
        }
    }

    public void UnpreviewPath(GridCell cell)
    {
        GridCell previewGridCell = previewGrid.GetGridCell(cell.x, cell.z);
        if(!previewGridCell.wall && previewGridCell.buildable)
        {
            previewGrid.GetGridCell(cell.x, cell.z).passable = true;
        }
        LineRenderer line = previewPathHolder.GetComponent<LineRenderer>();
        line.positionCount = 0;
    }

    public void PlaceBaseOrSpawner(int x, int z)
    {
        GridCell cell = mainGrid.GetGridCell(x, z);
        cell.buildable = false;
        foreach(GridCell neighbor in cell.GetNeighbors())
        {
            neighbor.buildable = false;
        }
    }
}
public enum GameMode {
    None, BuildWall, BuildTurret, DestroyWall, DestroyTurret
}