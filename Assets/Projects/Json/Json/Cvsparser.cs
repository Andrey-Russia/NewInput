using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Cvsparser : MonoBehaviour
{
    const string CsvPath = "Tablica";
    const string OutputPath = "DataBase";

    void Start()
    {
        ParseCsvAndSerialize();
    }

    void ParseCsvAndSerialize()
    {
        TextAsset csvResource = Resources.Load<TextAsset>(CsvPath);
        if (csvResource == null)
        {
            Debug.LogError("Ошибка: CSV-файл не найден!");
            return;
        }

        StringReader reader = new StringReader(csvResource.text);
        List<DataEntry> entries = new List<DataEntry>();

        reader.ReadLine();

        while (true)
        {
            string line = reader.ReadLine();
            if (line == null || line.Trim().Length == 0) break;

            string[] columns = line.Split(';', StringSplitOptions.RemoveEmptyEntries);

            if (columns.Length >= 2)
            {
                string name = columns[0].Trim('"');

                if (int.TryParse(columns[1], out int number))
                {
                    entries.Add(new DataEntry(name, number));
                    Debug.Log($"Добавлена запись: Имя - {name}, Номер - {number}");
                }
                else
                {
                    Debug.LogWarning($"Ошибка: неверный формат числа '{columns[1]}'");
                }
            }
            else
            {
                Debug.LogWarning($"Ошибка: недостаточно колонок в строке '{line}'");
            }
        }

        if (entries.Count == 0)
        {
            Debug.LogWarning("Ошибка: данные не найдены или некорректны!");
            return;
        }

        DataTable dataTable = new DataTable(entries);
        string jsonString = JsonConvert.SerializeObject(dataTable, Formatting.Indented);

        string outputFullPath = Path.Combine(Application.dataPath, $"Resources/{OutputPath}.json");
        File.WriteAllText(outputFullPath, jsonString);
        Debug.Log("Файл JSON успешно сохранён.");
    }
}
