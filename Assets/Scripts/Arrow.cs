using UnityEngine;

public class Arrow : MonoBehaviour
{

    public float arrowspeed = 5f;

    void start()
    {
        Destroy(this.gameObject,5f);
    }
  
    void Update()
    {
        transform.Translate(transform.right *Time.deltaTime * arrowspeed);
    }
}
