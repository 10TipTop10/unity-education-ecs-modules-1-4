using System.Collections;
using UnityEngine;

public class WaveAlertView : MonoBehaviour
{
    [SerializeField] private MissionController _missionController;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _interval = 0.2f;
    [SerializeField] private float _intensity = 0.3f;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        _missionController.StateChanged += OnStateChanged;
        OnStateChanged(_missionController.CurrentState);
    }

    private void OnStateChanged(MissionState state)
    {
        StopBlinking();

        if (state == MissionState.WaveIncoming)
        {
            _coroutine = StartCoroutine(BlinkCoroutine());
        }

        else
        {
            _canvasGroup.alpha = 0f;
        }
    }

    private void StopBlinking()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _canvasGroup.alpha = 0f;
    }

    private IEnumerator BlinkCoroutine()
    {
        WaitForSeconds interval = new WaitForSeconds(_interval);

        while (_missionController.CurrentState == MissionState.WaveIncoming)
        {
            _canvasGroup.alpha = 1f;
            yield return interval;

            _canvasGroup.alpha = _intensity;
            yield return interval;
        }

        _canvasGroup.alpha = 0f;
        _coroutine = null;
    }


    private void OnDisable()
    {
        _missionController.StateChanged -= OnStateChanged;
        StopBlinking();
    }
}
