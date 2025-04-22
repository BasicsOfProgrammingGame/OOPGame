using System.Collections;
using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed = 2f;
    public float waitTime = 1f;
    public float distanceThreshold = 0.1f;
    public LayerMask obstacleLayer;
    public int damageToPlayer = 1;

    private int currentPointIndex = 0;
    private bool waiting = false;
    private Rigidbody2D rb;
    private Vector2 currentTarget;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (patrolPoints.Length == 0)
        {
            Debug.LogError("No patrol points!");
            enabled = false;
            return;
        }

        transform.position = patrolPoints[currentPointIndex].position;
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
        Vector2 newPosition = Vector2.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
        rb.MovePosition(newPosition);

        if (Vector2.Distance(transform.position, currentTarget) < distanceThreshold)
        {
            StartCoroutine(WaitAtPoint());
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            currentTarget = patrolPoints[currentPointIndex].position;
            FlipDirection();
        }
    }

    private IEnumerator WaitAtPoint()
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);
        waiting = false;
    }

    private void FlipDirection()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void DetectObstacle()
    {
        float rayDistance = 0.5f;
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, obstacleLayer);
        if (hit.collider != null)
        {
            FlipDirection();
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            currentTarget = patrolPoints[currentPointIndex].position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            PlayerController playerInfo = collision.gameObject.GetComponent<PlayerController>();
            if (playerInfo.isGrounded && playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
            }
            FlipDirection();
        }
    }
}

