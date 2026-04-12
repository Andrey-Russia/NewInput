using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 2.0f;
    [SerializeField] private float jumpSpeed = 8.0f;
    [SerializeField] private float crouchJumpSpeed = 4.0f;
    [SerializeField] private float gravity = 20.0f;

    [SerializeField] private float crouchHeight = 1.0f;
    [SerializeField] private float standHeight = 2.0f;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float minVerticalAngle = -70f;
    [SerializeField] private float maxVerticalAngle = 70f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private bool isCrouching = false;
    private Vector3 originalCenter;

    private float verticalRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalCenter = controller.center;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleCameraRotation();
        HandleCrouch();
        HandleMovement();
        ApplyGravityAndMove();
    }

    void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && controller.isGrounded)
        {
            isCrouching = !isCrouching;
            controller.height = isCrouching ? crouchHeight : standHeight;
            controller.center = isCrouching ? new Vector3(originalCenter.x, originalCenter.y / 2, originalCenter.z) : originalCenter;
        }
    }

    void HandleMovement()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= isCrouching ? crouchSpeed : walkSpeed;

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = isCrouching ? crouchJumpSpeed : jumpSpeed;
            }
        }
    }

    void ApplyGravityAndMove()
    {
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}