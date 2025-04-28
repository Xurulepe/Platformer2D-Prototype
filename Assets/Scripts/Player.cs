using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.LightAnchor;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 moveInput;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Vector2 groundJumpDirection = Vector2.up;
    [SerializeField] private Vector2 wallJumpDirection;
    [SerializeField] private bool doubleJump;
    [SerializeField] private bool wallJumping;

    [Header("Ground")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.7f, 0.1f);
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall")]
    [SerializeField] private Transform wallCheckPos;
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.3f, 1.7f);
    [SerializeField] private LayerMask wallLayer;

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
        Flip();
    }

    private void FixedUpdate()
    {
        Move();
    }

    #region Player movement
    public void SetMove(InputAction.CallbackContext value)
    {
        moveInput.x = value.ReadValue<Vector2>().x;

        if (wallJumping && value.performed)
        {
            wallJumping = false;
        }
    }

    private void Move()
    {
        if (!wallJumping)  // move-se normalmente se não estiver pulando em paredes
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
        else if(wallJumping && IsGrounded())  // para de se mover após um salto de uma parede ao tocar no chão
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
        
    }
    #endregion

    #region Player jump
    public void SetJump(InputAction.CallbackContext value)
    {
        if (value.performed && IsGrounded())
        {
            Jump(groundJumpDirection);
            doubleJump = true;
        }
        else if (value.performed && IsWalled())
        {
            Jump(wallJumpDirection);
            doubleJump = true;
            wallJumping = true;
            ForceFlip();
        }
        else if (value.performed && doubleJump)
        {
            Jump(groundJumpDirection);
            doubleJump = false;
        }
    }

    private void Jump(Vector2 jumpDirection)
    {
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
    }
    #endregion

    #region Check ground and wall
    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0f, groundLayer);
    }

    private void GroundCheck()
    {
        if (IsGrounded())
        {
            doubleJump = true;
        }
    }

    private bool IsWalled()
    {
        if (Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0f, wallLayer))
        {
            wallJumpDirection = new Vector2(-transform.localScale.x, 1f);
            return true;
        }
        return false;
    }
    #endregion

    #region Flip
    private void Flip()
    {
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void ForceFlip()
    {
        float localScaleX = transform.localScale.x;
        transform.localScale = new Vector3(-localScaleX, 1, 1);
    }
#endregion
}
