using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private int _maxJumps = 2; 

    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _isGrounded;
    private int _currentJumps;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal * _speed, _rb.linearVelocity.y);
        _rb.linearVelocity = movement;

        if (Input.GetButtonDown("Jump") && CanJump())
        {
            Jump();
        }

        _animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));
        _animator.SetBool("IsGrounded", _isGrounded);
        _animator.SetInteger("CurrentJumps", _currentJumps);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
            _currentJumps = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }

    void Jump()
    {
        if (_currentJumps >= _maxJumps || !CanJump()) return;

        _currentJumps++;
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

        _animator.SetTrigger("Jump");
        if (_currentJumps == 2)
        {
            _animator.SetTrigger("DoubleJump");
        }
    }

    bool CanJump()
    {
        return _currentJumps < _maxJumps;
    }
}