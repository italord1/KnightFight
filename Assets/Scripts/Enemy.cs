using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3;

    public float walkSpeed = 1.5f;

    public Transform groundCheckPoint;
    public float distance = 0.3f;
    public LayerMask whatIsGround;

    private bool facingLeft;

    public float attackRangeRadious = 6f;
    public LayerMask whatIsPlayer;
    public Transform player;
    public float chaseSpeed = 2f;
    public float retriveDistance =3f;

    private Animator animator;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;

    private bool hasShaken = false;

    public GameObject floatingTextPrefab;
    public Transform textSpawnPoint;

    public Transform attackPoint;
    public float attackRadius = 1.5f;


    void Start()
    {
        facingLeft = true;
        animator = this.gameObject.GetComponent<Animator>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        boxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(maxHealth <= 0)
        {
            
            Die();
                
            return;
        }

       Collider2D collInfo = Physics2D.OverlapCircle(transform.position, attackRangeRadious, whatIsPlayer);

        if (collInfo == true)
        {

            if (player.position.x > transform.position.x && facingLeft )
            {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            }
            else if(player.position.x < transform.position.x && facingLeft==false)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft = true;
            }

                Vector2 targetPos = new Vector2(player.position.x, transform.position.y);

            if (Vector2.Distance(transform.position, targetPos) > retriveDistance)
            {
                animator.SetBool("Attack",false);
                transform.position = Vector2.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack",true);

            }


            
        }
        else
        {
            transform.Translate(Vector2.left * Time.deltaTime * walkSpeed);

            RaycastHit2D hitInfo = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, distance, whatIsGround);

            if (hitInfo == false)
            {
                if (facingLeft)
                {
                    transform.eulerAngles = new Vector3(0f, -180f, 0f);
                    facingLeft = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    facingLeft = true;
                }
            }
        }

 
    }

    public void Attack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsPlayer);
        if(collInfo)
        {
            if(collInfo.gameObject.GetComponent<Player>() != null)
            {
                Player.Instance.TakeDamage(1);
            }
        }

    }

    public void TakeDamage(int damageAmount)
    {
        if (maxHealth <= 0)
        {
            return;
        }
        maxHealth -= damageAmount;
        animator.SetTrigger("hit");
        CameraShake.instance.Shake(2.5f, .15f);

        Instantiate(floatingTextPrefab, textSpawnPoint.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,attackRangeRadious);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);

        if (groundCheckPoint == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;    
        Gizmos.DrawRay(groundCheckPoint.position, Vector2.down * distance);


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Arrow")
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    public void ShakeCamera()
    {
        CameraShake.instance.Shake(4f, .18f);
    }
    void Die()
    {
        animator.SetBool("Death",true);
        rb.gravityScale = 0f;
        boxCollider2D.enabled= false;
        if (!hasShaken)
        {
            hasShaken = true;
            FindAnyObjectByType<CameraShake>().Shake(2f, .12f);
        }
        Destroy(this.gameObject, 5f);
    }
}
