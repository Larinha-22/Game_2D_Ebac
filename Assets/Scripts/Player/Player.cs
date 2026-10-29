
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    [Header("Speed setup")]
    public Vector2 friction = new Vector2(20f, 0f);
    public float speed = 5f;
    public float speedRun = 8f;
    public float forceJump = 15f;

    [Header("Ground check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    private float horizontalInput;
    private bool isRunning;
    private bool isGrounded;

    private bool isDead = false;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isDead)
            return;

        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
            horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.RightArrow))
            horizontalInput = 1f;

        isRunning = Input.GetKey(KeyCode.LeftControl);

        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );
        }

        HandleJump();
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        float currentSpeed = isRunning ? speedRun : speed;

        if (horizontalInput != 0f)
        {
            myRigidbody.velocity = new Vector2(
                horizontalInput * currentSpeed,
                myRigidbody.velocity.y
            );
        }
        else
        {
            float newVelocityX = Mathf.MoveTowards(
                myRigidbody.velocity.x,
                0f,
                friction.x * Time.fixedDeltaTime
            );

            myRigidbody.velocity = new Vector2(
                newVelocityX,
                myRigidbody.velocity.y
            );
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            myRigidbody.velocity = new Vector2(
                myRigidbody.velocity.x,
                forceJump
            );
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        myRigidbody.velocity = Vector2.zero;
        myRigidbody.simulated = false;

        Destroy(gameObject);
    }
}
