using Assets.Scripts.Attributes;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    private const string attributePath = "CharacterMenuUI/CharacterMenuFrame/CharacterStatistics/Attributes/";
    private const string mainStatsPath = "CharacterMenuUI/CharacterMenuFrame/CharacterStatistics/MainStatistics/";
    private const string additionalStatsPath = "CharacterMenuUI/CharacterMenuFrame/CharacterStatistics/AdditionalStatistics/";
    private const string levelInfoPath = "CharacterMenuUI/CharacterMenuFrame/CharacterLevelInformation/";
    private const string attributesFrame = "CharacterMenuUI/CharacterAttributesFrame/Attributes/AttributesPanel/";

    public static bool IsOpened;
    public GameObject CharacterMenuUI;
    public StatManager StatManager;
    public AudioSource audioSource;
    public AudioClip menuOpenClip;
    public AudioClip menuCloseClip;
    private Attributes _attributes;

    public void SetAttributes(Attributes attributes)
    {
        _attributes = attributes;
        _attributes.OnAttributeChanged += Attributes_OnAttributesChanged;
        _attributes.OnSoulsChanged += Attributes_OnSoulsChanged;
        StatManager.OnHealthChanged += StatManager_OnHealthChanged;
        StatManager.OnManaChanged += StatManager_OnManaChanged;
        UpdateStatisticsVisuals();
        InitializeButtons();
    }

    private void Attributes_OnAttributesChanged(object sender, EventArgs e)
    {
        // Update visuals.
        UpdateStatisticsVisuals();
    }

    private void Attributes_OnSoulsChanged(object sender, EventArgs e)
    {
        UpdateSoulsCounter();
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
        IsOpened = false;
        CharacterMenuUI.SetActive(IsOpened);
        audioSource.clip = menuCloseClip;
        audioSource.Play();
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused) return;

        if (Input.GetButtonDown("OpenCharacterMenu"))
        {
            IsOpened = !IsOpened;

            CharacterMenuUI.SetActive(IsOpened);

            if (IsOpened)
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
        transform.Find(attributePath + "VigorBackgroundColor/VigorCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(AttributeType.Vigor).ToString();
        transform.Find(attributePath + "SpiritBackgroundColor/SpiritCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(AttributeType.Spirit).ToString();
        transform.Find(attributePath + "StrengthBackgroundColor/StrengthCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(AttributeType.Strength).ToString();
        transform.Find(attributePath + "IntelligenceBackgroundColor/IntelligenceCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(AttributeType.Intelligence).ToString();
        transform.Find(attributePath + "ResilienceBackgroundColor/ResilienceCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(AttributeType.Resilience).ToString();
        transform.Find(attributePath + "VitalityBackgroundColor/VitalityCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(AttributeType.Vitality).ToString();
        transform.Find(attributePath + "FocusBackgroundColor/FocusCounter").GetComponent<Text>().text = _attributes.GetAttributeAmount(AttributeType.Focus).ToString();

        // Update statistics.
        transform.Find(mainStatsPath + "HealthBackgroundColor/HealthCounter").GetComponent<Text>().text = $"{StatManager.CurrentHealth}/{StatManager.MaxHealth}";
        transform.Find(mainStatsPath + "ManaBackgroundColor/ManaCounter").GetComponent<Text>().text = $"{StatManager.CurrentMana}/{StatManager.MaxMana}";
        transform.Find(mainStatsPath + "PhysicalDamageBackgroundColor/PhysicalDamageCounter").GetComponent<Text>().text = $"+{StatManager.PhysicalDamage}";
        transform.Find(mainStatsPath + "MagicalDamageBackgroundColor/MagicalDamageCounter").GetComponent<Text>().text = $"+{StatManager.MagicalDamage}";
        transform.Find(mainStatsPath + "ArmorBackgroundColor/ArmorCounter").GetComponent<Text>().text = StatManager.Armor.ToString();
        transform.Find(additionalStatsPath + "HealthRegenBackgroundColor/HealthRegenCounter").GetComponent<Text>().text = $"{StatManager.HealthRegen}/s";
        transform.Find(additionalStatsPath + "ManaRegenBackgroundColor/ManaRegenCounter").GetComponent<Text>().text = $"{StatManager.ManaRegen}/s";

        // Update level information.
        transform.Find(levelInfoPath + "WatcherLevelBackgroundColor/WatcherLevelCounter").GetComponent<Text>().text = $"{_attributes.GetLevel()}";
        transform.Find(levelInfoPath + "SoulsRequiredBackgroundColor/SoulsRequiredCounter").GetComponent<Text>().text = $"{_attributes.GetSoulsRequired():#,0}";
        transform.Find(levelInfoPath + "CurrentSoulsBackgroundColor/CurrentSoulsCounter").GetComponent<Text>().text = $"{_attributes.GetSouls():#,0}";
    }

    private void UpdateHealthStatistics()
    {
        transform.Find(mainStatsPath + "HealthBackgroundColor/HealthCounter").GetComponent<Text>().text = $"{StatManager.CurrentHealth.ToString("0.00")}/{StatManager.MaxHealth}";
    }

    private void UpdateManaStatistics()
    {
        transform.Find(mainStatsPath + "ManaBackgroundColor/ManaCounter").GetComponent<Text>().text = $"{StatManager.CurrentMana.ToString("0.00")}/{StatManager.MaxMana}";
    }

    private void UpdateSoulsCounter()
    {
        transform.Find(levelInfoPath + "CurrentSoulsBackgroundColor/CurrentSoulsCounter").GetComponent<Text>().text = $"{_attributes.GetSouls():#,0}";
    }

    private void InitializeButtons()
    {
        transform.Find(attributesFrame + "incrVigor").GetComponent<Button>().onClick.AddListener(() => _attributes.IncreaseAttribute(AttributeType.Vigor));
        transform.Find(attributesFrame + "decrVigor").GetComponent<Button>().onClick.AddListener(() => _attributes.DecreaseAttribute(AttributeType.Vigor));
        transform.Find(attributesFrame + "incrSpirit").GetComponent<Button>().onClick.AddListener(() => _attributes.IncreaseAttribute(AttributeType.Spirit));
        transform.Find(attributesFrame + "decrSpirit").GetComponent<Button>().onClick.AddListener(() => _attributes.DecreaseAttribute(AttributeType.Spirit));
        transform.Find(attributesFrame + "incrStrength").GetComponent<Button>().onClick.AddListener(() => _attributes.IncreaseAttribute(AttributeType.Strength));
        transform.Find(attributesFrame + "decrStrength").GetComponent<Button>().onClick.AddListener(() => _attributes.DecreaseAttribute(AttributeType.Strength));
        transform.Find(attributesFrame + "incrIntelligence").GetComponent<Button>().onClick.AddListener(() => _attributes.IncreaseAttribute(AttributeType.Intelligence));
        transform.Find(attributesFrame + "decrIntelligence").GetComponent<Button>().onClick.AddListener(() => _attributes.DecreaseAttribute(AttributeType.Intelligence));
        transform.Find(attributesFrame + "incrResilience").GetComponent<Button>().onClick.AddListener(() => _attributes.IncreaseAttribute(AttributeType.Resilience));
        transform.Find(attributesFrame + "decrResilience").GetComponent<Button>().onClick.AddListener(() => _attributes.DecreaseAttribute(AttributeType.Resilience));
        transform.Find(attributesFrame + "incrVitality").GetComponent<Button>().onClick.AddListener(() => _attributes.IncreaseAttribute(AttributeType.Vitality));
        transform.Find(attributesFrame + "decrVitality").GetComponent<Button>().onClick.AddListener(() => _attributes.DecreaseAttribute(AttributeType.Vitality));
        transform.Find(attributesFrame + "incrFocus").GetComponent<Button>().onClick.AddListener(() => _attributes.IncreaseAttribute(AttributeType.Focus));
        transform.Find(attributesFrame + "decrFocus").GetComponent<Button>().onClick.AddListener(() => _attributes.DecreaseAttribute(AttributeType.Focus));
    }
}