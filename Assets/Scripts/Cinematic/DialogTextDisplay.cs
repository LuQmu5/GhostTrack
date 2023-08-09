using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class DialogTextDisplay : MonoBehaviour
{
    [SerializeField] private float _typeSpeed = 0.5f;
    [SerializeField] private float _timeToShowMessage = 2;
    [SerializeField] private TMP_Text _text;

    private Coroutine _coroutine;
    private float _currentMessageTimer;

    public event UnityAction DialogOver;

    private void OnEnable()
    {
        PlayerInput.SkipKeyPressed += OnSkipKeyPressed;
    }

    private void OnDisable()
    {
        PlayerInput.SkipKeyPressed -= OnSkipKeyPressed;
    }

    private void Update()
    {
        if (_coroutine != null && Input.GetKeyDown(KeyCode.Escape))
        {
            StopCoroutine(_coroutine);
            HandleEndOfDialog();
        }
    }

    private void HandleEndOfDialog()
    {
        _coroutine = null;
        _text.text = "";
        DialogOver?.Invoke();
    }

    public void StartDialog(string[] messages)
    {
        _coroutine = StartCoroutine(Printing(messages));
    }

    private IEnumerator Printing(string[] messages)
    {
        float delay = 1f;

        yield return new WaitForSecondsRealtime(delay);

        foreach (var message in messages)
        {
            yield return new WaitForEndOfFrame();

            _text.text = "";
            int i = 0;

            while (_text.text != message)
            {
                _text.text += message[i];
                i++;

                yield return new WaitForEndOfFrame();

                if (Input.GetKeyDown(PlayerInput.Instance.KeysMap[Keys.Skip]))
                    break;
            }

            _text.text = message;

            yield return new WaitForEndOfFrame();

            _currentMessageTimer = _timeToShowMessage;

            while (_currentMessageTimer > 0)
            {
                _currentMessageTimer -= Time.unscaledDeltaTime;

                yield return null;
            }
        }

        HandleEndOfDialog();
    }

    private void OnSkipKeyPressed()
    {
        _currentMessageTimer = 0;
    }
}
