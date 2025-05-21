using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public GameObject Trap;
    private PatrollingEnemy script;

    private void Start()
    {
        if (Trap != null)
        {
            script = Trap.GetComponent<PatrollingEnemy>();
            if (script == null)
                Debug.LogWarning("Компонент PatrollingEnemy не найден на объекте Trap");
        }
 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (script != null)
            {
                script.enabled = true;
                script.Initialize();
            }
            Destroy(gameObject);
        }
    }
}
