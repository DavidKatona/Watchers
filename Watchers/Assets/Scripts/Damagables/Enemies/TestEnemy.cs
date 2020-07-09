using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Damagables.Enemies
{
    public class TestEnemy : MonoBehaviour, IDamagable
    {
        // ToDo: Rewrite this to actual enemy classes with refined logic.
        public delegate void DestroyHandler();
        public event DestroyHandler Died;

        public int Health { get; set; } = 10;

        private void Awake()
        {
            Died += Destroy;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Died?.Invoke();
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
