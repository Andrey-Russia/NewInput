using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private GameObject _character1;
    [SerializeField] private GameObject _character2;
    [SerializeField] private float _typeSpeed = 0.05f;
    [SerializeField] private float _betweenMessage = 3f;

    private CancellationTokenSource _cts = new();
    private List<(string message, int speaker)> dialogeLineas = new()
    {
        ("Привет, как твои дела?", 1),
        ("Да нормально всё, спасибо.", 2),
        ("Собираешься завтра в колледж?", 1),
        ("Да, конечно, нельзя математику пропускать.", 2),
        ("Может, вместе пойдем?", 1),
        ("Хорошая идея, встретимся около метро.", 2)
    };

    private int _currentLineIndex = 0;

    async void Start()
    {
        await ShowNextMessage(_cts.Token);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            ShowNextMessageInstality();
        else if (Input.GetMouseButton(1))
            TogglePause();
    }

    async Task ShowNextMessage(CancellationToken cancellationToken)
    {
        Debug.Log("Starting ShowNextMessage");
        while (_currentLineIndex < dialogeLineas.Count)
        {
            var line = dialogeLineas[_currentLineIndex++];
            SetCharacterImage(line.speaker);

            var message = line.message;
            for (int i = 0; i < message.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Debug.Log("Cancellation requested");
                    break;
                }

                _textField.text += message[i].ToString();
                await UniTask.Delay((int)(_typeSpeed * 1000), cancellationToken: cancellationToken);
            }
            await UniTask.Delay((int)(_betweenMessage * 1000), cancellationToken: cancellationToken);
        }
        Debug.Log("Finished ShowNextMessage");
    }

    void ShowNextMessageInstality()
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new();
        ShowNextMessage(_cts.Token);
    }

    void TogglePause()
    {
        if (_cts.IsCancellationRequested)
        {
            _cts.Dispose();
            _cts = new CancellationTokenSource();
            ShowNextMessage(_cts.Token);
        }
        else
            _cts.Cancel();
    }

    void SetCharacterImage(int speaker)
    {
        _character1.SetActive(speaker == 1);
        _character2.SetActive(speaker == 2);
    }
}