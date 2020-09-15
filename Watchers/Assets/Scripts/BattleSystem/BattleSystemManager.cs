using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Audio;

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
        [SerializeField] private AudioClip _waveMusicClip;

        private BattleState _battleState;
        private GameObject _musicPlayerInstance;
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
            PlayWaveStartSound();
            _musicPlayerInstance = CreateBattleSystemMusicPlayer();
            _musicPlayerInstance.GetComponent<MusicPlayer>().PlayMusic();

            _battleState = BattleState.Active;
            InitializeWave(_minNumOfEnemies, _maxNumOfEnemies);
            OnWaveStarted?.Invoke(this, EventArgs.Empty);
        }

        public void EndWave()
        {
            PlayWaveEndSound();
            _musicPlayerInstance.GetComponent<MusicPlayer>().FadeOutMusic();
            Destroy(_musicPlayerInstance, 1f);

            _battleState = BattleState.Idle;
            WaveNumber++;

            IncrementWaveDifficulty();
            OnWaveEnded?.Invoke(this, EventArgs.Empty);
        }

        private void IncrementWaveDifficulty()
        {
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

        // Should move these to a different component/class.
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

        private GameObject CreateBattleSystemMusicPlayer()
        {
            GameObject musicPlayerObject = new GameObject("BattleSystem_MusicPlayer");
            var musicPlayerComponent = musicPlayerObject.AddComponent<MusicPlayer>();
            var audioSourceComponent = musicPlayerObject.AddComponent<AudioSource>();
            musicPlayerComponent.Setup(true, audioSourceComponent, _waveMusicClip);

            return musicPlayerObject;
        }
    }
}