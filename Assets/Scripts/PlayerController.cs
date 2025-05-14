using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private float HorizontalMove = 0f;
    [Range(0, 10f)] public float speed = 0.5f;
    [Range(0, 100f)] public float jumpForce = 10f;
    private float originalSpeed;
    private float originalGravityScale;

    public bool isGrounded = false;
    [Range(-5f, 5f)] public float checkGroundOffsetY = -0.6f;
    [Range(0, 5f)] public float checkGroundRadius = 0.3f;
    private bool FacingRight = true;

    public Animator animator;

    [Header("Shooting Settings")]
    public GameObject playerBulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float shootCooldown = 0.5f;
    private float timeSinceLastShot;
    public float shootingSlowdownFactor = 0.5f;
    private bool isShootingLeft = false;
    private bool isShootingRight = false;

    [Header("Hover Settings")]
    public float hoverForce = 5f;
    public float hoverDuration = 2f;
    public GameObject hoverCloudPrefab;
    public Vector3 cloudOffset = new Vector3(0, -0.5f, 0);
    private bool isHovering = false;
    private float currentHoverTime = 0f;
    private GameObject currentCloud;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        originalSpeed = speed;
        originalGravityScale = rigidbody.gravityScale; // Сохраняем исходную гравитацию
    }

    private void Update()
    {
        // Прыжок
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded && !isHovering)
        {
            rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
        }

        // Обработка движения
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (!isShootingLeft && !isShootingRight)
        {
            HorizontalMove = moveInput * speed;

            if (moveInput < 0 && FacingRight)
                Flip();
            else if (moveInput > 0 && !FacingRight)
                Flip();
        }
        else
        {
            HorizontalMove = moveInput * speed * shootingSlowdownFactor;
        }

        animator.SetFloat("HorizontalMove", Mathf.Abs(HorizontalMove));
        animator.SetBool("IsJumping", !isGrounded);

        // Обработка стрельбы
        timeSinceLastShot += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isShootingLeft = true;
            if (FacingRight) Flip();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            isShootingRight = true;
            if (!FacingRight) Flip();
        }

        if ((isShootingLeft || isShootingRight) && timeSinceLastShot >= shootCooldown)
        {
            int direction = isShootingLeft ? -1 : 1;
            Shoot(direction);
            timeSinceLastShot = 0f;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            isShootingLeft = false;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            isShootingRight = false;
        }

        // Зависание в воздухе
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isGrounded && !isHovering)
        {
            StartHover();
        }

        if (isHovering)
        {
            currentHoverTime += Time.deltaTime;

            // Плавное зависание
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, 0);
            rigidbody.AddForce(Vector2.up * hoverForce * Time.deltaTime * 60, ForceMode2D.Force);

            if (currentCloud != null)
            {
                currentCloud.transform.position = transform.position + cloudOffset;
            }

            // Автоматическое прекращение при истечении времени или приземлении
            if (currentHoverTime >= hoverDuration || isGrounded)
            {
                StopHover();
            }

            // Ручное прекращение при отпускании клавиши
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                StopHover();
            }
        }
    }

    private void Shoot(int direction)
    {
        GameObject bullet = Instantiate(playerBulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 bulletDirection = new Vector2(direction, 0);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = bulletDirection * bulletSpeed;

        if (direction < 0)
            bullet.transform.localScale = new Vector3(-1, 1, 1);

        animator.SetTrigger("Shoot");
    }

    private void StartHover()
    {
        isHovering = true;
        currentHoverTime = 0f;
        rigidbody.gravityScale = 0.2f; // Уменьшаем гравитацию для эффекта зависания

        if (hoverCloudPrefab != null)
        {
            currentCloud = Instantiate(hoverCloudPrefab,
                                    transform.position + cloudOffset,
                                    Quaternion.identity);
            currentCloud.transform.parent = transform; // Чтобы облако следовало за игроком
        }

        animator.SetBool("IsHovering", true);
    }

    private void StopHover()
    {
        if (!isHovering) return;

        isHovering = false;
        rigidbody.gravityScale = originalGravityScale; // Восстанавливаем исходную гравитацию

        if (currentCloud != null)
        {
            Destroy(currentCloud);
            currentCloud = null;
        }

        animator.SetBool("IsHovering", false);
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(HorizontalMove * 10f, rigidbody.linearVelocity.y);
        rigidbody.linearVelocity = Vector2.Lerp(rigidbody.linearVelocity, targetVelocity, 0.5f);
        CheckGround();
    }

    private void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            new Vector2(transform.position.x, transform.position.y + checkGroundOffsetY),
            checkGroundRadius);

        bool wasGrounded = isGrounded;
        isGrounded = colliders.Length > 1;

        // Если только что приземлились, прекращаем зависание
        if (!wasGrounded && isGrounded && isHovering)
        {
            StopHover();
        }
    }

    // Визуализация области проверки земли в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            new Vector2(transform.position.x, transform.position.y + checkGroundOffsetY),
            checkGroundRadius);
    }
}