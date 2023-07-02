using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    
    private Rigidbody2D rb;

    //Health Variables
    public int hp;

    //Movement variables
    public float speed;
    private int Direction=1;
    public LayerMask groundObjects;
    public Transform groundCheck;
    public Transform wallCheck;
    private bool groundAhead;
    private bool wallAhead;
    public float checkRadius;
    public float waitTime;
    private bool waiting;


    //Player Detection Script
    public LayerMask Target;
    public Transform RayOrigin;
    public float cooldown;
    private float shotCountdown;
    public float visionDistance;
    private bool playerSighted;

    //Shooting Variables

    private GameObject bullet;
    public GameObject fireBall;

    void FixedUpdate()
    {
        groundAhead = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);
        wallAhead = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundObjects);

        move();
        detectPlayer();
    }

    void Update()
    {
        if(hp<=0)
        {
            Destroy(gameObject);
        }
    }
    //Enemy movement
    private void move()
    {
        if(playerSighted)
        {
            shootFireball();
        }
        else
        {
            if(!waiting)
            {
                if(!groundAhead || wallAhead)
                {
                    waiting=true;
                    StartCoroutine(Wait());
                }
                else
                { 
                    transform.position+=(Vector3.right*speed*Direction)*Time.deltaTime;
          
                }
            }
        }
        
    }
        
        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(waitTime);
            Flip();
            waiting=false;
        }

    //Player Targeting
    private void detectPlayer()
    {
        RaycastHit2D objectSighted = Physics2D.Raycast(RayOrigin.position, Vector2.right, visionDistance*Direction);

        if(objectSighted.collider != null)
        {
            Debug.DrawRay(RayOrigin.position, Vector2.right*objectSighted.distance*new Vector2(Direction, 0f), Color.red);
            // Debug.Log("something spotted");
        }
        else
        {
            Debug.DrawRay(RayOrigin.position, Vector2.right*objectSighted.distance*Direction, Color.green);
        }
    }

        public void Flip()
        {
            transform.Rotate(0f, 180f, 0f);
            Direction=Direction*-1;
        }

        
        //Taking Damage
        public void TakeDamage(int damage)
        {
            hp-=damage;
        }

        public void shootFireball()
        {
            bullet = Instantiate(fireBall, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(0f, 0f, -90f));
            bullet.GetComponent<EnemyFireballScript>().direction=Direction;
        }

        




}
