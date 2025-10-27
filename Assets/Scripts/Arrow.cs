using UnityEngine;

public class Arrow : MonoBehaviour
{

    public float arrowspeed = 5f;

    void Start()
    {
        Destroy(this.gameObject,5f);
        
    }
  
    void Update()
    {
        transform.Translate(transform.right *Time.deltaTime * arrowspeed);
        
    }
}
