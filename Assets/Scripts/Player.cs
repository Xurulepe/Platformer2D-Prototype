using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private bool isWalking;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Vector2 groundJumpDirection = Vector2.up;
    [SerializeField] private Vector2 wallJumpDirection;
    [SerializeField] private bool doubleJump;
    [SerializeField] private bool wallJumping;
    [SerializeField] private bool isJumping;

    [Header("Ground")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.7f, 0.1f);
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall")]
    [SerializeField] private Transform wallCheckPos;
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.3f, 1.7f);
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private bool isWallSliding;
    [SerializeField] private float wallSlidindSpeed;

    [Header("Dash")]
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        GroundCheck();
        WallSlideCheck();
        Flip();
        Animation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    #region Player movement
    public void SetMove(InputAction.CallbackContext value)
    {
        moveInput.x = value.ReadValue<Vector2>().x;

        isWalking = value.performed;

        if (wallJumping && value.performed)
        {
            wallJumping = false;
        }
    }

    private void Move()
    {
        if (isDashing) return;  // se estiver dando dash, n�o se move

        if (isWallSliding)  // wall sliding
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlidindSpeed, float.MaxValue));
        }
        else if (!wallJumping)  // move-se normalmente se n�o estiver pulando em paredes
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
        else if(wallJumping && IsGrounded())  // para de se mover ap�s um salto de uma parede ao tocar no ch�o
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
            Invoke(nameof(ForceFlip), 0.1f);
        }
        else if (value.performed && doubleJump)
        {
            Jump(groundJumpDirection);
            doubleJump = false;
        }
    }

    private void Jump(Vector2 jumpDirection)
    {
        Invoke(nameof(UpdateJump), 0.1f);
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
    }

    void UpdateJump()
    {
        isJumping = true;
    }
    #endregion

    #region Player dash
    public void SetDash(InputAction.CallbackContext value)
    {
        if (value.performed && canDash)
        {
            Dash();
        }
    }

    private void Dash()
    {
        isDashing = true;
        canDash = false;

        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, rb.linearVelocity.y);

        Invoke(nameof(EndDash), dashDuration);
        Invoke(nameof(ResetDash), dashCooldown);
    }

    private void EndDash()
    {
        isDashing = false;
    }

    private void ResetDash()
    {
        canDash = true;
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
            isJumping = false;
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

    private void WallSlideCheck()
    {
        if (!IsGrounded() && IsWalled() && moveInput.x != 0)  // wall sliding
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
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

    #region Animation
    private void Animation()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isDashing", isDashing);
    }
    #endregion
}
