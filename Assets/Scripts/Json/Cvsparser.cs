using System.IO;
using System.Globalization;
using UnityEngine;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;

public class Cvsparser : MonoBehaviour
{
    const string CvsPath = "Tablica";
    const string Out = "DataBase";

    private void Start()
    {
        ParseCsvAndSerialize();
    }

    void ParseCsvAndSerialize()
    {
        TextAsset text = Resources.Load<TextAsset>(CvsPath);
        if (text == null)
        {
            Debug.LogError("Ошибка: CSV-файл не найден!");
            return;
        }
        else
        {
            Debug.Log("CSV-файл успешно загружен.");
        }

        StringReader reader = new StringReader(text.text);
        List<DataEntry> entries = new List<DataEntry>();

        reader.ReadLine(); // Пропускаем заголовки

        while (true)
        {
            string line = reader.ReadLine();
            if (line == null || line.Length == 0) break;

            Debug.Log($"Читаем строку: {line}");

            string[] columns = line.Split(';');

            if (columns.Length >= 2)
            {
                string name = columns[0].Trim('"');
                int number;

                if (int.TryParse(columns[1], out number))
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
        else
        {
            Debug.Log($"Количество записей: {entries.Count}");
        }

        DataTable table = new DataTable(entries);
        string json = JsonConvert.SerializeObject(table, Formatting.Indented);
        string fullOutputPath = Application.dataPath + "/Resources/" + Out;
        Debug.Log($"Путь к файлу: {fullOutputPath}");

        File.WriteAllText(fullOutputPath, json);
        Debug.Log("Файл JSON успешно сохранён.");
    }
}