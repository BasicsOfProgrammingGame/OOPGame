using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private float HorizontalMove = 0f;
    [Range(0, 10f)] public float speed = 0.5f;
    [Range(0, 100f)] public float jumpForce = 10f;
    private float originalSpeed;
    private float originalGravityScale;

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
        originalGravityScale = rigidbody.gravityScale;
    }

    private void Update()
    {
        // Прыжок
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && IsGroundedNow() && !isHovering)
        {
            rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
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
        animator.SetBool("IsJumping", !IsGroundedNow() && !isHovering);


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

        // Зависание
        if (Input.GetKeyDown(KeyCode.LeftShift) && !IsGroundedNow() && !isHovering)
        {
            StartHover();
        }

        if (isHovering)
        {
            currentHoverTime += Time.deltaTime;

            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, 0);
            rigidbody.AddForce(Vector2.up * hoverForce * Time.deltaTime * 60, ForceMode2D.Force);

            if (currentCloud != null)
                currentCloud.transform.position = transform.position + cloudOffset;

            if (currentHoverTime >= hoverDuration || IsGroundedNow())
            {
                StopHover();
            }

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
        {
            var x = bullet.transform.localScale.x;
            var y = bullet.transform.localScale.y;
            bullet.transform.localScale = new Vector3(-x, y, 1);
        }
       // animator.SetTrigger("Shoot");
    }

    private void StartHover()
    {
        isHovering = true;
        currentHoverTime = 0f;
        rigidbody.gravityScale = 0.2f;


        if (hoverCloudPrefab != null)
        {
            currentCloud = Instantiate(hoverCloudPrefab, transform.position + cloudOffset, Quaternion.identity);
            currentCloud.transform.parent = transform;
        }
    }

    private void StopHover()
    {
        if (!isHovering) return;

        isHovering = false;
        rigidbody.gravityScale = originalGravityScale;

        if (currentCloud != null)
        {
            Destroy(currentCloud);
            currentCloud = null;
        }
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(HorizontalMove * 10f, rigidbody.linearVelocity.y);
        rigidbody.linearVelocity = Vector2.Lerp(rigidbody.linearVelocity, targetVelocity, 0.5f);
    }

    private void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private bool IsGroundedNow()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            new Vector2(transform.position.x, transform.position.y + checkGroundOffsetY),
            checkGroundRadius);
        return colliders.Length > 1;
    }
}
