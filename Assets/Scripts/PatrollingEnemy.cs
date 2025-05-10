using System.Collections;
using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    [Header("Настройки патрулирования")]
    public Transform[] patrolPoints;
    public float speed = 2f;
    public float waitTime = 3f;
    public float distanceThreshold = 0.1f;
    public LayerMask obstacleLayer;
    public int damageToPlayer = 1;
    public Animator animator;
    public bool destroyAtLastPoint = false; // Новая галочка для уничтожения

    [Header("Настройки поворота")]
    public bool flipSprite = true;

    private int currentPointIndex = 0;
    private bool waiting = false;
    private Rigidbody2D rb;
    private Vector2 currentTarget;
    private bool isFacingLeft = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (patrolPoints.Length == 0)
        {
            Debug.LogError("Не заданы точки патрулирования!", this);
            enabled = false;
            return;
        }

        currentTarget = patrolPoints[currentPointIndex].position;
    }

    private void Update()
    {
        if (!waiting)
        {
            Patrol();
        }

        DetectObstacle();
    }

    private void Patrol()
    {
        // Движение к цели
        Vector2 newPosition = Vector2.MoveTowards(rb.position, currentTarget, speed * Time.deltaTime);
        rb.MovePosition(newPosition);

        // Определение направления
        float direction = currentTarget.x - rb.position.x;

        // Поворот спрайта
        if (flipSprite)
        {
            if (direction > 0 && isFacingLeft)
            {
                Flip();
            }
            else if (direction < 0 && !isFacingLeft)
            {
                Flip();
            }
        }

        // Анимация движения
        if (animator != null)
            animator.SetFloat("HorizontalMove", Mathf.Abs(direction));

        // Проверка достижения точки
        if (Vector2.Distance(rb.position, currentTarget) < distanceThreshold)
        {
            if (destroyAtLastPoint && currentPointIndex == patrolPoints.Length - 1)
            {
                Destroy(gameObject); // Уничтожаем объект
                return;
            }

            StartCoroutine(WaitAtPoint());
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            currentTarget = patrolPoints[currentPointIndex].position;
        }
    }

    private IEnumerator WaitAtPoint()
    {
        waiting = true;
        if (animator != null)
            animator.SetFloat("HorizontalMove", 0);
        yield return new WaitForSeconds(waitTime);
        waiting = false;
    }

    private void Flip()
    {
        isFacingLeft = !isFacingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void DetectObstacle()
    {
        float rayDistance = 0.5f;
        Vector2 direction = isFacingLeft ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, obstacleLayer);
        if (hit.collider != null)
        {
            Flip();
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            currentTarget = patrolPoints[currentPointIndex].position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject health = GameObject.FindGameObjectWithTag("Lives");

            if (health.GetComponent<PlayerHealth>() != null)
            {
                health.GetComponent<PlayerHealth>().TakeDamage(damageToPlayer, GameObject.FindGameObjectWithTag("Player").transform);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                {
                    Gizmos.DrawSphere(patrolPoints[i].position, 0.2f);
                    if (i < patrolPoints.Length - 1 && patrolPoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                    }
                }
            }
        }
    }
}