using Assets.Scripts.GameAssets;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro _textMesh;
    private float _disappearTimer;
    private Color _textColor;

    public static DamagePopup Create(Vector3 position, int damageAmount)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.prefabDamagePopup, position, Quaternion.identity);
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
        _disappearTimer = 0.5f;
    }

    private void Update()
    {
        float yAxisSpeed = 5f;
        transform.position += new Vector3(0, yAxisSpeed) * Time.deltaTime;

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
