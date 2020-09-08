using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.CollectibleSystem
{
    public class CollectibleSpawner : MonoBehaviour
    {
        [Header("Properties")]
        [Tooltip("Determines whether collectible type spawns should be randomized.")]
        [SerializeField] private bool _randomizeCollectibles;

        [Tooltip("Determines the type of collectible to spawn on this objects spawnpoint.")]
        [SerializeField] private CollectibleType _collectibleType;
        private Vector3 _spawnPoint;
        private Transform _collectibleInstance;

        private void Start()
        {
            // We offset the spawnpoint so it doesn't spawn into other colliders.
            var offset = 0.5f;
            _spawnPoint = transform.position + new Vector3(0, offset);
        }

        private void Update()
        {
            if (_collectibleInstance == null)
            {
                _collectibleInstance = GetItemToSpawn(_collectibleType, _randomizeCollectibles);
                StartCoroutine(SpawnCollectible(_collectibleInstance, 5f));
            }
        }

        private Transform GetItemToSpawn(CollectibleType collectibleType, bool randomize)
        {
            if (randomize)
            {
                var collectibles = Enum.GetValues(typeof(CollectibleType));
                var collectibleIndex = UnityEngine.Random.Range(0, collectibles.Length);
                Debug.Log(collectibleIndex);

                collectibleType = (CollectibleType)collectibleIndex;
            }

            switch (collectibleType)
            {
                case CollectibleType.Health:
                    return GameAssets.GameAssets.Instance.prefabCollectibleHealth;
                case CollectibleType.Mana:
                    return GameAssets.GameAssets.Instance.prefabCollectibleMana;
                default:
                    return GameAssets.GameAssets.Instance.prefabCollectibleHealth;
            }
        }

        private IEnumerator SpawnCollectible(Transform collectibleToSpawn, float delay)
        {
            yield return new WaitForSeconds(delay);
            _collectibleInstance = Instantiate(collectibleToSpawn, _spawnPoint, Quaternion.identity);
        }
    }
}
