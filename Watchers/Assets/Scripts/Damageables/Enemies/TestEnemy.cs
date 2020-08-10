using UnityEngine;

namespace Assets.Scripts.Damagables.Enemies
{
    public class TestEnemy : MonoBehaviour, IDamageable
    {
        // ToDo: Rewrite this to actual enemy classes with refined logic.
        // Enemy Factory creates enemies
        // It also hooks up the events of enemy to the components of interest
        // var enemy = new Enemy();
        // enemy.OnDeath += gameManager.IncrementDeadEnemiesCount()
        public delegate void DestroyHandler();
        public event DestroyHandler Died;
        public Transform healthBar;
        public AudioClip audioClipHit;
        public GameObject deathEffect;
        private AudioSource _audioSource;

        public float Health { get; set; } = 100;

        private void Awake()
        {
            // Leave subscription to be handle by another class like a GameManager or ScoreManager;
            Died += OnDeath;
            _audioSource = GetComponent<AudioSource>();
        }

        public void TakeDamage(float damage)
        {
            Vector2 localScale = healthBar.localScale;
            float unit = Health / localScale.x;
            localScale.x = (Health - damage) / unit;
            healthBar.localScale = localScale;
            
            Health -= damage;

            _audioSource.clip = audioClipHit;
            _audioSource.Play();

            if (Health <= 0)
            {
                Destroy(gameObject);
                Instantiate(deathEffect, transform.position, Quaternion.identity);
                Died?.Invoke();
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void OnDeath()
        {
            // Granting points should be done from a separate class like a GameManager or ScoreManager and not done here.
            Debug.Log($"{gameObject.name} died. +1 point");
        }
    }
}
