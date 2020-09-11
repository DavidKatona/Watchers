using UnityEngine;

namespace Assets.Scripts.Animation
{
    public class CreateSpellAnimationEvent : MonoBehaviour
    {
        [SerializeField] private GameObject _prefabObject;

        public void InstantiateSpell()
        {
            Instantiate(_prefabObject, transform.position, Quaternion.identity);
        }
    }
}
