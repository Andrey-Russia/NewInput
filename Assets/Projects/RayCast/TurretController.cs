using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private Transform turretHead;
    [SerializeField] private Transform turretBarrel;
    [SerializeField] private TurretBarrelRecoil barrelRecoil;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile projectilePrefab;

    [SerializeField] private float projectileSpeed = 35f;
    [SerializeField] private float maxShootDistance = 200f;

    [SerializeField] private float horizontalRotationSpeed = 90f;

    [SerializeField] private float verticalMoveSpeed = 2f;
    [SerializeField] private float minBarrelY = 0f;
    [SerializeField] private float maxBarrelY = 1.5f;

    [SerializeField] private LayerMask targetLayerMask;

    private RaycastHit _currentHit;
    private bool _hasHit;

    private Vector3 _initialBarrelLocalPosition;

    public RaycastHit CurrentHit => _currentHit;
    public bool HasHit => _hasHit;

    private void Awake()
    {
        _initialBarrelLocalPosition =
            turretBarrel.localPosition;
    }

    private void Update()
    {
        RotateHorizontally();
        MoveBarrelVertically();

        PerformForwardRaycast();
        HandleShoot();
    }

    private void RotateHorizontally()
    {
        float input = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            input = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            input = 1f;
        }

        turretHead.Rotate(
            Vector3.up,
            input * horizontalRotationSpeed * Time.deltaTime,
            Space.World);
    }

    private void MoveBarrelVertically()
    {
        float input = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            input = 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            input = -1f;
        }

        Vector3 localPosition =
            turretBarrel.localPosition;

        localPosition.y +=
            input *
            verticalMoveSpeed *
            Time.deltaTime;

        localPosition.y = Mathf.Clamp(
            localPosition.y,
            minBarrelY,
            maxBarrelY);

        turretBarrel.localPosition = new Vector3(
            _initialBarrelLocalPosition.x,
            localPosition.y,
            _initialBarrelLocalPosition.z);
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

        if (barrelRecoil != null)
        {
            barrelRecoil.PlayRecoil();
        }
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
                0.35f);
        }
    }
}