using Assets.Scripts.Collectibles;
using UnityEngine;

namespace Assets.Scripts.CollectibleSystem
{
    public class CollectiblePicker : MonoBehaviour
    {
        [SerializeField] private StatManager _statManager;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var collectible = other.GetComponentInChildren<ICollectible>();

            if (collectible != null)
            {
                collectible.Collect(_statManager);
            }
        }
    }
}
