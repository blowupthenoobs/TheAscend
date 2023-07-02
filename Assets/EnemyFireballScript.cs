using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireballScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public int direction;
    public float moveSpeed=30;
    public float lifeSpan;
    private float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position+=(Vector3.right*moveSpeed*direction)*Time.deltaTime;

        lifeTime+=Time.deltaTime;

        if(lifeTime>=lifeSpan)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag("Player")){
          
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(5);
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
