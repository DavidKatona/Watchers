using Assets.Scripts.Attributes;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    private const string attributePath = "CharacterMenuUI/CharacterMenuFrame/CharacterStatistics/Attributes/";

    public GameObject CharacterMenuUI;
    public AudioSource audioSource;
    public AudioClip menuOpenClip;
    public AudioClip menuCloseClip;
    private bool _isOpened;
    private Attributes _attributes;

    public void SetAttributes(Attributes attributes)
    {
        _attributes = attributes;
        _attributes.OnAttributeChanged += Attributes_OnAttributesChanged;
        UpdateStatisticsVisuals();
    }

    private void Attributes_OnAttributesChanged(object sender, EventArgs e)
    {
        // Update visuals.
        UpdateStatisticsVisuals();
    }

    public void Close()
    {
        _isOpened = false;
        CharacterMenuUI.SetActive(_isOpened);
        audioSource.clip = menuCloseClip;
        audioSource.Play();
    }

    void Update()
    {
        if (Input.GetButtonDown("OpenCharacterMenu"))
        {
            _isOpened = !_isOpened;

            CharacterMenuUI.SetActive(_isOpened);

            if (_isOpened)
            {
                audioSource.clip = menuOpenClip;
            }
            else
            {
                audioSource.clip = menuCloseClip;
            }

            audioSource.Play();
        }
    }

    private void UpdateStatisticsVisuals()
    {
        transform.Find(attributePath + "VigorBackgroundColor/VigorCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Vigor).ToString();
        transform.Find(attributePath + "SpiritBackgroundColor/SpiritCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Spirit).ToString();
        transform.Find(attributePath + "StrengthBackgroundColor/StrengthCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Strength).ToString();
        transform.Find(attributePath + "IntelligenceBackgroundColor/IntelligenceCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Intelligence).ToString();
        transform.Find(attributePath + "ResilienceBackgroundColor/ResilienceCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Resilience).ToString();
        transform.Find(attributePath + "VitalityBackgroundColor/VitalityCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Vitality).ToString();
        transform.Find(attributePath + "FocusBackgroundColor/FocusCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Focus).ToString();
    }
}