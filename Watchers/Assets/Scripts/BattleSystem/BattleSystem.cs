using System;
using UnityEngine;
using Assets.Scripts.BattleSystem;

public class BattleSystem : MonoBehaviour
{
    public event EventHandler OnWaveStarted;
    public event EventHandler OnWaveEnded;
    public int numberOfEnemiesRemaining { get; set; }

    [SerializeField] private Transform[] _enemySpawnPoints;
    [SerializeField] private GameObject[] _spawnableEnemies;
    private BattleState _battleState;
    private int _waveNumber = 1;
    private float _difficultyModifier = 1f;
    private int _minNumOfEnemies = 5;
    private int _maxNumOfEnemies = 10;

    private void Awake()
    {
        _battleState = BattleState.Idle;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_battleState == BattleState.Active) return;

            StartWave();
        }
    }

    public void StartWave()
    {
        Debug.Log($"Wave {_waveNumber} started!");

        _battleState = BattleState.Active;
        SpawnEnemies(_minNumOfEnemies, _maxNumOfEnemies);
        OnWaveStarted?.Invoke(this, EventArgs.Empty);
    }

    public void EndWave()
    {
        Debug.Log($"Wave {_waveNumber} ended!");

        _battleState = BattleState.Idle;
        _waveNumber++;

        IncrementWaveDifficulty(_difficultyModifier);
        OnWaveEnded?.Invoke(this, EventArgs.Empty);
    }

    private void IncrementWaveDifficulty(float difficultyModifier)
    {
        _difficultyModifier += (_waveNumber / 10f);

        _minNumOfEnemies += _waveNumber;
        _minNumOfEnemies = Mathf.Clamp(_minNumOfEnemies, 5, 50);
        _maxNumOfEnemies += _waveNumber;
        _maxNumOfEnemies = Mathf.Clamp(_maxNumOfEnemies, 10, 100);
    }

    private void SpawnEnemies(int minNumberOfEnemies, int maxNumberOfEnemies)
    {
        int numberOfEnemiesToSpawn = UnityEngine.Random.Range(minNumberOfEnemies, maxNumberOfEnemies + 1);
        int numberOfSpawns = _enemySpawnPoints.Length;

        for (int i = 0; i < numberOfEnemiesToSpawn / numberOfSpawns; i++)
        {
            foreach (var spawn in _enemySpawnPoints)
            {
                int unitToSpawn = UnityEngine.Random.Range(0, _spawnableEnemies.Length);
                var objectToSpawn = Instantiate(_spawnableEnemies[unitToSpawn], spawn.position, Quaternion.identity);

                var spawnableEnemy = objectToSpawn.GetComponent<ISpawnable>();

                if (spawnableEnemy != null)
                {
                    numberOfEnemiesRemaining++;
                    spawnableEnemy.OnSpawnableDestroyed += ISpawnable_OnSpawnableDestroyed;
                }
                else
                {
                    // We ensure that only those objects remain which implement the ISpawnable interface, otherwise we delete them and they do not count towards
                    // the number of remaining enemies in a wave.
                    Destroy(objectToSpawn);
                }
            }
        }
    }

    private void ISpawnable_OnSpawnableDestroyed(object sender, EventArgs e)
    {
        numberOfEnemiesRemaining--;

        if (numberOfEnemiesRemaining == 0)
        {
            EndWave();
        }
    }
}
