using Assets.Scripts.GameAssets;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    private Text _tutorialText;
    private Color _tutorialTextColor;
    private Image _tutorialBackground;
    private Color _tutorialBackgroundColor;
    private float _disappearTimer;

    void Awake()
    {
        _tutorialText = GetComponentInChildren<Text>(true);
        _tutorialBackground = GetComponent<Image>();

        _tutorialTextColor = _tutorialText.color;
        _tutorialBackgroundColor = _tutorialBackground.color;
    }

    public static TutorialPopup Create(string tutorialMessage, float duration, Transform parentCanvas)
    {
        Transform tutorialPopupTransform = Instantiate(GameAssets.i.prefabTutorialPopup, parentCanvas);
        TutorialPopup tutorialPopup = tutorialPopupTransform.GetComponent<TutorialPopup>();

        tutorialPopup.Setup(tutorialMessage, duration);
        return tutorialPopup;
    }

    private void Setup(string tutorialMessage, float duration)
    {
        _tutorialText.text = tutorialMessage;
        _disappearTimer = duration;

        _tutorialTextColor.a = 0;
        _tutorialBackgroundColor.a = 0;
    }

    private void Update()
    {
        _disappearTimer -= Time.deltaTime;

        if (_disappearTimer > _disappearTimer * 0.8f)
        {
            float fadeInSpeed = 5f;

            _tutorialTextColor.a += fadeInSpeed * Time.deltaTime;
            _tutorialTextColor.a = Mathf.Clamp(_tutorialTextColor.a, 0.0f, 0.8f);
            _tutorialText.color = _tutorialTextColor;

            _tutorialBackgroundColor.a += fadeInSpeed * Time.deltaTime;
            _tutorialBackgroundColor.a = Mathf.Clamp(_tutorialTextColor.a, 0.0f, 0.8f);
            _tutorialBackground.color = _tutorialBackgroundColor;
        }

        if (_disappearTimer < 0)
        {
            float disapperSpeed = 3f;

            _tutorialTextColor.a -= disapperSpeed * Time.deltaTime;
            _tutorialText.color = _tutorialTextColor;

            _tutorialBackgroundColor.a -= disapperSpeed * Time.deltaTime;
            _tutorialBackground.color = _tutorialBackgroundColor;

            if (_tutorialBackgroundColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}