using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 100f;
    [SerializeField] private float JumpForce = 100f;
    [SerializeField] private bool IsFacingRight = true;

    private Rigidbody2D _rb;
    private Animator _anim;
    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        _rb.linearVelocity = new Vector2(horizontalInput * MoveSpeed, _rb.linearVelocity.y);

        if ((horizontalInput > 0 && !IsFacingRight) || (horizontalInput < 0 && IsFacingRight))
            Flip();

        _anim.SetBool("isRunning", Mathf.Abs(_rb.linearVelocity.x) > 0.1f);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        _rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        _anim.SetTrigger("isJumping");
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            _isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            _isGrounded = false;
    }

    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}