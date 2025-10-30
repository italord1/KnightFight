using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float walkSpeed = 1.5f;

    public Transform groundCheckPoint;
    public float distance = 0.3f;
    public LayerMask whatIsGround;

    private bool facingLeft;
    void Start()
    {
        facingLeft = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left *Time.deltaTime * walkSpeed);

      RaycastHit2D hitInfo =  Physics2D.Raycast(groundCheckPoint.position, Vector2.down, distance, whatIsGround);

        if (hitInfo == false)
        {
            if (facingLeft)
            {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft=false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft=true;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;    
        Gizmos.DrawRay(groundCheckPoint.position, Vector2.down * distance);
    }
}
