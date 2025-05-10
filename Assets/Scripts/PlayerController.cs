using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private float HorizontalMove = 0f;
    [Range(0,10f)] public float speed = 0.5f;
    [Range(0, 100f)] public float jumpForce = 10f;

    public bool isGrounded = false;
    [Range(-5f, 5f)] public float checkGroundOffsetY = -0.6f;
    [Range(0, 5f)] public float checkGroundRadius = 0.3f;
    private bool FacingRight = true;

    public Animator animator;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        HorizontalMove = Input.GetAxisRaw("Horizontal") * speed;
        animator.SetFloat("HorizontalMove", Mathf.Abs(HorizontalMove));

        if (isGrounded == false)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
        if (HorizontalMove < 0 && FacingRight)
            Flip();
        else if (HorizontalMove > 0 && !FacingRight)
            Flip();
        
    }
    private void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(HorizontalMove * 10f, rigidbody.linearVelocity.y);
        rigidbody.linearVelocity = targetVelocity;
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
            new Vector2(transform.position.x, transform.position.y + checkGroundOffsetY), checkGroundRadius);

        if (colliders.Length > 1)
            isGrounded = true;
        else isGrounded = false;

    }
}
