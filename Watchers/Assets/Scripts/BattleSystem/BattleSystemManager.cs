using System;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.BattleSystem
{
    public class BattleSystemManager : MonoBehaviour
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
            if (Input.GetButtonDown("Interact"))
            {
                if (_battleState == BattleState.Active) return;

                StartWave();
            }
        }

        public void StartWave()
        {
            Debug.Log($"Wave {_waveNumber} started!");

            _battleState = BattleState.Active;
            InitializeWave(_minNumOfEnemies, _maxNumOfEnemies);
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

        private void InitializeWave(int minNumberOfEnemies, int maxNumberOfEnemies)
        {
            int numberOfEnemiesToSpawn = UnityEngine.Random.Range(minNumberOfEnemies, maxNumberOfEnemies + 1);
            numberOfEnemiesRemaining = numberOfEnemiesToSpawn;

            StartCoroutine(SpawnEnemies(numberOfEnemiesToSpawn, 1.0f));
        }

        private IEnumerator SpawnEnemies(int amount, float delay)
        {
            for (int i = 0; i < amount; i++)
            {
                var objectToSpawn = Instantiate(GenerateRandomEnemy(), GenerateRandomSpawnPoint(), Quaternion.identity);
                var spawnableEnemy = objectToSpawn.GetComponent<ISpawnable>();

                if (spawnableEnemy != null)
                {
                    spawnableEnemy.OnSpawnableDestroyed += ISpawnable_OnSpawnableDestroyed;
                    // We don't want all enemies to be spawned at once so we delay their instantiation.
                    yield return new WaitForSeconds(delay);
                }
            }
        }

        private Vector3 GenerateRandomSpawnPoint()
        {
            var randomIndex = UnityEngine.Random.Range(0, _enemySpawnPoints.Length);
            return _enemySpawnPoints[randomIndex].position;
        }

        private GameObject GenerateRandomEnemy()
        {
            // This loop ensures that we only return GameObjects which implement the ISpawnable interface.
            while (true)
            {
                var randomIndex = UnityEngine.Random.Range(0, _spawnableEnemies.Length);
                var spawnableEnemy = _spawnableEnemies[randomIndex];

                if (spawnableEnemy.GetComponent<ISpawnable>() != null)
                {
                    return spawnableEnemy;
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
}
