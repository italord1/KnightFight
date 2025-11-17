using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public float moveSpeed = 5f;
    private float movement;
    public Rigidbody2D rb;
    public float jumphight=7f;
    private bool isGround;

    private bool facingRight;

    public Transform groudCheckPoint;
    public float groundCheckRadious = .2f;
    public LayerMask whatIsGround;

    private Animator animator;

    public GameObject arrowPrefab;
    public Transform spawnPosition;
    public float arrowSpeed = 7f;

    public int maxHealth = 5;

    public GameObject explosionPrefab;
    public Transform explosionSpawnPoint;

    private int currentDiamonds;
    public GameObject collectEffectPredab;

    public Text currentDiamond_Text;
    public Text currentHeart_Text;



    void Start()
    {
        isGround = true;
        facingRight = true;
        animator = this.gameObject.GetComponent<Animator>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        currentDiamonds = 0;
    }

   
    void Update()
    {

        if (maxHealth <= 0)
        {
            Die();
        }
       movement = Input.GetAxis("Horizontal");

       if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

      Collider2D collInfo =  Physics2D.OverlapCircle(groudCheckPoint.position, groundCheckRadious,whatIsGround);

        if (collInfo == true)
        {
            isGround = true;
            
        }

        Flip();
        PlayRunningAnimation();

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Fire");
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement*moveSpeed, 0f, 0f) * Time.fixedDeltaTime;
    }

    public void FireArrow()
    {
       GameObject tempArrowPrefab =  Instantiate(arrowPrefab, spawnPosition.position, spawnPosition.rotation);
        tempArrowPrefab.GetComponent<Rigidbody2D>().linearVelocity = spawnPosition.right * arrowSpeed;

    }

    private void PlayRunningAnimation()
    {
        if(Mathf.Abs(movement) > 0f)
        {
            animator.SetFloat("Run", 1f);
        }
        else if(movement < 0.1f)
        {
            animator.SetFloat("Run", 0f);
        }
    }

    private void Flip()
    {
        if (movement < 0f && facingRight)
        { 
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight=false;
        }
        else if(movement > 0f && facingRight == false)
        { 
            transform.eulerAngles = new Vector3(0f, 0f, 0f);  
            facingRight = true ;
        }
        

    }
    void Jump()
    {
        if (isGround)
        {
            Vector2 velocity = rb.linearVelocity;
            velocity.y = jumphight;
            rb.linearVelocity = velocity;
            isGround = false;
            animator.SetBool("Jump", true);
        }
        
     
      
    }

    public void TakeDamage(int damageAmount)
    {
        if (maxHealth <= 0)
        {
            return;
        }
        else
        {
            maxHealth -= damageAmount;
            animator.SetTrigger("Hurt");
            CameraShake.instance.Shake(2f, .12f);
            currentHeart_Text.text = maxHealth.ToString();
        }
    }

       private void OnCollisionEnter2D(Collision2D collision)
        {
                if (collision.gameObject.tag == "Ground")
                {
                    animator.SetBool("Jump", false);
                }
        }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Diamond")
        {
            currentDiamonds++;
            currentDiamond_Text.text= currentDiamonds.ToString();
           GameObject tempCollect_Effect =  Instantiate(collectEffectPredab, coll.gameObject.transform.position, Quaternion.identity);
            Destroy(tempCollect_Effect, .401f);
            Destroy(coll.gameObject);
        }

        if(coll.gameObject.tag =="Heart")
        {
            maxHealth++;
            currentHeart_Text.text = maxHealth.ToString();
            GameObject tempCollect_Effect = Instantiate(collectEffectPredab, coll.gameObject.transform.position, Quaternion.identity);
            Destroy(tempCollect_Effect, .401f);
            Destroy(coll.gameObject);

        }
    }

    private void OnDrawGizmosSelected()
    {
        if(groudCheckPoint == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groudCheckPoint.position,groundCheckRadious);
    }

    public void Die()
    {
        Debug.Log("player died!!");
        Instantiate(explosionPrefab,explosionSpawnPoint.position, Quaternion.identity);
       
        Destroy(this.gameObject);   
    }

  
}
