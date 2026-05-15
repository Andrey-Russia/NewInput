using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform turretHead;
    [SerializeField] private TurretBarrelRecoil turretBarrel;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile projectilePrefab;

    [SerializeField] private float projectileSpeed = 35f;
    [SerializeField] private float maxShootDistance = 200f;

    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float maxRotationAngle = 80f;

    [SerializeField] private LayerMask targetLayerMask;

    private RaycastHit _currentHit;
    private bool _hasHit;

    public RaycastHit CurrentHit => _currentHit;
    public bool HasHit => _hasHit;

    private void Update()
    {
        RotateTurretToCursor();
        PerformForwardRaycast();
        HandleShoot();
    }

    private void RotateTurretToCursor()
    {
        Ray ray =
            playerCamera.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane =
            new Plane(Vector3.up, Vector3.zero);

        if (!groundPlane.Raycast(ray, out float distance))
        {
            return;
        }

        Vector3 targetPoint =
            ray.GetPoint(distance);

        Vector3 direction =
            targetPoint - turretHead.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        turretHead.rotation = Quaternion.Slerp(
            turretHead.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed);

        ClampRotation();
    }

    private void ClampRotation()
    {
        Vector3 localEuler =
            turretHead.localEulerAngles;

        float yAngle = localEuler.y;

        if (yAngle > 180f)
        {
            yAngle -= 360f;
        }

        yAngle = Mathf.Clamp(
            yAngle,
            -maxRotationAngle,
            maxRotationAngle);

        turretHead.localEulerAngles =
            new Vector3(0f, yAngle, 0f);
    }

    private void PerformForwardRaycast()
    {
        Ray ray = new Ray(
            firePoint.position,
            firePoint.forward);

        if (Physics.Raycast(
                ray,
                out RaycastHit hitInfo,
                maxShootDistance,
                targetLayerMask))
        {
            _currentHit = hitInfo;
            _hasHit = true;
        }
        else
        {
            _hasHit = false;
        }
    }

    private void HandleShoot()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        Projectile projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation);

        projectile.Initialize(projectileSpeed);

        turretBarrel.PlayRecoil();
    }

    private void OnDrawGizmos()
    {
        if (firePoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;

        Gizmos.DrawRay(
            firePoint.position,
            firePoint.forward * maxShootDistance);

        if (_hasHit)
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawSphere(
                _currentHit.point,
                0.4f);
        }
    }
}