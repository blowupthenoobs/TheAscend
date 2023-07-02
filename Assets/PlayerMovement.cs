using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Fireball Variables
    public GameObject Fireball;
    private GameObject Bullet;

    //Movement Variables
    public float maxMoveSpeed;
    public float moveSpeed;
    public float accelleration;
    public float decelleration;
    private float moveDirection;
    private float extraVelocity;

    //Physics Variables
    private Rigidbody2D rb;
    private bool facingRight=true;


    //Jumping Variables
    public float jumpForce;
    private bool isJumping=false;

    //Knockback Variables
    public float bounceForce;
    private bool knockedBack=false;
    public float wallPush;

    //Dash Variables
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private bool dashing=false;
    private int Direction;
    public float dashBounce;

    //Terrain Check Variables
    public Transform groundCheck;
    public LayerMask groundObjects;
    private bool isGrounded;
    public float checkRadius;
    public Transform wallCheck;
    private bool touchWall;
    public float maxHang;
    public float hangTime;
    public float slideSpeed;

    //Mana Stuffs
    public int maxmana;
    public int mana;
    public int magicCooldown;
    private float cooldown;
    private float countdown;

    //Combat Stuffs
    public int maxHealth;
    public int health;


    //Math Code Cheats
    private float half=.5f;
    private float quarter=.25f;

    private void Awake(){
        rb=GetComponent<Rigidbody2D>();
    }

    void Update(){

        ProcessInputs();

        Animate();

        regainMana();

        if(health<=0)
        {
            die();
        }

    }

    private void FixedUpdate()
    {
        isGrounded=Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);
        touchWall=Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundObjects);

        Move();
    }
 
    private void Move()
    {

        if(!touchWall||isGrounded)
        {
            hangTime=0;
        }

        //Jumping and Wall bounce
        if(isJumping && isGrounded)
            {
                rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
            }
        else if(isJumping && touchWall)
            {
                if(dashing){
                    rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
                    if(facingRight){
                        moveSpeed=-dashBounce;
                    }
                    else{
                        moveSpeed=dashBounce;
                    }
                    FlipCharacter();

                }
                else{
                    rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
                    if(facingRight)
                    {
                        moveSpeed=-wallPush-moveSpeed/3;
                    }
                    else
                    {
                        moveSpeed=wallPush-moveSpeed/3;
                    }
                    FlipCharacter();
                }
                
            }

        

        if(!dashing)
        {
            //Movement Script
            rb.velocity=new Vector2(moveSpeed, rb.velocity.y);

            //Accelleration Code
            if(Input.GetKey(KeyCode.D) && moveSpeed<maxMoveSpeed){
                moveSpeed+=accelleration*Time.deltaTime;
            }
            else if(Input.GetKey(KeyCode.A) && moveSpeed>-maxMoveSpeed){
                moveSpeed+=-accelleration*Time.deltaTime;
            }

            //Decelleration Code
            
                else if(!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)){
                    if(moveSpeed<2 && moveSpeed>-2){
                        moveSpeed=0;
                    }
                    if(moveSpeed<0){
                        moveSpeed+=10*decelleration*Time.deltaTime;
                    }
                    else if(moveSpeed>0){
                        moveSpeed-=10*decelleration*Time.deltaTime;
                    }
                }
                else if(moveSpeed>0){
                    moveSpeed-=decelleration*Time.deltaTime;
                }
                else if(moveSpeed<0){
                    moveSpeed+=decelleration*Time.deltaTime;
                }
            
            //HangTime
            if(touchWall && !isGrounded && rb.velocity.y<=0)
            {
                if(hangTime<maxHang)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    hangTime+=Time.deltaTime;
                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
                }
            }

            
        }
        else{
            rb.velocity=new Vector2(Direction*dashSpeed, 0);
            dashTime-=Time.deltaTime;
            if(dashTime<=0)
            {
                dashing=false;
            }
        }
        if(extraVelocity>-2 && extraVelocity<2)
        {
            extraVelocity=0;
        }
        else if(extraVelocity>0)
        {
            extraVelocity-=5*Time.deltaTime;
            moveDirection=-1;
        }
        else if(extraVelocity<0)
        {
            extraVelocity+=5*Time.deltaTime;
            moveDirection=1;
        }
        
        
        
        isJumping=false;
    }

    private void ProcessInputs()
    {
        moveDirection=Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump"))
        {
            isJumping=true;
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            ShootFireball();
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            dodge();
        }
    }
    //Abilities

        private void dodge()
        {
            if(mana>=5)
            {
                useMana();
                mana-=5;
                extraVelocity=0;
            if(facingRight)
            {
                Direction=1;
            }
            else
            {
                Direction=-1;
            }
            dashTime=startDashTime;
            dashing=true;
            }  
        }
        private void ShootFireball()
        {
            if(mana>=5)
            {
                mana-=5;
                useMana();
                if(Input.GetKey(KeyCode.S))
                {
                Bullet = Instantiate(Fireball, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(0f, 0f, -0f));
                Bullet.GetComponent<FireballScript>().Direction=-1;
                Bullet.GetComponent<FireballScript>().Vert=true;
                    if(isGrounded)
                    {
                    rb.AddForce(transform.up*bounceForce*2, ForceMode2D.Impulse);
                    }
                    else{
                    rb.AddForce(transform.up*bounceForce*3, ForceMode2D.Impulse);
                    }
                }
                else if(Input.GetKey(KeyCode.W)){
                Bullet = Instantiate(Fireball, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(0f, 0f, -0f));
                Bullet.GetComponent<FireballScript>().Direction=1;
                Bullet.GetComponent<FireballScript>().Vert=true;
                }
                else{
                    Bullet = Instantiate(Fireball, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(0f, 0f, -90f));
                    if(facingRight)
                    {
                        Bullet.GetComponent<FireballScript>().Direction=1;
                        Bullet.GetComponent<FireballScript>().Vert=false;
                        knockedBack=true;
                    if(isGrounded)
                    {
                        extraVelocity+=-3*half*bounceForce;
                    }
                    else{
                        extraVelocity+=-3*bounceForce;
                    }
                }
                else
                {
                    Bullet.GetComponent<FireballScript>().Direction=-1;
                    Bullet.GetComponent<FireballScript>().Vert=false;
                    knockedBack=true;
                    if(isGrounded)
                        {
                            extraVelocity+=6*half*bounceForce;
                    }
                    else{
                        extraVelocity+=6*half*bounceForce;
                    }
                }
                StartCoroutine(routine:knockTime());
                }
            }
            
        }


    //Mana & Health
        private void useMana()
        {
            cooldown=0;
            countdown=0;
        }

        private void regainMana()
        {
            if(countdown<2.5)
            {
                countdown+=Time.deltaTime;
            }
            else
            {
                countdown=0;
                if(cooldown<magicCooldown)
                {
                    countdown+=1;
                }
                if(mana<maxmana)
                {
                    mana+=1;
                }
            }
        }

        private void die()
        {
            
        }

        public void TakeDamage(int Damage)
        {
            health-=Damage;
        }
    


    //Coroutines
        private IEnumerator knockTime()
        {
            yield return new WaitForSeconds(.5f);
            knockedBack=false;
        }

    //Animation
        private void Animate()
        {
            if(moveDirection>0 && !facingRight){
                FlipCharacter();
            }
            else if(moveDirection<0 && facingRight){
                FlipCharacter();
            }
        }

        private void FlipCharacter(){
            facingRight=!facingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    

    

   
}
