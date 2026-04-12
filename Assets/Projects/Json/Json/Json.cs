using Newtonsoft.Json;
using System;
using UnityEngine;

public class Json : MonoBehaviour
{
    const string JsonPath = "DataBase";

    void Start()
    {
        DeserializeFromJson();
    }

    void DeserializeFromJson()
    {
        Resources.UnloadAsset(Resources.Load(JsonPath));
        TextAsset textAsset = Resources.Load<TextAsset>(JsonPath);
        if (textAsset == null)
        {
            Debug.LogError("Ошибка: файл JSON не найден!");
            return;
        }

        try
        {
            DataTable table = JsonConvert.DeserializeObject<DataTable>(textAsset.text);

            if (table.Entries == null || table.Entries.Count == 0)
            {
                Debug.LogWarning("Ошибка: данные пусты или некорректны!");
                return;
            }

            foreach (var entry in table.Entries)
                Debug.Log($"Имя: {entry.Name}, Номер: {entry.Number}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка десериализации: {ex.Message}");
        }
    }
}
