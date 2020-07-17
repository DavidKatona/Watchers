using Assets.Scripts.Attributes;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
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
        transform.Find("CharacterMenuUI/CharacterMenuFrame/CharacterStatistics/Attributes/VigorBackgroundColor/VigorCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Vigor).ToString();
        transform.Find("CharacterMenuUI/CharacterMenuFrame/CharacterStatistics/Attributes/SpiritBackgroundColor/SpiritCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Spirit).ToString();
        transform.Find("CharacterMenuUI/CharacterMenuFrame/CharacterStatistics/Attributes/StrengthBackgroundColor/StrengthCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Strength).ToString();
        transform.Find("CharacterMenuUI/CharacterMenuFrame/CharacterStatistics/Attributes/IntelligenceBackgroundColor/IntelligenceCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Intelligence).ToString();
        transform.Find("CharacterMenuUI/CharacterMenuFrame/CharacterStatistics/Attributes/ResilienceBackgroundColor/ResilienceCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Resilience).ToString();
        transform.Find("CharacterMenuUI/CharacterMenuFrame/CharacterStatistics/Attributes/VitalityBackgroundColor/VitalityCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Vitality).ToString();
        transform.Find("CharacterMenuUI/CharacterMenuFrame/CharacterStatistics/Attributes/FocusBackgroundColor/FocusCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Focus).ToString();
    }
}