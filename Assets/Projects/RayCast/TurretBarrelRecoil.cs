using UnityEngine;

public class TurretBarrelRecoil : MonoBehaviour
{
    [SerializeField] private float recoilDistance = 0.5f;
    [SerializeField] private float recoilSpeed = 12f;
    [SerializeField] private float returnSpeed = 6f;

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
        _targetLocalPosition = _initialLocalPosition - Vector3.forward * recoilDistance;
        _isRecoiling = true;
    }

    private void AnimateRecoil()
    {
        if (_isRecoiling)
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                _targetLocalPosition,
                Time.deltaTime * recoilSpeed);

            if (Vector3.Distance(transform.localPosition, _targetLocalPosition) < 0.01f)
            {
                _targetLocalPosition = _initialLocalPosition;
                _isRecoiling = false;
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                _initialLocalPosition,
                Time.deltaTime * returnSpeed);
        }
    }
}