using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyLives : MonoBehaviour
{
    public int currentHealth = 3;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }
}
