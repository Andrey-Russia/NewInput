using UnityEngine;

public class ThirdPersonTurretCamera : MonoBehaviour
{
    [SerializeField] private Transform turretHead;

    [SerializeField]
    private Vector3 offset =
        new Vector3(0f, 2.35f, -8f);

    [SerializeField] private float followSpeed = 20f;
    [SerializeField] private float rotationSpeed = 20f;

    private void LateUpdate()
    {
        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition()
    {
        Vector3 desiredPosition =
            turretHead.position +
            turretHead.rotation * offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            Time.deltaTime * followSpeed);
    }

    private void UpdateRotation()
    {
        Quaternion targetRotation =
            Quaternion.LookRotation(
                turretHead.position - transform.position);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed);
    }
}