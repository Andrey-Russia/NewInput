using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private GameObject _character1;
    [SerializeField] private GameObject _character2;
    [SerializeField] private float _typeSpeed = 0.05f;
    [SerializeField] private float _betweenMessage = 3f;

    private CancellationTokenSource _cts;
    private bool isPaused;
    private bool skipTyping;

    private int _currentLineIndex;

    private readonly List<(string message, int speaker)> dialogLines = new()
    {
        ("Привет, как твои дела?", 1),
        ("Да нормально всё, спасибо.", 2),
        ("Собираешься завтра в колледж?", 1),
        ("Да, конечно, нельзя математику пропускать.", 2),
        ("Может, вместе пойдем?", 1),
        ("Хорошая идея, встретимся около метро.", 2)
    };

    private void Start()
    {
        _cts = new CancellationTokenSource();
        PlayDialog(_cts.Token).Forget();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            skipTyping = true;

        if (Input.GetMouseButtonDown(1))
            isPaused = !isPaused;
    }

    private async UniTaskVoid PlayDialog(CancellationToken token)
    {
        while (_currentLineIndex < dialogLines.Count)
        {
            await WaitIfPaused(token);

            var line = dialogLines[_currentLineIndex];
            SetCharacterImage(line.speaker);

            skipTyping = false;
            _textField.text = "";

            await TypeText(line.message, token);

            await WaitDelayOrSkip(token);

            _currentLineIndex++;
        }
    }

    private async UniTask TypeText(string text, CancellationToken token)
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (skipTyping)
            {
                _textField.text = text;
                return;
            }

            await WaitIfPaused(token);

            _textField.text += text[i];
            await UniTask.Delay(TimeSpan.FromSeconds(_typeSpeed), cancellationToken: token);
        }
    }

    private async UniTask WaitDelayOrSkip(CancellationToken token)
    {
        float timer = 0f;

        while (timer < _betweenMessage)
        {
            if (skipTyping)
                return;

            await WaitIfPaused(token);

            timer += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    private async UniTask WaitIfPaused(CancellationToken token)
    {
        while (isPaused)
            await UniTask.Yield(token);
    }

    private void SetCharacterImage(int speaker)
    {
        _character1.SetActive(speaker == 1);
        _character2.SetActive(speaker == 2);
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
