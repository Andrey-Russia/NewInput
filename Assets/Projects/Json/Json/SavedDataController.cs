using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class SavedDataController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    private const string _jsonPath = "SoundData";
    private float _previousVolume;

    private void Start()
    {
        LoadVolume();
        _previousVolume = _audioSource.volume;
    }

    private void Update()
    {
        if (Mathf.Abs(_audioSource.volume - _previousVolume) > 0.01f)
        {
            SaveVolume();
            _previousVolume = _audioSource.volume;
        }
    }

    private void LoadVolume()
    {
        Resources.UnloadAsset(Resources.Load(_jsonPath));
        TextAsset textAsset = Resources.Load<TextAsset>(_jsonPath);
        if (textAsset != null)
        {
            float volume = JsonConvert.DeserializeObject<float>(textAsset.text);
            _audioSource.volume = volume;
            Debug.Log($"Загружена громкость: {volume}");
        }
        else
        {
            Debug.Log("Файл SoundData.json не найден. Создаём новый.");
            SaveVolume();
        }
    }

    private void SaveVolume()
    {
        string json = JsonConvert.SerializeObject(_audioSource.volume);
        string fullPath = Path.Combine(Application.dataPath, $"Resources/{_jsonPath}.json");
        File.WriteAllText(fullPath, json);
        Debug.Log($"Сохранена громкость: {_audioSource.volume}");
    }
}