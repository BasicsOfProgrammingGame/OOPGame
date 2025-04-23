using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject health = GameObject.FindGameObjectWithTag("Lives");

            if (health.GetComponent<PlayerHealth>() != null)
            {
                Debug.Log("AAA");
                health.GetComponent<PlayerHealth>().TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}




