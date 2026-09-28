using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionView : MonoBehaviour
{
    [SerializeField] private MissionController _missionController;

    [Header("Текст")]
    [SerializeField] private TMP_Text _headerText;
    [SerializeField] private TMP_Text _stateText;
    [SerializeField] private TMP_Text _timeText;

    [Header("Объекты")]
    [SerializeField] private GameObject _timeIcon;
    [SerializeField] private GameObject _nextWaveButton;
    [SerializeField] private GameObject _restartButton;

    private Coroutine _countdownCoroutine;

    private void OnEnable()
    {
        _missionController.StateChanged += UpdateView;
        UpdateView(_missionController.CurrentState);
    }

    private void UpdateView(MissionState state)
    {
        StopCountdown();

        _restartButton.SetActive(state == MissionState.Victory || state == MissionState.Defeat);

        switch (state)
        {
            case MissionState.NonStarted:
                ShowNonStarted();
                break;
            case MissionState.Preparation:
                ShowPreparation();
                break;
            case MissionState.WaveIncoming:
                ShowWaveIncoming();
                break;
            case MissionState.Wave:
                ShowWave();
                break;
            case MissionState.Victory:
                ShowVictory();
                break;
            case MissionState.Defeat:
                ShowDefeat();
                break;
        }
    }

    public void RestartMission()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ShowNonStarted()
    {
        _headerText.text = "МИССИЯ";
        _stateText.text = string.Empty;
        SetPreparationInfoVisible(false);
    }

    private void ShowPreparation()
    {
        UpdateWaveHeader();

        _stateText.text = "<b>ПОДГОТОВКА</b>\n<size=17><color=#82939C>Подготовьтесь к следующей волне</color></size>";

        SetPreparationInfoVisible(true);

        _countdownCoroutine = StartCoroutine(UpdatePreparationCountdown());

    }

    private void ShowWaveIncoming()
    {
        UpdateWaveHeader();

        _stateText.text = "<b>ВОЛНА ПРИБЛИЖАЕТСЯ</b>\n<size=17><color=#82939C>Зафиксировано движение противника</color></size>";

        SetPreparationInfoVisible(false);
    }

    private void ShowWave()
    {
        UpdateWaveHeader();

        _stateText.text = "<b>ЗАЩИЩАЙТЕ МОДУЛЬ</b>\n<size=17><color=#82939C>Уничтожьте всех противников</color></size>";

        SetPreparationInfoVisible(false);
    }

    private void ShowVictory()
    {
        _headerText.text = "МИССИЯ ЗАВЕРШЕНА";

        _stateText.text = "<b>ИСКРа</b>\n<size=17><color=#82939C>Все противники уничтожены</color></size>";

        SetPreparationInfoVisible(false);
    }

    private void ShowDefeat()
    {
        _headerText.text = "МИССИЯ ПРОВАЛЕНА";

        _stateText.text = "<b>МОДУЛЬ УНИЧТОЖЕН</b>\n<size=17><color=#82939C>Задание завершено неудачей</color></size>";

        SetPreparationInfoVisible(false);
    }

    private void StopCountdown()
    {
        if (_countdownCoroutine == null) return;

        StopCoroutine(_countdownCoroutine);
        _countdownCoroutine = null;
    }

    private void SetPreparationInfoVisible(bool setVisible)
    {
        _timeIcon.SetActive(setVisible);
        _timeText.gameObject.SetActive(setVisible);
        _nextWaveButton.SetActive(setVisible);
    }

    private void UpdateWaveHeader()
    {
        _headerText.text = $"ВОЛНА {_missionController.CurrentWave} / {_missionController.TotalWaves}";
    }

    private IEnumerator UpdatePreparationCountdown()
    {
        while (_missionController.CurrentState == MissionState.Preparation)
        {
            int totalSeconds = Mathf.CeilToInt(_missionController.PreparationTimeRemaining);

            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            _timeText.text = $"{minutes:00}:{seconds:00}";

            yield return null;
        }
    }

    private void OnDisable()
    {
        _missionController.StateChanged -= UpdateView;
        StopCountdown();
    }
}
