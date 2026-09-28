using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WaveSettings[] _waves;
    [SerializeField] private EnemyRoute _route;
    [SerializeField] private Durability _baseTarget;
    [SerializeField] private Durability _playerTarget;
    [SerializeField] private ScrapStorage _scrapStorage;
    [SerializeField] private float _waveIncomingDuration = 2f;
    private readonly List<GameObject> _aliveEnemies = new();
    private bool _isPreparing;
    private bool _startWaveRequested;

    public event Action<int, int, float> PreparationStarted;
    public event Action<int, int> WaveIncomingStarted;
    public event Action<int, int> WaveStarted;
    public event Action AllWavesCompleted;


    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    public void SkipPreparation()
    {
        if (!_isPreparing) return;

        _startWaveRequested = true;
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);

        enemy.GetComponent<EnemyMovement>().SetRoute(_route);
        enemy.GetComponent<EnemyAttack>().SetTargets(_baseTarget, _playerTarget);
        if(enemy.TryGetComponent(out ScrapReward reward))
        {
            reward.SetStorage(_scrapStorage);
        }
        _aliveEnemies.Add(enemy);
    }

    private IEnumerator SpawnWaves()
    {
        for (int waveIndex = 0; waveIndex < _waves.Length; waveIndex++)
        {
            WaveSettings wave = _waves[waveIndex];

            int waveNumber = waveIndex + 1;

            PreparationStarted?.Invoke(waveNumber, _waves.Length, wave.PreparationTime);

            _isPreparing = true;
            _startWaveRequested = false;

            float preparationEndTime = Time.time + wave.PreparationTime;

            while (Time.time < preparationEndTime && !_startWaveRequested)
            {
                if(_baseTarget==null || _baseTarget.CurrentDurability <= 0)
                {
                    _isPreparing = false;
                    yield break;
                }

                yield return null;
            }

            _isPreparing = false;

            if (_baseTarget == null || _baseTarget.CurrentDurability <= 0) yield break;

            WaveIncomingStarted?.Invoke(waveNumber, _waves.Length);

            yield return new WaitForSeconds(_waveIncomingDuration);

            if (_baseTarget == null || _baseTarget.CurrentDurability <= 0) yield break;

            WaveStarted?.Invoke(waveNumber, _waves.Length);

            for (int i = 0; i < wave.EnemyCount; i++)
            {
                if (_baseTarget == null || _baseTarget.CurrentDurability <= 0) yield break;
                
                SpawnEnemy(wave.EnemyPrefab);

                if (i < wave.EnemyCount - 1)
                {
                    yield return new WaitForSeconds(wave.SpawnInterval);
                }
            }
            while(_aliveEnemies.Count > 0)
            {
                if (_baseTarget == null || _baseTarget.CurrentDurability <= 0) yield break;

                _aliveEnemies.RemoveAll(enemy => enemy == null);

                yield return null;
            }
        }
        AllWavesCompleted?.Invoke();
    }
}
