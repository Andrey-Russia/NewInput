using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class inputs : MonoBehaviour
{
    public UnityEvent ShootEvent = new();
    public UnityEvent JumpEvent = new();
    public Vector2 Move;
    public Vector2 look;
    public bool Shoot;
    public bool Jump;

    public void OnMove(InputValue value)
    {
        Move = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }

    public void OnShoot(InputValue value)
    {
        Shoot = value.isPressed;
        ShootEvent?.Invoke();
    }

    public void OnJump(InputValue value)
    {
        Jump = value.isPressed;
        JumpEvent?.Invoke();
    }
}
  