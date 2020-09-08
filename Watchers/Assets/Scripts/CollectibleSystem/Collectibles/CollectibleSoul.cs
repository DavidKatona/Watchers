using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Assets.Scripts.Collectibles
{
    public class CollectibleSoul : MonoBehaviour, ICollectible
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Light2D _light2D;

        [Range(1, 60)]
        [Tooltip("Determines the total lifetime of the object after which it will start fading out.")]
        [SerializeField] private float _lifetime = 10f;

        [Range(0.1f, 1)]
        [Tooltip("Controls how fast the object will fade out after it's lifetime.")]
        [SerializeField] private float _fadeOutSpeed = 1;

        [SerializeField] private int _souls = 10000;
        private float _lifetimeCounter;
        private Color _spriteColor;

        public void Collect(StatManager statManager)
        {
            var souls = statManager.GetAttributes().GetSouls();
            statManager.GetAttributes().SetSouls(souls + _souls);

            var parentObject = transform.parent.gameObject;
            Instantiate(GameAssets.GameAssets.Instance.prefabCollectibleSoulPicked, transform.position, Quaternion.identity);
            Destroy(parentObject);
        }

        private void Start()
        {
            _lifetimeCounter = _lifetime;
            _spriteColor = _spriteRenderer.color;
            _spriteColor.a = 0f;
            _spriteRenderer.color = _spriteColor;
        }

        private void Update()
        {
            _lifetimeCounter -= Time.deltaTime;

            // We fade in the object in the first 10th of it's lifetime to ensure it appears smoothly in the scene.
            if (_lifetimeCounter > _lifetime * 0.9f)
            {
                _spriteColor.a += Time.deltaTime;
                // We clamp the alpha of spriteColor between 0 and 1 to avoid setting the alpha too high or too low.
                _spriteColor.a = Mathf.Clamp(_spriteColor.a, 0, 1);
                _spriteRenderer.color = _spriteColor;
            }
            
            if (_lifetimeCounter < 0)
            {
                _spriteColor.a -= _fadeOutSpeed * Time.deltaTime;
                _spriteRenderer.color = _spriteColor;
                _light2D.intensity -= _fadeOutSpeed * Time.deltaTime;

                if (_spriteRenderer.color.a < 0)
                {
                    var parentObject = transform.parent.gameObject;
                    Destroy(parentObject);
                }
            }
        }
    }
}