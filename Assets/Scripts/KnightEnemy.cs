using UnityEngine;

public class KnightEnemy : MonoBehaviour
{
    public int maxHealth = 3;

    [Header("Movement Settings")]
    public float walkSpeed = 1.5f;
    public float chaseSpeed = 2f;

    [Header("Detection Settings")]
    public float attackRangeRadius = 6f;  // when to start chasing
    public float retrieveDistance = 4f;   // when to start attacking
    public Transform player;
    public LayerMask whatIsPlayer;

    [Header("Attack Settings")]
  

    public GameObject arrowPrefab;
    public Transform spawnPosition;
    public float arrowSpeed = 7f;

    [Header("Other Settings")]
    public GameObject floatingTextPrefab;
    public Transform textSpawnPoint;

    private Animator animator;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private bool facingLeft = true;
    private bool hasShaken = false;

    private enum EnemyState { Idle, Chase, Attack }
    private EnemyState currentState = EnemyState.Idle;

    private float fallLimit = -300f;

    private bool isDead = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        
        if (player == null)
        {
            animator.SetFloat("Walk", 0f);
            animator.SetBool("Attack", false);
            return;
        }

        if (!isDead && (maxHealth <= 0 || transform.position.y < fallLimit))
        {
            Die();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // --- STATE CONTROL ---
        if (distanceToPlayer <= retrieveDistance)
            currentState = EnemyState.Attack;
        else if (distanceToPlayer <= attackRangeRadius)
            currentState = EnemyState.Chase;
        else
            currentState = EnemyState.Idle;

        // --- STATE BEHAVIOR ---
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Chase:
                ChasePlayer();
                break;
            case EnemyState.Attack:
                AttackPlayer();
                break;
        }
    }

    // ---------------- STATES ----------------
    void Idle()
    {
        animator.SetFloat("Walk", 0f);
        animator.SetBool("Attack", false);
    }

    void ChasePlayer()
    {
        animator.SetBool("Attack", false);

        // Face player
        if (player.position.x > transform.position.x && facingLeft)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingLeft = false;
        }
        else if (player.position.x < transform.position.x && !facingLeft)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingLeft = true;
        }

        // Move toward player
        Vector2 targetPos = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);

        // Play walk animation like player
        animator.SetFloat("Walk", 1f);
    }

    void AttackPlayer()
    {
        animator.SetFloat("Walk", 0f);
        animator.SetBool("Attack", true);

    }

    // ---------------- ATTACK LOGIC ----------------


    public void FireArrow()
    {
        GameObject tempArrowPrefab = Instantiate(arrowPrefab, spawnPosition.position, spawnPosition.rotation);
        tempArrowPrefab.GetComponent<Rigidbody2D>().linearVelocity = spawnPosition.right * arrowSpeed;

    }

    // ---------------- DAMAGE / DEATH ----------------
    public void TakeDamage(int damageAmount)
    {
        if (maxHealth <= 0)
            return;

        maxHealth -= damageAmount;
        animator.SetTrigger("hit");
        CameraShake.instance.Shake(2.5f, .15f);

        Instantiate(floatingTextPrefab, textSpawnPoint.position, Quaternion.identity);
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        animator.SetBool("Death", true);
        rb.gravityScale = 0f;
        boxCollider2D.enabled = false;

        if (!hasShaken)
        {
            hasShaken = true;
            CameraShake.instance.Shake(2f, .12f);
        }

        Destroy(gameObject, 5f);
        GameManager.Instance.EnemyKilled();

    }

    // ---------------- DEBUG ----------------
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRangeRadius);

    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Arrow"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}
