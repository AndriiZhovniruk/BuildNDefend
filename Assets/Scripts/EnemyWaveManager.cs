using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance { get; private set; }

    public event EventHandler OnWaveNumberChanged;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private float enemySpawnTimeDecrease;
    private float maxEnemySpawnTime;
    private int remainingEnemySpawnAmount;
    private Vector3 spawnPosition;
    private State state;
    private int waveNumber;
    private int waveDifficultyIncrease;

    [SerializeField] private List<Transform> spawnPositionTransformList;
    [SerializeField] private Transform nextWaveSpawnPositionTransform;
    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave,
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        state = State.WaitingToSpawnNextWave;
        spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPositionTransform.position = spawnPosition;
        nextEnemySpawnTimer = 20f;
        enemySpawnTimeDecrease = 0.01f;
        maxEnemySpawnTime = .6f;
        waveDifficultyIncrease = 0;
    }
    private void Update()
    {
        WaveSpawnTimer();
    }
    private void SpawnWave()
    {
        if(waveNumber % 10 == 0)
        {
            waveDifficultyIncrease++;
        }
        remainingEnemySpawnAmount = 5 + (2 + waveDifficultyIncrease) *waveNumber;
        state = State.SpawningWave;
        waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    private void WaveSpawnTimer()
    {
        switch (state)
        {
            case State.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer < 0f)
                {
                    //SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyWaveStarting);
                    SpawnWave();
                }
                break;

            case State.SpawningWave:
                
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f)
                    {
                        maxEnemySpawnTime -= enemySpawnTimeDecrease;
                        maxEnemySpawnTime = Mathf.Clamp(maxEnemySpawnTime, 0.1f, enemySpawnTimeDecrease);
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0f, maxEnemySpawnTime);
                        Enemy.Create(spawnPosition + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(0f, 10f));
                        remainingEnemySpawnAmount--;

                        if(remainingEnemySpawnAmount <= 0)
                        {
                            state = State.WaitingToSpawnNextWave;
                            spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
                            nextWaveSpawnPositionTransform.position = spawnPosition;
                            nextWaveSpawnTimer = 20f;
                        }
                    }
                }
                break;
        }

    }
    public int GetWaveNumber()
    {
        return waveNumber;
    }
    public float GetNextWaveSpawnTimer()
    {
        return nextWaveSpawnTimer;
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
    public int RemainingEnemySpawnAmount()
    {
        return remainingEnemySpawnAmount;
    }
}
