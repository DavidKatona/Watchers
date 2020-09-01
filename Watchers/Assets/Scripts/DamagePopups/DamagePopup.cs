using Assets.Scripts.GameAssets;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private const float DISAPPER_TIMER_MAX = 0.5f;
    private const float MOVE_VECTOR_MODIFIER = 2.5f;
    private TextMeshPro _textMesh;
    private float _disappearTimer;
    private Color _textColor;
    private Vector3 _moveVector;

    public static DamagePopup Create(Vector3 position, int damageAmount)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.Instance.prefabDamagePopup, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();

        damagePopup.Setup(damageAmount);
        return damagePopup;
    }

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount)
    {
        _textMesh.SetText(damageAmount.ToString());
        _textColor = _textMesh.color;
        _disappearTimer = DISAPPER_TIMER_MAX;

        _moveVector = new Vector3(1, 1) * MOVE_VECTOR_MODIFIER;

        var randomDirection = Random.Range(-1, 1);
        _moveVector.x = (randomDirection >= 0) ? _moveVector.x * 1 : _moveVector.x * -1;
    }

    private void Update()
    {
        transform.position += _moveVector * Time.deltaTime;
        _moveVector -= _moveVector * MOVE_VECTOR_MODIFIER * Time.deltaTime;

        if (_disappearTimer > DISAPPER_TIMER_MAX * 0.5f)
        {
            // First half of the fade out animation.
            float scaleModifier = 1f;
            transform.localScale += Vector3.one * scaleModifier * Time.deltaTime;
        }
        else
        {
            // Second half of the fade out animation.
            float scaleModifier = 1f;
            transform.localScale -= Vector3.one * scaleModifier * Time.deltaTime;
        }

        _disappearTimer -= Time.deltaTime;

        if (_disappearTimer < 0)
        {
            float disapperSpeed = 3f;
            _textColor.a -= disapperSpeed * Time.deltaTime;
            _textMesh.color = _textColor;

            if (_textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
