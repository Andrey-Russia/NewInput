using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private int _jumpCount;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal * _speed, _rb.linearVelocity.y);
        _rb.linearVelocity = movement;

        if (Input.GetButtonDown("Jump"))
            Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            _jumpCount = 2;
    }

    void Jump()
    {
        if (_jumpCount > 0)
            _jumpCount--;

        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
}
