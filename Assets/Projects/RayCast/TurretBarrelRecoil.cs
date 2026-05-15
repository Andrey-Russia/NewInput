using UnityEngine;

public class TurretBarrelRecoil : MonoBehaviour
{
    [SerializeField] private float recoilDistance = 0.2f;
    [SerializeField] private float recoilSpeed = 18f;
    [SerializeField] private float returnSpeed = 10f;

    private Vector3 _initialLocalPosition;
    private Vector3 _targetLocalPosition;

    private bool _isRecoiling;

    private void Awake()
    {
        _initialLocalPosition = transform.localPosition;
        _targetLocalPosition = _initialLocalPosition;
    }

    private void Update()
    {
        AnimateRecoil();
    }

    public void PlayRecoil()
    {
        _targetLocalPosition =
            _initialLocalPosition - Vector3.right * recoilDistance;

        _isRecoiling = true;
    }

    private void AnimateRecoil()
    {
        float speed =
            _isRecoiling
            ? recoilSpeed
            : returnSpeed;

        Vector3 targetPosition =
            _isRecoiling
            ? _targetLocalPosition
            : _initialLocalPosition;

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPosition,
            Time.deltaTime * speed);

        if (_isRecoiling &&
            Vector3.Distance(
                transform.localPosition,
                targetPosition) < 0.01f)
        {
            _isRecoiling = false;
        }
    }
}