using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Assets.Scripts.Collectibles
{
    public class CollectibleSoul : MonoBehaviour, ICollectible
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Light2D _light2D;
        private float _lifetime = 10f;
        private int _souls = 10000;
        private float _lifetimeCounter;

        public void Collect(StatManager statManager)
        {
            var souls = statManager.GetAttributes().GetSouls();
            statManager.GetAttributes().SetSouls(souls + _souls);

            var parentObject = transform.parent.gameObject;
            Instantiate(GameAssets.GameAssets.Instance.prefabCollectiblePicked, transform.position, Quaternion.identity);
            Destroy(parentObject);
        }

        private void Start()
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0);
        }

        private void Update()
        {

            _lifetimeCounter += Time.deltaTime;
            
            if (_lifetimeCounter <= _lifetime * 0.1f)
            {
                var spriteColor = _spriteRenderer.color;
                spriteColor.a += Time.deltaTime;
                _spriteRenderer.color = spriteColor;
            }

            if (_lifetimeCounter >= _lifetime)
            {
                var parentObject = transform.parent.gameObject;
                Destroy(parentObject);
            }

            if (_lifetimeCounter > _lifetime * 0.9f)
            {
                var spriteColor = _spriteRenderer.color;
                spriteColor.a -= Time.deltaTime;
                _spriteRenderer.color = spriteColor;

                _light2D.intensity -= Time.deltaTime;
            }
        }
    }
}