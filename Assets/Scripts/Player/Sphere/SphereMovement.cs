using UnityEngine;
using UnityEngine.InputSystem;

public class SphereMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    Controls controls;
    private InputAction movement;
    private float movementInput;

    [Header ("Movement Paramters")]
    [SerializeField] private float force;
    [SerializeField] private float jumpForce;
    [SerializeField] private float topXSpeed;
    [SerializeField] private float topYSpeed;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float wallModifier;

    private float jumpTimer;
    private bool isGrounded;
    private bool onWall;

    private RaycastHit2D hitRight;
    private RaycastHit2D hitLeft;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new Controls();
        if (topYSpeed > 0)
            topYSpeed *= -1;
        if (wallSlidingSpeed > 0)
            wallSlidingSpeed *= -1;

        jumpTimer = Mathf.Infinity;
        isGrounded = false;
    }

    private void OnEnable()
    {
        movement = controls.Sphere.Movement;
        movement.Enable();

        controls.Sphere.Jump.performed += DoJump;
        controls.Sphere.Jump.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        controls.Sphere.Jump.Disable();
    }

    void Update()
    {
        movementInput = movement.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        AddForceAndLimitSpeed();

        hitRight = Physics2D.Raycast(new Vector3(transform.position.x + 0.56f, transform.position.y, transform.position.z), Vector2.right, 0.01f);
        hitLeft = Physics2D.Raycast(new Vector3(transform.position.x - 0.56f, transform.position.y, transform.position.z), Vector2.left, 0.01f);

        if ((hitLeft && hitLeft.collider.CompareTag("Ground")) || (hitRight && hitRight.collider.CompareTag("Ground")))
            onWall = true;
        else
            onWall = false;

        jumpTimer += Time.fixedDeltaTime;
    }

    private void AddForceAndLimitSpeed()
    {
        rb.AddForce(new Vector2(movementInput, 0) * force);

        if (onWall)
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -topXSpeed, topXSpeed), Mathf.Clamp(rb.velocity.y, wallSlidingSpeed, -topYSpeed));
        else
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -topXSpeed, topXSpeed), Mathf.Clamp(rb.velocity.y, topYSpeed, -topYSpeed));
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (!isGrounded)
            return;

        if (obj.ReadValue<float>() > 0 && jumpTimer >= jumpCooldown)
        {
            jumpTimer = 0;

            if (onWall)
            {
                if (hitRight) rb.AddForce(new Vector2(-0.75f*jumpForce, wallModifier * jumpForce), ForceMode2D.Impulse);
                else if (hitLeft) rb.AddForce(new Vector2(0.75f*jumpForce, wallModifier * jumpForce), ForceMode2D.Impulse);
            }
            else
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
    }
}
