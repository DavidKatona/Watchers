using Assets.Scripts.Attributes;
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

    void Start()
    {
        Attributes attributes = new Attributes(11, 11, 11, 11, 11, 11, 11);

        SetAttributes(attributes);
        RecalculateAllStats();
        RestoreHealthAndMana();

        _characterMenu.SetAttributes(attributes);
        _debugMenu.SetAttributes(attributes);
        _playerHUD.SetAttributes(attributes);
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
        RecalculateAllStats();
        RestoreHealthAndMana();
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

    // Sould there be more stats, it would be wise to abstract stats away to an interface.
    private void CalculateHealth()
    {
        // Health is equal to points spent on vigor multiplied by 10.
        MaxHealth = _attributes.GetAttributeAmount(Attributes.AttributeType.Vigor) * 10;
    }

    private void CalculatMana()
    {
        // Mana is equal to points spent on spirit multiplied by 5.
        MaxMana = _attributes.GetAttributeAmount(Attributes.AttributeType.Spirit) * 10;
    }

    private void CalculatePhysicalDamage()
    {
        // Physical damage is equal to points spent on strength multiplied by 2.
        PhysicalDamage = _attributes.GetAttributeAmount(Attributes.AttributeType.Strength) * 2;
        // Determine lower and upper bounds of physical damage.
        PhysicalDamageLowerBound = _attributes.GetAttributeAmount(Attributes.AttributeType.Strength) * 1.7f;
        PhysicalDamageUpperBound = _attributes.GetAttributeAmount(Attributes.AttributeType.Strength) * 2.1f;
    }

    private void CalculateMagicalDamage()
    {
        // Magical damage is equal to points spent on intelligence multiplied  by 3.
        MagicalDamage = _attributes.GetAttributeAmount(Attributes.AttributeType.Intelligence) * 3;
    }

    private void CalculateArmor()
    {
        // Armor is equal to points spent on resilience halved.
        Armor = _attributes.GetAttributeAmount(Attributes.AttributeType.Resilience) * 0.5f;
    }

    private void CalculateHealthRegen()
    {
        HealthRegen = _attributes.GetAttributeAmount(Attributes.AttributeType.Vitality) * 0.1f;
    }

    private void CalculateManaRegen()
    {
        ManaRegen = _attributes.GetAttributeAmount(Attributes.AttributeType.Focus) * 0.1f;
    }
}