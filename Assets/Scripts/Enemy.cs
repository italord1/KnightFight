using UnityEngine;

public class Enemy : MonoBehaviour
{

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
    void Start()
    {
        facingLeft = true;
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,attackRangeRadious);

        if (groundCheckPoint == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;    
        Gizmos.DrawRay(groundCheckPoint.position, Vector2.down * distance);


    }
}
