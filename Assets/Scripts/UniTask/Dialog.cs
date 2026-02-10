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
    private List<(string message, int speaker)> dialogLines = new()
    {
        ("Привет, как твои дела?", 1),
        ("Да нормально всё, спасибо.", 2),
        ("Собираешься завтра в колледж?", 1),
        ("Да, конечно, нельзя математику пропускать.", 2),
        ("Может, вместе пойдем?", 1),
        ("Хорошая идея, встретимся около метро.", 2)
    };

    private int _currentLineIndex = 0;
    private bool isPaused = false;
    private bool isTyping = false; // чтобы отслеживать, идет ли вывод текста

    async void Start()
    {
        await ShowNextMessage(_cts.Token);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Левый клик: показать всю строку
            ShowFullLine();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            // Правый клик: остановить/продолжить
            TogglePause();
        }
    }

    private async void ShowFullLine()
    {
        if (_currentLineIndex >= dialogLines.Count || isTyping)
            return;

        // Отменяем текущую задачу
        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();

        var line = dialogLines[_currentLineIndex];
        SetCharacterImage(line.speaker);

        string message = line.message;
        _textField.text = message; // показываем всю строку сразу

        isTyping = false; // т.к. полностью показали строку

        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_betweenMessage), cancellationToken: _cts.Token);
        }
        catch (OperationCanceledException)
        {
            // Игнорируем отмену
        }

        _currentLineIndex++;
        await ShowNextMessage(_cts.Token);
    }

    async Task ShowNextMessage(CancellationToken cancellationToken)
    {
        Debug.Log("Starting ShowNextMessage");
        while (_currentLineIndex < dialogLines.Count)
        {
            if (isPaused)
            {
                try
                {
                    await UniTask.WaitUntil(() => !isPaused, cancellationToken: cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return; // при отмене выхода
                }
            }

            var line = dialogLines[_currentLineIndex];
            SetCharacterImage(line.speaker);

            var message = line.message;
            _textField.text = "";
            isTyping = true;

            for (int i = 0; i < message.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested || isPaused)
                {
                    Debug.Log("Cancellation or pause requested");
                    isTyping = false;
                    return;
                }

                try
                {
                    _textField.text += message[i].ToString();
                    await UniTask.Delay(TimeSpan.FromSeconds(_typeSpeed), cancellationToken: cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    // Игнорируем отмену
                    isTyping = false;
                    return;
                }
            }

            isTyping = false;

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_betweenMessage), cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Игнорируем отмену
            }

            _currentLineIndex++;
        }

        Debug.Log("Finished ShowNextMessage");
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (!isPaused)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();

            // Продолжаем выполнение
            Task.Run(async () => await ShowNextMessage(_cts.Token));
        }
    }

    private void SetCharacterImage(int speaker)
    {
        _character1.SetActive(speaker == 1);
        _character2.SetActive(speaker == 2);
    }
}