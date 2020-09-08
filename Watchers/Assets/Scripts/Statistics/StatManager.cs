using Assets.Scripts.Attributes;
using Assets.Scripts.BattleSystem;
using System;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnManaChanged;

    [SerializeField] private CharacterMenu _characterMenu;
    [SerializeField] private DebugMenu _debugMenu;
    [SerializeField] private PlayerHUD _playerHUD;
    private Attributes _attributes;
    public Attributes GetAttributes()
    {
        return _attributes;
    }

    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public float MaxMana { get; private set; }
    public float CurrentMana { get; private set; }
    public float PhysicalDamage { get; private set; }
    public float PhysicalDamageLowerBound { get; private set; }
    public float PhysicalDamageUpperBound { get; private set; }
    public float MagicalDamage { get; private set; }
    public float Armor { get; private set; }
    public float HealthRegen { get; private set; }
    public float ManaRegen { get; private set; }

    private void Start()
    {
        Attributes attributes = new Attributes(11, 11, 11, 11, 11, 11, 11);

        SetAttributes(attributes);
        RecalculateAllStats();
        RestoreHealthAndMana();

        _characterMenu.SetAttributes(attributes);
        _debugMenu.SetAttributes(attributes);
        _playerHUD.SetAttributes(attributes);
    }

    private void Update()
    {
        RegenerateHealthAndMana();
    }

    public void SetAttributes(Attributes attributes)
    {
        _attributes = attributes;
        attributes.OnAttributeChanged += Attributes_OnAttributesChanged;
    }

    public float GetHealthPercentage()
    {
        return CurrentHealth / MaxHealth;
    }

    public void SetCurrentHealth(float amount)
    {
        CurrentHealth = Mathf.Clamp(amount, 0, MaxHealth);
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public float GetManaPercentage()
    {
        return CurrentMana / MaxMana;
    }

    public void SetCurrentMana(float amount)
    {
        CurrentMana = Mathf.Clamp(amount, 0, MaxMana);
        OnManaChanged?.Invoke(this, EventArgs.Empty);
    }

    public float GetRandomPhysicalDamage()
    {
        return UnityEngine.Random.Range(PhysicalDamageLowerBound, PhysicalDamageUpperBound);
    }

    private void Attributes_OnAttributesChanged(object sender, EventArgs e)
    {
        var healthPercentage = GetHealthPercentage();
        var manaPercentage = GetManaPercentage();
        RecalculateAllStats();
        PreserveHealthAndManaPercentage(healthPercentage, manaPercentage);
    }

    private void RecalculateAllStats()
    {
        // Recalculate all stats so they align with attributes.
        CalculateHealth();
        CalculatMana();
        CalculatePhysicalDamage();
        CalculateMagicalDamage();
        CalculateArmor();
        CalculateHealthRegen();
        CalculateManaRegen();
    }

    private void RestoreHealthAndMana()
    {
        // Restore all missing health and manapoints.
        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;
    }

    private void PreserveHealthAndManaPercentage(float healthPercentage, float manaPercentage)
    {
        CurrentHealth = MaxHealth * healthPercentage;
        CurrentMana = MaxMana * manaPercentage;
    }

    private void RegenerateHealthAndMana()
    {
        if (CurrentHealth != MaxHealth)
        {
            SetCurrentHealth(CurrentHealth += HealthRegen * Time.deltaTime);
        }

        if (CurrentMana != MaxMana)
        {
            SetCurrentMana(CurrentMana += ManaRegen * Time.deltaTime);
        }
    }

    private void CalculateHealth()
    {
        // Health is equal to points spent on vigor multiplied by 10.
        MaxHealth = _attributes.GetAttributeAmount(AttributeType.Vigor) * 10;
    }

    private void CalculatMana()
    {
        // Mana is equal to points spent on spirit multiplied by 5.
        MaxMana = _attributes.GetAttributeAmount(AttributeType.Spirit) * 10;
    }

    private void CalculatePhysicalDamage()
    {
        // Physical damage is equal to points spent on strength multiplied by 2.
        PhysicalDamage = _attributes.GetAttributeAmount(AttributeType.Strength) * 2;
        // Determine lower and upper bounds of physical damage.
        PhysicalDamageLowerBound = _attributes.GetAttributeAmount(AttributeType.Strength) * 1.7f;
        PhysicalDamageUpperBound = _attributes.GetAttributeAmount(AttributeType.Strength) * 2.1f;
    }

    private void CalculateMagicalDamage()
    {
        // Magical damage is equal to points spent on intelligence multiplied  by 3.
        MagicalDamage = _attributes.GetAttributeAmount(AttributeType.Intelligence) * 3;
    }

    private void CalculateArmor()
    {
        // Armor is equal to points spent on resilience halved.
        Armor = _attributes.GetAttributeAmount(AttributeType.Resilience) * 0.5f;
    }

    private void CalculateHealthRegen()
    {
        HealthRegen = _attributes.GetAttributeAmount(AttributeType.Vitality) * 0.1f;
    }

    private void CalculateManaRegen()
    {
        ManaRegen = _attributes.GetAttributeAmount(AttributeType.Focus) * 0.1f;
    }
}