using Assets.Scripts.GameAssets;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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
        public SpriteRenderer spriteRenderer;
        public Color damagedColor;
        private Color _originalColor;
        private AudioSource _audioSource;

        public float Health { get; set; } = 100;

        private void Awake()
        {
            // Leave subscription to be handle by another class like a GameManager or ScoreManager;
            Died += OnDeath;
            _audioSource = GetComponent<AudioSource>();
            _originalColor = spriteRenderer.color;
        }

        public void TakeDamage(float damage)
        {
            Vector2 localScale = healthBar.localScale;
            float unit = Health / localScale.x;
            localScale.x = (Health - damage) / unit;
            healthBar.localScale = localScale;

            Health -= damage;

            StartCoroutine(Flash());
            _audioSource.clip = audioClipHit;
            _audioSource.Play();

            if (Health <= 0)
            {
                Instantiate(GameAssets.GameAssets.i.prefabDeathEffect, new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z), Quaternion.identity);
                HitStop.Instance.Stop(0.04f);
                Destroy(gameObject);
                Died?.Invoke();
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        private IEnumerator Flash()
        {
            spriteRenderer.color = damagedColor;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = _originalColor;
        }

        public void OnDeath()
        {
            // Granting points should be done from a separate class like a GameManager or ScoreManager and not done here.
            Debug.Log($"{gameObject.name} died. +1 point");
        }
    }
}
