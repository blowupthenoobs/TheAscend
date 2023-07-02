using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public int Direction;
    public float moveSpeed=30;
    public float lifeSpan;
    private float lifeTime;
    public bool Vert;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
      if(!Vert){
        transform.position+=(Vector3.right*moveSpeed*Direction)*Time.deltaTime;
      }
      else
      {
        transform.position+=(Vector3.up*moveSpeed*Direction)*Time.deltaTime;
      }

        lifeTime+=Time.deltaTime;

      

      if(lifeTime>=lifeSpan)
        {
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag("Enemy")){

              collision.gameObject.GetComponent<EnemyScript>().TakeDamage(5);
              Explode();
            }
            else if(collision.gameObject.CompareTag("FlyingEnemy"))
            {
              collision.gameObject.GetComponent<FlyingEnemyScript>().TakeDamage(5);
              Explode();

            }
        if(collision.gameObject.layer==3)
        {
            Explode();
        }
      }

      void Explode(){
        Destroy(gameObject);
      }


}
