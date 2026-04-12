using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControler : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    private const string VOLUME_PREF = "MasterVolume";

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_PREF, 0f);
        SetVolume(savedVolume);
        volumeSlider.value = savedVolume;
    }

    private void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(VOLUME_PREF, volume);
        PlayerPrefs.Save();
    }
}
