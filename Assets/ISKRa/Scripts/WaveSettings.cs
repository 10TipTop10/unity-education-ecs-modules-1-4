using System;
using UnityEngine;

[Serializable]
public class WaveSettings
{
    public GameObject EnemyPrefab;
    public int EnemyCount = 3;
    public float SpawnInterval = 2f;
    public float PreparationTime = 3f;
}
