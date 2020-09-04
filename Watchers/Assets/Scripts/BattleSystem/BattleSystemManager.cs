using System;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.BattleSystem
{
    public class BattleSystemManager : MonoBehaviour
    {
        public event EventHandler OnWaveStarted;
        public event EventHandler OnWaveEnded;
        public event EventHandler OnEnemyNumbersReduced;
        public int WaveNumber { get; private set; } = 1;
        public int NumberOfEnemiesRemaining { get; private set; }

        [SerializeField] private Transform[] _enemySpawnPoints;
        [SerializeField] private GameObject[] _spawnableEnemies;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _waveStartSound;
        [SerializeField] private AudioClip _waveEndSound;
        private BattleState _battleState;
        private float _difficultyModifier = 1f;
        private int _minNumOfEnemies = 5;
        private int _maxNumOfEnemies = 10;

        private void Awake()
        {
            _battleState = BattleState.Idle;
        }

        private void Update()
        {
            if (Input.GetButtonDown("Interact"))
            {
                if (_battleState == BattleState.Active) return;

                StartWave();
            }
        }

        public void StartWave()
        {
            Debug.Log($"Wave {WaveNumber} started!");
            PlayWaveStartSound();

            _battleState = BattleState.Active;
            InitializeWave(_minNumOfEnemies, _maxNumOfEnemies);
            OnWaveStarted?.Invoke(this, EventArgs.Empty);
        }

        public void EndWave()
        {
            Debug.Log($"Wave {WaveNumber} ended!");
            PlayWaveEndSound();

            _battleState = BattleState.Idle;
            WaveNumber++;

            IncrementWaveDifficulty(_difficultyModifier);
            OnWaveEnded?.Invoke(this, EventArgs.Empty);
        }

        private void IncrementWaveDifficulty(float difficultyModifier)
        {
            _difficultyModifier += (WaveNumber / 10f);

            _minNumOfEnemies += WaveNumber;
            _minNumOfEnemies = Mathf.Clamp(_minNumOfEnemies, 5, 50);
            _maxNumOfEnemies += WaveNumber;
            _maxNumOfEnemies = Mathf.Clamp(_maxNumOfEnemies, 10, 100);
        }

        private void InitializeWave(int minNumberOfEnemies, int maxNumberOfEnemies)
        {
            int numberOfEnemiesToSpawn = UnityEngine.Random.Range(minNumberOfEnemies, maxNumberOfEnemies + 1);
            NumberOfEnemiesRemaining = numberOfEnemiesToSpawn;

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
            // This loop ensures that we only return GameObjects which implement the ISpawnable interface. Probably not the best implementation as having an array with no ISpawnable objects will result
            // in an infinite loop.
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
            NumberOfEnemiesRemaining--;
            OnEnemyNumbersReduced?.Invoke(this, EventArgs.Empty);

            if (NumberOfEnemiesRemaining == 0)
            {
                EndWave();
            }
        }

        private void PlayWaveStartSound()
        {
            if (_audioSource != null && _waveStartSound != null)
            {
                _audioSource.PlayOneShot(_waveStartSound);
            }
        }

        private void PlayWaveEndSound()
        {
            if (_audioSource != null && _waveEndSound != null)
            {
                _audioSource.PlayOneShot(_waveEndSound);
            }
        }
    }
}
