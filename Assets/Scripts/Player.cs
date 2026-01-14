using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public inputs _inputs;
    public float Speed;
    public float JumpForce;
    public float MouseSence;

    private Rigidbody _rb;
    private Camera _camera;

    private float _xRotation = 0;
    private float _yRotation = 0;

    private void Awake()
    {
        _inputs = GetComponent<inputs>();
        _rb = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();

        _inputs.ShootEvent.AddListener(Onshoot);
        _inputs.JumpEvent.AddListener(OnJump);
    }

    private void Update()
    {
        OnMove();
        OnLook();
    }

    private void OnMove()
    {
        _rb.AddRelativeForce(new Vector3(_inputs.Move.x, 0, _inputs.Move.y) * Speed * Time.deltaTime);
    }

    private void OnLook()
    {
        _xRotation -= _inputs.look.y;
        _xRotation = Mathf.Clamp(_xRotation, -60f, 30f);

        _yRotation += _inputs.look.x;

        _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, _yRotation, 0);

        var scrollAmount = Input.mouseScrollDelta.y;
        if (scrollAmount != 0)
            AdjustCameraDistance(scrollAmount);
    }

    private void AdjustCameraDistance(float delta)
    {
        const float maxDistance = 10f;
        const float minDistance = 1f;
        const float zoomSensivity = 5f;

        var currentDistance = (_camera.transform.position - transform.position).magnitude;
        var desiredDistance = currentDistance - delta * zoomSensivity;

        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);

        var lookDirecton = (_camera.transform.position - transform.position).normalized;
        _camera.transform.position = transform.position + lookDirecton * desiredDistance;
    }

    private void OnJump()
    {
        _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }

    private void Onshoot()
    {
        print("ïèó");
    }
}
