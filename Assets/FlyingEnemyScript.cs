using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float hp;
    public float moveSpeed;
    public Transform[] patrolPoints;
    public float waitTime;
    private int currentPointIndex;
    private bool waiting;


    //Targeting variables
    public float minimumDistance;
    public float visionDistance;
    public float checkRadius;
    public bool mainTargetNearby;
    public Transform mainTarget;
    public LayerMask mainTargetLayer;

    //Attack Variables
    private float currentCooldown;
    public float attackCooldown;
    private bool isAttacking;
    public Transform attackPos;
    public float attackRange;
    public LayerMask attackTargets;
    public int damage;

    // public Transform subTarget;
    public LayerMask subTargetLayer;


    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Move();
        CheckforPlayer();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hp<=0)
        {
            Destroy(gameObject);
        }
    }

    //Move Script
    private void Move()
    {
        if(mainTargetNearby)
        {
            if(Vector2.Distance(transform.position, mainTarget.position) > minimumDistance)
            {
                isAttacking=false;
                transform.position=Vector2.MoveTowards(transform.position, mainTarget.position, moveSpeed*Time.deltaTime);
            }
            else
            {
                isAttacking=true;
            }

            // if(rb.velocity.x<0)
            // {
            //     gameObject.transform.eulerAngles.y=180f;
            //     Debug.Log("flipped");
            // }
            // else if(rb.velocity.x>0)
            // {
            //     gameObject.transform.eulerAngles.y=0f;
            //     Debug.Log("flipped");
            // }
        }
        else
        {
            Patrol();
        }
    }

    public void Flip()
        {
            transform.Rotate(0f, 180f, 0f);
        }

    private void CheckforPlayer()
    {
        mainTargetNearby=Physics2D.OverlapCircle(transform.position, checkRadius, mainTargetLayer);
    }

    private void Patrol()
    {
        if(transform.position !=patrolPoints[currentPointIndex].position)
        {
            transform.position=Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex].position, moveSpeed*Time.deltaTime);
        }
        else
        {
            if(!waiting)
            {
                waiting=true;
                StartCoroutine(Wait());
            }
            
        }  
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        if(currentPointIndex+1<patrolPoints.Length)
        {
            currentPointIndex++;
        }
        else
        {
            currentPointIndex=0;
        }
        waiting=false;
    }

    private void attack()
    {
        // if(currentCooldown<=0)
        // {
        //     if(isAttacking)
        //     {
        //         Collider2D attackVictim=Physics2D.OverlapCircleAll(attackRange, attackPos.position, mainTarget);
        //         attackVictim.GetComponent<PlayerMovement>().TakeDamage(damage);
        //     }
        // }
    }

    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color=Color.red;
    //     OnDrawGizmosSelected.DrawWireSphere(attackPos.position, attackRange);
    // }

    public void TakeDamage(int damage)
        {
            hp-=damage;
        }
}
