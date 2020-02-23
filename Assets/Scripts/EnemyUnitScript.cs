using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitScript : MonoBehaviour, Observer
{
    public float speed;
    public float attackSpeed;
    public int damage;
    [SerializeField]
    private BaseScript target;
    private GameObject gridHolder;
    private GridCell currentCell;
    private List<Vector3> pathVectorList;
    private int currentPathIndex;
    [SerializeField]
    private bool moving;
    [SerializeField]
    private bool attacking;
    private float timeToAttack;


    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Base")[0].GetComponent<BaseScript>();
        gridHolder = GameObject.FindGameObjectsWithTag("GridHolder")[0];
        SetTargetPosition(target.transform.position);

        foreach(GridCell cell in gridHolder.GetComponent<Grid>().gridCells)
        {
            cell.AddObserver(this);
        }
    }

    void Update()
    {
        if (moving)
        {
            HandleMovement();
        }
        if (attacking)
        {
            HandleAttack();
        }
    }

    public void OnNotify(PublisherEvent ev)
    {
        switch(ev)
        {
            case PublisherEvent.BuiltWall:
                SetTargetPosition(target.transform.position);
                break;
            default:
                break;
        }
    }

    private void HandleMovement()
    {
        if (pathVectorList != null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                    StartAttacking();
                }
            }
        }
    }

    private void HandleAttack()
    {
        timeToAttack -= Time.deltaTime;
        if(timeToAttack < 0)
        {
            target.DealDamage(damage);
            timeToAttack = attackSpeed;
        }
    }

    private void StopMoving()
    {
        pathVectorList = null;
        moving = false;
    }

    private void StartAttacking()
    {
        timeToAttack = attackSpeed;
        attacking = true;
    }

        private void StopAttacking()
    {
        attacking = false;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = Pathfinder.FindPath(transform.position, target.transform.position);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            moving = true;
            pathVectorList.RemoveAt(0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Base")
        {
            StopMoving();
            StartAttacking();
        }
    }
}
