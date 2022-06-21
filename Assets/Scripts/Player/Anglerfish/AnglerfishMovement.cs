using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnglerfishMovement : MonoBehaviour
{
    [Header ("Movement Parameters")]
    [SerializeField] private float swimForce;
    [SerializeField] private float topSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float fasterRotationMultiplier;

    [Header("Dash Parameters")]
    [SerializeField] private float dashMultiplier;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float drag;
    private float dashTimer;

    Controls controls;
    private InputAction movement;

    private float direction;
    private bool isSwimming;
    private bool isDashing;

    private Animator anim;
    private Rigidbody2D rb;
    private UIManager uiManager;

    private float originalTopSpeed;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.drag = drag;
        controls = new Controls();
        uiManager = GetComponentInParent<Transformation>().uiManager;
        originalTopSpeed = topSpeed;
    }
    private void Start()
    {
        dashTimer = Mathf.Infinity;
        direction = 1f;
    }

    private void OnEnable()
    {
        movement = controls.Anglerfish.Movement;
        movement.Enable();

        controls.Anglerfish.Dash.performed += Dash;
        controls.Anglerfish.Dash.Enable();
    }

    private void OnDisable()
    {
        //BurstFailsafe();
        movement.Disable();
    }

    private void Update()
    {
        if (movement.ReadValue<Vector2>() != new Vector2())
            isSwimming = true;
        else
            isSwimming = false;
        
        anim.SetBool("isSwimming", isSwimming);
        uiManager.SetBurstValue(dashTimer / dashCooldown);
    }

    void FixedUpdate()
    {
        if (isSwimming)
            Move();

        dashTimer += Time.fixedDeltaTime;
    }

    void Move()
    {
        Vector2 values = movement.ReadValue<Vector2>();
        float rotation_z = Mathf.Atan2(values.y, values.x) * Mathf.Rad2Deg;

        rb.AddForce(values * swimForce);
        if (Mathf.Abs(rb.velocity.x) > topSpeed)
            rb.velocity = new Vector3(Mathf.Sign(rb.velocity.x) * topSpeed, rb.velocity.y, 0);
        if (Mathf.Abs(rb.velocity.y) > topSpeed)
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Sign(rb.velocity.y) * topSpeed, 0);

        float oldDirection = direction;

        if ((rotation_z > 90 || rotation_z <= -90) && direction > 0)
            direction = -1f;
        else if ((rotation_z < 90 && rotation_z > -90) && direction < 0)
            direction = 1f;

        if (direction != oldDirection)
            transform.localScale = new Vector3(transform.localScale.x, direction, transform.localScale.z);

        if (Mathf.Abs(Mathf.DeltaAngle(rb.rotation, rotation_z)) <= 90)
            rb.rotation = Mathf.Lerp(rb.rotation, rb.rotation + Mathf.DeltaAngle(rb.rotation, rotation_z), rotationSpeed * Time.fixedDeltaTime);
        else
            rb.rotation = Mathf.Lerp(rb.rotation, rb.rotation + Mathf.DeltaAngle(rb.rotation, rotation_z), rotationSpeed * Time.fixedDeltaTime * fasterRotationMultiplier);

        //Old mouse movement system
        /*Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = 10;
        Vector3 currentMouse = Camera.main.ScreenToWorldPoint(mousePos);
        
        
        Vector3 formerRBPos = rb.position;

        Vector3 diff = currentMouse - formerRBPos;
        diff.Normalize();
        float rotation_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        Vector3 m_Input = new Vector3(currentMouse.x, currentMouse.y, 0);
        rb.AddForce((m_Input - formerRBPos).normalized * swimForce);

        if (Mathf.Abs(rb.velocity.x) > topSpeed)
            rb.velocity = new Vector3(Mathf.Sign(rb.velocity.x) * topSpeed, rb.velocity.y, 0);
        if (Mathf.Abs(rb.velocity.y) > topSpeed)
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Sign(rb.velocity.y) * topSpeed, 0);

        float oldDirection = direction;

        if ((rotation_z > 90 || rotation_z < -90) && direction > 0)
            direction = -1f;
        else if ((rotation_z < 90 && rotation_z > -90) && direction < 0)
            direction = 1f;

        if (direction != oldDirection)
            transform.localScale = new Vector3(transform.localScale.x, direction, transform.localScale.z);

        rb.rotation = rotation_z;*/
    }

    void Dash(InputAction.CallbackContext obj)
    {
        if (isDashing) return;

        if (dashTimer >= dashCooldown)
        {
            dashTimer = 0;
            StartCoroutine(DashDuration());
        }
    }

    IEnumerator DashDuration()
    {
        isDashing = true;
        anim.SetBool("dashing", isDashing);
        topSpeed *= dashMultiplier;
        swimForce *= dashMultiplier;
        yield return new WaitForSeconds(dashDuration);
        topSpeed /= dashMultiplier;
        swimForce /= dashMultiplier;
        isDashing = false;
        anim.SetBool("dashing", isDashing);
        yield return null;
    }
}
