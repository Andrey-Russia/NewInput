using DG.Tweening;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _turnDuration = 0.5f;

    private bool _isFacingRight = true;
    private bool _canJump = true;
    private Transform _characterTransform;
    private Rigidbody _rb;

    private void Start()
    {
        _characterTransform = transform;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
        {
            Vector3 moveDirection = new Vector3(horizontal, 0, 0);
            _characterTransform.Translate(moveDirection.normalized * _moveSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetButtonDown("Jump") && _canJump)
        {
            Jump();
        }
    }

    private void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TurnLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            TurnRight();
        }
    }

    private void Jump()
    {
        _canJump = false;

        Vector3 jumpDir = _isFacingRight ? Vector3.right : Vector3.left;
        Vector3 targetPos = _characterTransform.position + jumpDir * _jumpHeight;

        Sequence seq = DOTween.Sequence();
        seq.Append(_characterTransform.DOLocalMove(targetPos, 1));
        seq.Join(_characterTransform.DOJump(targetPos, _jumpHeight, 1, 1));

        seq.AppendInterval(5f); 

        seq.OnComplete(() => { _canJump = true; });
    }

    private void TurnLeft()
    {
        if (!_isFacingRight) return;
        _isFacingRight = false;
        _characterTransform.DORotate(new Vector3(0, -90, 0), _turnDuration);
    }

    private void TurnRight()
    {
        if (_isFacingRight) return;
        _isFacingRight = true;
        _characterTransform.DORotate(Vector3.zero, _turnDuration);
    }

    private void CheckGround()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(_characterTransform.position, Vector3.down, out hitInfo, 1.1f))
        {
            if (hitInfo.collider.CompareTag("Ground"))
            {
                _canJump = true;
            }
        }
    }
}