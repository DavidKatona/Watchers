using Assets.Scripts.BattleSystem;
using Assets.Scripts.Damagables;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Damageables.Enemies
{
    public class FlyingBat : MonoBehaviour, IDamageable, ISpawnable
    {
        public event EventHandler OnSpawnableDestroyed;
        public Transform healthBar;
        public AudioClip audioClipHit;
        public SpriteRenderer spriteRenderer;
        public Color damagedColor;
        [SerializeField] private float _health = 100;
        private Color _originalColor;
        private AudioSource _audioSource;
        private Animator _animator;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _audioSource = GetComponent<AudioSource>();
            _originalColor = spriteRenderer.color;
        }

        public void TakeDamage(float damage)
        {
            Vector2 localScale = healthBar.localScale;
            float unit = _health / localScale.x;
            localScale.x = (_health - damage) / unit;
            healthBar.localScale = localScale;

            _health -= damage;

            StartCoroutine(Stagger(0.8f));
            StartCoroutine(Flash());
            _animator.SetTrigger("Damaged");
            _audioSource.clip = audioClipHit;
            _audioSource.Play();

            if (_health <= 0)
            {
                // Instantiate death effect.
                Instantiate(GameAssets.GameAssets.Instance.prefabDeathEffect, new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z), Quaternion.identity);

                // Instantiate collectible soul prefab.
                Instantiate(GameAssets.GameAssets.Instance.prefabCollectibleSoul, transform.position, Quaternion.identity);
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

        private IEnumerator Stagger(float duration)
        {
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
            yield return new WaitForSeconds(duration);
            _rigidbody2D.constraints = RigidbodyConstraints2D.None;
            _rigidbody2D.freezeRotation = true;
        }
    }
}