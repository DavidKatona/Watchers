using Assets.Scripts.BattleSystem;
using Assets.Scripts.GameAssets;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Damagables.Enemies
{
    public class TestEnemy : MonoBehaviour, IDamageable, ISpawnable
    {
        public event EventHandler OnSpawnableDestroyed;
        public Transform healthBar;
        public AudioClip audioClipHit;
        public SpriteRenderer spriteRenderer;
        public Color damagedColor;
        private Color _originalColor;
        private AudioSource _audioSource;

        public float Health { get; set; } = 100;

        private void Awake()
        {
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
                OnSpawnableDestroyed?.Invoke(this, EventArgs.Empty);
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
    }
}
