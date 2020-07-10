using System;
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

        public int Health { get; set; } = 10;

        private void Awake()
        {
            // Leave subscription to be handle by another class like a GameManager or ScoreManager;
            Died += OnDeath;
        }

        public void ApplyDamage(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Destroy(gameObject);
                Died?.Invoke();
            }
        }

        public void OnDeath()
        {
            // Granting points should be done from a separate class like a GameManager or ScoreManager and not done here.
            Debug.Log($"{gameObject.name} died. +1 point");
        }
    }
}
