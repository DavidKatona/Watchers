using Assets.Scripts.Attributes;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    private const string attributePath = "CharacterMenuUI/CharacterMenuFrame/CharacterStatistics/Attributes/";
    private const string mainStatsPath = "CharacterMenuUI/CharacterMenuFrame/CharacterStatistics/MainStatistics/";
    private const string additionalStatsPath = "CharacterMenuUI/CharacterMenuFrame/CharacterStatistics/AdditionalStatistics/";

    public GameObject CharacterMenuUI;
    public StatManager StatManager;
    public AudioSource audioSource;
    public AudioClip menuOpenClip;
    public AudioClip menuCloseClip;
    private Attributes _attributes;
    private bool _isOpened;

    public void SetAttributes(Attributes attributes)
    {
        _attributes = attributes;
        _attributes.OnAttributeChanged += Attributes_OnAttributesChanged;
        StatManager.OnHealthChanged += StatManager_OnHealthChanged;
        StatManager.OnManaChanged += StatManager_OnManaChanged;
        UpdateStatisticsVisuals();
    }

    private void Attributes_OnAttributesChanged(object sender, EventArgs e)
    {
        // Update visuals.
        UpdateStatisticsVisuals();
    }

    private void StatManager_OnHealthChanged(object sender, EventArgs e)
    {
        UpdateHealthStatistics();
    }

    private void StatManager_OnManaChanged(object sender, EventArgs e)
    {
        UpdateManaStatistics();
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
        // Update attributes.
        transform.Find(attributePath + "VigorBackgroundColor/VigorCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Vigor).ToString();
        transform.Find(attributePath + "SpiritBackgroundColor/SpiritCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Spirit).ToString();
        transform.Find(attributePath + "StrengthBackgroundColor/StrengthCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Strength).ToString();
        transform.Find(attributePath + "IntelligenceBackgroundColor/IntelligenceCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Intelligence).ToString();
        transform.Find(attributePath + "ResilienceBackgroundColor/ResilienceCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Resilience).ToString();
        transform.Find(attributePath + "VitalityBackgroundColor/VitalityCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Vitality).ToString();
        transform.Find(attributePath + "FocusBackgroundColor/FocusCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(Attributes.AttributeType.Focus).ToString();

        // Update statistics.
        transform.Find(mainStatsPath + "HealthBackgroundColor/HealthCounter").GetComponent<Text>().text = $"{StatManager.CurrentHealth}/{StatManager.MaxHealth}";
        transform.Find(mainStatsPath + "ManaBackgroundColor/ManaCounter").GetComponent<Text>().text = $"{StatManager.CurrentMana}/{StatManager.MaxMana}";
        transform.Find(mainStatsPath + "PhysicalDamageBackgroundColor/PhysicalDamageCounter").GetComponent<Text>().text = $"+{StatManager.PhysicalDamage}";
        transform.Find(mainStatsPath + "MagicalDamageBackgroundColor/MagicalDamageCounter").GetComponent<Text>().text = $"+{StatManager.MagicalDamage}";
        transform.Find(mainStatsPath + "ArmorBackgroundColor/ArmorCounter").GetComponent<Text>().text = StatManager.Armor.ToString();
        transform.Find(additionalStatsPath + "HealthRegenBackgroundColor/HealthRegenCounter").GetComponent<Text>().text = $"{StatManager.HealthRegen}/s";
        transform.Find(additionalStatsPath + "ManaRegenBackgroundColor/ManaRegenCounter").GetComponent<Text>().text = $"{StatManager.ManaRegen}/s";
    }

    private void UpdateHealthStatistics()
    {
        transform.Find(mainStatsPath + "HealthBackgroundColor/HealthCounter").GetComponent<Text>().text = $"{StatManager.CurrentHealth}/{StatManager.MaxHealth}";
    }

    private void UpdateManaStatistics()
    {
        transform.Find(mainStatsPath + "ManaBackgroundColor/ManaCounter").GetComponent<Text>().text = $"{StatManager.CurrentMana}/{StatManager.MaxMana}";
    }
}