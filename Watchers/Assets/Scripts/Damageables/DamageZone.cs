using UnityEngine;

namespace Assets.Scripts.Damagables
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DamageZone : MonoBehaviour
    {
        [SerializeField] private int _zoneDamage;

        private void OnTriggerStay2D(Collider2D collision)
        {
            var damagable = collision.GetComponent<IDamageable>();
            if (damagable != null && collision.tag == "Player")
            {
                damagable.TakeDamage(_zoneDamage);
            }
        }
    }
}


