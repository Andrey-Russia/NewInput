using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image _progressBarImage;
    [SerializeField] private float _timerDuration = 10f;

    void Start()
    {
        _progressBarImage.fillAmount = 1f;

        DOTween.To(() => _progressBarImage.fillAmount,
                   x => _progressBarImage.fillAmount = x,
                   0f, _timerDuration)
               .OnComplete(() =>
               {
                   Debug.Log("Timer finished!");
               });
    }
}
