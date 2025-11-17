using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public int damage = 1;
    public float arrowspeed = 5f;

    void Start()
    {
        Destroy(this.gameObject, 5f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If arrow hits the player
        if (collision.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(damage);
            Destroy(gameObject);
        }


    }
}
