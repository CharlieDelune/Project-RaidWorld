using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour, Observer
{
    public float speed;
    public float attackSpeed;
    public int damage;
    private Globals globals;
    private Base target;
    private List<Vector3> pathVectorList;
    private int currentPathIndex;
    [SerializeField]
    private bool moving;
    [SerializeField]
    private bool attacking;
    private float timeToAttack;
    [SerializeField]
    private int health;
    [SerializeField]
    private int armor;
    [SerializeField]
    private int shield;
    [SerializeField]
    private int worth;
    [SerializeField]
    private bool flying;


    void Start()
    {
        globals = GameObject.FindGameObjectWithTag("Globals").GetComponent<Globals>();
        target = globals.mainBase;
        SetTargetPosition(target.transform.position);
        timeToAttack = 0;
        Publisher.AddObserver(this);
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
                if(!attacking){
                    SetTargetPosition(target.transform.position);
                }
                break;
            case PublisherEvent.RemovedWall:
                if(!attacking){
                    SetTargetPosition(target.transform.position);
                }
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
        attacking = true;
    }

        private void StopAttacking()
    {
        attacking = false;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = globals.FindVectorPath(transform.position, target.transform.position);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            moving = true;
            pathVectorList.RemoveAt(0);
        }
    }

    public void TakeDamage(int dam)
    {
        this.health -= (dam - armor);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Base")
        {
            StopMoving();
            StartAttacking();
        }
    }
}
