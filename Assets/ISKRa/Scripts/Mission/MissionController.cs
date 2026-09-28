using System;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private Durability _baseDurability;
    private float _preparationEndTime;

    public int CurrentWave { get; private set; }
    public int TotalWaves { get; private set; }
    public float PreparationDuration { get; private set; }
    public MissionState CurrentState { get; private set; }
    public event Action<MissionState> StateChanged;

    private void OnEnable()
    {
        _enemySpawner.PreparationStarted += OnPreparationStarted;
        _enemySpawner.WaveStarted += OnWaveStarted;
        _enemySpawner.AllWavesCompleted += OnAllWavesCompleted;
        _enemySpawner.WaveIncomingStarted += OnWaveIncomingStarted;

        _baseDurability.Depleted += OnBaseDepleted;

    }

    private void SetState(MissionState state)
    {
        if(CurrentState == state) return;

        CurrentState = state;
        StateChanged?.Invoke(CurrentState);
        Debug.Log($"Состояние миссии: {CurrentState}"); 
    }

    private void OnPreparationStarted(int waveNumber, int totalWaves, float duration)
    {
        CurrentWave = waveNumber;
        TotalWaves = totalWaves;
        PreparationDuration = duration;

        _preparationEndTime = Time.time + duration;

        SetState(MissionState.Preparation);
    }

    private void OnWaveIncomingStarted(int waveNumber, int totalWaves)
    {
        CurrentWave = waveNumber;
        TotalWaves = totalWaves;

        SetState(MissionState.WaveIncoming);
    }

    private void OnWaveStarted(int waveNumber, int totalWaves)
    {
        CurrentWave = waveNumber;
        TotalWaves = totalWaves;

        SetState(MissionState.Wave);
    }

    private void OnAllWavesCompleted()
    {
        if (CurrentState == MissionState.Defeat) return;
        SetState(MissionState.Victory);
    }

    private void OnBaseDepleted()
    {
        SetState(MissionState.Defeat);
    }


    public void StartWaveEarly()
    {
        if (CurrentState != MissionState.Preparation) return;

        _enemySpawner.SkipPreparation();
    }

    public float PreparationTimeRemaining
    {
        get
        {
            if (CurrentState != MissionState.Preparation) return 0f;

            return Mathf.Max(0f, _preparationEndTime - Time.time);
        }
    }

    private void OnDisable()
    {
        _enemySpawner.PreparationStarted -= OnPreparationStarted;
        _enemySpawner.WaveStarted -= OnWaveStarted;
        _enemySpawner.AllWavesCompleted -= OnAllWavesCompleted;
        _enemySpawner.WaveIncomingStarted -= OnWaveIncomingStarted;

        _baseDurability.Depleted -= OnBaseDepleted;
    }
}
