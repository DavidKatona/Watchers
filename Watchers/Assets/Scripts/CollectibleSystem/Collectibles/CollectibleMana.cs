using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Assets.Scripts.Collectibles
{
    public class CollectibleMana : MonoBehaviour, ICollectible
    {
        [Header("Dependencies")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Light2D _light2D;

        [Header("Properties")]
        [SerializeField] private int _mana;
        private Color _spriteColor;
        private float _lightIntensity;

        public void Collect(StatManager statManager)
        {
            var mana = statManager.CurrentMana;
            statManager.SetCurrentMana(mana + _mana);

            var parentObject = transform.parent.gameObject;
            Instantiate(GameAssets.GameAssets.Instance.prefabCollectibleManaPicked, transform.position, Quaternion.identity);
            Destroy(parentObject);
        }

        private void Start()
        {
            _spriteColor = _spriteRenderer.color;
            _spriteColor.a = 0f;
            _spriteRenderer.color = _spriteColor;

            _lightIntensity = _light2D.intensity;
            _light2D.intensity = 0;
        }

        private void Update()
        {
            if (_spriteRenderer.color.a <= 1)
            {
                _spriteColor.a += Time.deltaTime;
                // We clamp the alpha of spriteColor between 0 and 1 to avoid setting the alpha too high or too low.
                _spriteColor.a = Mathf.Clamp(_spriteColor.a, 0, 1);
                _spriteRenderer.color = _spriteColor;

                // We clamp the intensity of the lightsource to be between 0 and it's original value.
                _light2D.intensity += Time.deltaTime;
                _light2D.intensity = Mathf.Clamp(_light2D.intensity, 0, _lightIntensity);
            }
        }
    }
}
