using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 moveInput;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool doubleJump;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        GroundCheck();
    }

    private void FixedUpdate()
    {
        Move();
    }

    #region Player movement
    public void SetMove(InputAction.CallbackContext value)
    {
        moveInput.x = value.ReadValue<Vector2>().x;
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }
    #endregion

    #region Player jump
    public void SetJump(InputAction.CallbackContext value)
    {
        if (value.performed && IsGrounded())
        {
            Jump();
            doubleJump = true;
        }
        else if (value.performed && doubleJump)
        {
            Jump();
            doubleJump = false;
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.7f, 0.1f), 0f, groundLayer);
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheck.position, new Vector2(0.7f, 0.1f), 0f, groundLayer))
        {
            doubleJump = true;
        }
    }
    #endregion
}
