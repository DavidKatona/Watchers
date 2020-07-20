using Assets.Scripts.Attributes;
using System;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [SerializeField] private CharacterMenu _characterMenu;
    [SerializeField] private DebugMenu _debugMenu;
    [SerializeField] private PlayerHUD _playerHUD;
    private Attributes _attributes;

    // Encapsulate stats to SingleStat private class similar to Attributes or use interfaces (since the implementation of their calculations differ).
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; set; }
    public float MaxMana { get; private set; }
    public float CurrentMana { get; private set; }
    public float PhysicalDamage { get; private set; }
    public float MagicalDamage { get; private set; }
    public float Armor { get; private set; }
    public float HealthRegen { get; private set; }
    public float ManaRegen { get; private set; }

    void Start()
    {
        Attributes attributes = new Attributes(11, 11, 11, 11, 11, 11, 11);

        SetAttributes(attributes);
        RecalculateAllStats();

        _characterMenu.SetAttributes(attributes);
        _debugMenu.SetAttributes(attributes);
        _playerHUD.SetAttributes(attributes);
    }

    public float GetHealthPercentage()
    {
        return CurrentHealth / MaxHealth;
    }

    public float GetManaPercentage()
    {
        return CurrentMana / MaxMana;
    }

    public void SetAttributes(Attributes attributes)
    {
        _attributes = attributes;
        attributes.OnAttributeChanged += Attributes_OnAttributesChanged;
    }

    private void Attributes_OnAttributesChanged(object sender, EventArgs e)
    {
        RecalculateAllStats();
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

        // Restore all missing health and manapoints.
        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;
    }

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