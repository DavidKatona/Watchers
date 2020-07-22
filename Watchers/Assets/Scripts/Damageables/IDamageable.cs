using UnityEngine;

namespace Assets.Scripts.Damagables
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
        Vector3 GetPosition();
    }
}